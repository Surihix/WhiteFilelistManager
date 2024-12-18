using System.Text;
using System.Text.Json;
using static WhiteFilelistManager.CoreForm;

namespace WhiteFilelistManager.FilelistHelpers
{
    internal class JsonProcesses
    {
        public static void JsonUnpackProcess(FilelistVariables filelistVariables, GameCode gameCode)
        {
            using (var jsonStream = new MemoryStream())
            {
                var jsonWriterOptions = new JsonWriterOptions
                {
                    Indented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                using (var jsonWriter = new Utf8JsonWriter(jsonStream, jsonWriterOptions))
                {
                    jsonWriter.WriteStartObject();

                    if (filelistVariables.IsEncrypted)
                    {
                        jsonWriter.WriteBoolean("encrypted", true);
                        jsonWriter.WriteNumber("seedA", filelistVariables.SeedA);
                        jsonWriter.WriteNumber("seedB", filelistVariables.SeedB);
                        jsonWriter.WriteNumber("encryptionTag(DO_NOT_CHANGE)", filelistVariables.EncTag);
                    }

                    jsonWriter.WriteNumber("fileCount", filelistVariables.TotalFiles);
                    jsonWriter.WriteNumber("chunkCount", filelistVariables.TotalChunks);
                    jsonWriter.WriteStartObject("data");

                    using (var entriesStream = new MemoryStream())
                    {
                        entriesStream.Write(filelistVariables.EntriesData, 0, filelistVariables.EntriesData.Length);
                        entriesStream.Seek(0, SeekOrigin.Begin);

                        using (var entriesReader = new BinaryReader(entriesStream))
                        {
                            int chunkNumberJson = -1;
                            long entriesReadPos = 0;

                            for (int f = 0; f < filelistVariables.TotalFiles; f++)
                            {
                                FilelistProcesses.GetCurrentFileEntry(gameCode, entriesReader, entriesReadPos, filelistVariables);
                                entriesReadPos += 8;

                                if (gameCode == GameCode.ff131)
                                {
                                    OpenCloseChunkArray(filelistVariables.ChunkNumber, ref chunkNumberJson, f, jsonWriter);

                                    jsonWriter.WriteStartObject();
                                    jsonWriter.WriteNumber("fileCode", filelistVariables.FileCode);
                                }
                                else if (gameCode == GameCode.ff132)
                                {
                                    OpenCloseChunkArray(filelistVariables.CurrentChunkNumber, ref chunkNumberJson, f, jsonWriter);

                                    jsonWriter.WriteStartObject();
                                    jsonWriter.WriteNumber("fileCode", filelistVariables.FileCode);
                                    jsonWriter.WriteNumber("fileTypeID", filelistVariables.FileTypeID);
                                }

                                jsonWriter.WriteString("filePath", filelistVariables.PathString);
                                jsonWriter.WriteEndObject();
                            }
                        }
                    }

                    jsonWriter.WriteEndArray();
                    jsonWriter.WriteEndObject();
                    jsonWriter.WriteEndObject();
                }

                var outJsonFile = Path.Combine(filelistVariables.MainFilelistDirectory, filelistVariables.FilelistOutName + ".json");
                SharedFunctions.IfFileExistsDel(outJsonFile);

                jsonStream.Seek(0, SeekOrigin.Begin);
                File.WriteAllBytes(outJsonFile, jsonStream.ToArray());
            }
        }

        private static void OpenCloseChunkArray(int chunkNumber, ref int chunkNumberJson, int f, Utf8JsonWriter jsonWriter)
        {
            if (chunkNumber != chunkNumberJson)
            {
                chunkNumberJson = chunkNumber;

                if (f != 0)
                {
                    jsonWriter.WriteEndArray();
                }

                jsonWriter.WriteStartArray($"Chunk_{chunkNumber}");
            }
        }


        public static void JsonRepackProcess(string jsonFile, GameCode gameCode)
        {
            var filelistVariables = new FilelistVariables();

            var jsonData = File.ReadAllBytes(jsonFile);

            var options = new JsonReaderOptions
            {
                AllowTrailingCommas = true,
                CommentHandling = JsonCommentHandling.Skip
            };

            var jsonReader = new Utf8JsonReader(jsonData, options);
            _ = jsonReader.Read();


            if (gameCode == GameCode.ff132)
            {
                JsonFunctions.CheckJSONProperty(ref jsonReader, "Bool", "encrypted");
                filelistVariables.IsEncrypted = jsonReader.GetBoolean();

                if (filelistVariables.IsEncrypted)
                {
                    JsonFunctions.CheckJSONProperty(ref jsonReader, "Number", "seedA");
                    filelistVariables.SeedA = jsonReader.GetUInt64();

                    JsonFunctions.CheckJSONProperty(ref jsonReader, "Number", "seedB");
                    filelistVariables.SeedB = jsonReader.GetUInt64();

                    JsonFunctions.CheckJSONProperty(ref jsonReader, "Number", "encryptionTag(DO_NOT_CHANGE)");
                    filelistVariables.EncTag = jsonReader.GetUInt32();
                }
            }

            JsonFunctions.CheckJSONProperty(ref jsonReader, "Number", "fileCount");
            filelistVariables.TotalFiles = jsonReader.GetUInt32();

            JsonFunctions.CheckJSONProperty(ref jsonReader, "Number", "chunkCount");
            filelistVariables.TotalChunks = jsonReader.GetUInt32();

            JsonFunctions.CheckJSONProperty(ref jsonReader, "Object", "data");

            var newChunksDict = new Dictionary<int, List<byte>>();
            FilelistProcesses.CreateEmptyNewChunksDict(filelistVariables, newChunksDict);

            var oddChunkNumValues = new List<int>();
            if (gameCode == GameCode.ff132 && filelistVariables.TotalChunks > 1)
            {
                FilelistProcesses.CreateOddChunkNumList(filelistVariables, ref oddChunkNumValues);
            }

            using (var entriesStream = new MemoryStream())
            {
                using (var entriesWriter = new BinaryWriter(entriesStream))
                {

                    var oddChunkCounter = 0;
                    long entriesWriterPos = 0;

                    for (int c = 0; c < filelistVariables.TotalChunks; c++)
                    {
                        JsonFunctions.CheckJSONProperty(ref jsonReader, "Array", $"Chunk_{c}");

                        while (true)
                        {
                            _ = jsonReader.Read();

                            if (jsonReader.TokenType == JsonTokenType.EndArray)
                            {
                                oddChunkCounter++;
                                break;
                            }

                            JsonFunctions.CheckJSONProperty(ref jsonReader, "Number", "fileCode");
                            filelistVariables.FileCode = jsonReader.GetUInt32();

                            entriesWriter.BaseStream.Position = entriesWriterPos;
                            entriesWriter.WriteBytesUInt32(filelistVariables.FileCode, false);

                            if (gameCode == GameCode.ff131)
                            {
                                entriesWriter.BaseStream.Position = entriesWriterPos + 4;
                                entriesWriter.WriteBytesUInt16((ushort)c, false);

                                entriesWriter.BaseStream.Position = entriesWriterPos + 6;
                                entriesWriter.WriteBytesUInt16(0, false);
                            }
                            else
                            {
                                JsonFunctions.CheckJSONProperty(ref jsonReader, "Number", "fileTypeID");
                                filelistVariables.FileTypeID = jsonReader.GetByte();

                                entriesWriter.BaseStream.Position = entriesWriterPos + 4;

                                if (oddChunkNumValues.Contains(c))
                                {
                                    oddChunkCounter = oddChunkNumValues.IndexOf(c);
                                    entriesWriter.WriteBytesUInt16(32768, false);
                                }
                                else
                                {
                                    entriesWriter.WriteBytesUInt16(0, false);
                                }

                                entriesWriter.BaseStream.Position = entriesWriterPos + 6;
                                entriesWriter.Write((byte)oddChunkCounter);

                                entriesWriter.BaseStream.Position = entriesWriterPos + 7;
                                entriesWriter.Write(filelistVariables.FileTypeID);
                            }

                            JsonFunctions.CheckJSONProperty(ref jsonReader, "String", "filePath");
                            filelistVariables.PathString = jsonReader.GetString();

                            newChunksDict[c].AddRange(Encoding.UTF8.GetBytes(filelistVariables.PathString + "\0"));

                            _ = jsonReader.Read();

                            entriesWriterPos += 8;
                        }
                    }

                    filelistVariables.EntriesData = new byte[entriesStream.Length];
                    entriesStream.Seek(0, SeekOrigin.Begin);
                    entriesStream.Read(filelistVariables.EntriesData, 0, filelistVariables.EntriesData.Length);
                }
            }

            filelistVariables.MainFilelistDirectory = Path.GetDirectoryName(jsonFile);
            filelistVariables.FilelistOutName = Path.GetFileNameWithoutExtension(jsonFile);
            filelistVariables.MainFilelistFile = Path.Combine(filelistVariables.MainFilelistDirectory, filelistVariables.FilelistOutName);
        }
    }
}
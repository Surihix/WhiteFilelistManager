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
                    else
                    {
                        jsonWriter.WriteBoolean("encrypted", false);
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

            JsonFunctions.CheckTokenType("PropertyName", ref jsonReader, "encrypted");
            JsonFunctions.CheckPropertyName(ref jsonReader, "encrypted");
            JsonFunctions.CheckTokenType("Bool", ref jsonReader, "encrypted");
            filelistVariables.IsEncrypted = jsonReader.GetBoolean();

            if (gameCode == GameCode.ff131 && filelistVariables.IsEncrypted)
            {
                SharedFunctions.Error("XIII-1 filelists cannot be encrypted. make sure the encrypted property's value is set to false.");
            }

            if (gameCode == GameCode.ff132 && filelistVariables.IsEncrypted)
            {
                JsonFunctions.CheckTokenType("PropertyName", ref jsonReader, "seedA");
                JsonFunctions.CheckPropertyName(ref jsonReader, "seedA");
                JsonFunctions.CheckTokenType("Number", ref jsonReader, "seedA");
                filelistVariables.SeedA = jsonReader.GetUInt64();

                JsonFunctions.CheckTokenType("PropertyName", ref jsonReader, "seedB");
                JsonFunctions.CheckPropertyName(ref jsonReader, "seedB");
                JsonFunctions.CheckTokenType("Number", ref jsonReader, "seedB");
                filelistVariables.SeedB = jsonReader.GetUInt64();

                JsonFunctions.CheckTokenType("PropertyName", ref jsonReader, "encryptionTag(DO_NOT_CHANGE)");
                JsonFunctions.CheckPropertyName(ref jsonReader, "encryptionTag(DO_NOT_CHANGE)");
                JsonFunctions.CheckTokenType("Number", ref jsonReader, "encryptionTag(DO_NOT_CHANGE)");
                filelistVariables.EncTag = jsonReader.GetUInt32();
            }

            JsonFunctions.CheckTokenType("PropertyName", ref jsonReader, "fileCount");
            JsonFunctions.CheckPropertyName(ref jsonReader, "fileCount");
            JsonFunctions.CheckTokenType("Number", ref jsonReader, "fileCount");
            filelistVariables.TotalFiles = jsonReader.GetUInt32();

            JsonFunctions.CheckTokenType("PropertyName", ref jsonReader, "chunkCount");
            JsonFunctions.CheckPropertyName(ref jsonReader, "chunkCount");
            JsonFunctions.CheckTokenType("Number", ref jsonReader, "chunkCount");
            filelistVariables.TotalChunks = jsonReader.GetUInt32();

            JsonFunctions.CheckTokenType("PropertyName", ref jsonReader, "data");
            JsonFunctions.CheckPropertyName(ref jsonReader, "data");
            JsonFunctions.CheckTokenType("Object_Start", ref jsonReader, "data");

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

                }
            }

        }
    }
}
using System.Text.Json;
using static WhiteFilelistManager.CoreForm;

namespace WhiteFilelistManager.FilelistHelpers
{
    internal class JsonFunctions
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


        public static void JsonRepackProcess(FilelistVariables filelistVariables, string jsonFile, GameCode gameCode)
        {

        }
    }
}
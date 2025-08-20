using System.Text.Json;
using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools
{
    internal class BatchGenProcesses
    {
        public static string DirectoryPath { get; set; }

        public static string[] GetFilePathsFromDir()
        {
            var filesInDir = Directory.GetFiles(DirectoryPath, "*.*", SearchOption.AllDirectories);
            Array.Sort(filesInDir);

            string currentFilePath;
            var dirNameLength = DirectoryPath.Length;

            for (int i = 0; i < filesInDir.Length; i++)
            {
                currentFilePath = filesInDir[i].Remove(0, dirNameLength + 1).Replace("\\", "/");
                filesInDir[i] = currentFilePath;
            }

            return filesInDir;
        }


        public static void BuildIDBasedPathsDict()
        {
            GenerationVariables.IdBasedPathsDataDict = new Dictionary<string, List<string>>();

            using (var sr = new StreamReader(GenerationVariables.IdBasedPathsTxtFile))
            {
                string currentLine;
                string[] currentLineData;
                while ((currentLine = sr.ReadLine()) != null)
                {
                    currentLineData = currentLine.Split('|');

                    if (currentLineData.Length == 1)
                    {
                        continue;
                    }

                    GenerationVariables.IdBasedPathsDataDict.Add(currentLineData[0], new List<string>());

                    for (int i = 1; i < currentLineData.Length; i++)
                    {
                        GenerationVariables.IdBasedPathsDataDict[currentLineData[0]].Add(currentLineData[i]);
                    }
                }
            }
        }


        public static void CreateFilelistForDir(Dictionary<string, (uint, int)> processedDataDict, GameID gameID)
        {
            var outFilelistFile = Path.Combine(Path.GetDirectoryName(DirectoryPath), "#batch_filelist.bin");

            var pass1Dict = new Dictionary<int, List<uint>>();

            foreach (var item in processedDataDict)
            {
                if (!pass1Dict.ContainsKey(item.Value.Item2))
                {
                    pass1Dict.Add(item.Value.Item2, new List<uint>());
                }

                pass1Dict[item.Value.Item2].Add(item.Value.Item1);
            }

            foreach (var item in pass1Dict)
            {
                pass1Dict[item.Key].Sort();
            }
        }


        public static void CreateJSONOutputForDir(Dictionary<string, (uint, int)> processedDataDict, GameID gameID)
        {
            var outJsonFile = Path.Combine(Path.GetDirectoryName(DirectoryPath), "#batch_json.json");

            using (var jsonStream = new MemoryStream())
            {
                using (var jsonWriter = new Utf8JsonWriter(jsonStream, new JsonWriterOptions { Indented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }))
                {
                    jsonWriter.WriteStartObject();
                    jsonWriter.WriteNumber("fileCount", processedDataDict.Count);

                    jsonWriter.WriteStartArray("data");

                    foreach (var item in processedDataDict)
                    {
                        jsonWriter.WriteStartObject();
                        jsonWriter.WriteNumber("fileCode", item.Value.Item1);

                        if (gameID != GameID.xiii)
                        {
                            jsonWriter.WriteNumber("fileTypeID", item.Value.Item2);
                        }

                        jsonWriter.WriteString("filePath", $"0:0:0:{item.Key}");
                        jsonWriter.WriteEndObject();
                    }

                    jsonWriter.WriteEndArray();
                    jsonWriter.WriteEndObject();
                }

                jsonStream.Seek(0, SeekOrigin.Begin);
                SharedFunctions.IfFileExistsDel(outJsonFile);

                File.WriteAllBytes(outJsonFile, jsonStream.ToArray());
            }
        }


        public static void CreateTxtOutputForDir(Dictionary<string, (uint, int)> processedDataDict, GameID gameID)
        {
            var outTxtFile = Path.Combine(Path.GetDirectoryName(DirectoryPath), "#batch_txt.txt");
            SharedFunctions.IfFileExistsDel(outTxtFile);

            using (var sw = new StreamWriter(outTxtFile, true))
            {
                foreach (var item in processedDataDict)
                {
                    sw.Write(item.Value.Item1 + "|");

                    if (gameID != GameID.xiii)
                    {
                        sw.Write(item.Value.Item2 + "|");
                    }

                    sw.WriteLine($"0:0:0:{item.Key}");
                }
            }
        }
    }
}
using System.Text.Json;
using WhiteFilelistManager.FilelistTools.FilelistHelpers;
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


        private static readonly int MaxChunkLimit = 8250;
        private static readonly string Position = "fffff";
        private static readonly string UnCmpSize = "ffffff";
        private static readonly string CmpSize = "ffff00";
        private static readonly string[] UncmpExtns = new string[] { ".bik", ".scd" };
        public static void CreateFilelistForDir(Dictionary<string, (uint, int)> processedDataDict, GameID gameID)
        {
            var filetypeDict = new Dictionary<int, List<uint>>();

            foreach (var item in processedDataDict)
            {
                if (!filetypeDict.ContainsKey(item.Value.Item2))
                {
                    filetypeDict.Add(item.Value.Item2, new List<uint>());
                }

                filetypeDict[item.Value.Item2].Add(item.Value.Item1);
            }

            foreach (var item in filetypeDict)
            {
                filetypeDict[item.Key].Sort();
            }

            var fileTypeSorted = new List<int>();
            fileTypeSorted.AddRange(filetypeDict.Keys);
            fileTypeSorted.Sort();

            var filelistDataDict = new Dictionary<string, (uint, int)>();

            foreach (var fileType in fileTypeSorted)
            {
                var currentFileCodeList = filetypeDict[fileType];

                foreach (var fileCode in currentFileCodeList)
                {
                    foreach (var path in processedDataDict)
                    {
                        if ((path.Value.Item1, path.Value.Item2) == (fileCode, fileType))
                        {
                            filelistDataDict.Add(path.Key, (fileCode, fileType));
                            break;
                        }
                    }
                }
            }

            var outFilelistFile = Path.Combine(Path.GetDirectoryName(DirectoryPath), $"filelist_{Path.GetFileName(DirectoryPath)}.bin");
            SharedFunctions.IfFileExistsDel(outFilelistFile);

            var outFilelistUnpackedDir = Path.Combine(Path.GetDirectoryName(outFilelistFile), $"_{Path.GetFileName(outFilelistFile)}");
            if (Directory.Exists(outFilelistUnpackedDir))
            {
                Directory.Delete(outFilelistUnpackedDir, true);
            }

            Directory.CreateDirectory(outFilelistUnpackedDir);

            var filelistDataDictKeys = new List<string>();
            filelistDataDictKeys.AddRange(filelistDataDict.Keys);

            var fileCount = filelistDataDictKeys.Count;
            var chunkCounter = 0;

            for (int f = 0; f < fileCount; f++)
            {
                var currentChunkSize = 0;

                using (var chunkWriter = new StreamWriter(Path.Combine(outFilelistUnpackedDir, $"Chunk_{chunkCounter}.txt")))
                {
                    while (true)
                    {
                        if (f == fileCount)
                        {
                            break;
                        }

                        var filePath = filelistDataDictKeys[f];
                        var filelistData = filelistDataDict[filePath];

                        string currentPath;

                        if (UncmpExtns.Contains(Path.GetExtension(filePath)))
                        {
                            currentPath = $"{Position}:{UnCmpSize}:{UnCmpSize}:{filePath}";
                        }
                        else
                        {
                            currentPath = $"{Position}:{UnCmpSize}:{CmpSize}:{filePath}";
                        }

                        currentChunkSize += System.Text.Encoding.UTF8.GetByteCount(currentPath + "\0");

                        if (currentChunkSize > MaxChunkLimit)
                        {
                            f--;
                            break;
                        }

                        string currentChunkTxtLine;

                        if (gameID == GameID.xiii)
                        {
                            currentChunkTxtLine = $"{filelistData.Item1}|{currentPath}";
                        }
                        else
                        {
                            currentChunkTxtLine = $"{filelistData.Item1}|{filelistData.Item2}|{currentPath}";
                        }

                        chunkWriter.WriteLine(currentChunkTxtLine);

                        f++;
                    }
                }

                chunkCounter++;
            }

            using (var infoWriter = new StreamWriter(Path.Combine(outFilelistUnpackedDir, "#info.txt")))
            {
                if (gameID != GameID.xiii)
                {
                    infoWriter.WriteLine("encrypted: false");
                }

                infoWriter.WriteLine($"fileCount: {fileCount}");
                infoWriter.WriteLine($"chunkCount: {chunkCounter}");
            }

            var gameCode = gameID == GameID.xiii ? GameCode.ff131 : GameCode.ff132;
            FilelistTools.TxtsProcesses.TxtsRepackProcess(outFilelistUnpackedDir, gameCode);
            Directory.Delete(outFilelistUnpackedDir, true);

            if (gameCode == GameCode.ff132)
            {
                var filelistData = File.ReadAllBytes(outFilelistFile);
                SharedFunctions.IfFileExistsDel(outFilelistFile);

                byte[] md5Hash;

                using (var md5 = System.Security.Cryptography.MD5.Create())
                {
                    md5Hash = md5.ComputeHash(filelistData);
                }

                using (var filelistPreEncStream = new FileStream(outFilelistFile, FileMode.Append, FileAccess.Write))
                {
                    filelistPreEncStream.Write(md5Hash);
                    filelistPreEncStream.Write(new byte[4]);
                    filelistPreEncStream.Write(BitConverter.GetBytes((uint)501232760));
                    filelistPreEncStream.Write(new byte[8]);
                    filelistPreEncStream.Write(filelistData);
                }

                FilelistCrypto.EncryptProcess(outFilelistFile);
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
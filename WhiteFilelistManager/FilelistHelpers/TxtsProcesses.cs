using System.Text;
using static WhiteFilelistManager.CoreForm;

namespace WhiteFilelistManager.FilelistHelpers
{
    internal class TxtsProcesses
    {
        public static void TxtsUnpackProcess(FilelistVariables filelistVariables, GameCode gameCode)
        {
            var extractedFilelistDir = Path.Combine(filelistVariables.MainFilelistDirectory, "_" + filelistVariables.FilelistOutName);
            if (Directory.Exists(extractedFilelistDir))
            {
                Directory.Delete(extractedFilelistDir, true);
            }

            Directory.CreateDirectory(extractedFilelistDir);

            var infoFile = Path.Combine(extractedFilelistDir, "#info.txt");
            var chunkTxtFilePathPrefix = Path.Combine(extractedFilelistDir, $"Chunk_");

            using (var infoStreamWriter = new StreamWriter(infoFile, true))
            {
                if (filelistVariables.IsEncrypted)
                {
                    infoStreamWriter.WriteLine("encrypted: true");
                    infoStreamWriter.WriteLine($"seedA: {filelistVariables.SeedA}");
                    infoStreamWriter.WriteLine($"seedB: {filelistVariables.SeedB}");
                    infoStreamWriter.WriteLine($"encryptionTag(DO_NOT_CHANGE): {filelistVariables.EncTag}");
                }

                infoStreamWriter.WriteLine($"fileCount: {filelistVariables.TotalFiles}");
                infoStreamWriter.WriteLine($"chunkCount: {filelistVariables.TotalChunks}");
            }

            var outChunksDict = new Dictionary<int, List<string>>();

            for (int c = 0; c < filelistVariables.TotalChunks; c++)
            {
                var chunkDataList = new List<string>();
                outChunksDict.Add(c, chunkDataList);
            }

            using (var entriesStream = new MemoryStream())
            {
                entriesStream.Write(filelistVariables.EntriesData, 0, filelistVariables.EntriesData.Length);
                entriesStream.Seek(0, SeekOrigin.Begin);

                using (var entriesReader = new BinaryReader(entriesStream))
                {
                    long entriesReadPos = 0;
                    var stringData = "";

                    for (int f = 0; f < filelistVariables.TotalFiles; f++)
                    {
                        FilelistProcesses.GetCurrentFileEntry(gameCode, entriesReader, entriesReadPos, filelistVariables);
                        entriesReadPos += 8;

                        stringData = "";

                        if (gameCode == GameCode.ff131)
                        {
                            stringData += filelistVariables.FileCode + "|";
                            stringData += filelistVariables.PathString;

                            outChunksDict[filelistVariables.ChunkNumber].Add(stringData);
                        }
                        else
                        {
                            stringData += filelistVariables.FileCode + "|";
                            stringData += filelistVariables.FileTypeID + "|";
                            stringData += filelistVariables.PathString;

                            outChunksDict[filelistVariables.CurrentChunkNumber].Add(stringData);
                        }
                    }
                }
            }

            for (int d = 0; d < filelistVariables.TotalChunks; d++)
            {
                using (var chunkWriter = new StreamWriter(chunkTxtFilePathPrefix + d + ".txt", true, new UTF8Encoding(false)))
                {
                    foreach (var stringData in outChunksDict[d])
                    {
                        chunkWriter.WriteLine(stringData);
                    }
                }
            }
        }


        public static void TxtsRepackProcess(string txtsDir, GameCode gameCode)
        {

        }
    }
}
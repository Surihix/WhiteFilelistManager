using System.Text;
using WhiteFilelistManager.FilelistTools.FilelistHelpers;
using WhiteFilelistManager.FilelistTools.Support;
using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.FilelistTools
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
                if (gameCode == GameCode.ff132)
                {
                    infoStreamWriter.WriteLine($"encrypted: {filelistVariables.IsEncrypted.ToString().ToLower()}");

                    if (filelistVariables.IsEncrypted)
                    {
                        infoStreamWriter.WriteLine($"seedA: {filelistVariables.SeedA}");
                        infoStreamWriter.WriteLine($"seedB: {filelistVariables.SeedB}");
                        infoStreamWriter.WriteLine($"encryptionTag(DO_NOT_CHANGE): {filelistVariables.EncTag}");
                    }
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
            var infoFile = Path.Combine(txtsDir, "#info.txt");

            if (!File.Exists(infoFile))
            {
                SharedFunctions.Error("Missing #info.txt file in the unpacked filelist folder");
            }

            var infoFileLines = File.ReadAllLines(infoFile);

            var filelistVariables = new FilelistVariables();

            if (gameCode == GameCode.ff131)
            {
                if (infoFileLines.Length < 2)
                {
                    SharedFunctions.Error("Not enough data present in the #info.txt file");
                }

                TxtsFunctions.CheckPropertyInInfoFile(infoFileLines[0], "fileCount: ", "Uint");
                filelistVariables.TotalFiles = uint.Parse(infoFileLines[0].Split(' ')[1]);

                TxtsFunctions.CheckPropertyInInfoFile(infoFileLines[1], "chunkCount: ", "Uint");
                filelistVariables.TotalChunks = uint.Parse(infoFileLines[1].Split(' ')[1]);

            }
            else if (gameCode == GameCode.ff132)
            {
                if (infoFileLines.Length < 3)
                {
                    SharedFunctions.Error("Not enough data present in the #info.txt file");
                }

                TxtsFunctions.CheckPropertyInInfoFile(infoFileLines[0], "encrypted: ", "Boolean");
                filelistVariables.IsEncrypted = bool.Parse(infoFileLines[0].Split(' ')[1]);

                if (filelistVariables.IsEncrypted)
                {
                    TxtsFunctions.CheckPropertyInInfoFile(infoFileLines[1], "seedA: ", "Ulong");
                    filelistVariables.SeedA = ulong.Parse(infoFileLines[1].Split(' ')[1]);

                    TxtsFunctions.CheckPropertyInInfoFile(infoFileLines[2], "seedB: ", "Ulong");
                    filelistVariables.SeedB = ulong.Parse(infoFileLines[2].Split(' ')[1]);

                    TxtsFunctions.CheckPropertyInInfoFile(infoFileLines[3], "encryptionTag(DO_NOT_CHANGE): ", "Uint");
                    filelistVariables.EncTag = uint.Parse(infoFileLines[3].Split(' ')[1]);

                    TxtsFunctions.CheckPropertyInInfoFile(infoFileLines[4], "fileCount: ", "Uint");
                    filelistVariables.TotalFiles = uint.Parse(infoFileLines[4].Split(' ')[1]);

                    TxtsFunctions.CheckPropertyInInfoFile(infoFileLines[5], "chunkCount: ", "Uint");
                    filelistVariables.TotalChunks = uint.Parse(infoFileLines[5].Split(' ')[1]);

                    using (var encHeaderStream = new MemoryStream())
                    {
                        using (var encHeaderWriter = new BinaryWriter(encHeaderStream))
                        {
                            encHeaderStream.Seek(0, SeekOrigin.Begin);

                            encHeaderWriter.WriteBytesUInt64(filelistVariables.SeedA, false);
                            encHeaderWriter.WriteBytesUInt64(filelistVariables.SeedB, false);
                            encHeaderWriter.WriteBytesUInt32(0, false);
                            encHeaderWriter.WriteBytesUInt32(filelistVariables.EncTag, false);
                            encHeaderWriter.WriteBytesUInt64(0, false);

                            encHeaderStream.Seek(0, SeekOrigin.Begin);
                            filelistVariables.EncryptedHeaderData = new byte[32];
                            filelistVariables.EncryptedHeaderData = encHeaderStream.ToArray();
                        }
                    }
                }
                else
                {
                    TxtsFunctions.CheckPropertyInInfoFile(infoFileLines[1], "fileCount: ", "Uint");
                    filelistVariables.TotalFiles = uint.Parse(infoFileLines[1].Split(' ')[1]);

                    TxtsFunctions.CheckPropertyInInfoFile(infoFileLines[2], "chunkCount: ", "Uint");
                    filelistVariables.TotalChunks = uint.Parse(infoFileLines[2].Split(' ')[1]);
                }
            }

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
                        filelistVariables.LastChunkNumber = c;
                        var currentChunkFile = Path.Combine(txtsDir, $"Chunk_{c}.txt");

                        if (!File.Exists(currentChunkFile))
                        {
                            SharedFunctions.Error($"Missing 'Chunk_{c}.txt' file in the unpacked filelist folder");
                        }

                        var currentChunkData = File.ReadAllLines(currentChunkFile);

                        for (int l = 0; l < currentChunkData.Length; l++)
                        {
                            var currentEntryData = currentChunkData[l].Split('|');

                            if (gameCode == GameCode.ff131)
                            {
                                if (currentEntryData.Length < 2)
                                {
                                    SharedFunctions.Error("");
                                }

                                TxtsFunctions.CheckChunkEntryData(currentEntryData[0], "Uint", c, l);
                                filelistVariables.FileCode = uint.Parse(currentEntryData[0]);

                                entriesWriter.BaseStream.Position = entriesWriterPos;
                                entriesWriter.WriteBytesUInt32(filelistVariables.FileCode, false);

                                entriesWriter.BaseStream.Position = entriesWriterPos + 4;
                                entriesWriter.WriteBytesUInt16((ushort)c, false);

                                entriesWriter.BaseStream.Position = entriesWriterPos + 6;
                                entriesWriter.WriteBytesUInt16(0, false);

                                filelistVariables.PathString = currentEntryData[1];
                            }
                            else if (gameCode == GameCode.ff132)
                            {
                                if (currentEntryData.Length < 3)
                                {
                                    SharedFunctions.Error("");
                                }

                                TxtsFunctions.CheckChunkEntryData(currentEntryData[0], "Uint", c, l);
                                filelistVariables.FileCode = uint.Parse(currentEntryData[0]);

                                TxtsFunctions.CheckChunkEntryData(currentEntryData[1], "Byte", c, l);
                                filelistVariables.FileTypeID = byte.Parse(currentEntryData[1]);

                                entriesWriter.BaseStream.Position = entriesWriterPos;
                                entriesWriter.WriteBytesUInt32(filelistVariables.FileCode, false);

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

                                filelistVariables.PathString = currentEntryData[2];
                            }

                            newChunksDict[c].AddRange(Encoding.UTF8.GetBytes(filelistVariables.PathString + "\0"));

                            entriesWriterPos += 8;
                        }

                        oddChunkCounter++;
                    }

                    filelistVariables.EntriesData = new byte[entriesStream.Length];
                    entriesStream.Seek(0, SeekOrigin.Begin);
                    entriesStream.Read(filelistVariables.EntriesData, 0, filelistVariables.EntriesData.Length);
                }
            }

            filelistVariables.MainFilelistDirectory = Path.GetDirectoryName(txtsDir);
            filelistVariables.FilelistOutName = Path.GetFileName(txtsDir).Remove(0, 1);
            filelistVariables.MainFilelistFile = Path.Combine(filelistVariables.MainFilelistDirectory, filelistVariables.FilelistOutName);

            SharedFunctions.IfFileExistsDel(filelistVariables.MainFilelistFile);

            RepackFilelistData.BuildFilelist(filelistVariables, newChunksDict, gameCode);

            if (filelistVariables.IsEncrypted)
            {
                FilelistCrypto.EncryptProcess(filelistVariables.MainFilelistFile);
            }
        }
    }
}
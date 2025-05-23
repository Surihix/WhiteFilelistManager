﻿namespace WhiteFilelistManager.FilelistTools.FilelistHelpers
{
    internal class FilelistChunksPrep
    {
        public static void GetFilelistOffsets(BinaryReader filelistReader, FilelistVariables filelistVariables)
        {
            var readStartPosition = new uint();
            var adjustOffset = new uint();

            switch (filelistVariables.IsEncrypted)
            {
                case true:
                    readStartPosition = 32;
                    adjustOffset = 32;
                    break;

                case false:
                    readStartPosition = 0;
                    adjustOffset = 0;
                    break;
            }

            filelistReader.BaseStream.Position = readStartPosition;
            filelistVariables.ChunkInfoSectionOffset = filelistReader.ReadUInt32() + adjustOffset;
            filelistVariables.ChunkDataSectionOffset = filelistReader.ReadUInt32() + adjustOffset;
            filelistVariables.TotalFiles = filelistReader.ReadUInt32();

            var entriesSectionPosition = filelistReader.BaseStream.Position;

            filelistReader.BaseStream.Position = entriesSectionPosition;
            filelistVariables.EntriesData = new byte[(int)filelistVariables.TotalFiles * 8];
            filelistReader.BaseStream.Read(filelistVariables.EntriesData, 0, filelistVariables.EntriesData.Length);

            filelistVariables.ChunkInfoSize = filelistVariables.ChunkDataSectionOffset - filelistVariables.ChunkInfoSectionOffset;
            filelistVariables.TotalChunks = filelistVariables.ChunkInfoSize / 12;
        }


        public static void BuildChunks(FileStream filelistStream, FilelistVariables filelistVariables)
        {
            // Make a memorystream for holding all Chunks info
            using (var chunkInfoStream = new MemoryStream())
            {
                filelistStream.Seek(filelistVariables.ChunkInfoSectionOffset, SeekOrigin.Begin);
                var chunkInfoBuffer = new byte[filelistVariables.ChunkInfoSize];
                filelistStream.Read(chunkInfoBuffer, 0, chunkInfoBuffer.Length);
                chunkInfoStream.Write(chunkInfoBuffer, 0, chunkInfoBuffer.Length);

                // Make memorystream for all Chunks compressed data
                using (var chunkStream = new MemoryStream())
                {
                    filelistStream.Seek(filelistVariables.ChunkDataSectionOffset, SeekOrigin.Begin);
                    filelistStream.CopyTo(chunkStream);

                    // Open a binary reader and read each chunk's info.
                    // Then, add the decompressed data to a dictionary
                    using (var chunkInfoReader = new BinaryReader(chunkInfoStream))
                    {

                        var chunkInfoReadVal = (uint)0;
                        for (int c = 0; c < filelistVariables.TotalChunks; c++)
                        {
                            chunkInfoReader.BaseStream.Position = chunkInfoReadVal + 4;
                            filelistVariables.ChunkCmpSize = chunkInfoReader.ReadUInt32();
                            filelistVariables.ChunkStartOffset = chunkInfoReader.ReadUInt32();

                            chunkStream.Seek(filelistVariables.ChunkStartOffset, SeekOrigin.Begin);
                            using (var chunkToDcmp = new MemoryStream())
                            {
                                var chunkBuffer = new byte[filelistVariables.ChunkCmpSize];
                                var readCmpBytes = chunkStream.Read(chunkBuffer, 0, chunkBuffer.Length);
                                chunkToDcmp.Write(chunkBuffer, 0, readCmpBytes);

                                filelistVariables.ChunkDataDict.Add(c, ZlibFunctions.ZlibDecompressBuffer(chunkToDcmp));
                            }

                            chunkInfoReadVal += 12;
                            filelistVariables.ChunkFNameCount++;
                        }
                    }
                }
            }
        }
    }
}
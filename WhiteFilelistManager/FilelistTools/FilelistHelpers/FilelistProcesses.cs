﻿using System.Text;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.FilelistTools.FilelistHelpers
{
    internal class FilelistProcesses
    {
        public static void PrepareFilelistVars(FilelistVariables filelistVariables, string filelistFile)
        {
            filelistVariables.MainFilelistFile = filelistFile;

            var inFilelistFilePath = Path.GetFullPath(filelistVariables.MainFilelistFile);
            filelistVariables.MainFilelistDirectory = Path.GetDirectoryName(inFilelistFilePath);
            filelistVariables.TmpDcryptFilelistFile = Path.Combine(filelistVariables.MainFilelistDirectory, "filelist_tmp.bin");
        }


        public static void GetCurrentFileEntry(GameCode gameCode, BinaryReader entriesReader, long entriesReadPos, FilelistVariables filelistVariables)
        {
            entriesReader.BaseStream.Position = entriesReadPos;
            filelistVariables.FileCode = entriesReader.ReadUInt32();

            if (gameCode == GameCode.ff131)
            {
                filelistVariables.ChunkNumber = entriesReader.ReadUInt16();
                filelistVariables.PathStringPos = entriesReader.ReadUInt16();
                filelistVariables.LastChunkNumber = filelistVariables.ChunkNumber;

                GeneratePathString(filelistVariables.PathStringPos, filelistVariables.ChunkDataDict[filelistVariables.ChunkNumber], filelistVariables);
            }
            else if (gameCode == GameCode.ff132)
            {
                filelistVariables.PathStringPos = entriesReader.ReadUInt16();
                filelistVariables.ChunkNumber = entriesReader.ReadByte();
                filelistVariables.FileTypeID = entriesReader.ReadByte();
                filelistVariables.LastChunkNumber = filelistVariables.CurrentChunkNumber;

                if (filelistVariables.PathStringPos == 0)
                {
                    filelistVariables.CurrentChunkNumber++;
                }

                if (filelistVariables.PathStringPos == 32768)
                {
                    filelistVariables.CurrentChunkNumber++;
                    filelistVariables.PathStringPos -= 32768;
                }

                if (filelistVariables.PathStringPos > 32768)
                {
                    filelistVariables.PathStringPos -= 32768;
                }

                GeneratePathString(filelistVariables.PathStringPos, filelistVariables.ChunkDataDict[filelistVariables.CurrentChunkNumber], filelistVariables);
            }
        }


        private static void GeneratePathString(ushort pathPos, byte[] currentChunkData, FilelistVariables filelistVariables)
        {
            var length = 0;

            for (int i = pathPos; i < currentChunkData.Length && currentChunkData[i] != 0; i++)
            {
                length++;
            }

            filelistVariables.PathString = Encoding.UTF8.GetString(currentChunkData, pathPos, length);
        }


        public static void CreateEmptyNewChunksDict(FilelistVariables filelistVariables, Dictionary<int, List<byte>> newChunksDict)
        {
            for (int c = 0; c < filelistVariables.TotalChunks; c++)
            {
                var chunkDataList = new List<byte>();
                newChunksDict.Add(c, chunkDataList);
            }
        }


        public static void CreateOddChunkNumList(FilelistVariables filelistVariables, ref List<int> oddChunkNumValues)
        {
            var nextChunkNo = 1;
            for (int i = 0; i < filelistVariables.TotalChunks; i++)
            {
                if (i == nextChunkNo)
                {
                    oddChunkNumValues.Add(i);
                    nextChunkNo += 2;
                }
            }
        }
    }
}
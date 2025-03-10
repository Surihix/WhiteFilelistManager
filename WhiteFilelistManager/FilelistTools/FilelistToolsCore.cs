﻿using WhiteFilelistManager.FilelistTools.FilelistHelpers;
using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.FilelistTools
{
    internal class FilelistToolsCore
    {
        public static void UnpackFilelist(GameCode gameCode, string filelistFile, ParseType parseType)
        {
            var filelistVariables = new FilelistVariables
            {
                FilelistOutName = Path.GetFileName(filelistFile),
                MainFilelistFile = filelistFile
            };

            var inFilelistFilePath = Path.GetFullPath(filelistVariables.MainFilelistFile);
            filelistVariables.MainFilelistDirectory = Path.GetDirectoryName(inFilelistFilePath);
            filelistVariables.TmpDcryptFilelistFile = Path.Combine(filelistVariables.MainFilelistDirectory, "filelist_tmp.bin");

            FilelistCrypto.DecryptProcess(gameCode, filelistVariables);

            using (var filelistStream = new FileStream(filelistVariables.MainFilelistFile, FileMode.Open, FileAccess.Read))
            {
                using (var filelistReader = new BinaryReader(filelistStream))
                {
                    FilelistChunksPrep.GetFilelistOffsets(filelistReader, filelistVariables);
                    FilelistChunksPrep.BuildChunks(filelistStream, filelistVariables);
                }
            }

            if (gameCode == GameCode.ff132)
            {
                filelistVariables.CurrentChunkNumber = -1;
            }

            if (filelistVariables.IsEncrypted)
            {
                SharedFunctions.IfFileExistsDel(filelistVariables.TmpDcryptFilelistFile);
                filelistVariables.MainFilelistFile = filelistFile;

                using (var encDataReader = new BinaryReader(File.Open(filelistFile, FileMode.Open, FileAccess.Read)))
                {
                    encDataReader.BaseStream.Position = 0;
                    filelistVariables.SeedA = encDataReader.ReadUInt64();
                    filelistVariables.SeedB = encDataReader.ReadUInt64();

                    encDataReader.BaseStream.Position += 4;
                    filelistVariables.EncTag = encDataReader.ReadUInt32();
                }
            }

            if (parseType == ParseType.json)
            {
                JsonProcesses.JsonUnpackProcess(filelistVariables, gameCode);
            }
            else
            {
                TxtsProcesses.TxtsUnpackProcess(filelistVariables, gameCode);
            }
        }


        public static void RepackFilelist(ParseType parseType, string jsonFileOrTxtDir, GameCode gameCode)
        {
            BackupOldFilelist(jsonFileOrTxtDir, parseType);

            if (parseType == ParseType.json)
            {
                JsonProcesses.JsonRepackProcess(jsonFileOrTxtDir, gameCode);
            }
            else
            {
                TxtsProcesses.TxtsRepackProcess(jsonFileOrTxtDir, gameCode);
            }
        }


        private static void BackupOldFilelist(string jsonFileOrTxtDir, ParseType parseType)
        {            
            var bckupResult = MessageBox.Show("Backup old Filelist file if present?", "Filelist Backup", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (bckupResult == DialogResult.Yes)
            {
                var filelistFile = string.Empty;

                if (parseType == ParseType.json)
                {
                    filelistFile = Path.Combine(Path.GetDirectoryName(jsonFileOrTxtDir), Path.GetFileNameWithoutExtension(jsonFileOrTxtDir));
                }
                else
                {
                    filelistFile = Path.Combine(Path.GetDirectoryName(jsonFileOrTxtDir), Path.GetFileName(jsonFileOrTxtDir).Remove(0, 1));
                }

                if (File.Exists(filelistFile))
                {
                    SharedFunctions.IfFileExistsDel(filelistFile + ".bak");
                    File.Copy(filelistFile, filelistFile + ".bak");
                }
            }
        }
    }
}
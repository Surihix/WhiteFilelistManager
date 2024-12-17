using static WhiteFilelistManager.CoreForm;

namespace WhiteFilelistManager.FilelistHelpers
{
    internal class FilelistFunctions
    {
        public enum ParseType
        {
            json,
            txt
        }


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

            switch (parseType)
            {
                case ParseType.json:
                    JsonProcesses.JsonUnpackProcess(filelistVariables, gameCode);
                    break;

                case ParseType.txt:
                    TxtsProcesses.TxtsUnpackProcess(filelistVariables, gameCode);
                    break;
            }
        }


        public static void RepackFilelist(ParseType parseType, string jsonFileOrTxtDir, GameCode gameCode)
        {
            switch (parseType)
            {
                case ParseType.json:
                    JsonProcesses.JsonRepackProcess(jsonFileOrTxtDir, gameCode);
                    break;

                case ParseType.txt:
                    TxtsProcesses.TxtsRepackProcess(jsonFileOrTxtDir, gameCode);
                    break;
            }
        }
    }
}
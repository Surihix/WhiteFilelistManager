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

        public static void UnpackFilelist(ParseType parseType, GameCode gameCode, string filelistFile)
        {
            var filelistVariables = new FilelistVariables();
            filelistVariables.FilelistOutName = Path.GetFileName(filelistFile);

            FilelistProcesses.PrepareFilelistVars(filelistVariables, filelistFile);
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
                    JsonFunctions.JsonUnpackProcess(filelistVariables, gameCode);
                    break;

                case ParseType.txt:
                    TxtsFunctions.TxtsUnpackProcess(filelistVariables);
                    break;
            }
        }
    }
}
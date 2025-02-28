using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class DbDir
    {
        public static void ProcessDbPath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    DbPathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                    ZonePathXIII2(virtualPathData, virtualPath);
                    break;

                case GameID.xiii3:
                    ZonePathXIIILR(virtualPathData, virtualPath);
                    break;
            }
        }


        #region XIII
        private static void DbPathXIII(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (fileExtn != ".wdb")
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;
            int fileNameNum;
            string fileNameNumBits;
            var dbCategoryBits = string.Empty;
            string reservedBits;

            string fileCode;

            // 8 bits
            var mainTypeBits = string.Empty;

            if (virtualPathData.Length == 3)
            {
                switch (startingPortion)
                {
                    case "db/ai":
                    case "db/bg":
                    case "db/script":
                        mainTypeBits = Convert.ToString(64, 2).PadLeft(8, '0');

                        // 8 bits
                        dbCategoryBits = Convert.ToString(DetermineDbCategory(virtualPathData[1]), 2).PadLeft(8, '0');

                        // 8 bits
                        reservedBits = "00000000";

                        // 8 bits
                        fileNameNum = GenerationFunctions.DeriveNumFromString(Path.GetFileName(virtualPath));
                        GenerationFunctions.CheckDerivedNumber(fileNameNum, "file", 255);

                        fileNameNumBits = Convert.ToString(fileNameNum, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += dbCategoryBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += fileNameNumBits;

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    default:
                        SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                        break;
                }
            }
            else if (virtualPathData.Length == 5)
            {
                startingPortion += "/" + virtualPathData[2] + "/" + virtualPathData[3];
                switch (startingPortion)
                {
                    case "db/ai/party/interestpoint":
                    case "db/ai/party/talk":
                    case "db/ai/party/voice":
                        switch (virtualPathData[3])
                        {
                            case "interestpoint":
                                mainTypeBits = Convert.ToString(117, 2).PadLeft(8, '0');

                                // 8 bits
                                dbCategoryBits = Convert.ToString(128, 2).PadLeft(8, '0');
                                break;

                            case "voice":
                                mainTypeBits = Convert.ToString(121, 2).PadLeft(8, '0');

                                // 8 bits
                                dbCategoryBits = Convert.ToString(128, 2).PadLeft(8, '0');
                                break;

                            case "talk":
                                mainTypeBits = Convert.ToString(122, 2).PadLeft(8, '0');

                                // 8 bits
                                dbCategoryBits = "00000000";
                                break;
                        }

                        // 8 bits
                        reservedBits = "00000000";

                        // 8 bits
                        fileNameNum = GenerationFunctions.DeriveNumFromString(Path.GetFileName(virtualPath));
                        GenerationFunctions.CheckDerivedNumber(fileNameNum, "file", 255);

                        fileNameNumBits = Convert.ToString(fileNameNum, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += dbCategoryBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += fileNameNumBits;

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    default:
                        SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                        break;
                }
            }
            else
            {
                SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
            }
        }
        #endregion


        #region XIII-2
        private static void ZonePathXIII2(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (fileExtn != ".wdb")
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;

            // 8 bits
            string reservedABits;

            if (virtualPathData.Length == 3)
            {
                reservedABits = "00000000";

                int fileNameNum;
                string fileNameNumBits;
                string dbCategoryBits;
                string reservedBBits;

                switch (startingPortion)
                {
                    case "db/ai":
                    case "db/bg":
                    case "db/btscenetable":
                    case "db/script":
                    case "db/select":
                        // 8 bits
                        dbCategoryBits = Convert.ToString(DetermineDbCategory(virtualPathData[1]), 2).PadLeft(8, '0');

                        // 4 bits
                        reservedBBits = "0000";

                        // 12 bits
                        fileNameNum = GenerationFunctions.DeriveNumFromString(Path.GetFileName(virtualPath));
                        GenerationFunctions.CheckDerivedNumber(fileNameNum, "file", 998);

                        fileNameNumBits = Convert.ToString(fileNameNum, 2).PadLeft(12, '0');

                        // Assemble bits
                        finalComputedBits += reservedABits;
                        finalComputedBits += dbCategoryBits;
                        finalComputedBits += reservedBBits;
                        finalComputedBits += fileNameNumBits;

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "64";
                        break;


                    default:
                        SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                        break;
                }
            }
            else
            {
                SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
            }
        }
        #endregion


        #region XIII-LR
        private static void ZonePathXIIILR(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (fileExtn != ".wdb")
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;

            // 8 bits
            string reservedABits;

            if (virtualPathData.Length == 3)
            {
                reservedABits = "00000000";

                int fileNameNum;
                string fileNameNumBits;
                string dbCategoryBits;
                string reservedBBits;

                switch (startingPortion)
                {
                    case "db/bg":
                    case "db/btscenetable":
                    case "db/script":
                    case "db/select":
                        // 8 bits
                        dbCategoryBits = Convert.ToString(DetermineDbCategory(virtualPathData[1]), 2).PadLeft(8, '0');

                        // 4 bits
                        reservedBBits = "0000";

                        // 12 bits
                        fileNameNum = GenerationFunctions.DeriveNumFromString(Path.GetFileName(virtualPath));
                        GenerationFunctions.CheckDerivedNumber(fileNameNum, "file", 998);

                        fileNameNumBits = Convert.ToString(fileNameNum, 2).PadLeft(12, '0');

                        // Assemble bits
                        finalComputedBits += reservedABits;
                        finalComputedBits += dbCategoryBits;
                        finalComputedBits += reservedBBits;
                        finalComputedBits += fileNameNumBits;

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "64";
                        break;


                    default:
                        SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                        break;
                }
            }
            else if (virtualPathData.Length == 5)
            {
                if (virtualPath.StartsWith("db/ai/npc/pack"))
                {

                }
                else
                {
                    SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                }
            }
            else
            {
                SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
            }
        }
        #endregion


        private static int DetermineDbCategory(string dirName)
        {
            var categoryID = 0;

            switch (dirName)
            {
                case "script":
                    categoryID = 4;
                    break;

                case "ai":
                    categoryID = 10;
                    break;

                case "bg":
                    categoryID = 12;
                    break;

                case "btscenetable":
                    categoryID = 16;
                    break;

                case "select":
                    categoryID = 18;
                    break;
            }

            return categoryID;
        }
    }
}
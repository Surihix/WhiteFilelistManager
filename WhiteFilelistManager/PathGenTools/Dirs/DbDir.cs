﻿using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class DbDir
    {
        private static string ParsingErrorMsg = string.Empty;

        public static void ProcessDbPath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    DbPathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                case GameID.xiii3:
                    ZonePathXIII2LR(virtualPathData, virtualPath);
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
            var extraInfo = string.Empty;

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
                        if (fileNameNum == -1)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = $"Unable to determine file number from path";
                            }
                            else
                            {
                                ParsingErrorMsg = $"Unable to determine file number from filename for a file.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        if (fileNameNum > 255)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 255.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 255.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        fileNameNumBits = Convert.ToString(fileNameNum, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += dbCategoryBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += fileNameNumBits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Category (8 bits): {dbCategoryBits}\r\n\r\n";
                        extraInfo += $"Reserved (8 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"File number (8 bits): {fileNameNumBits}";
                        finalComputedBits.Reverse();

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
                        if (fileNameNum == -1)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = $"Unable to determine file number from path";
                            }
                            else
                            {
                                ParsingErrorMsg = $"Unable to determine file number from filename for a file.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        if (fileNameNum > 255)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 255.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 255.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        fileNameNumBits = Convert.ToString(fileNameNum, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += dbCategoryBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += fileNameNumBits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Category (8 bits): {dbCategoryBits}\r\n\r\n";
                        extraInfo += $"Reserved (8 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"File number (8 bits): {fileNameNumBits}";
                        finalComputedBits.Reverse();

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


        #region XIII-2 and XIII-LR
        private static void ZonePathXIII2LR(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (fileExtn != ".wdb")
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;
            var extraInfo = string.Empty;

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
                        // 8 bits
                        dbCategoryBits = Convert.ToString(DetermineDbCategory(virtualPathData[1]), 2).PadLeft(8, '0');

                        // 4 bits
                        reservedBBits = "0000";

                        // 12 bits
                        fileNameNum = GenerationFunctions.DeriveNumFromString(Path.GetFileName(virtualPath));
                        if (fileNameNum == -1)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = $"Unable to determine file number from path";
                            }
                            else
                            {
                                ParsingErrorMsg = $"Unable to determine file number from filename for a file.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        if (fileNameNum > 998)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 998.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 998.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        fileNameNumBits = Convert.ToString(fileNameNum, 2).PadLeft(12, '0');

                        // Assemble bits
                        finalComputedBits += reservedABits;
                        finalComputedBits += dbCategoryBits;
                        finalComputedBits += reservedBBits;
                        finalComputedBits += fileNameNumBits;

                        extraInfo += $"ReservedA (8 bits): {reservedABits}\r\n\r\n";
                        extraInfo += $"Category (8 bits): {dbCategoryBits}\r\n\r\n";
                        extraInfo += $"ReservedB (4 bits): {reservedBBits}\r\n\r\n";
                        extraInfo += $"File number (12 bits): {fileNameNumBits}";
                        finalComputedBits.Reverse();

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
            }

            return categoryID;
        }
    }
}
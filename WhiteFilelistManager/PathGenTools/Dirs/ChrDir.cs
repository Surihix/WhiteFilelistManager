using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class ChrDir
    {
        private static string ParsingErrorMsg = string.Empty;

        public static void ProcessChrPath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    ChrPathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                    ChrPathXIII2(virtualPathData, virtualPath);
                    break;

                case GameID.xiii3:
                    ChrPathXIIILR(virtualPathData, virtualPath);
                    break;
            }
        }


        #region XIII
        private static void ChrPathXIII(string[] virtualPathData, string virtualPath)
        {
            var validExtensions = new List<string>
            {
                ".trb", ".imgb"
            };

            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (!validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;
            int fileExtnID = 0;
            string fileExtnBits;

            string fileCode = string.Empty;
            string extraInfo = string.Empty;

            // 4 bits
            var mainTypeBits = string.Empty;

            if (virtualPathData.Length > 2)
            {
                switch (startingPortion)
                {
                    case "chr/fa":
                    case "chr/mon":
                    case "chr/npc":
                    case "chr/pc":
                    case "chr/summon":
                    case "chr/weapon":
                        mainTypeBits = Convert.ToString(1, 2).PadLeft(4, '0');

                        // 5 bits
                        var chrCategoryBits = Convert.ToString(DetermineChrCategory(virtualPathData[1]), 2).PadLeft(5, '0');

                        // 10 bits
                        var modelID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        if (modelID == -1)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "Model number in the path is invalid";
                            }
                            else
                            {
                                ParsingErrorMsg = $"Model number in the path is invalid.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        if (modelID > 999)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "Model number in the path is too large. must be from 0 to 999.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"Model number in the path is too large. must be from 0 to 999.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        var modelIDbits = Convert.ToString(modelID, 2).PadLeft(10, '0');

                        // 5 bits
                        fileExtnID = fileExtn == ".imgb" ? 0 : 1;
                        fileExtnBits = Convert.ToString(fileExtnID, 2).PadLeft(5, '0');

                        // 8 bits (remaining)
                        var reservedBits = "00000000";

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += chrCategoryBits;
                        finalComputedBits += modelIDbits;
                        finalComputedBits += fileExtnBits;
                        finalComputedBits += reservedBits;

                        extraInfo += $"MainType (4 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Category (5 bits): {chrCategoryBits}\r\n\r\n";
                        extraInfo += $"ModelID (10 bits): {modelIDbits}\r\n\r\n";
                        extraInfo += $"ModelExtension Type (5 bits): {fileExtnBits}\r\n\r\n";
                        extraInfo += $"Reserved (8 bits): {reservedBits}";
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


        #region XIII-2
        private static void ChrPathXIII2(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            var validExtensions = new List<string>
            {
                ".trb", ".imgb", ".mpk"
            };

            if (!validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;
            int fileExtnID = 0;
            string fileExtnBits;

            string fileCode = string.Empty;
            string extraInfo = string.Empty;

            // 4 bits
            var reservedBits = "0000";

            if (virtualPathData.Length > 2)
            {
                switch (startingPortion)
                {
                    case "chr/exte":
                    case "chr/fa":
                    case "chr/mon":
                    case "chr/npc":
                    case "chr/pc":
                    case "chr/summon":
                    case "chr/weapon":
                        // 5 bits
                        var chrCategoryBits = Convert.ToString(DetermineChrCategory(virtualPathData[1]), 2).PadLeft(5, '0');

                        // 10 bits
                        var modelID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        if (modelID == -1)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "Model number specified is invalid";
                            }
                            else
                            {
                                ParsingErrorMsg = $"Model number specified is invalid.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        if (modelID > 999)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "Model number in the path is too large. must be from 0 to 999.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"Model number in the path is too large. must be from 0 to 999.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        var modelIDbits = Convert.ToString(modelID, 2).PadLeft(10, '0');

                        // 5 bits
                        switch (fileExtn)
                        {
                            case ".imgb":
                                fileExtnID = 0;
                                break;

                            case ".trb":
                                fileExtnID = 1;
                                break;

                            case ".mpk":
                                fileExtnID = 4;
                                break;
                        }

                        fileExtnBits = Convert.ToString(fileExtnID, 2).PadLeft(5, '0');

                        // 8 bits
                        var mpkID = 0;
                        if (fileExtn == ".mpk")
                        {
                            if (virtualPath.Contains("_rain"))
                            {
                                mpkID = 1;
                            }
                            else if (virtualPath.Contains("_snow"))
                            {
                                mpkID = 2;
                            }
                        }
                        var mpkIDbits = Convert.ToString(mpkID, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += reservedBits;
                        finalComputedBits += chrCategoryBits;
                        finalComputedBits += modelIDbits;
                        finalComputedBits += fileExtnBits;
                        finalComputedBits += mpkIDbits;

                        extraInfo += $"Reserved (4 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"Category (5 bits): {chrCategoryBits}\r\n\r\n";
                        extraInfo += $"ModelID (10 bits): {modelIDbits}\r\n\r\n";
                        extraInfo += $"ModelExtension Type (5 bits): {fileExtnBits}\r\n\r\n";
                        extraInfo += $"MpkID (8 bits): {mpkIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "16";
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
        private static void ChrPathXIIILR(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            var validExtensions = new List<string>
            {
                ".trb", ".imgb"
            };

            if (!validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;
            int fileExtnID = 0;
            string fileExtnBits;

            string fileCode = string.Empty;
            string extraInfo = string.Empty;

            // 4 bits
            var reservedBits = "0000";

            if (virtualPathData.Length > 2)
            {
                switch (startingPortion)
                {
                    case "chr/exte":
                    case "chr/fa":
                    case "chr/mon":
                    case "chr/npc":
                    case "chr/pc":
                    case "chr/summon":
                    case "chr/weapon":
                        // 5 bits
                        var chrCategoryBits = Convert.ToString(DetermineChrCategory(virtualPathData[1]), 2).PadLeft(5, '0');

                        // 10 bits
                        var modelID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        if (modelID == -1)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "Model number specified is invalid";
                            }
                            else
                            {
                                ParsingErrorMsg = $"Model number specified is invalid.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        if (modelID > 999)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "Model number in the path is too large. must be from 0 to 999.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"Model number in the path is too large. must be from 0 to 999.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        var modelIDbits = Convert.ToString(modelID, 2).PadLeft(10, '0');

                        // 5 bits
                        switch (fileExtn)
                        {
                            case ".imgb":
                                fileExtnID = 0;
                                break;

                            case ".trb":
                                fileExtnID = 1;
                                break;
                        }

                        fileExtnBits = Convert.ToString(fileExtnID, 2).PadLeft(5, '0');

                        // 8 bits
                        var reserved2Bits = "00000000";

                        // Assemble bits
                        finalComputedBits += reservedBits;
                        finalComputedBits += chrCategoryBits;
                        finalComputedBits += modelIDbits;
                        finalComputedBits += fileExtnBits;
                        finalComputedBits += reserved2Bits;

                        extraInfo += $"Reserved (4 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"Category (5 bits): {chrCategoryBits}\r\n\r\n";
                        extraInfo += $"ModelID (10 bits): {modelIDbits}\r\n\r\n";
                        extraInfo += $"ModelExtension Type (5 bits): {fileExtnBits}\r\n\r\n";
                        extraInfo += $"Reserved2 (8 bits): {reserved2Bits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "16";
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


        private static int DetermineChrCategory(string dirName)
        {
            var categoryID = 0;

            switch (dirName)
            {
                case "pc":
                    categoryID = 2;
                    break;

                case "exte":
                    categoryID = 4;
                    break;

                case "fa":
                    categoryID = 5;
                    break;

                case "mon":
                    categoryID = 12;
                    break;

                case "npc":
                    categoryID = 13;
                    break;

                case "summon":
                    categoryID = 18;
                    break;

                case "weapon":
                    categoryID = 22;
                    break;

                default:
                    if (GenerationVariables.GenerationType == GenerationType.single)
                    {
                        ParsingErrorMsg = "Unable to determine category from the filename. check if the chr filename, starts with a valid category string.";
                    }
                    else
                    {
                        ParsingErrorMsg = $"Unable to determine category from the filename. check if the chr filename, starts with a valid category string.\n{GenerationVariables.PathErrorStringForBatch}";
                    }

                    SharedFunctions.Error(ParsingErrorMsg);
                    break;
            }

            return categoryID;
        }
    }
}
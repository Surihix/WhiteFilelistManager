using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class VfxDir
    {
        private static GameID _gameID = new GameID();

        private static string ParsingErrorMsg = string.Empty;

        private static readonly List<string> _validExtensions = new List<string>()
        {
            ".imgb", ".xfv"
        };

        public static void ProcessVfxPath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            _gameID = gameID;

            switch (gameID)
            {
                case GameID.xiii:
                    VfxPathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                case GameID.xiii3:
                    VfxPathXIII2LR(virtualPathData, virtualPath);
                    break;
            }
        }


        #region XIII
        private static void VfxPathXIII(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;
            int fileExtnID = 0;
            string fileExtnBits;

            string fileCode;
            var extraInfo = string.Empty;

            string mainTypeBits;

            if (virtualPathData.Length > 3)
            {
                switch (startingPortion)
                {
                    case "vfx/ac":
                    case "vfx/event":
                        // 8 bits
                        mainTypeBits = Convert.ToString(130, 2).PadLeft(8, '0');

                        // 4 bits
                        var reservedBits = "0000";

                        // 8 bits
                        var categoryBits = Convert.ToString(8, 2).PadLeft(8, '0');

                        // 12 bits
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            GenerationFunctions.UserInput("Enter file number", "Must be from 0 to 4095", 0, 4095);
                        }
                        else
                        {
                            var hasFileID = false;

                            if (GenerationVariables.HasIdPathsTxtFile && GenerationVariables.IdBasedPathsDataDict.ContainsKey(virtualPath))
                            {
                                if (GenerationVariables.IdBasedPathsDataDict[virtualPath].Count > 0)
                                {
                                    GenerationVariables.NumInput = int.TryParse(GenerationVariables.IdBasedPathsDataDict[virtualPath][0], out int result) ? result : 0;
                                    hasFileID = true;
                                }
                            }

                            if (!hasFileID)
                            {
                                SharedFunctions.Error($"Unable to determine file number for a file.\n{GenerationVariables.PathErrorStringForBatch}");
                            }
                        }

                        var fileID = GenerationVariables.NumInput;
                        var fileIDbits = Convert.ToString(fileID, 2).PadLeft(12, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Reserved (4 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"Category (8 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"File ID (12 bits): {fileIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    case "vfx/chr":
                        // 4 bits
                        mainTypeBits = Convert.ToString(1, 2).PadLeft(4, '0');

                        // 5 bits
                        var chrCategoryBits = Convert.ToString(DetermineVfxChrCategory(virtualPathData[2][0]), 2).PadLeft(5, '0');

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
                        fileExtnID = fileExtn == ".imgb" ? 2 : 3;
                        fileExtnBits = Convert.ToString(fileExtnID, 2).PadLeft(5, '0');

                        // 8 bits
                        int version;
                        if (virtualPathData.Length > 3)
                        {
                            version = GenerationFunctions.DeriveNumFromString(virtualPathData[3]);

                            if (version == -1)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = "Version number in the path is invalid";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"Version number in the path is invalid.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }
                        }
                        else
                        {
                            version = 0;
                        }

                        var versionBits = Convert.ToString(version, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += chrCategoryBits;
                        finalComputedBits += modelIDbits;
                        finalComputedBits += fileExtnBits;
                        finalComputedBits += versionBits;

                        extraInfo += $"MainType (4 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Category (5 bits): {chrCategoryBits}\r\n\r\n";
                        extraInfo += $"ModelID (10 bits): {modelIDbits}\r\n\r\n";
                        extraInfo += $"ModelExtension Type (5 bits): {fileExtnBits}\r\n\r\n";
                        extraInfo += $"Version (8 bits): {versionBits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    case "vfx/field":
                        // 4 bits
                        mainTypeBits = Convert.ToString(3, 2).PadLeft(4, '0');

                        // 8 bits
                        var locID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);

                        if (locID == -1)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "loc number in the path is invalid";
                            }
                            else
                            {
                                ParsingErrorMsg = $"loc number in the path is invalid.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        if (locID > 255)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "loc number in the path is too large. must be from 0 to 255.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"loc number in the path is too large. must be from 0 to 255.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        var locIDbits = Convert.ToString(locID, 2).PadLeft(8, '0');

                        // 12 bits
                        var fileNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[3]);

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

                        if (fileNameNum > 999)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 999.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 999.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        var fileNameNumBits = Convert.ToString(fileNameNum, 2).PadLeft(12, '0');

                        // 8 bits
                        var fileName = Path.GetFileName(virtualPath);
                        var fileCategoryBits = string.Empty;

                        if (fileName.StartsWith("veffs"))
                        {
                            var fileCategory = fileExtn == ".imgb" ? 2 : 3;
                            fileCategoryBits = Convert.ToString(fileCategory, 2).PadLeft(8, '0');
                        }
                        else
                        {
                            SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                        }

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += locIDbits;
                        finalComputedBits += fileNameNumBits;
                        finalComputedBits += fileCategoryBits;

                        extraInfo += $"MainType (4 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"LocID (8 bits): {locIDbits}\r\n\r\n";
                        extraInfo += $"File number (12 bits): {fileNameNumBits}\r\n\r\n";
                        extraInfo += $"Category (8 bits): {fileCategoryBits}";
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
        private static void VfxPathXIII2LR(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;
            int fileExtnID;
            string fileExtnBits;

            string fileCode;
            string extraInfo = string.Empty;

            // 4 bits
            var reservedBits = "0000";

            if (virtualPathData.Length > 3)
            {
                switch (startingPortion)
                {
                    case "vfx/chr":
                        // 5 bits
                        var chrCategoryBits = Convert.ToString(DetermineVfxChrCategory(virtualPathData[2][0]), 2).PadLeft(5, '0');

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
                        fileExtnID = fileExtn == ".imgb" ? 2 : 3;
                        fileExtnBits = Convert.ToString(fileExtnID, 2).PadLeft(5, '0');

                        // 8 bits
                        int version;
                        if (virtualPathData.Length > 3)
                        {
                            version = GenerationFunctions.DeriveNumFromString(virtualPathData[3]);

                            if (version == -1)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = "Version number in the path is invalid";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"Version number in the path is invalid.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }
                        }
                        else
                        {
                            version = 0;
                        }

                        var versionBits = Convert.ToString(version, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += reservedBits;
                        finalComputedBits += chrCategoryBits;
                        finalComputedBits += modelIDbits;
                        finalComputedBits += fileExtnBits;
                        finalComputedBits += versionBits;

                        extraInfo += $"Reserved (4 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"Category (5 bits): {chrCategoryBits}\r\n\r\n";
                        extraInfo += $"ModelID (10 bits): {modelIDbits}\r\n\r\n";
                        extraInfo += $"ModelExtension Type (5 bits): {fileExtnBits}\r\n\r\n";
                        extraInfo += $"Version (8 bits): {versionBits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "16";
                        break;


                    case "vfx/ac":
                    case "vfx/event":
                        // 8 bits
                        var categoryBits = Convert.ToString(32, 2).PadLeft(8, '0');

                        // 8 bits
                        var category2Bits = Convert.ToString(8, 2).PadLeft(8, '0');

                        // 12 bits
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            GenerationFunctions.UserInput("Enter file number", "Must be from 0 to 4095", 0, 4095);
                        }
                        else
                        {
                            var hasFileID = false;

                            if (GenerationVariables.HasIdPathsTxtFile && GenerationVariables.IdBasedPathsDataDict.ContainsKey(virtualPath))
                            {
                                if (GenerationVariables.IdBasedPathsDataDict[virtualPath].Count > 0)
                                {
                                    GenerationVariables.NumInput = int.TryParse(GenerationVariables.IdBasedPathsDataDict[virtualPath][0], out int result) ? result : 0;
                                    hasFileID = true;
                                }
                            }

                            if (!hasFileID)
                            {
                                SharedFunctions.Error($"Unable to determine file number for a file.\n{GenerationVariables.PathErrorStringForBatch}");
                            }
                        }

                        var fileID = GenerationVariables.NumInput;
                        var fileIDbits = Convert.ToString(fileID, 2).PadLeft(12, '0');

                        // Assemble bits
                        finalComputedBits += reservedBits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += category2Bits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"Reserved (4 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"Category (8 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"Category2 (8 bits): {category2Bits}\r\n\r\n";
                        extraInfo += $"File ID (12 bits): {fileIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "128";
                        break;


                    case "vfx/field":
                        if (virtualPathData[2] == "weather")
                        {
                            // 4 bits
                            categoryBits = Convert.ToString(8, 2).PadLeft(4, '0');

                            // 12 bits
                            reservedBits = "000000000000";

                            // 12 bits
                            var weatherID = GenerationFunctions.DeriveNumFromString(virtualPathData[3]);

                            if (weatherID == -1)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = "weather number in the path is invalid";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"weather number in the path is invalid.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            if (weatherID > 99)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = "weather number in the path is too large. must be from 0 to 99.";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"weather number in the path is too large. must be from 0 to 99.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            var weatherIDbits = Convert.ToString(weatherID, 2).PadLeft(12, '0');

                            // 4 bits
                            fileExtnID = fileExtn == ".xfv" ? 0 : 1;
                            fileExtnBits = Convert.ToString(fileExtnID, 2).PadLeft(4, '0');

                            // Assemble bits
                            finalComputedBits += categoryBits;
                            finalComputedBits += reservedBits;
                            finalComputedBits += weatherIDbits;
                            finalComputedBits += fileExtnBits;

                            extraInfo += $"Category (4 bits): {categoryBits}\r\n\r\n";
                            extraInfo += $"Reserved (12 bits): {reservedBits}\r\n\r\n";
                            extraInfo += $"Weather ID (12 bits): {weatherIDbits}\r\n\r\n";
                            extraInfo += $"Extension Type (4 bits): {fileExtnBits}";
                            finalComputedBits.Reverse();

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            GenerationVariables.FileTypeID = "128";
                        }
                        else
                        {
                            // 12 bits
                            var locID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);

                            if (locID == -1)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = "loc number in the path is invalid";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"loc number in the path is invalid.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            if (locID > 998)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = "loc number in the path is too large. must be from 0 to 998.";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"loc number in the path is too large. must be from 0 to 998.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            if (_gameID == GameID.xiii3)
                            {
                                locID--;
                                locID = locID < 0 ? 0 : locID;
                            }

                            var locIDbits = Convert.ToString(locID, 2).PadLeft(12, '0');

                            // 12 bits
                            var fileNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[3]);

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

                            if (fileNameNum > 999)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 999.";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 999.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            var fileNameNumBits = Convert.ToString(fileNameNum, 2).PadLeft(12, '0');

                            // 8 bits
                            var fileCategory = fileExtn == ".imgb" ? 2 : 3;
                            var fileCategoryBits = Convert.ToString(fileCategory, 2).PadLeft(8, '0');

                            // Assemble bits
                            finalComputedBits += locIDbits;
                            finalComputedBits += fileNameNumBits;
                            finalComputedBits += fileCategoryBits;

                            extraInfo += $"LocID (12 bits): {locIDbits}\r\n\r\n";
                            extraInfo += $"File number (12 bits): {fileNameNumBits}\r\n\r\n";
                            extraInfo += $"Category (8 bits): {fileCategoryBits}";
                            finalComputedBits.Reverse();

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            GenerationVariables.FileTypeID = "48";
                        }
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


        private static int DetermineVfxChrCategory(char startChara)
        {
            var categoryID = 0;

            switch (startChara)
            {
                case 'c':
                    categoryID = 2;
                    break;

                case 'f':
                    categoryID = 5;
                    break;

                case 'm':
                    categoryID = 12;
                    break;

                case 'n':
                    categoryID = 13;
                    break;

                case 's':
                    categoryID = 18;
                    break;

                case 'w':
                    categoryID = 22;
                    break;

                default:
                    if (GenerationVariables.GenerationType == GenerationType.single)
                    {
                        ParsingErrorMsg = "Unable to determine category from the filename. check if the vfx filename, starts with a valid category string.";
                    }
                    else
                    {
                        ParsingErrorMsg = $"Unable to determine category from the filename. check if the vfx filename, starts with a valid category string.\n{GenerationVariables.PathErrorStringForBatch}";
                    }

                    SharedFunctions.Error(ParsingErrorMsg);
                    break;
            }

            return categoryID;
        }
    }
}
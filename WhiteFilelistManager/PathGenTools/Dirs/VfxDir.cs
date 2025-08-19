using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class VfxDir
    {
        private static GameID _gameID = new();

        private static string ParsingErrorMsg = string.Empty;

        private static readonly List<string> _validExtensions = new()
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

        private static readonly Dictionary<char, int> VfxChrCategoryDict = new()
        {
            { 'c', 2 },
            { 'f', 5 },
            { 'm', 12 },
            { 'n', 13 },
            { 's', 18 },
            { 'w', 22 }
        };


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
            int fileExtnID;
            string fileExtnBits;

            string fileCode;

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

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "130";
                        break;


                    case "vfx/chr":
                        // 4 bits
                        mainTypeBits = Convert.ToString(1, 2).PadLeft(4, '0');

                        // 5 bits
                        if (!VfxChrCategoryDict.ContainsKey(virtualPathData[2][0]))
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                SharedFunctions.Error("Unable to determine category from the filename. check if the vfx filename, starts with a valid category string.");
                            }
                            else
                            {
                                SharedFunctions.Error($"Unable to determine category from the filename. check if the vfx filename, starts with a valid category string.\n{GenerationVariables.PathErrorStringForBatch}");
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        var chrCategoryBits = Convert.ToString(VfxChrCategoryDict[virtualPathData[2][0]], 2).PadLeft(5, '0');

                        // 10 bits
                        var modelID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        GenerationFunctions.CheckDerivedNumber(modelID, "model", 999);

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

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "1";
                        break;


                    case "vfx/field":
                        // 4 bits
                        mainTypeBits = Convert.ToString(3, 2).PadLeft(4, '0');

                        // 8 bits
                        var locID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        GenerationFunctions.CheckDerivedNumber(locID, "loc", 255);

                        var locIDbits = Convert.ToString(locID, 2).PadLeft(8, '0');

                        // 12 bits
                        var fileNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[3]);
                        GenerationFunctions.CheckDerivedNumber(fileNameNum, "file", 999);

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

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "3";
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

            // 4 bits
            var reservedBits = "0000";

            if (virtualPathData.Length > 3)
            {
                switch (startingPortion)
                {
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

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "128";
                        break;


                    case "vfx/chr":
                        // 5 bits
                        if (!VfxChrCategoryDict.ContainsKey(virtualPathData[2][0]))
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                SharedFunctions.Error("Unable to determine category from the filename. check if the vfx filename, starts with a valid category string.");
                            }
                            else
                            {
                                SharedFunctions.Error($"Unable to determine category from the filename. check if the vfx filename, starts with a valid category string.\n{GenerationVariables.PathErrorStringForBatch}");
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        var chrCategoryBits = Convert.ToString(VfxChrCategoryDict[virtualPathData[2][0]], 2).PadLeft(5, '0');

                        // 10 bits
                        var modelID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        GenerationFunctions.CheckDerivedNumber(modelID, "model", 999);

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

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "16";
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
                            GenerationFunctions.CheckDerivedNumber(weatherID, "weather", 99);

                            var weatherIDbits = Convert.ToString(weatherID, 2).PadLeft(12, '0');

                            // 4 bits
                            fileExtnID = fileExtn == ".xfv" ? 0 : 1;
                            fileExtnBits = Convert.ToString(fileExtnID, 2).PadLeft(4, '0');

                            // Assemble bits
                            finalComputedBits += categoryBits;
                            finalComputedBits += reservedBits;
                            finalComputedBits += weatherIDbits;
                            finalComputedBits += fileExtnBits;

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            GenerationVariables.FileTypeID = "128";
                        }
                        else
                        {
                            // 12 bits
                            var locID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                            GenerationFunctions.CheckDerivedNumber(locID, "loc", 998);

                            if (_gameID == GameID.xiii3)
                            {
                                locID--;
                                locID = locID < 0 ? 0 : locID;
                            }

                            var locIDbits = Convert.ToString(locID, 2).PadLeft(12, '0');

                            // 12 bits
                            var fileNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[3]);
                            GenerationFunctions.CheckDerivedNumber(fileNameNum, "file", 999);

                            var fileNameNumBits = Convert.ToString(fileNameNum, 2).PadLeft(12, '0');

                            // 8 bits
                            var fileCategory = fileExtn == ".imgb" ? 2 : 3;
                            var fileCategoryBits = Convert.ToString(fileCategory, 2).PadLeft(8, '0');

                            // Assemble bits
                            finalComputedBits += locIDbits;
                            finalComputedBits += fileNameNumBits;
                            finalComputedBits += fileCategoryBits;

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
    }
}
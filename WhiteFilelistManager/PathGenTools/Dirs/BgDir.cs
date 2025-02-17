using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class BgDir
    {
        private static string ParsingErrorMsg = string.Empty;

        public static void ProcessBgPath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    BgPathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                    BgPathXIII2(virtualPathData, virtualPath);
                    break;

                case GameID.xiii3:
                    BgPathXIIILR(virtualPathData, virtualPath);
                    break;
            }
        }


        #region XIII
        private static void BgPathXIII(string[] virtualPathData, string virtualPath)
        {
            var validExtensions = new List<string>
            {
                ".imgb", ".trb", ".xwb"
            };

            var fileExtn = Path.GetExtension(virtualPath);

            if (!validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;
            var extraInfo = string.Empty;

            int locID;
            string locIDbits;

            int fileNameNum;
            string fileNameNumBits;

            var fileName = Path.GetFileName(virtualPath);
            int fileCategory;
            var fileCategoryBits = string.Empty;

            // 4 bits
            var mainTypeBits = Convert.ToString(3, 2).PadLeft(4, '0');

            if (virtualPathData.Length > 4)
            {
                if (virtualPath.StartsWith("bg/loc"))
                {
                    // 8 bits
                    locID = GenerationFunctions.DeriveNumFromString(virtualPathData[1]);

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

                    locIDbits = Convert.ToString(locID, 2).PadLeft(8, '0');

                    // 12 bits
                    fileNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);

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

                    fileNameNumBits = Convert.ToString(fileNameNum, 2).PadLeft(12, '0');

                    // 8 bits
                    if (fileName.StartsWith("block"))
                    {
                        fileCategory = fileExtn == ".imgb" ? 0 : 1;
                        fileCategoryBits = Convert.ToString(fileCategory, 2).PadLeft(8, '0');
                    }
                    else if (fileName.StartsWith("ext"))
                    {
                        fileCategory = fileExtn == ".xwb" ? 4 : 6;
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


        #region XIII-2
        private static void BgPathXIII2(string[] virtualPathData, string virtualPath)
        {
            var validExtensions = new List<string>
            {
                ".imgb", ".mpk", ".trb", ".xwb"
            };

            var fileExtn = Path.GetExtension(virtualPath);

            if (!validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;
            var extraInfo = string.Empty;

            int locID;
            string locIDbits;

            int fileNameNum;
            string fileNameNumBits;

            var fileName = Path.GetFileName(virtualPath);
            int fileCategory = 0;
            string fileCategoryBits;

            if (virtualPathData.Length > 4)
            {
                if (virtualPath.StartsWith("bg/loc"))
                {
                    // 12 bits
                    locID = GenerationFunctions.DeriveNumFromString(virtualPathData[1]);

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

                    locIDbits = Convert.ToString(locID, 2).PadLeft(12, '0');

                    // 12 bits
                    fileNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);

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

                    fileNameNumBits = Convert.ToString(fileNameNum, 2).PadLeft(12, '0');

                    // 8 bits
                    if (fileName.StartsWith("block"))
                    {
                        if (fileExtn == ".mpk")
                        {
                            switch (fileName.Substring(9))
                            {
                                case "def.win32.mpk":
                                    fileCategory = 8;
                                    break;

                                case "rain.win32.mpk":
                                    fileCategory = 9;
                                    break;

                                case "snow.win32.mpk":
                                    fileCategory = 10;
                                    break;
                            }
                        }
                        else
                        {
                            fileCategory = fileExtn == ".imgb" ? 0 : 1;
                        }
                    }
                    else if (fileName.StartsWith("ext"))
                    {
                        fileCategory = fileExtn == ".xwb" ? 4 : 6;
                    }
                    else
                    {
                        SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                    }

                    fileCategoryBits = Convert.ToString(fileCategory, 2).PadLeft(8, '0');

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


        #region XIII-LR
        private static void BgPathXIIILR(string[] virtualPathData, string virtualPath)
        {
            var validExtensions = new List<string>
            {
                ".imgb", ".trb", ".xwb"
            };

            var fileExtn = Path.GetExtension(virtualPath);

            if (!validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;
            var extraInfo = string.Empty;

            int locID;
            string locIDbits;

            int fileNameNum;
            string fileNameNumBits;

            var fileName = Path.GetFileName(virtualPath);
            int fileCategory = 0;
            string fileCategoryBits;

            if (virtualPathData.Length > 4)
            {
                if (virtualPath.StartsWith("bg/loc"))
                {
                    // 12 bits
                    locID = GenerationFunctions.DeriveNumFromString(virtualPathData[1]);

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

                    locIDbits = Convert.ToString(locID, 2).PadLeft(12, '0');

                    // 12 bits
                    fileNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);

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

                    fileNameNumBits = Convert.ToString(fileNameNum, 2).PadLeft(12, '0');

                    // 8 bits
                    if (fileName.StartsWith("block"))
                    {
                        fileCategory = fileExtn == ".imgb" ? 0 : 1;
                    }
                    else if (fileName.StartsWith("ext"))
                    {
                        if (fileName[6] == '_' && fileName.Length == 19)
                        {
                            var extNum2 = GenerationFunctions.DeriveNumFromString(fileName.Substring(7));

                            if (extNum2 == -1)
                            {
                                fileCategory = 4;
                            }
                            else
                            {
                                fileCategory = GetFileCategoryForXIIILR(32, extNum2);                                
                            }
                        }
                        else
                        {
                            fileCategory = 4;
                        }
                    }
                    else if (fileName.StartsWith("BL00"))
                    {
                        if (fileName[11] == '_' && fileName.Length == 25)
                        {
                            var bl00Num = GenerationFunctions.DeriveNumFromString(fileName.Substring(12));

                            if (bl00Num == -1)
                            {
                                fileCategory = 4;
                            }
                            else
                            {
                                fileCategory = GetFileCategoryForXIIILR(64, bl00Num);
                            }
                        }
                    }
                    else
                    {
                        SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                    }

                    fileCategoryBits = Convert.ToString(fileCategory, 2).PadLeft(8, '0');

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


        private static int GetFileCategoryForXIIILR(int categoryCounter, int num2)
        {
            var fileCategory = categoryCounter;

            for (int i = 0; i < num2 + 1; i++)
            {
                if (i == num2)
                {
                    break;
                }
                else
                {
                    fileCategory++;
                }
            }

            return fileCategory;
        }
    }
}
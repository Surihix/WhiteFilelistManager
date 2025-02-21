using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    class RailcamDir
    {
        private static string ParsingErrorMsg = string.Empty;

        public static void ProcessRailcamPath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    RailcamPathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                    RailcamPathXIII2(virtualPathData, virtualPath);
                    break;
            }
        }


        #region XIII
        private static void RailcamPathXIII(string[] virtualPathData, string virtualPath)
        {
            var fileExtn = Path.GetExtension(virtualPath);

            if (fileExtn != ".xwp")
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

            string fileCategoryBits;

            // 4 bits
            var mainTypeBits = Convert.ToString(3, 2).PadLeft(4, '0');

            if (virtualPathData.Length > 2)
            {
                if (virtualPath.StartsWith("railcam/loc"))
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
                    fileCategoryBits = Convert.ToString(5, 2).PadLeft(8, '0');

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
        private static void RailcamPathXIII2(string[] virtualPathData, string virtualPath)
        {
            var fileExtn = Path.GetExtension(virtualPath);

            if (fileExtn != ".xwp")
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

            string fileCategoryBits;

            if (virtualPathData.Length > 2)
            {
                if (virtualPath.StartsWith("railcam/loc"))
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
                    fileCategoryBits = Convert.ToString(5, 2).PadLeft(8, '0');

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
    }
}
using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class SceneDir
    {
        private static string ParsingErrorMsg = string.Empty;

        private static readonly List<string> _validExtensions = new()
        {
            ".bin", ".imgb", ".lyb", ".vinsbin", ".wdb", ".wpb", ".xwp"
        };

        public static void ProcessScenePath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    ScenePathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                    ScenePathXIII2(virtualPathData, virtualPath);
                    break;

                case GameID.xiii3:
                    ScenePathXIIILR(virtualPathData, virtualPath);
                    break;
            }
        }


        #region XIII
        private static void ScenePathXIII(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;
            var extraInfo = string.Empty;
            var categoryBits = string.Empty;
            string reservedBits;

            var fileName = Path.GetFileName(virtualPath);
            int fileNameNum;
            string fileNameNumBits;

            // 8 bits
            var mainTypeBits = string.Empty;

            switch (virtualPathData.Length)
            {
                case 2:
                    if (virtualPath.StartsWith("scene/s"))
                    {
                        mainTypeBits = Convert.ToString(112, 2).PadLeft(8, '0');

                        // 16 bits
                        reservedBits = "0000000000000000";

                        // 8 bits
                        fileNameNum = GenerationFunctions.DeriveNumFromString(fileName);
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
                        finalComputedBits += reservedBits;
                        finalComputedBits += fileNameNumBits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Reserved (16 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"File number (8 bits): {fileNameNumBits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                    }
                    else
                    {
                        SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                    }
                    break;


                case 3:
                    if (startingPortion == "scene/ai" || startingPortion == "scene/talk")
                    {
                        mainTypeBits = startingPortion == "scene/ai" ? Convert.ToString(112, 2).PadLeft(8, '0') : Convert.ToString(119, 2).PadLeft(8, '0');

                        // 8 bits
                        categoryBits = Convert.ToString(128, 2).PadLeft(8, '0');

                        // 8 bits
                        reservedBits = "00000000";

                        // 8 bits
                        fileNameNum = GenerationFunctions.DeriveNumFromString(fileName);
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
                        finalComputedBits += categoryBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += fileNameNumBits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Category (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Reserved (8 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"File number (8 bits): {fileNameNumBits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                    }
                    else
                    {
                        SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                    }
                    break;


                case 4:
                case 8:
                    if (virtualPathData.Length == 4 && startingPortion.StartsWith("scene/camera"))
                    {
                        mainTypeBits = Convert.ToString(114, 2).PadLeft(8, '0');
                    }
                    else if (virtualPathData.Length == 8)
                    {
                        mainTypeBits = Convert.ToString(116, 2).PadLeft(8, '0');
                    }

                    // 16 bits
                    reservedBits = "0000000000000000";

                    // 8 bits
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
                    finalComputedBits += reservedBits;
                    finalComputedBits += fileNameNumBits;

                    extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                    extraInfo += $"Reserved (16 bits): {reservedBits}\r\n\r\n";
                    extraInfo += $"File number (8 bits): {fileNameNumBits}";
                    finalComputedBits.Reverse();

                    fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                    GenerationVariables.FileCode = fileCode;
                    break;


                case 5:

                    break;


                default:
                    SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                    break;
            }
        }
        #endregion


        #region XIII-2
        private static void ScenePathXIII2(string[] virtualPathData, string virtualPath)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region XIII-LR
        private static void ScenePathXIIILR(string[] virtualPathData, string virtualPath)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
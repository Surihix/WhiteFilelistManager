﻿using WhiteFilelistManager.Support;
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
            string categoryBits;
            string reservedBits;

            var fileName = Path.GetFileName(virtualPath);
            int fileNameNum;
            string fileNameNumBits;

            var mainTypeBits = string.Empty;

            switch (virtualPathData.Length)
            {
                case 2:
                    if (virtualPath.StartsWith("scene/s"))
                    {
                        // 8 bits
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
                        // 8 bits
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
                        extraInfo += $"Category (8 bits): {categoryBits}\r\n\r\n";
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
                    string errorNumType;

                    if (virtualPathData.Length == 4 && startingPortion.StartsWith("scene/camera"))
                    {
                        // 8 bits
                        mainTypeBits = Convert.ToString(114, 2).PadLeft(8, '0');
                        errorNumType = "File";
                    }
                    else
                    {
                        // 8 bits
                        mainTypeBits = Convert.ToString(116, 2).PadLeft(8, '0');
                        errorNumType = "scene";
                    }

                    // 16 bits
                    reservedBits = "0000000000000000";

                    // 8 bits
                    fileNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                    if (fileNameNum == -1)
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            ParsingErrorMsg = $"Unable to determine {errorNumType} number from path";
                        }
                        else
                        {
                            ParsingErrorMsg = $"Unable to determine {errorNumType} number from filename for a file.\n{GenerationVariables.PathErrorStringForBatch}";
                        }

                        SharedFunctions.Error(ParsingErrorMsg);
                    }

                    if (fileNameNum > 255)
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            ParsingErrorMsg = $"{errorNumType} number in the path is too large. must be from 0 to 255.";
                        }
                        else
                        {
                            ParsingErrorMsg = $"{errorNumType} number in the path is too large. must be from 0 to 255.\n{GenerationVariables.PathErrorStringForBatch}";
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
                    int sceneNameNum;
                    string sceneNameNumBits;

                    if (startingPortion.StartsWith("scene/lay") && virtualPathData[2].StartsWith("scene"))
                    {
                        // Get scene number
                        sceneNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        if (sceneNameNum == -1)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = $"Unable to determine scene number from path";
                            }
                            else
                            {
                                ParsingErrorMsg = $"Unable to determine scene number from filename for a file.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        if (sceneNameNum > 255)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = $"scene number in the path is too large. must be from 0 to 255.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"scene number in the path is too large. must be from 0 to 255.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        if (fileExtn == ".imgb") // scene#####_##_split_##.win32.imgb file
                        {
                            // 5 bits
                            mainTypeBits = Convert.ToString(18, 2).PadLeft(5, '0');

                            var splitName = fileName.Split('_');

                            if (splitName.Length < 4)
                            {
                                SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                            }

                            // 5 bits
                            var preSplitID = Convert.ToInt32(splitName[1], 16);

                            if (preSplitID == -1)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = $"Unable to determine pre split file number from path";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"Unable to determine pre split file number from filename for a file.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            if (preSplitID > 31)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = $"Pre split file number in the path is too large. must be from 0 to 31.";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"Pre split file number in the path is too large. must be from 0 to 31.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            var preSplitIDBits = Convert.ToString(preSplitID, 2).PadLeft(5, '0');

                            // 8 bits
                            var splitIDinFile = Convert.ToInt32(splitName[3].Replace(".win32.imgb", ""), 16);

                            if (splitIDinFile > 255)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = $"Split file number in the path is too large. must be from 0 to ff.";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"Split file number in the path is too large. must be from 0 to ff.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            var splitID = GetSplitID(splitIDinFile);
                            var splitIDbits = Convert.ToString(splitID, 2).PadLeft(8, '0');

                            // 6 bits
                            reservedBits = "000000";

                            // 8 bits
                            sceneNameNumBits = Convert.ToString(sceneNameNum, 2).PadLeft(8, '0');

                            // Assemble bits
                            finalComputedBits += mainTypeBits;
                            finalComputedBits += preSplitIDBits;
                            finalComputedBits += splitIDbits;
                            finalComputedBits += reservedBits;
                            finalComputedBits += sceneNameNumBits;

                            extraInfo += $"MainType (5 bits): {mainTypeBits}\r\n\r\n";
                            extraInfo += $"Pre split number (5 bits): {preSplitIDBits}\r\n\r\n";
                            extraInfo += $"Split number (8 bits): {splitIDbits}\r\n\r\n";
                            extraInfo += $"Reserved (6 bits): {reservedBits}\r\n\r\n";
                            extraInfo += $"Scene number (8 bits): {sceneNameNumBits}";
                            finalComputedBits.Reverse();

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                        }
                        else
                        {
                            // 8 bits
                            switch (fileExtn)
                            {
                                case ".bin":
                                    mainTypeBits = Convert.ToString(117, 2).PadLeft(8, '0');
                                    break;

                                case ".lyb":
                                    mainTypeBits = Convert.ToString(113, 2).PadLeft(8, '0');
                                    break;

                                case ".xwp":
                                    mainTypeBits = Convert.ToString(119, 2).PadLeft(8, '0');
                                    break;
                            }

                            // 16 bits
                            reservedBits = "0000000000000000";

                            // 8 bits                           
                            sceneNameNumBits = Convert.ToString(sceneNameNum, 2).PadLeft(8, '0');

                            // Assemble bits
                            finalComputedBits += mainTypeBits;
                            finalComputedBits += reservedBits;
                            finalComputedBits += sceneNameNumBits;

                            extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                            extraInfo += $"Reserved (16 bits): {reservedBits}\r\n\r\n";
                            extraInfo += $"Scene number (8 bits): {sceneNameNumBits}";
                            finalComputedBits.Reverse();

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                        }
                    }
                    else
                    {
                        SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                    }
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
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;
            var extraInfo = string.Empty;
            string categoryBits;

            string mainTypeBits;
            string reservedBits;
            string reservedAbits;
            string reservedBbits;

            var fileName = Path.GetFileName(virtualPath);
            int fileNameNum;
            string fileNameNumBits;

            int sceneNameNum;
            string sceneNameNumBits;

            switch (virtualPathData.Length)
            {
                case 2:
                    if (virtualPath.StartsWith("scene/s"))
                    {
                        reservedAbits = "00000000";

                        // 12 bits
                        reservedBbits = "000000000000";

                        // 12 bits
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
                        finalComputedBits += reservedAbits;
                        finalComputedBits += reservedBbits;
                        finalComputedBits += fileNameNumBits;

                        extraInfo += $"ReservedA (8 bits): {reservedAbits}\r\n\r\n";
                        extraInfo += $"ReservedB (12 bits): {reservedBbits}\r\n\r\n";
                        extraInfo += $"File number (12 bits): {fileNameNumBits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "112";
                    }
                    else
                    {
                        SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                    }
                    break;


                case 3:
                    if (startingPortion == "scene/ai" || startingPortion == "scene/talk")
                    {
                        // 8 bits
                        mainTypeBits = startingPortion == "scene/ai" ? "00000000" : Convert.ToString(7, 2).PadLeft(8, '0');

                        // 8 bits
                        categoryBits = Convert.ToString(128, 2).PadLeft(8, '0');

                        // 4 bits
                        reservedBits = "0000";

                        // 12 bits
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
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += fileNameNumBits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Category (8 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"Reserved (8 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"File number (8 bits): {fileNameNumBits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "112";
                    }
                    break;


                case 5:
                    if (startingPortion.StartsWith("scene/lay") && virtualPathData[2].StartsWith("scene"))
                    {
                        // Get scene number
                        sceneNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        if (sceneNameNum == -1)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = $"Unable to determine scene number from path";
                            }
                            else
                            {
                                ParsingErrorMsg = $"Unable to determine scene number from filename for a file.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        if (sceneNameNum > 998)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = $"scene number in the path is too large. must be from 0 to 998.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"scene number in the path is too large. must be from 0 to 998.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }
                        
                        if (fileExtn == ".imgb") // scene#####_##_split_##.win32.imgb file
                        {
                            // 5 bits
                            reservedAbits = "00000";

                            var splitName = fileName.Split('_');

                            if (splitName.Length < 4)
                            {
                                SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                            }

                            // 5 bits
                            var preSplitID = Convert.ToInt32(splitName[1], 16);

                            if (preSplitID == -1)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = $"Unable to determine pre split file number from path";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"Unable to determine pre split file number from filename for a file.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            if (preSplitID > 31)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = $"Pre split file number in the path is too large. must be from 0 to 31.";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"Pre split file number in the path is too large. must be from 0 to 31.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            var preSplitIDBits = Convert.ToString(preSplitID, 2).PadLeft(5, '0');

                            // 8 bits
                            var splitIDinFile = Convert.ToInt32(splitName[3].Replace(".win32.imgb", ""), 16);

                            if (splitIDinFile > 255)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = $"Split file number in the path is too large. must be from 0 to ff.";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"Split file number in the path is too large. must be from 0 to ff.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            var splitID = GetSplitID(splitIDinFile);
                            var splitIDbits = Convert.ToString(splitID, 2).PadLeft(8, '0');

                            // 2 bits
                            reservedBbits = "00";

                            // 12 bits                 
                            sceneNameNumBits = Convert.ToString(sceneNameNum, 2).PadLeft(12, '0');

                            // Assemble bits
                            finalComputedBits += reservedAbits;
                            finalComputedBits += preSplitIDBits;
                            finalComputedBits += splitIDbits;
                            finalComputedBits += reservedBbits;
                            finalComputedBits += sceneNameNumBits;

                            extraInfo += $"ReservedA (5 bits): {reservedAbits}\r\n\r\n";
                            extraInfo += $"Pre split number (5 bits): {preSplitIDBits}\r\n\r\n";
                            extraInfo += $"Split number (8 bits): {splitIDbits}\r\n\r\n";
                            extraInfo += $"ReservedB (2 bits): {reservedBbits}\r\n\r\n";
                            extraInfo += $"Scene number (12 bits): {sceneNameNumBits}";
                            finalComputedBits.Reverse();

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            GenerationVariables.FileTypeID = "144";
                        }
                        else
                        {
                            // 8 bits
                            mainTypeBits = string.Empty;

                            switch (fileExtn)
                            {
                                case ".bin":
                                    mainTypeBits = Convert.ToString(5, 2).PadLeft(8, '0');
                                    break;

                                case ".lyb":
                                    mainTypeBits = Convert.ToString(1, 2).PadLeft(8, '0');
                                    break;

                                case ".xwp":
                                    mainTypeBits = Convert.ToString(7, 2).PadLeft(8, '0');
                                    break;
                            }

                            // 12 bits
                            reservedBits = "000000000000";

                            // 12 bits
                            sceneNameNumBits = Convert.ToString(sceneNameNum, 2).PadLeft(12, '0');

                            // Assemble bits
                            finalComputedBits += mainTypeBits;
                            finalComputedBits += reservedBits;
                            finalComputedBits += sceneNameNumBits;

                            extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                            extraInfo += $"Reserved (12 bits): {reservedBits}\r\n\r\n";
                            extraInfo += $"Scene number (16 bits): {sceneNameNumBits}";
                            finalComputedBits.Reverse();

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            GenerationVariables.FileTypeID = "112";
                        }
                    }
                    else
                    {
                        SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                    }
                    break;


                case 8:
                    // 8 bits
                    mainTypeBits = Convert.ToString(4, 2).PadLeft(8, '0');

                    // 12 bits
                    reservedBits = "000000000000";

                    // 12 bits
                    sceneNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                    if (sceneNameNum == -1)
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            ParsingErrorMsg = $"Unable to determine scene number from path";
                        }
                        else
                        {
                            ParsingErrorMsg = $"Unable to determine scene number from filename for a file.\n{GenerationVariables.PathErrorStringForBatch}";
                        }

                        SharedFunctions.Error(ParsingErrorMsg);
                    }

                    if (sceneNameNum > 998)
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            ParsingErrorMsg = $"scene number in the path is too large. must be from 0 to 998.";
                        }
                        else
                        {
                            ParsingErrorMsg = $"scene number in the path is too large. must be from 0 to 998.\n{GenerationVariables.PathErrorStringForBatch}";
                        }

                        SharedFunctions.Error(ParsingErrorMsg);
                    }

                    sceneNameNumBits = Convert.ToString(sceneNameNum, 2).PadLeft(12, '0');

                    // Assemble bits
                    finalComputedBits += mainTypeBits;
                    finalComputedBits += reservedBits;
                    finalComputedBits += sceneNameNumBits;

                    extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                    extraInfo += $"Reserved (12 bits): {reservedBits}\r\n\r\n";
                    extraInfo += $"Scene number (12 bits): {sceneNameNumBits}";
                    finalComputedBits.Reverse();

                    fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                    GenerationVariables.FileCode = fileCode;
                    GenerationVariables.FileTypeID = "112";
                    break;


                default:
                    SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                    break;
            }
        }
        #endregion


        #region XIII-LR
        private static void ScenePathXIIILR(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                if (fileExtn != ".wpk")
                {
                    SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
                }
            }

            var finalComputedBits = string.Empty;

            string fileCode;
            var extraInfo = string.Empty;
            string categoryBits;

            string mainTypeBits;
            string reservedBits;
            string reservedAbits;
            string reservedBbits;

            var fileName = Path.GetFileName(virtualPath);
            int fileNameNum;
            string fileNameNumBits;

            int sceneNameNum;
            string sceneNameNumBits;

            switch (virtualPathData.Length)
            {
                case 3:
                    if (startingPortion == "scene/ai" || startingPortion == "scene/talk")
                    {
                        // 8 bits
                        mainTypeBits = startingPortion == "scene/ai" ? "00000000" : Convert.ToString(7, 2).PadLeft(8, '0');

                        // 8 bits
                        categoryBits = Convert.ToString(128, 2).PadLeft(8, '0');

                        // 4 bits
                        reservedBits = "0000";

                        // 12 bits
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
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += fileNameNumBits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Category (8 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"Reserved (8 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"File number (8 bits): {fileNameNumBits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "112";
                    }
                    break;


                case 5:
                    if (startingPortion.StartsWith("scene/lay") && virtualPathData[2].StartsWith("scene"))
                    {
                        // Get scene number
                        sceneNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        if (sceneNameNum == -1)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = $"Unable to determine scene number from path";
                            }
                            else
                            {
                                ParsingErrorMsg = $"Unable to determine scene number from filename for a file.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        if (sceneNameNum > 998)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = $"scene number in the path is too large. must be from 0 to 998.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"scene number in the path is too large. must be from 0 to 998.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }
                        
                        if (fileExtn == ".imgb") // scene#####_##_split_##.win32.imgb file
                        {
                            // 5 bits
                            reservedAbits = "00000";

                            var splitName = fileName.Split('_');

                            if (splitName.Length < 4)
                            {
                                SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                            }

                            // 5 bits
                            var preSplitID = Convert.ToInt32(splitName[1], 16);

                            if (preSplitID == -1)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = $"Unable to determine pre split file number from path";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"Unable to determine pre split file number from filename for a file.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            if (preSplitID > 31)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = $"Pre split file number in the path is too large. must be from 0 to 31.";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"Pre split file number in the path is too large. must be from 0 to 31.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            var preSplitIDBits = Convert.ToString(preSplitID, 2).PadLeft(5, '0');

                            // 8 bits
                            var splitIDinFile = Convert.ToInt32(splitName[3].Replace(".win32.imgb", ""), 16);

                            if (splitIDinFile > 255)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = $"Split file number in the path is too large. must be from 0 to ff.";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"Split file number in the path is too large. must be from 0 to ff.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            var splitID = GetSplitID(splitIDinFile);
                            var splitIDbits = Convert.ToString(splitID, 2).PadLeft(8, '0');

                            // 2 bits
                            reservedBbits = "00";

                            // 12 bits                 
                            sceneNameNumBits = Convert.ToString(sceneNameNum, 2).PadLeft(12, '0');

                            // Assemble bits
                            finalComputedBits += reservedAbits;
                            finalComputedBits += preSplitIDBits;
                            finalComputedBits += splitIDbits;
                            finalComputedBits += reservedBbits;
                            finalComputedBits += sceneNameNumBits;

                            extraInfo += $"ReservedA (5 bits): {reservedAbits}\r\n\r\n";
                            extraInfo += $"Pre split number (5 bits): {preSplitIDBits}\r\n\r\n";
                            extraInfo += $"Split number (8 bits): {splitIDbits}\r\n\r\n";
                            extraInfo += $"ReservedB (2 bits): {reservedBbits}\r\n\r\n";
                            extraInfo += $"Scene number (12 bits): {sceneNameNumBits}";
                            finalComputedBits.Reverse();

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            GenerationVariables.FileTypeID = "144";
                        }
                        else if (fileExtn == ".wpk") // scene#####_p###.win32.wpk file
                        {
                            // 12 bits                 
                            sceneNameNumBits = Convert.ToString(sceneNameNum, 2).PadLeft(12, '0');

                            var splitName = fileName.Split('_');

                            if (splitName.Length < 2)
                            {
                                SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                            }

                            // 12 bits
                            var pID = GenerationFunctions.DeriveNumFromString(splitName[1]);

                            if (pID == -1)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = $"Unable to determine 'p' file number from path";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"Unable to determine 'p' file number from filename for a file.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            if (pID > 999)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = $"'p' file number in the path is too large. must be from 0 to 999.";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"'p' file number in the path is too large. must be from 0 to 999.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            var pIDbits = Convert.ToString(pID, 2).PadLeft(12, '0');

                            // 8 bits
                            categoryBits = Convert.ToString(7, 2).PadLeft(8, '0');

                            // Assemble bits
                            finalComputedBits += sceneNameNumBits;
                            finalComputedBits += pIDbits;
                            finalComputedBits += categoryBits;

                            extraInfo += $"Scene number (12 bits): {sceneNameNumBits}\r\n\r\n";
                            extraInfo += $"'p' ID (12 bits): {pIDbits}\r\n\r\n";
                            extraInfo += $"Category (8 bits): {categoryBits}";
                            finalComputedBits.Reverse();

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            GenerationVariables.FileTypeID = "48";
                        }
                        else
                        {
                            // 8 bits
                            mainTypeBits = string.Empty;

                            switch (fileExtn)
                            {
                                case ".bin":
                                    mainTypeBits = Convert.ToString(5, 2).PadLeft(8, '0');
                                    break;

                                case ".lyb":
                                    mainTypeBits = Convert.ToString(1, 2).PadLeft(8, '0');
                                    break;

                                case ".xwp":
                                    mainTypeBits = Convert.ToString(7, 2).PadLeft(8, '0');
                                    break;
                            }

                            // 12 bits
                            reservedBits = "000000000000";

                            // 12 bits
                            sceneNameNumBits = Convert.ToString(sceneNameNum, 2).PadLeft(12, '0');

                            // Assemble bits
                            finalComputedBits += mainTypeBits;
                            finalComputedBits += reservedBits;
                            finalComputedBits += sceneNameNumBits;

                            extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                            extraInfo += $"Reserved (12 bits): {reservedBits}\r\n\r\n";
                            extraInfo += $"Scene number (12 bits): {sceneNameNumBits}";
                            finalComputedBits.Reverse();

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            GenerationVariables.FileTypeID = "112";
                        }
                    }
                    else
                    {
                        SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                    }
                    break;


                case 8:
                    // 8 bits
                    mainTypeBits = Convert.ToString(4, 2).PadLeft(8, '0');

                    // 12 bits
                    reservedBits = "000000000000";

                    // 12 bits
                    sceneNameNum = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                    if (sceneNameNum == -1)
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            ParsingErrorMsg = $"Unable to determine scene number from path";
                        }
                        else
                        {
                            ParsingErrorMsg = $"Unable to determine scene number from filename for a file.\n{GenerationVariables.PathErrorStringForBatch}";
                        }

                        SharedFunctions.Error(ParsingErrorMsg);
                    }

                    if (sceneNameNum > 998)
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            ParsingErrorMsg = $"scene number in the path is too large. must be from 0 to 998.";
                        }
                        else
                        {
                            ParsingErrorMsg = $"scene number in the path is too large. must be from 0 to 998.\n{GenerationVariables.PathErrorStringForBatch}";
                        }

                        SharedFunctions.Error(ParsingErrorMsg);
                    }

                    sceneNameNumBits = Convert.ToString(sceneNameNum, 2).PadLeft(12, '0');

                    // Assemble bits
                    finalComputedBits += mainTypeBits;
                    finalComputedBits += reservedBits;
                    finalComputedBits += sceneNameNumBits;

                    extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                    extraInfo += $"Reserved (12 bits): {reservedBits}\r\n\r\n";
                    extraInfo += $"Scene number (12 bits): {sceneNameNumBits}";
                    finalComputedBits.Reverse();

                    fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                    GenerationVariables.FileCode = fileCode;
                    GenerationVariables.FileTypeID = "112";
                    break;


                default:
                    SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                    break;
            }
        }
        #endregion


        private static int GetSplitID(int splitID)
        {
            var currentSplitID = 0;

            for (int i = 0; i < splitID + 1; i++)
            {
                if (i == splitID)
                {
                    break;
                }
                else
                {
                    currentSplitID += 2;
                }
            }

            return currentSplitID;
        }
    }
}
using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class GuiDir
    {
        private static string ParsingErrorMsg = string.Empty;

        private static readonly List<string> _validExtensions = new List<string>()
        {
            ".imgb", ".xgr"
        };

        public static void ProcessGuiPath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    GuiPathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                case GameID.xiii3:
                    GuiPathXIII2LR(virtualPathData, virtualPath);
                    break;
            }
        }


        #region XIII
        private static void GuiPathXIII(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode = string.Empty;
            string extraInfo = string.Empty;

            // 4 bits
            var mainTypeBits = string.Empty;

            if (virtualPathData.Length > 3)
            {
                mainTypeBits = Convert.ToString(8, 2).PadLeft(4, '0');

                string reservedBits;
                string categoryBits;
                string grpBits;

                int numInFileName;
                int fileID;
                string fileIDbits;
                int grpID;

                switch (startingPortion + "/" + virtualPathData[2])
                {
                    case "gui/resident/autoclip":
                        // 5 bits
                        reservedBits = "00000";

                        // 11 bits
                        categoryBits = Convert.ToString(3, 2).PadLeft(11, '0');

                        // 12 bits
                        numInFileName = GenerationFunctions.DeriveNumFromString(Path.GetFileName(virtualPath));
                        if (numInFileName == -1)
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

                        if (numInFileName > 999)
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

                        fileID = GetFileID(virtualPath, 2, 1, fileExtn);
                        fileIDbits = Convert.ToString(fileID, 2).PadLeft(12, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"MainType (4 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Reserved (5 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"Category (11 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"File number (12 bits): {fileIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    case "gui/resident/clipbg":
                        // 5 bits
                        reservedBits = "00000";

                        // 11 bits
                        categoryBits = Convert.ToString(5, 2).PadLeft(11, '0');

                        // 12 bits
                        numInFileName = GenerationFunctions.DeriveNumFromString(Path.GetFileName(virtualPath));
                        if (numInFileName == -1)
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

                        if (numInFileName > 99)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "File number in the path is too large. must be from 0 to 99.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 99.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        fileID = GetFileID(virtualPath, 0, 0, fileExtn);
                        fileIDbits = Convert.ToString(fileID, 2).PadLeft(12, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"MainType (4 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Reserved (5 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"Category (11 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"File number (12 bits): {fileIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    case "gui/resident/monster":
                        // 5 bits
                        reservedBits = "00000";

                        // 11 bits
                        categoryBits = Convert.ToString(1, 2).PadLeft(11, '0');

                        // 12 bits
                        numInFileName = GenerationFunctions.DeriveNumFromString(Path.GetFileName(virtualPath));
                        if (numInFileName == -1)
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

                        if (numInFileName > 999)
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

                        fileID = GetFileID(virtualPath, 0, 0, fileExtn);
                        fileIDbits = Convert.ToString(fileID, 2).PadLeft(12, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"MainType (4 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Reserved (5 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"Category (11 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"File number (12 bits): {fileIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    case "gui/resident/mission":
                        // 5 bits
                        categoryBits = Convert.ToString(4, 2).PadLeft(5, '0');

                        numInFileName = GenerationFunctions.DeriveNumFromString(Path.GetFileName(virtualPath));
                        if (numInFileName == -1)
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

                        if (numInFileName > 99)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "File number in the path is too large. must be from 0 to 99.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 99.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        var grpIDiterator = 0;
                        var currentGrpID = 16;
                        var currentFileID = 0;
                        var currentFileIDinitial = 0;

                        for (int i = 0; i < numInFileName + 1; i++)
                        {
                            if (i == numInFileName)
                            {
                                currentFileID = fileExtn == ".xgr" ? currentFileID + 1 : currentFileID + 2;
                                break;
                            }

                            currentFileIDinitial += 256;
                            currentFileID = currentFileIDinitial;
                            grpIDiterator++;

                            if (grpIDiterator == 16)
                            {
                                grpIDiterator = 0;
                                currentGrpID++;
                                currentFileID = 0;
                                currentFileIDinitial = 0;
                            }
                        }

                        // 11 bits
                        grpID = currentGrpID;
                        grpBits = Convert.ToString(grpID, 2).PadLeft(11, '0');

                        // 12 bits
                        fileID = currentFileID;
                        fileIDbits = Convert.ToString(fileID, 2).PadLeft(12, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += grpBits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"MainType (4 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Category (5 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"Group number (11 bits): {grpBits}\r\n\r\n";
                        extraInfo += $"File number (12 bits): {fileIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    case "gui/resident/shop":
                        // 5 bits
                        reservedBits = "00000";

                        // 11 bits
                        categoryBits = Convert.ToString(0, 2).PadLeft(11, '0');

                        // 12 bits
                        numInFileName = GenerationFunctions.DeriveNumFromString(Path.GetFileName(virtualPath));
                        if (numInFileName == -1)
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

                        if (numInFileName > 99)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 99.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 99.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        fileID = GetFileID(virtualPath, 32, 0, fileExtn);
                        fileIDbits = Convert.ToString(fileID, 2).PadLeft(12, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"MainType (4 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Reserved (5 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"Category (11 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"File number (12 bits): {fileIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    case "gui/resident/tutorial":
                        // 5 bits
                        reservedBits = "00000";

                        // 11 bits
                        categoryBits = Convert.ToString(4, 2).PadLeft(11, '0');

                        // 12 bits
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            GenerationFunctions.UserInput("Enter file number", "Must be from 32 to 4095", 32, 4095);
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

                        fileID = GenerationVariables.NumInput;

                        fileIDbits = Convert.ToString(fileID, 2).PadLeft(12, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"MainType (4 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Reserved (5 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"Category (11 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"File number (12 bits): {fileIDbits}";
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
        private static void GuiPathXIII2LR(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode = string.Empty;
            string extraInfo = string.Empty;

            // 4 bits
            var reservedABits = string.Empty;

            if (virtualPathData.Length > 3)
            {
                reservedABits = "0000";

                string categoryBits;
                string reservedBbits;

                int fileID;
                string fileIDbits;

                switch (startingPortion + "/" + virtualPathData[2])
                {
                    case "gui/resident/autoclip":
                        // Determine category and
                        // reservedB bits
                        // Collectively they are 16 bits 
                        var numInFileName = 0;
                        if (virtualPathData[3].StartsWith("autoclip"))
                        {
                            numInFileName = GenerationFunctions.DeriveNumFromString(virtualPathData[3]);
                        }

                        bool rangeTypeV1;
                        if (numInFileName <= 127 && numInFileName != 0)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                MessageBox.Show("Detected autoclip number in filename is used in 13-1. Code generation will slightly differ!\n\nDo not confuse this with the file number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            rangeTypeV1 = true;
                            reservedBbits = "00000";
                            categoryBits = Convert.ToString(3, 2).PadLeft(11, '0');
                        }
                        else
                        {
                            rangeTypeV1 = false;
                            categoryBits = Convert.ToString(2, 2).PadLeft(5, '0');
                            reservedBbits = "00000000000";
                        }

                        // 12 bits
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            if (rangeTypeV1)
                            {
                                GenerationFunctions.UserInput("Enter file number", "Must be from 2 to 4095", 2, 4095);
                                fileID = GenerationVariables.NumInput;

                                fileIDbits = Convert.ToString(fileID, 2).PadLeft(12, '0');
                            }
                            else
                            {
                                GenerationFunctions.UserInput("Enter file number", "Must be from 0 to 4095", 0, 4095);
                                fileID = GenerationVariables.NumInput;

                                fileIDbits = Convert.ToString(fileID, 2).PadLeft(12, '0');
                            }
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

                            fileID = GenerationVariables.NumInput;

                            fileIDbits = Convert.ToString(fileID, 2).PadLeft(12, '0');
                        }

                        // Assemble bits
                        finalComputedBits += reservedABits;

                        extraInfo += $"ReservedA (4 bits): {reservedABits}\r\n\r\n";

                        if (rangeTypeV1)
                        {
                            finalComputedBits += reservedBbits;
                            finalComputedBits += categoryBits;

                            extraInfo += $"ReservedB (5 bits): {reservedBbits}\r\n\r\n";
                            extraInfo += $"Category (11 bits): {categoryBits}\r\n\r\n";
                        }
                        else
                        {
                            finalComputedBits += categoryBits;
                            finalComputedBits += reservedBbits;

                            extraInfo += $"Category (5 bits): {categoryBits}\r\n\r\n";
                            extraInfo += $"ReservedB (11 bits): {reservedBbits}\r\n\r\n";
                        }

                        finalComputedBits += fileIDbits;

                        extraInfo += $"File number (12 bits): {fileIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "128";
                        break;


                    case "gui/resident/tutorial":
                        // 5 bits
                        reservedBbits = "00000";

                        // 11 bits
                        categoryBits = Convert.ToString(4, 2).PadLeft(11, '0');

                        // 12 bits
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            GenerationFunctions.UserInput("Enter file number", "Must be from 32 to 4095", 32, 4095);
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

                        fileID = GenerationVariables.NumInput;

                        fileIDbits = Convert.ToString(fileID, 2).PadLeft(12, '0');

                        // Assemble bits
                        finalComputedBits += reservedABits;
                        finalComputedBits += reservedBbits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"ReservedA (4 bits): {reservedABits}\r\n\r\n";
                        extraInfo += $"ReservedB (5 bits): {reservedBbits}\r\n\r\n";
                        extraInfo += $"Category (11 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"File number (12 bits): {fileIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "128";
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



        private static int GetFileID(string fileName, int idCounterStart, int counterStart, string fileExtn)
        {
            var numInFileName = GenerationFunctions.DeriveNumFromString(fileName);
            var currentFileID = idCounterStart;

            for (int i = counterStart; i < numInFileName + 1; i++)
            {
                if (i == numInFileName)
                {
                    currentFileID = fileExtn == ".xgr" ? currentFileID : currentFileID + 1;
                    break;
                }
                else
                {
                    currentFileID += 2;
                }
            }

            return currentFileID;
        }
    }
}
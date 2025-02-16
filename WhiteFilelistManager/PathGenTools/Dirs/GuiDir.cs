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

            string fileCode;
            var extraInfo = string.Empty;

            // 4 bits
            string mainTypeBits;

            if (virtualPathData.Length > 3)
            {
                string categoryBits;

                int numInFileName;
                int fileID;
                string fileIDbits;
                int currentFileID = 0;

                switch (startingPortion + "/" + virtualPathData[2])
                {
                    case "gui/resident/autoclip":
                        // 8 bits
                        mainTypeBits = Convert.ToString(128, 2).PadLeft(8, '0');

                        // 12 bits
                        categoryBits = Convert.ToString(3, 2).PadLeft(12, '0');

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
                        finalComputedBits += categoryBits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Category (12 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"File ID (12 bits): {fileIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    case "gui/resident/clipbg":
                        // 8 bits
                        mainTypeBits = Convert.ToString(128, 2).PadLeft(8, '0');

                        // 12 bits
                        categoryBits = Convert.ToString(5, 2).PadLeft(12, '0');

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
                        finalComputedBits += categoryBits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Category (12 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"File ID (12 bits): {fileIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    case "gui/resident/mission":
                        // 8 bits
                        mainTypeBits = Convert.ToString(130, 2).PadLeft(8, '0');

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

                        // 12 bits
                        var grpBits = Convert.ToString(currentGrpID, 2).PadLeft(12, '0');

                        // 12 bits
                        fileIDbits = Convert.ToString(currentFileID, 2).PadLeft(12, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += grpBits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Group ID (12 bits): {grpBits}\r\n\r\n";
                        extraInfo += $"File ID (12 bits): {fileIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    case "gui/resident/monster":
                        // 8 bits
                        mainTypeBits = Convert.ToString(128, 2).PadLeft(8, '0');

                        // 12 bits
                        categoryBits = Convert.ToString(1, 2).PadLeft(12, '0');

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
                        finalComputedBits += categoryBits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Category (12 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"File ID (12 bits): {fileIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    case "gui/resident/pack":
                        // 8 bits
                        mainTypeBits = Convert.ToString(128, 2).PadLeft(8, '0');

                        // 12 bits
                        var reservedBits = "000000000000";

                        // 4 bits
                        categoryBits = Convert.ToString(4, 2).PadLeft(4, '0');

                        // 8 bits
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

                        fileID = GetFileID(virtualPath, 0, 0, fileExtn);
                        fileIDbits = Convert.ToString(fileID, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += reservedBits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Reserved (12 bits): {reservedBits}\r\n\r\n";
                        extraInfo += $"Category (4 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"File ID (8 bits): {fileIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    case "gui/resident/shop":
                        // 8 bits
                        mainTypeBits = Convert.ToString(128, 2).PadLeft(8, '0');

                        // 12 bits
                        categoryBits = Convert.ToString(0, 2).PadLeft(12, '0');

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
                        finalComputedBits += categoryBits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Category (12 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"File ID (12 bits): {fileIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        break;


                    case "gui/resident/tutorial":
                        // 8 bits
                        mainTypeBits = Convert.ToString(128, 2).PadLeft(8, '0');

                        // 12 bits
                        categoryBits = Convert.ToString(4, 2).PadLeft(12, '0');

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

                        if (numInFileName > 127)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "File number in the path is too large. must be from 0 to 127.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 127.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        var langID = Path.GetFileNameWithoutExtension(virtualPath).Replace(".win32", "");
                        langID = langID.Substring(langID.Length - 2);

                        currentFileID = 32;

                        for (int i = 1; i < numInFileName + 1; i++)
                        {
                            if (i == numInFileName)
                            {
                                if (langID != "jp")
                                {
                                    switch (langID)
                                    {
                                        case "us":
                                            currentFileID += 2;
                                            currentFileID = fileExtn == ".xgr" ? currentFileID : currentFileID + 1;
                                            break;

                                        case "uk":
                                            currentFileID += 4;
                                            currentFileID = fileExtn == ".xgr" ? currentFileID : currentFileID + 1;
                                            break;

                                        case "it":
                                            currentFileID += 6;
                                            currentFileID = fileExtn == ".xgr" ? currentFileID : currentFileID + 1;
                                            break;

                                        case "gr":
                                            currentFileID += 8;
                                            currentFileID = fileExtn == ".xgr" ? currentFileID : currentFileID + 1;
                                            break;

                                        case "fr":
                                            currentFileID += 10;
                                            currentFileID = fileExtn == ".xgr" ? currentFileID : currentFileID + 1;
                                            break;

                                        case "sp":
                                            currentFileID += 12;
                                            currentFileID = fileExtn == ".xgr" ? currentFileID : currentFileID + 1;
                                            break;
                                    }
                                }
                                else
                                {
                                    currentFileID = fileExtn == ".xgr" ? currentFileID : currentFileID + 1;
                                }

                                break;
                            }
                            else
                            {
                                currentFileID += 14;
                                currentFileID += 18;
                            }
                        }

                        fileID = currentFileID;
                        fileIDbits = Convert.ToString(fileID, 2).PadLeft(12, '0');

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"MainType (8 bits): {mainTypeBits}\r\n\r\n";
                        extraInfo += $"Category (12 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"File ID (12 bits): {fileIDbits}";
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

            string fileCode;
            var extraInfo = string.Empty;

            // 4 bits
            string reservedABits;

            if (virtualPathData.Length > 3)
            {
                reservedABits = "0000";

                string categoryBits;
                string reservedBbits;
                string reservedCbits;

                int numInFileName;
                int fileID;
                string fileIDbits;
                int currentFileID = 0;

                switch (startingPortion + "/" + virtualPathData[2])
                {
                    case "gui/resident/autoclip":
                        // Determine category and
                        // reservedB bits
                        // collectively
                        numInFileName = GenerationFunctions.DeriveNumFromString(virtualPathData[3]);
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

                        bool rangeTypeV1;
                        if (numInFileName > 0 && numInFileName <= 409)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                MessageBox.Show("Detected autoclip number in filename is used in 13-1. Code generation will slightly differ!\n\nDo not confuse this with the file number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            rangeTypeV1 = true;

                            // 5 bits
                            reservedBbits = "00000";

                            // 11 bits
                            categoryBits = Convert.ToString(3, 2).PadLeft(11, '0');
                        }
                        else
                        {
                            rangeTypeV1 = false;

                            // 5 bits
                            categoryBits = Convert.ToString(2, 2).PadLeft(5, '0');

                            // 11 bits
                            reservedBbits = "00000000000";
                        }

                        // 12 bits
                        if (rangeTypeV1)
                        {
                            fileID = GetFileID(virtualPath, 2, 1, fileExtn);
                        }
                        else
                        {
                            fileID = GetFileID(virtualPath, 0, 500, fileExtn);
                        }

                        fileIDbits = Convert.ToString(fileID, 2).PadLeft(12, '0');

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

                        extraInfo += $"File ID (12 bits): {fileIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "128";
                        break;


                    case "gui/resident/pack":
                        // 5 bits
                        reservedBbits = "00000";

                        // 11 bits
                        reservedCbits = "00000000000";

                        // 4 bits
                        categoryBits = Convert.ToString(4, 2).PadLeft(4, '0');

                        // 8 bits
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

                        fileID = GetFileID(virtualPath, 0, 0, fileExtn);
                        fileIDbits = Convert.ToString(fileID, 2).PadLeft(8, '0');

                        // Assemble bits
                        finalComputedBits += reservedABits;
                        finalComputedBits += reservedBbits;
                        finalComputedBits += reservedCbits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"ReservedA (4 bits): {reservedABits}\r\n\r\n";
                        extraInfo += $"ReservedB (5 bits): {reservedBbits}\r\n\r\n";
                        extraInfo += $"ReservedC (11 bits): {reservedCbits}\r\n\r\n";
                        extraInfo += $"Category (4 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"File ID (8 bits): {fileIDbits}";
                        finalComputedBits.Reverse();

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "128";
                        break;


                    case "gui/resident/shop":
                        // 5 bits
                        reservedBbits = "00000";

                        // 11 bits
                        reservedCbits = "00000000000";

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
                        finalComputedBits += reservedABits;
                        finalComputedBits += reservedBbits;
                        finalComputedBits += reservedCbits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"ReservedA (4 bits): {reservedABits}\r\n\r\n";
                        extraInfo += $"ReservedB (5 bits): {reservedBbits}\r\n\r\n";
                        extraInfo += $"ReservedC (11 bits): {reservedCbits}\r\n\r\n";
                        extraInfo += $"File ID (12 bits): {fileIDbits}";
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

                        if (numInFileName > 127)
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                ParsingErrorMsg = "File number in the path is too large. must be from 0 to 127.";
                            }
                            else
                            {
                                ParsingErrorMsg = $"File number in the path is too large. must be from 0 to 127.\n{GenerationVariables.PathErrorStringForBatch}";
                            }

                            SharedFunctions.Error(ParsingErrorMsg);
                        }

                        var langID = Path.GetFileNameWithoutExtension(virtualPath).Replace(".win32", "");
                        langID = langID.Substring(langID.Length - 2);

                        currentFileID = 32;

                        for (int i = 1; i < numInFileName + 1; i++)
                        {
                            if (i == numInFileName)
                            {
                                if (langID != "jp")
                                {
                                    switch (langID)
                                    {
                                        case "us":
                                            currentFileID += 2;
                                            currentFileID = fileExtn == ".xgr" ? currentFileID : currentFileID + 1;
                                            break;

                                        case "it":
                                            currentFileID += 6;
                                            currentFileID = fileExtn == ".xgr" ? currentFileID : currentFileID + 1;
                                            break;

                                        case "gr":
                                            currentFileID += 8;
                                            currentFileID = fileExtn == ".xgr" ? currentFileID : currentFileID + 1;
                                            break;

                                        case "fr":
                                            currentFileID += 10;
                                            currentFileID = fileExtn == ".xgr" ? currentFileID : currentFileID + 1;
                                            break;

                                        case "sp":
                                            currentFileID += 12;
                                            currentFileID = fileExtn == ".xgr" ? currentFileID : currentFileID + 1;
                                            break;

                                        case "kr":
                                            currentFileID += 16;
                                            currentFileID = fileExtn == ".xgr" ? currentFileID : currentFileID + 1;
                                            break;

                                        case "ch":
                                            currentFileID += 20;
                                            currentFileID = fileExtn == ".xgr" ? currentFileID : currentFileID + 1;
                                            break;
                                    }
                                }
                                else
                                {
                                    currentFileID = fileExtn == ".xgr" ? currentFileID : currentFileID + 1;
                                }

                                break;
                            }
                            else
                            {
                                currentFileID += 22;
                                currentFileID += 10;
                            }
                        }

                        fileID = currentFileID;
                        fileIDbits = Convert.ToString(fileID, 2).PadLeft(12, '0');

                        // Assemble bits
                        finalComputedBits += reservedABits;
                        finalComputedBits += reservedBbits;
                        finalComputedBits += categoryBits;
                        finalComputedBits += fileIDbits;

                        extraInfo += $"ReservedA (4 bits): {reservedABits}\r\n\r\n";
                        extraInfo += $"ReservedB (5 bits): {reservedBbits}\r\n\r\n";
                        extraInfo += $"Category (11 bits): {categoryBits}\r\n\r\n";
                        extraInfo += $"File ID (12 bits): {fileIDbits}";
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
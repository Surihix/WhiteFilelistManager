﻿using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class BtsceneDir
    {
        private static string ParsingErrorMsg = string.Empty;

        private static readonly List<string> _validExtensions = new()
        {
            ".bin", ".wdb"
        };


        public static void ProcessBtscenePath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    BtscenePathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                    BtscenePathXIII2(virtualPathData, virtualPath);
                    break;

                case GameID.xiii3:
                    BtscenePathXIIILR(virtualPathData, virtualPath);
                    break;
            }
        }


        #region XIII
        private static void BtscenePathXIII(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;

            string mainTypeBits;

            if (virtualPathData.Length > 2)
            {
                if (startingPortion == "btscene/wdb" && virtualPathData[2] == "entry")
                {
                    // 4 bits
                    mainTypeBits = Convert.ToString(11, 2).PadLeft(4, '0');

                    // 2 bits
                    var categoryBits = Convert.ToString(2, 2).PadLeft(2, '0');

                    // 10 bits
                    var reservedBits = "0000000000";

                    // 16 bits
                    var fileId = GenerationFunctions.DeriveNumFromString(virtualPathData[3]);
                    
                    if (fileId == -1)
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            ParsingErrorMsg = "btscene file number in the path is invalid";
                        }
                        else
                        {
                            ParsingErrorMsg = $"btscene file number in the path is invalid.\n{GenerationVariables.PathErrorStringForBatch}";
                        }

                        SharedFunctions.Error(ParsingErrorMsg);
                    }

                    fileId++;

                    if (fileId > 65535)
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            ParsingErrorMsg = "btscene file number in the path is too large. must be from 0 to 65534.";
                        }
                        else
                        {
                            ParsingErrorMsg = $"btscene file number in the path is too large. must be from 0 to 65534.\n{GenerationVariables.PathErrorStringForBatch}";
                        }

                        SharedFunctions.Error(ParsingErrorMsg);
                    }

                    var fileIdBits = Convert.ToString(fileId, 2).PadLeft(16, '0');

                    // Assemble bits
                    finalComputedBits += mainTypeBits;
                    finalComputedBits += categoryBits;
                    finalComputedBits += reservedBits;
                    finalComputedBits += fileIdBits;

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
        private static void BtscenePathXIII2(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;

            if (virtualPathData.Length > 2)
            {
                switch (startingPortion)
                {
                    case "btscene/pack":
                        if (virtualPathData.Length > 3)
                        {
                            string letterIdBits;

                            switch (virtualPathData[2])
                            {
                                case "clb":
                                case "wdb":
                                    if (virtualPathData[2] == "clb" && virtualPathData[3].StartsWith("z"))
                                    {
                                        // 8 bits
                                        var categoryAbits = Convert.ToString(1, 2).PadLeft(8, '0');

                                        // 9 bits
                                        var categoryBbits = Convert.ToString(2, 2).PadLeft(9, '0');

                                        // 15 bits
                                        var zoneID = GenerationFunctions.DeriveNumFromString(virtualPathData[3]);
                                        GenerationFunctions.CheckDerivedNumber(zoneID, "zone", 998);

                                        var zoneIDbits = Convert.ToString(zoneID, 2).PadLeft(15, '0');

                                        // Assemble bits
                                        finalComputedBits += categoryAbits;
                                        finalComputedBits += categoryBbits;
                                        finalComputedBits += zoneIDbits;

                                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                                        GenerationVariables.FileCode = fileCode;
                                        GenerationVariables.FileTypeID = "64";
                                    }
                                    else if (!virtualPath.EndsWith("vwx.bin"))
                                    {
                                        // 5 bits
                                        letterIdBits = Convert.ToString(GetLetterID(virtualPathData[3]), 2).PadLeft(5, '0');

                                        // 12 bits
                                        var fileId = GenerationFunctions.DeriveNumFromString(virtualPathData[3]);
                                        GenerationFunctions.CheckDerivedNumber(fileId, "file", 999);

                                        var fileIdBits = Convert.ToString(fileId, 2).PadLeft(12, '0');

                                        // 15 bits
                                        var paddedBits = Convert.ToString(short.MaxValue, 2);

                                        // Assemble bits
                                        finalComputedBits += letterIdBits;
                                        finalComputedBits += fileIdBits;
                                        finalComputedBits += paddedBits;

                                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                                        GenerationVariables.FileCode = fileCode;

                                        if (virtualPathData[2] == "wdb")
                                        {
                                            GenerationVariables.FileTypeID = "177";
                                        }
                                        else if (virtualPathData[2] == "clb") 
                                        {
                                            GenerationVariables.FileTypeID = "178";
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
                        break;


                    case "btscene/wdb":
                        if (virtualPathData[2] == "entry" && virtualPathData.Length > 3)
                        {
                            // 6 bits
                            var categoryBits = Convert.ToString(2, 2).PadLeft(6, '0');

                            // 10 bits
                            var reservedBits = "0000000000";

                            // 16 bits
                            var fileId = GenerationFunctions.DeriveNumFromString(virtualPathData[3]);

                            if (fileId == -1)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = "btsc file number in the path is invalid";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"btsc file number in the path is invalid.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            fileId++;

                            if (fileId > 65535)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = "btsc file number in the path is too large. must be from 0 to 65534.";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"btsc file number in the path is too large. must be from 0 to 65534.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            var fileIdBits = Convert.ToString(fileId, 2).PadLeft(16, '0');

                            // Assemble bits
                            finalComputedBits += categoryBits;
                            finalComputedBits += reservedBits;
                            finalComputedBits += fileIdBits;

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            GenerationVariables.FileTypeID = "176";
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
            else
            {
                SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
            }
        }
        #endregion


        #region XIII-LR
        private static void BtscenePathXIIILR(string[] virtualPathData, string virtualPath)
        {
            var startingPortion = virtualPathData[0] + "/" + virtualPathData[1];
            var fileExtn = Path.GetExtension(virtualPath);

            if (!_validExtensions.Contains(fileExtn))
            {
                SharedFunctions.Error(GenerationVariables.CommonExtnErrorMsg);
            }

            var finalComputedBits = string.Empty;

            string fileCode;

            if (virtualPathData.Length > 2)
            {
                switch (startingPortion)
                {
                    case "btscene/pack":
                        if (virtualPathData.Length > 3)
                        {
                            // 5 bits
                            var letterIdBits = Convert.ToString(GetLetterID(virtualPathData[3]), 2).PadLeft(5, '0');

                            // 12 bits
                            var fileId = GenerationFunctions.DeriveNumFromString(virtualPathData[3]);
                            GenerationFunctions.CheckDerivedNumber(fileId, "file", 999);

                            var fileIdBits = Convert.ToString(fileId, 2).PadLeft(12, '0');

                            // 15 bits
                            var paddedBits = Convert.ToString(short.MaxValue, 2);

                            // Assemble bits
                            finalComputedBits += letterIdBits;
                            finalComputedBits += fileIdBits;
                            finalComputedBits += paddedBits;

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            GenerationVariables.FileTypeID = "177";
                        }
                        else
                        {
                            SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
                        }
                        break;


                    case "btscene/wdb":
                        if (virtualPathData[2] == "entry" && virtualPathData.Length > 3)
                        {
                            // 6 bits
                            var categoryBits = Convert.ToString(2, 2).PadLeft(6, '0');

                            // 10 bits
                            var reservedBits = "0000000000";

                            // 16 bits
                            var fileId = GenerationFunctions.DeriveNumFromString(virtualPathData[3]);

                            if (fileId == -1)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = "btsc file number in the path is invalid";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"btsc file number in the path is invalid.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            fileId++;

                            if (fileId > 65535)
                            {
                                if (GenerationVariables.GenerationType == GenerationType.single)
                                {
                                    ParsingErrorMsg = "btsc file number in the path is too large. must be from 0 to 65534.";
                                }
                                else
                                {
                                    ParsingErrorMsg = $"btsc file number in the path is too large. must be from 0 to 65534.\n{GenerationVariables.PathErrorStringForBatch}";
                                }

                                SharedFunctions.Error(ParsingErrorMsg);
                            }

                            var fileIdBits = Convert.ToString(fileId, 2).PadLeft(16, '0');

                            // Assemble bits
                            finalComputedBits += categoryBits;
                            finalComputedBits += reservedBits;
                            finalComputedBits += fileIdBits;

                            fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                            GenerationVariables.FileCode = fileCode;
                            GenerationVariables.FileTypeID = "176";
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
            else
            {
                SharedFunctions.Error(GenerationVariables.CommonErrorMsg);
            }
        }
        #endregion


        private static int GetLetterID(string fileName)
        {
            if (!GenerationFunctions.LettersList.Contains(fileName[0]))
            {
                if (GenerationVariables.GenerationType == GenerationType.single)
                {
                    ParsingErrorMsg = "Unable to get letter from filename";
                }
                else
                {
                    ParsingErrorMsg = $"Unable to get letter from filename.\n{GenerationVariables.PathErrorStringForBatch}";
                }

                SharedFunctions.Error(ParsingErrorMsg);
            }

            return GenerationFunctions.LettersList.IndexOf(fileName[0]) + 1;
        }
    }
}
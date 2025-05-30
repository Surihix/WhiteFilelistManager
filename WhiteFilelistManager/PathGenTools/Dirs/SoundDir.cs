﻿using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class SoundDir
    {
        private static readonly List<string> _validExtensions = new()
        {
            ".scd", ".wpd"
        };

        public static void ProcessSoundPath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    SoundPathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                case GameID.xiii3:
                    SoundPathXIII2LR(virtualPathData, virtualPath);
                    break;
            }
        }


        #region XIII
        private static void SoundPathXIII(string[] virtualPathData, string virtualPath)
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
            string mainTypeBits;

            if (virtualPathData.Length > 2)
            {
                switch (startingPortion)
                {
                    case "sound/pack":
                        mainTypeBits = Convert.ToString(10, 2).PadLeft(4, '0');

                        // 2 bits
                        if (fileExtn == ".scd")
                        {
                            fileExtnID = 2;
                        }
                        else
                        {
                            fileExtnID = 3;
                        }

                        fileExtnBits = Convert.ToString(fileExtnID, 2).PadLeft(2, '0');

                        // 26 bits
                        var soundDirID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        GenerationFunctions.CheckDerivedNumber(soundDirID, "sound-dir", 9999);

                        // If .wpd, then do not
                        // prompt for file number
                        string soundIDbits;

                        if (fileExtn == ".wpd")
                        {
                            soundIDbits = Convert.ToString(soundDirID, 2).PadLeft(26, '0');
                        }
                        else
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                GenerationFunctions.UserInput("Enter SCD number", "Must be from 0 to 999", 0, 999);
                            }
                            else
                            {
                                var hasScdFileID = false;

                                if (GenerationVariables.HasIdPathsTxtFile && GenerationVariables.IdBasedPathsDataDict.ContainsKey(virtualPath))
                                {
                                    if (GenerationVariables.IdBasedPathsDataDict[virtualPath].Count > 0)
                                    {
                                        GenerationVariables.NumInput = int.TryParse(GenerationVariables.IdBasedPathsDataDict[virtualPath][0], out int result) ? result : 0;
                                        hasScdFileID = true;
                                    }
                                }

                                if (!hasScdFileID)
                                {
                                    SharedFunctions.Error($"Unable to determine SCD number for a file.\n{GenerationVariables.PathErrorStringForBatch}");
                                }
                            }

                            var scdFileID = GenerationVariables.NumInput;

                            var scdFileIDmerged = int.Parse(soundDirID.ToString() + "" + scdFileID.ToString().PadLeft(3, '0'));
                            soundIDbits = Convert.ToString(scdFileIDmerged, 2).PadLeft(26, '0');
                        }

                        // Assemble bits
                        finalComputedBits += mainTypeBits;
                        finalComputedBits += fileExtnBits;
                        finalComputedBits += soundIDbits;

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
        private static void SoundPathXIII2LR(string[] virtualPathData, string virtualPath)
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

            if (virtualPathData.Length > 2)
            {
                switch (startingPortion)
                {
                    case "sound/pack":
                        // 2 bits
                        if (fileExtn == ".scd")
                        {
                            fileExtnID = 2;
                        }
                        else
                        {
                            fileExtnID = 3;
                        }

                        fileExtnBits = Convert.ToString(fileExtnID, 2).PadLeft(2, '0');

                        // 26 bits
                        var soundDirID = GenerationFunctions.DeriveNumFromString(virtualPathData[2]);
                        GenerationFunctions.CheckDerivedNumber(soundDirID, "sound-dir", 9999);

                        // If .wpd, then do not
                        // prompt for file number
                        string soundIDbits;

                        if (fileExtn == ".wpd")
                        {
                            soundIDbits = Convert.ToString(soundDirID, 2).PadLeft(26, '0');
                        }
                        else
                        {
                            if (GenerationVariables.GenerationType == GenerationType.single)
                            {
                                GenerationFunctions.UserInput("Enter SCD number", "Must be from 0 to 999", 0, 999);
                            }
                            else
                            {
                                var hasScdFileID = false;

                                if (GenerationVariables.HasIdPathsTxtFile && GenerationVariables.IdBasedPathsDataDict.ContainsKey(virtualPath))
                                {
                                    if (GenerationVariables.IdBasedPathsDataDict[virtualPath].Count > 0)
                                    {
                                        GenerationVariables.NumInput = int.TryParse(GenerationVariables.IdBasedPathsDataDict[virtualPath][0], out int result) ? result : 0;
                                        hasScdFileID = true;
                                    }
                                }

                                if (!hasScdFileID)
                                {
                                    SharedFunctions.Error($"Unable to determine SCD number for a file.\n{GenerationVariables.PathErrorStringForBatch}");
                                }
                            }

                            var scdFileID = GenerationVariables.NumInput;

                            var scdFileIDmerged = int.Parse(soundDirID.ToString() + "" + scdFileID.ToString().PadLeft(3, '0'));
                            soundIDbits = Convert.ToString(scdFileIDmerged, 2).PadLeft(26, '0');
                        }

                        // Assemble bits
                        finalComputedBits += reservedBits;
                        finalComputedBits += fileExtnBits;
                        finalComputedBits += soundIDbits;

                        fileCode = finalComputedBits.BinaryToUInt(0, 32).ToString();

                        GenerationVariables.FileCode = fileCode;
                        GenerationVariables.FileTypeID = "160";
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
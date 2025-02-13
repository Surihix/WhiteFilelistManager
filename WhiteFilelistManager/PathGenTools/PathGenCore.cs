using WhiteFilelistManager.PathGenTools.Dirs;
using WhiteFilelistManager.Support;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools
{
    internal class PathGenCore
    {
        public static string GenerateOutput(string virtualPath, ParseType parseType, GameID gameID)
        {
            GenerationVariables.FileCode = string.Empty;
            GenerationVariables.FileTypeID = string.Empty;
            GenerationVariables.GenerationType = GenerationType.single;

            if (string.IsNullOrEmpty(virtualPath) || string.IsNullOrWhiteSpace(virtualPath))
            {
                SharedFunctions.Error("A valid virtual path was not specified. Please specify the virtual path before using this option.");
            }

            GenerationVariables.CommonExtnErrorMsg = "Path does not contain a valid file extension for this root directory";
            GenerationVariables.CommonErrorMsg = "Unable to generate filecode. check if the path starts with a valid directory.";

            ProcessPath(virtualPath, gameID);

            if (parseType == ParseType.json)
            {
                var jsonOutput = "{\r\n     ";

                if (gameID == GameID.xiii)
                {
                    jsonOutput += $"\"fileCode\": {GenerationVariables.FileCode},\r\n     ";
                    jsonOutput += $"\"filePath\": \"0:0:0:{virtualPath}\"\r\n";
                    jsonOutput += "}";
                }
                else
                {
                    jsonOutput += $"\"fileCode\": {GenerationVariables.FileCode},\r\n     ";
                    jsonOutput += $"\"fileTypeID\": {GenerationVariables.FileTypeID},\r\n     ";
                    jsonOutput += $"\"filePath\": \"0:0:0:{virtualPath}\"\r\n";
                    jsonOutput += "}\r\n";
                }

                return jsonOutput;
            }
            else
            {
                var txtOutput = string.Empty;

                if (gameID == GameID.xiii)
                {
                    txtOutput += $"{GenerationVariables.FileCode}|";
                    txtOutput += $"0:0:0:{virtualPath}";
                }
                else
                {
                    txtOutput += $"{GenerationVariables.FileCode}|";
                    txtOutput += $"{GenerationVariables.FileTypeID}|";
                    txtOutput += $"0:0:0:{virtualPath}";
                }

                return txtOutput;
            }
        }


        public static void GenerateForDir(ParseType parseType, string directoryPath, GameID gameID)
        {
            GenerationVariables.FileCode = string.Empty;
            GenerationVariables.FileTypeID = string.Empty;
            GenerationVariables.GenerationType = GenerationType.batch;
            GenerationVariables.IdBasedPathsTxtFile = Path.Combine(directoryPath, "#id-based_paths.txt");
            BatchGenProcesses.DirectoryPath = directoryPath;

            var filesInDir = BatchGenProcesses.GetFilePathsFromDir();

            if (File.Exists(GenerationVariables.IdBasedPathsTxtFile))
            {
                GenerationVariables.HasIdPathsTxtFile = true;
                BatchGenProcesses.BuildIDBasedPathsDict();
            }
            else
            {
                GenerationVariables.HasIdPathsTxtFile = false;
            }

            var processedDataDict = new Dictionary<string, (uint, int)>();

            var excludedFiles = new string[]
            {
                "#id-based_paths.txt", "desktop.ini", "AlbumArtSmall.jpg", "Folder.jpg"
            };

            foreach (var filePath in filesInDir)
            {
                if (excludedFiles.Contains(filePath))
                {
                    continue;
                }

                GenerationVariables.PathErrorStringForBatch = $"Error occured parsing path: {filePath}";
                GenerationVariables.CommonExtnErrorMsg = $"Path does not contain a valid file extension for this root directory.\n{GenerationVariables.PathErrorStringForBatch}";
                GenerationVariables.CommonErrorMsg = $"Unable to generate filecode. check if the path starts with a valid directory.\n{GenerationVariables.PathErrorStringForBatch}";

                ProcessPath(filePath, gameID);

                if (gameID == GameID.xiii)
                {
                    processedDataDict.Add(filePath, (uint.Parse(GenerationVariables.FileCode), 0));
                }
                else
                {
                    processedDataDict.Add(filePath, (uint.Parse(GenerationVariables.FileCode), int.Parse(GenerationVariables.FileTypeID)));
                }
            }

            if (processedDataDict.Count > 0)
            {
                if (parseType == ParseType.json)
                {
                    BatchGenProcesses.CreateJSONOutputForDir(processedDataDict, gameID);
                }
                else
                {
                    BatchGenProcesses.CreateTxtOutputForDir(processedDataDict, gameID);
                }
            }
        }


        private static void ProcessPath(string virtualPath, GameID gameID)
        {
            var virtualPathData = virtualPath.Split('/');

            string pathErrorMsg;

            if (virtualPathData.Length < 2)
            {
                if (GenerationVariables.GenerationType == GenerationType.single)
                {
                    pathErrorMsg = "A Valid path is not specified";
                }
                else
                {
                    pathErrorMsg = $"A Valid path is not specified for a file.\n{GenerationVariables.PathErrorStringForBatch}";
                }

                SharedFunctions.Error(pathErrorMsg);
            }

            switch (virtualPathData[0])
            {
                case "btscene":
                    BtsceneDir.ProcessBtscenePath(virtualPathData, virtualPath, gameID);
                    break;

                case "chr":
                    ChrDir.ProcessChrPath(virtualPathData, virtualPath, gameID);
                    break;

                case "event":
                    EventDir.ProcessEventPath(virtualPathData, virtualPath, gameID);
                    break;

                case "gui":
                    GuiDir.ProcessGuiPath(virtualPathData, virtualPath, gameID);
                    break;

                case "mot":
                    MotDir.ProcessMotPath(virtualPathData, virtualPath, gameID);
                    break;

                case "movie":
                case "movie_win":
                    if (gameID == GameID.xiii2 || gameID == GameID.xiii3)
                    {
                        MovieDir.ProcessMoviePath(virtualPathData, virtualPath);
                    }
                    else
                    {
                        if (GenerationVariables.GenerationType == GenerationType.single)
                        {
                            pathErrorMsg = "Valid root directory is not specified for the specified game id";
                        }
                        else
                        {
                            pathErrorMsg = $"Valid root directory is not specified for the specified game id.\n{GenerationVariables.PathErrorStringForBatch}";
                        }

                        SharedFunctions.Error(pathErrorMsg);
                    }
                    break;

                case "sound":
                    SoundDir.ProcessSoundPath(virtualPathData, virtualPath, gameID);
                    break;

                case "txtres":
                    TxtresDir.ProcessTxtresPath(virtualPathData, virtualPath, gameID);
                    break;

                case "vfx":
                    VfxDir.ProcessVfxPath(virtualPathData, virtualPath, gameID);
                    break;

                case "zone":
                    ZoneDir.ProcessZonePath(virtualPathData, virtualPath, gameID);
                    break;

                default:
                    if (GenerationVariables.GenerationType == GenerationType.single)
                    {
                        pathErrorMsg = "Valid root directory is not specified";
                    }
                    else
                    {
                        pathErrorMsg = $"Valid root directory is not specified for a file path\n{GenerationVariables.PathErrorStringForBatch}";
                    }

                    SharedFunctions.Error(pathErrorMsg);
                    break;
            }
        }
    }
}
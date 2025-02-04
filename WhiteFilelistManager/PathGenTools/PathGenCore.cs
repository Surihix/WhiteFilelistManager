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

            var filesInDir = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);
            Array.Sort(filesInDir);

            string currentFilePath;
            var dirNameLength = directoryPath.Length;

            for (int i = 0; i < filesInDir.Length; i++)
            {
                currentFilePath = filesInDir[i].Remove(0, dirNameLength + 1).Replace("\\", "/");
                filesInDir[i] = currentFilePath;
            }

            if (File.Exists(GenerationVariables.IdBasedPathsTxtFile))
            {
                GenerationVariables.IdBasedPathsData = File.ReadAllLines(GenerationVariables.IdBasedPathsTxtFile);
            }

            var processedDataDict = new Dictionary<string, (int, int)>();

            foreach (var filePath in filesInDir)
            {
                GenerationVariables.CommonExtnErrorMsg = $"Path does not contain a valid file extension for this root directory.\nError occured when parsing path: {filePath}";
                GenerationVariables.CommonErrorMsg = $"Unable to generate filecode. check if the path starts with a valid directory.\nError occured when parsing path: {filePath}";
                
                ProcessPath(filePath, gameID);

                if (gameID == GameID.xiii)
                {
                    processedDataDict.Add(filePath, (int.Parse(GenerationVariables.FileCode), 0));
                }
                else
                {
                    processedDataDict.Add(filePath, (int.Parse(GenerationVariables.FileCode), int.Parse(GenerationVariables.FileTypeID)));
                }
            }
        }


        private static void ProcessPath(string virtualPath, GameID gameID)
        {
            var virtualPathData = virtualPath.Split('/');

            string pathErrorMsg;

            if (virtualPathData.Length < 2)
            {
                pathErrorMsg = GenerationVariables.GenerationType == GenerationType.single ? "A Valid path is not specified" : $"A Valid path is not specified for a file.\nError occured when parsing path: {virtualPath}";
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
                    pathErrorMsg = GenerationVariables.GenerationType == GenerationType.single ? "Valid root directory is not specified" : $"Valid root directory is not specified for a file path\nDetected virtual path: {virtualPath}";
                    SharedFunctions.Error(pathErrorMsg);
                    break;
            }
        }
    }
}
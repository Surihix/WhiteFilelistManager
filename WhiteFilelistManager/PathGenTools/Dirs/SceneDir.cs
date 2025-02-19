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
            throw new NotImplementedException();
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
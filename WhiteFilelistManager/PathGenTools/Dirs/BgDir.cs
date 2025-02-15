using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    internal class BgDir
    {
        private static string ParsingErrorMsg = string.Empty;

        public static void ProcessBgPath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    BgPathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                    BgPathXIII2(virtualPathData, virtualPath);
                    break;

                case GameID.xiii3:
                    BgPathXIIILR(virtualPathData, virtualPath);
                    break;
            }
        }


        #region XIII
        private static void BgPathXIII(string[] virtualPathData, string virtualPath)
        {

        }
        #endregion


        #region XIII-2
        private static void BgPathXIII2(string[] virtualPathData, string virtualPath)
        {

        }
        #endregion


        #region XIII-LR
        private static void BgPathXIIILR(string[] virtualPathData, string virtualPath)
        {

        }
        #endregion
    }
}
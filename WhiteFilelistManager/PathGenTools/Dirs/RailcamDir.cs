using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager.PathGenTools.Dirs
{
    class RailcamDir
    {
        public static void ProcessRailcamPath(string[] virtualPathData, string virtualPath, GameID gameID)
        {
            switch (gameID)
            {
                case GameID.xiii:
                    RailcamPathXIII(virtualPathData, virtualPath);
                    break;

                case GameID.xiii2:
                    RailcamPathXIII2(virtualPathData, virtualPath);
                    break;
            }
        }


        #region XIII
        private static void RailcamPathXIII(string[] virtualPathData, string virtualPath)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region XIII-2
        private static void RailcamPathXIII2(string[] virtualPathData, string virtualPath)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
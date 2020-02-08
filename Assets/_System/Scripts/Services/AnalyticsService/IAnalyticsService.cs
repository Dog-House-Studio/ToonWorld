using DogScaffold;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// IAnalytics Service is a definition for all
    /// analytics services. An analytics service is
    /// responsible for sending analytics data to 
    /// a server.
    /// </summary>
    public interface IAnalyticsService : IService
    {
        void SendSceneLoaded(string sceneName);
        void SendSettingMenuEntered();
        void SendNewGameStarted();
        void SendCreditsMenuEntered();
        void SendExitButtonPressed();

        //Still needs to be implemented
        void SendPauseMenuEntered();

        //Still needs to be implemented
        void SendStartUnitChoosen(string unitName);

        //Still needs to be implemented
        void SendStorePageOpened();

        //Still needs to be implemented
        void SendShopItemPressed(string itemName);

        //Still Needs to be implemented
        void SendShopPurchase(string itemName, int value);


        void SendMapLocationEntered(string locationName);

        //Still needs to be implemented
        void SendMapCompleted(int mapNumber);

        //Still needs to be implemented
        void SendMapFailed(int mapNumber, int stepNumber, string startUnitName);

        //Still needs to be implemented
        void SendPlayerUnitKilled(string unitName, string unitType, string killerUnitType);

        //Still needs to be implmented
        void SendEnemyUnitKilled(string unitType, string killerUnitType);

        //Still needs to be implemented
        void SendCutSceneStarted(int cutSceneNumber);

        //Still needs to be implemented
        void SendCutSceneSkipped(int cutSceneNumber);

        //Still needs to be implemented
        void SendUnitLeveledUp(int level);

        //Still needs to be implemented
        void SendAchievementAquired(string achievementName);

        //Still needs to be implemented
        void SendBattleCompleted(int enemyUnitsKilled, int playerUnitsKilled, int turnsTaken, int attacksUsed, int attacksReceived);
    }
}

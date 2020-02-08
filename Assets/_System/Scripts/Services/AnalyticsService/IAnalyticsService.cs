using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        void SendPauseMenuEntered();

        void SendStartUnitChoosen(string unitName);

        void SendStorePageOpened();
        void SendShopItemPressed(string itemName);
        void SendShopPurchase(string itemName, int value);

        void SendMapLocationEntered(string locationName);
        void SendMapCompleted(int mapNumber);
        void SendMapFailed(int mapNumber, int stepNumber, string startUnitName);

        void SendPlayerUnitKilled(string unitName, string unitType, string killerUnitType);
        void SendEnemyUnitKilled(string unitType, string killerUnitType);

        void SendCutSceneStarted(int cutSceneNumber);
        void SendCutSceneSkipped(int cutSceneNumber);

        void SendUnitLeveledUp(int level);

        void SendAchievementAquired(string achievementName);

        void SendBattleCompleted(int enemyUnitsKilled, int playerUnitsKilled, int turnsTaken, int attacksUsed, int attacksReceived);
    }
}

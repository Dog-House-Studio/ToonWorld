using UnityEngine;
using DogScaffold;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// AnalyticsServiceBackdoor is a script that 
    /// can be used to access the analytics service.
    /// </summary>
    public class AnalyticsServiceBackdoor : MonoBehaviour, IAnalyticsService
    {
        #region Private Variables
        private ServiceReference<IAnalyticsService> m_analyticsService 
            = new ServiceReference<IAnalyticsService>();
        #endregion

        #region Main Methods
        public void RegisterService() {}

        public void SendAchievementAquired(string achievementName)
        {
            m_analyticsService.Reference.SendAchievementAquired(achievementName);
        }

        public void SendBattleCompleted(int enemyUnitsKilled, int playerUnitsKilled, int turnsTaken, int attacksUsed, int attacksReceived)
        {
            m_analyticsService.Reference.SendBattleCompleted(enemyUnitsKilled, playerUnitsKilled, turnsTaken, attacksUsed, attacksReceived);
        }

        public void SendCreditsMenuEntered()
        {
            m_analyticsService.Reference.SendCreditsMenuEntered();
        }

        public void SendCutSceneSkipped(int cutSceneNumber)
        {
            m_analyticsService.Reference.SendCutSceneSkipped(cutSceneNumber);
        }

        public void SendCutSceneStarted(int cutSceneNumber)
        {
            m_analyticsService.Reference.SendCutSceneStarted(cutSceneNumber);
        }

        public void SendEnemyUnitKilled(string unitType, string killerUnitType)
        {
            m_analyticsService.Reference.SendEnemyUnitKilled(unitType, killerUnitType);
        }

        public void SendExitButtonPressed()
        {
            m_analyticsService.Reference.SendExitButtonPressed();
        }

        public void SendMapCompleted(int mapNumber)
        {
            m_analyticsService.Reference?.SendMapCompleted(mapNumber);
        }

        public void SendMapFailed(int mapNumber, int stepNumber, string startUnitName)
        {
            m_analyticsService.Reference.SendMapFailed(mapNumber, stepNumber, startUnitName);
        }

        public void SendMapLocationEntered(string locationName)
        {
            m_analyticsService.Reference.SendMapLocationEntered(locationName);
        }

        public void SendNewGameStarted()
        {
            m_analyticsService.Reference.SendNewGameStarted();
        }

        public void SendPauseMenuEntered()
        {
            m_analyticsService.Reference.SendPauseMenuEntered();
        }

        public void SendPlayerUnitKilled(string unitName, string unitType, string killerUnitType)
        {
            m_analyticsService.Reference.SendPlayerUnitKilled(unitName, unitType, killerUnitType);
        }

        public void SendSceneLoaded(string sceneName)
        {
            m_analyticsService.Reference.SendSceneLoaded(sceneName);
        }

        public void SendSettingMenuEntered()
        {
            m_analyticsService.Reference.SendSettingMenuEntered();
        }

        public void SendShopItemPressed(string itemName)
        {
            m_analyticsService.Reference.SendShopItemPressed(itemName);
        }

        public void SendShopPurchase(string itemName, int value)
        {
            m_analyticsService.Reference.SendShopPurchase(itemName, value);
        }

        public void SendStartUnitChoosen(string unitName)
        {
            m_analyticsService.Reference.SendStartUnitChoosen(unitName);
        }

        public void SendStorePageOpened()
        {
            m_analyticsService.Reference.SendStorePageOpened();
        }

        public void SendUnitLeveledUp(int level)
        {
            m_analyticsService.Reference.SendUnitLeveledUp(level);
        }
        #endregion
    }
}

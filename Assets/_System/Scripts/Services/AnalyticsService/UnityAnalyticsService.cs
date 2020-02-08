using DogScaffold;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// UnityAnalyticsService is an implementation of 
    /// the analytics service for ToonWorld. This
    /// uses the unity analytics service for tracking.
    /// </summary>
    public class UnityAnalyticsService : BaseService<IAnalyticsService>,
        IAnalyticsService
    {
        #region Private Variables
        //TODO : This will be fetched from a VersionService in the future
        private const string VERSION = "0.0";
        private const string VERSION_KEY = "Version";

        private const string ACHIEVEMENT_AQUIRED = "Achievement_Aquired";
        private const string BATTLE_COMPLETED = "Battle_Completed";
        private const string CREDITS_ENTERED = "Credits_Entered";
        private const string CUTSCENE_SKIPPED = "Cutscene_Skipped";
        private const string CUTSCENE_STARTED = "Cutscene_Started";
        private const string ENEMY_UNIT_KILLED = "Enemy_Unit_Killed";
        private const string EXIT_BUTTON_PRESSED = "Exit_Button_Pressed";
        private const string MAP_COMPLETED = "Map_Completed";
        private const string MAP_FAILED = "Map_Failed";
        private const string MAP_LOCATION_ENTERED = "Map_Location_Entered";
        private const string NEW_GAME_STARTED = "New_Game_Started";
        private const string PAUSE_MENU_ENTERED = "Pause_Menu_Started";
        private const string PLAYER_UNIT_KILLED = "Player_Unit_Killed";
        private const string SCENE_LOADED = "Scene_Loaded";
        private const string SETTING_MENU_ENTERED = "Settings_Menu_Entered";
        private const string SHOP_ITEM_PRESSED = "Shop_Item_Pressed";
        private const string SHOP_PURCHASE = "Shop_Item_Purchased";
        private const string START_UNIT_CHOOSEN = "Start_Unit_Choosen";
        private const string STORE_PAGE_OPENED = "Store_Page_Opened";
        private const string UNIT_LEVELED_UP = "Unit_Leveled_Up";
        #endregion

        #region Main Methods
        public void SendAchievementAquired(string achievementName)
        {
            
        }

        public void SendBattleCompleted(int enemyUnitsKilled, int playerUnitsKilled, int turnsTaken, int attacksUsed, int attacksReceived)
        {
            
        }

        public void SendCreditsMenuEntered()
        {
            
        }

        public void SendCutSceneSkipped(int cutSceneNumber)
        {
            
        }

        public void SendCutSceneStarted(int cutSceneNumber)
        {
            
        }

        public void SendEnemyUnitKilled(string unitType, string killerUnitType)
        {
            
        }

        public void SendExitButtonPressed()
        {
            
        }

        public void SendMapCompleted(int mapNumber)
        {
            
        }

        public void SendMapFailed(int mapNumber, int stepNumber, string startUnitName)
        {
            
        }

        public void SendMapLocationEntered(string locationName)
        {
            
        }

        public void SendNewGameStarted()
        {
            
        }

        public void SendPauseMenuEntered()
        {
            
        }

        public void SendPlayerUnitKilled(string unitName, string unitType, string killerUnitType)
        {
            
        }

        public void SendSceneLoaded(string sceneName)
        {
            
        }

        public void SendSettingMenuEntered()
        {
            
        }

        public void SendShopItemPressed(string itemName)
        {
            
        }

        public void SendShopPurchase(string itemName, int value)
        {
            
        }

        public void SendStartUnitChoosen(string unitName)
        {
            
        }

        public void SendStorePageOpened()
        {
            
        }

        public void SendUnitLeveledUp(int level)
        {
            
        }
        #endregion

        #region Utility Methods
        private void SendEvent(string EventID,
                               Dictionary<string, object> parameters = null)
        {
            Dictionary<string, object> eventParams
                = new Dictionary<string, object>();

            if (parameters != null) eventParams = parameters;

            eventParams.Add(VERSION_KEY, VERSION);

            Analytics.CustomEvent(EventID, eventParams);
        }
        #endregion
    }
}

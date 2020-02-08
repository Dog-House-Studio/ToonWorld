using DogScaffold;
using System.Collections.Generic;

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
            SendEvent(ACHIEVEMENT_AQUIRED,
                new Dictionary<string, object>
                {
                    {"Achievement Name", achievementName }
                });
        }

        public void SendBattleCompleted(int enemyUnitsKilled, int playerUnitsKilled, 
            int turnsTaken, int attacksUsed, int attacksReceived)
        {
            SendEvent(BATTLE_COMPLETED,
                new Dictionary<string, object>
                {
                    {"Enemy Units Killed", enemyUnitsKilled },
                    {"Player Units Killed", playerUnitsKilled },
                    {"Turns Taken", turnsTaken },
                    { "Attacks Used", attacksUsed},
                    { "Attacks Received", attacksReceived}
                });
        }

        public void SendCreditsMenuEntered()
        {
            SendEvent(CREDITS_ENTERED);
        }

        public void SendCutSceneSkipped(int cutSceneNumber)
        {
            SendEvent(CUTSCENE_SKIPPED, new Dictionary<string, object>
            {
                {"Cut Scene Number", cutSceneNumber }
            });
        }

        public void SendCutSceneStarted(int cutSceneNumber)
        {
            SendEvent(CUTSCENE_STARTED, new Dictionary<string, object>
            {
                {"Cut Scene Number", cutSceneNumber }
            });
        }

        public void SendEnemyUnitKilled(string unitType, string killerUnitType)
        {
            SendEvent(ENEMY_UNIT_KILLED, new Dictionary<string, object>
            {
                {"Unit Type", unitType },
                {"Killer Unit Type", killerUnitType }
            });
        }

        public void SendExitButtonPressed()
        {
            SendEvent(EXIT_BUTTON_PRESSED);
        }

        public void SendMapCompleted(int mapNumber)
        {
            SendEvent(MAP_COMPLETED, new Dictionary<string, object>
            {
                {"Map Number", mapNumber }
            });
        }

        public void SendMapFailed(int mapNumber, int stepNumber, string startUnitName)
        {
            SendEvent(MAP_FAILED, new Dictionary<string, object>
            {
                {"Map Number", mapNumber},
                { "Step Number", stepNumber},
                {"Start Unit Name", startUnitName }
            });
        }

        public void SendMapLocationEntered(string locationName)
        {
            SendEvent(MAP_LOCATION_ENTERED, new Dictionary<string, object>
            {
                {"Location Name", locationName }
            });
        }

        public void SendNewGameStarted()
        {
            SendEvent(NEW_GAME_STARTED);
        }

        public void SendPauseMenuEntered()
        {
            SendEvent(PAUSE_MENU_ENTERED);
        }

        public void SendPlayerUnitKilled(string unitName, string unitType, string killerUnitType)
        {
            SendEvent(PLAYER_UNIT_KILLED, new Dictionary<string, object>
            {
                { "Unit Name", unitName},
                {"Unit Type", unitType },
                {"Killer Unit Type", killerUnitType }
            });
        }

        public void SendSceneLoaded(string sceneName)
        {
            SendEvent(SCENE_LOADED, new Dictionary<string, object>
            {
                {"Scene Name", sceneName }
            });
        }

        public void SendSettingMenuEntered()
        {
            SendEvent(SETTING_MENU_ENTERED);
        }

        public void SendShopItemPressed(string itemName)
        {
            SendEvent(SHOP_ITEM_PRESSED, new Dictionary<string, object>
            {
                { "Item Name", itemName}
            });
        }

        public void SendShopPurchase(string itemName, int value)
        {
            SendEvent(SHOP_PURCHASE, new Dictionary<string, object>
            {
                {"Item Name", itemName },
                {"Value", value }
            });
        }

        public void SendStartUnitChoosen(string unitName)
        {
            SendEvent(START_UNIT_CHOOSEN, new Dictionary<string, object>
            {
                {"Unit Name", unitName }
            });
        }

        public void SendStorePageOpened()
        {
            SendEvent(STORE_PAGE_OPENED);
        }

        public void SendUnitLeveledUp(int level)
        {
            SendEvent(UNIT_LEVELED_UP, new Dictionary<string, object>
            {
                {"Level", level }
            });
        }
        #endregion

        #region Utility Methods
        private void SendEvent(string EventID,
                               Dictionary<string, object> parameters = null)
        {
            #if !UNITY_EDITOR
            Dictionary<string, object> eventParams
                = new Dictionary<string, object>();

            if (parameters != null) eventParams = parameters;

            eventParams.Add(VERSION_KEY, VERSION);

            Analytics.CustomEvent(EventID, eventParams);
            #endif
        }
        #endregion
    }
}

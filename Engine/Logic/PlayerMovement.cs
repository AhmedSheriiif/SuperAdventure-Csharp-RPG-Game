using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Logic
{
    public static class PlayerMovement
    {
        private static Player CurrentPlayer;
        private static Location RequiredLocation;


        // Method to Initiate Movement
        private static void InitiateMovementVariables(Player player, Location location)
        {
            CurrentPlayer = player;
            RequiredLocation = location;
        }

        // Method to Check if Location is Available
        private static bool IsLocationAvaliable()
        {
            if (RequiredLocation == null)
                return false;

            return true;
        }

        // Method to Check if Player Has Required Item to Enter the Location
        private static bool DoesPlayerHaveRequiredItem()
        {
            // Location Requires items to Enter
            if (RequiredLocation.ItemRequiredToEnter != null)
            {
                bool itemFound = false;
                foreach (Item item in CurrentPlayer.InventoryItems.Keys)
                {
                    if ((RequiredLocation.ItemRequiredToEnter.ID == item.ID) && (CurrentPlayer.InventoryItems[item] > 0))
                    {
                        itemFound = true;
                    }
                }

                return itemFound;
            }

            return true;
        }

        // Check if Location Quest is Completed
        private static bool IsQuestCompleted()
        {
            // Check if there is a Quest
            if(RequiredLocation.QuestAvailable != null)
            {
                // Check if it's not completed yet
                if(RequiredLocation.QuestAvailable.Completed == false)
                {
                    return false;
                }
            }
            return true;
        }

        

        private static int MoveToMessageNumber()
        {

            if (!IsLocationAvaliable()) return 1;
            if (!DoesPlayerHaveRequiredItem()) return 2;
            if (!IsQuestCompleted()) return 3;
            return 0;
        }

        public static int MoveTo(Player player, Location location)
        {
            InitiateMovementVariables(player, location);
            int Result = MoveToMessageNumber();

            return Result;
        }
    }
}

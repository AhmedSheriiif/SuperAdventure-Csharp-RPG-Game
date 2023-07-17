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
        private static Player _currentPlayer;
        private static Location _requiredLocation;


        // Method to Initiate Movement
        private static void InitiateMovementVariables(Player player, Location location)
        {
            _currentPlayer = player;
            _requiredLocation = location;
        }

        // Method to Check if Location is Available
        private static bool IsLocationAvaliable()
        {
            if (_requiredLocation == null)
                return false;

            return true;
        }

        // Method to Check if Player Has Required Item to Enter the Location
        private static bool DoesPlayerHaveRequiredItem()
        {
            // Location Requires items to Enter
            if (_requiredLocation.ItemRequiredToEnter != null)
            {
                bool itemFound = false;
                foreach (Item item in _currentPlayer.InventoryItems.Keys)
                {
                    if ((_requiredLocation.ItemRequiredToEnter.ID == item.ID) && (_currentPlayer.InventoryItems[item] > 0))
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
            if(_requiredLocation.QuestAvailable != null)
            {
                // Check if it's not completed yet
                if(_requiredLocation.QuestAvailable.Completed == false)
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

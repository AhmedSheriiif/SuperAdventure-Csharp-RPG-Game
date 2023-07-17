using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Logic
{
    public static class QuestCompleting
    {
        private static Player _player;
        private static Quest _currentQuest;

        private static void InitiateQuestCompleteVariables(Player player, Quest currentQuest)
        {
            _player = player;
            _currentQuest = currentQuest;
        }

        private static bool IsQuestComplete()
        {
            if(_currentQuest.Completed)
                return true;
            return false;
        }

        private static bool DoesPlayerHaveItemsToCompleteQuest()
        {
            foreach(var itemEntry in _currentQuest.RequiredItemsToComplete)
            {
                int itemID = itemEntry.Key.ID;
                int itemAmount = itemEntry.Value;
                bool PlayerHasEnoughtItems = false;

                foreach(var inventoryItem in _player.InventoryItems)
                {
                    if(inventoryItem.Key.ID == itemID && inventoryItem.Value >= itemAmount)
                        PlayerHasEnoughtItems = true;
                }
                if (!PlayerHasEnoughtItems) return false;

            }

            return true;
        }

        private static void RemovePlayerItemsToCompleteQuest()
        {
            // Remove Items from the Player
            foreach (var itemEntry in _currentQuest.RequiredItemsToComplete)
            {
                int itemID = itemEntry.Key.ID;
                int itemAmount = itemEntry.Value;

                foreach (var inventoryItem in _player.InventoryItems)
                {
                    if (inventoryItem.Key.ID == itemID)
                    {
                        int updatedAmount = inventoryItem.Value - itemAmount;
                        _player.InventoryItems[inventoryItem.Key] = updatedAmount;
                    }
                }
            }
        }

        private static void PlayerGetRewards()
        {
            _player.Gold += _currentQuest.RewardGold;
            _player.ExperiencePoints += _currentQuest.RewardExperiencePoints;
            _player.AddItem(_currentQuest.RewardItem);
        }

        public static int CompleteTheQuest(Player player, Quest quest)
        {
            InitiateQuestCompleteVariables(player, quest);

            if (IsQuestComplete()) return 1;
            if (!DoesPlayerHaveItemsToCompleteQuest()) return 2;

            RemovePlayerItemsToCompleteQuest();
            quest.Completed = true;
            return 0;
        }


    }
}

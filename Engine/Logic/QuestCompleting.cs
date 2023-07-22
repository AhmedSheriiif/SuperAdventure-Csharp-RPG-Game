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
            return _currentQuest.RequiredItemsToComplete
                 .All(questEntry => _player.InventoryItems.Any(inventoryItem =>
                 inventoryItem.Key == questEntry.Key && inventoryItem.Value >= questEntry.Value));
        }

        private static void RemovePlayerItemsToCompleteQuest()
        {
            foreach (var questItem in _currentQuest.RequiredItemsToComplete)
            {
                int amount = questItem.Value;
                for (int i = 1; i <= amount; i++)
                {
                    _player.UseItemFromInventory(questItem.Key);
                }
            }

        }

        private static void PlayerGetRewards()
        {
            _player.Gold += _currentQuest.RewardGold;
            _player.AddExperiencePoints(_currentQuest.RewardExperiencePoints);
            _player.AddItemToInventory(_currentQuest.RewardItem.ID);
        }

        public static int CompleteTheQuest(Player player, Quest quest)
        {
            InitiateQuestCompleteVariables(player, quest);

            if (IsQuestComplete()) return 1;
            if (!DoesPlayerHaveItemsToCompleteQuest()) return 2;

            RemovePlayerItemsToCompleteQuest();
            PlayerGetRewards();
            quest.Completed = true;
            return 0;
        }


    }
}

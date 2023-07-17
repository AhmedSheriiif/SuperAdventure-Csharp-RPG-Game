using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Quest : IEntity, IReward
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RewardExperiencePoints { get; set; }
        public int RewardGold { get; set; }
        public Item RewardItem { get; set; }
        public Dictionary<Item, int> RequiredItemsToComplete { get; set; } // Describes the Required Items to Complete the Quest {ItemName: Quantity}

        public Quest(string iD, string name, string description, int rewardExperiencePoints, int rewardGold, Item rewardItem = null)
        {
            ID = iD;
            Name = name;
            Description = description;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            RewardItem = rewardItem;
            RequiredItemsToComplete = new Dictionary<Item, int>();
        }
    }
}

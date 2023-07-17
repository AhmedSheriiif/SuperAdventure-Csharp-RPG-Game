using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Monster : LivingCreature, IReward
    {

        public int RewardExperiencePoints { get; set; }

        public int RewardGold { get; set; }

        public int MaxDamage { get; set; }

        public Dictionary<int, int> LootItems { get; set; } // Items Dropped from Monster and Percentage i.e : {ItemNameID: 90}  this means that the item may drop at 90%

        public Monster() { }
        public Monster(int iD, string name, int currentHitPoints, int maxHitPoints, int rewardExperiencePoints, int rewardGold, int maxDamage) : base(iD, name, currentHitPoints, maxHitPoints)
        {
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            MaxDamage = maxDamage;
            LootItems = new Dictionary<int, int>();
        }

        public void AddLootItem(int itemID, int percentageToDrop)
        {
            LootItems[itemID] = percentageToDrop;
        }

    }
}

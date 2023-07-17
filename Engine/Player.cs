using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Player : LivingCreature
    {
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public Location CurrentLocation { get; set; }

        public Dictionary<Item, int> InventoryItems { get; set; }   // Describes the Items in The Inventory of Player and their Quantity  {ItemName: Quantity} mean it has 10 ItemName in inventory
        public Player(int iD, string name, int maxHitPoints, int currentHitPoints,  int gold, int experiencePoints, int level) : base(iD, name, currentHitPoints, maxHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Level = level;
            InventoryItems = new Dictionary<Item, int>();
        }
    }
}

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
        public int ExperiencePoints { get; private set; }
        public int Level
        {
            get { return ((ExperiencePoints / 10) + 1); }
        }
        public int CurrentLocationID { get; set; }

        public int CurrentWeaponID { get; set; }

        public Dictionary<int, int> InventoryItems { get; set; }   // Describes the Items in The Inventory of Player and their Quantity  {itemID: Quantity} mean it has 10 ItemName in inventory

        public Player() 
        {
            InventoryItems = new Dictionary<int, int>();
        }
        public Player(int iD, string name, int maxHitPoints, int currentHitPoints,  int gold, int experiencePoints) : base(iD, name, currentHitPoints, maxHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            InventoryItems = new Dictionary<int, int>();
            Alive = true;
        }

        public void AddItem(int itemID)
        {
            if(InventoryItems.ContainsKey(itemID))
            {
                InventoryItems[itemID] = InventoryItems[itemID] + 1;
            }
            else
            {
                InventoryItems.Add(itemID, 1);
            }
        }

        public void UseItem(int itemID)
        {
            if (InventoryItems.ContainsKey(itemID))
            {
                InventoryItems[itemID] = InventoryItems[itemID] - 1;
            }
        }

        public void UseHealingPotion(int itemID)
        {
            if (InventoryItems.ContainsKey(itemID))
            {
                InventoryItems[itemID] = InventoryItems[itemID] - 1;

                HealingPotion currentPotion = (HealingPotion)World.ItemByID(itemID);
                CurrentHitPoints += currentPotion.AmountToHeal;

                if(CurrentHitPoints > MaxHitPoints)
                    CurrentHitPoints = MaxHitPoints;
            }

        }

        public void AddExperiencePoints(int experiencePointsToAdd)
        {
            ExperiencePoints += experiencePointsToAdd;
            MaxHitPoints = ((Level) * 10);
        }
    }
}

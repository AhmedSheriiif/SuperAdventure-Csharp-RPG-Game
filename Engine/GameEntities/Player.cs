using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Player : LivingCreature
    {
        // Delegates and Events
        public delegate void onInventoryItemsChangedHandler(int itemID);
        public event onInventoryItemsChangedHandler OnInventoryItemAdded;
        public event onInventoryItemsChangedHandler OnInventoryItemUsed;

        // Fields
        private int _gold;
        private int _experiencePoints;
        private int _currentLocationID;
        private int _currentWeaponID;
        private Dictionary<int, int> _inventoryItems;

        // Properties
        public int Gold 
        {
            get { return _gold; }
            set
            {
                _gold = value;
                OnPropertyChanged("Gold");
            }
        }

        public int ExperiencePoints 
        {
            get { return _experiencePoints; }
            private set
            {
                _experiencePoints = value;
                OnPropertyChanged("ExperiencePoints");
                OnPropertyChanged("Level");
            }
        }
        public int Level
        {
            get { return ((ExperiencePoints / 10) + 1); }
        }

        public int CurrentLocationID 
        {
            get { return _currentLocationID; }
            set
            {
                _currentLocationID = value;
                OnPropertyChanged("CurrentLocationID");
            }
        }



        public int CurrentWeaponID
        { 
            get { return _currentWeaponID; }
            set
            {
                _currentWeaponID = value;
                OnPropertyChanged("CurrentWeaponID");
            }
        }

        public Dictionary<int, int> InventoryItems {
            get { return _inventoryItems; }
            set { _inventoryItems = value; }
        }

        
        public Player() 
        {
            _inventoryItems = new Dictionary<int, int>();
        }
        public Player(int iD, string name, int maxHitPoints, int currentHitPoints,  int gold, int experiencePoints, Dictionary<int, int> inventoryItems, bool alive) : base(iD, name, currentHitPoints, maxHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Alive = alive;
            InventoryItems = inventoryItems;
        }

        public void AddItemToInventory(int itemID)
        {
            if(_inventoryItems.ContainsKey(itemID))
            {
                _inventoryItems[itemID] = _inventoryItems[itemID] + 1;
            }
            else
            {
                _inventoryItems.Add(itemID, 1);
            }
            // Publish The Event
            OnInventoryItemAdded( itemID);
        }

        public void UseItemFromInventory(int itemID)
        {
            if (_inventoryItems.ContainsKey(itemID))
            {
                _inventoryItems[itemID] = _inventoryItems[itemID] - 1;
            }
            OnInventoryItemUsed(itemID);
        }

        public void UseHealingPotion(int itemID)
        {
            if (_inventoryItems.ContainsKey(itemID))
            {
                _inventoryItems[itemID] = _inventoryItems[itemID] - 1;

                HealingPotion currentPotion = (HealingPotion)World.ItemByID(itemID);
                CurrentHitPoints += currentPotion.AmountToHeal;

                if(CurrentHitPoints > MaxHitPoints)
                    CurrentHitPoints = MaxHitPoints;
            }
            OnInventoryItemUsed(itemID);

        }

        public void AddExperiencePoints(int experiencePointsToAdd)
        {
            ExperiencePoints += experiencePointsToAdd;
            MaxHitPoints = ((Level) * 10);
        }
    }
}

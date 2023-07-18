using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using Engine.Logic;

namespace SuperAdventure
{
    public partial class SuperAdventure : Form
    {
        private Color MAIN_MAP_COLOR = Color.AliceBlue;
        private Color CURRENT_POSITION_MAP_COLOR = Color.DarkBlue;

        private Player _player;
        private Monster _currentFightingMonster;

        public SuperAdventure()
        {
            InitializeComponent();
            InitiateMap();
            _player = new Player(iD: 0, name: "Ahmed", maxHitPoints: 10, currentHitPoints: 10, gold: 20, experiencePoints: 0, level: 1);
            _player.InventoryItems.Add(World.ITEM_ID_SNAKE_FANG, 3); // Adding 3 Snake Fangs for Testing.
            _player.InventoryItems.Add(World.ITEM_ID_CLUB, 2);

            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            UpdateUI();

          

        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToNorth);
            UpdateUI();
        }
        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToSouth);
            UpdateUI();
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToEast);
            UpdateUI();
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToWest);
            UpdateUI();
        }

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            if (cboWeapons.SelectedItem != null)
            {
                Weapon currentUsedWeapon = cboWeapons.SelectedItem as Weapon;
                UseWeaponToAttackMonster(currentUsedWeapon);
            }

            UpdateUI();
        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            if (cboPotions.SelectedItem != null)
            {
                HealingPotion currentUsedPotion = cboPotions.SelectedItem as HealingPotion;
                UseHealingPotion(currentUsedPotion);
            }

            UpdateUI();
        }


        private void InitiateMap()
        {
            foreach (var entity in World.MapLocationEntites)
            {
                Button btn = new Button();
                btn.BackColor = MAIN_MAP_COLOR;
                btn.Name = "btn" + World.LocationByID(entity.LocationID).Name;
                btn.Dock = DockStyle.Fill;
                btn.Text = World.LocationByID(entity.LocationID).Name;
                tlpMapPanel.Controls.Add(btn, entity.LocationX, entity.LocationY);
            }
        }

        // This Method to Collect all UI Updating Methods in one method
        private void UpdateUI()
        {
            //rtbMessages.Clear();
            rtbLocations.Clear();
            rtbMonster.Clear();

            UpdateCurrentLocation(_player.CurrentLocation);
            UpdatePlayerPositionColors(_player.CurrentLocation);
            UpdateMonsterDetailsInCurrentLocation(_player.CurrentLocation);
            UpdateInvnetoryListInUI();
            UpdatePotionAndWeaponsInUI();
            UpdatePlayerDetailsInUI();

        }

        private void MoveTo(Location location)
        {
            int MovementResult = PlayerMovement.MoveTo(_player, location);

            // Success Movement
            if (MovementResult == 0)
            {
                _player.CurrentLocation = location;
            }
            else if (MovementResult == 1)
            {
                rtbMessages.Text += "There is no location there" + Environment.NewLine;
            }
            else if (MovementResult == 2)
            {
                rtbMessages.Text += "Sorry, item " + location.ItemRequiredToEnter.Name + " is required to enter this location" + Environment.NewLine;
            }
            else if (MovementResult == 3)
            {
                SolveQuest(location);
            }

        }

        private void SolveQuest(Location location)
        {
            Quest quest = location.QuestAvailable;
            int QuestCompletionResult = QuestCompleting.CompleteTheQuest(_player, quest);
            if (QuestCompletionResult == 0)
            {
                rtbMessages.Text += "Quest " + quest.Name + " Completed" + Environment.NewLine;
                rtbMessages.Text += "You got " + quest.RewardExperiencePoints + " Experience Points" + Environment.NewLine;
                rtbMessages.Text += "You got " + quest.RewardGold + " Gold" + Environment.NewLine;
                rtbMessages.Text += "You got " + quest.RewardItem.Name + " and added to your inventory " + Environment.NewLine;
                _player.CurrentLocation = location;
            }
            else if (QuestCompletionResult == 1)
            {
                rtbMessages.Text += "Quest " + quest.Name + " is already Completed" + Environment.NewLine;
            }
            else if (QuestCompletionResult == 2)
            {
                rtbMessages.Text += "Sorry, you don't have enought items to Complete this Quest" + Environment.NewLine;
                rtbMessages.Text += "This quest requires: " + Environment.NewLine;
                foreach(var entry in quest.RequiredItemsToComplete)
                {
                    rtbMessages.Text += entry.Value + " " + World.ItemByID(entry.Key).NamePlural + Environment.NewLine;
                }
            }
        }


        private void UpdateCurrentLocation(Location location)
        {
            _player.CurrentLocation = location;

            // Show - Hide Moving Buttons
            btnNorth.Enabled = (location.LocationToNorth != null);
            btnSouth.Enabled = (location.LocationToSouth != null);
            btnEast.Enabled = (location.LocationToEast != null);
            btnWest.Enabled = (location.LocationToWest != null);

            rtbLocations.Clear();
            rtbLocations.Text = "Current Location: " + Environment.NewLine;
            rtbLocations.Text += "----------------------------------------" + Environment.NewLine;
            rtbLocations.Text += "Name: " + _player.CurrentLocation.Name + Environment.NewLine;
            rtbLocations.Text += "Description: " + _player.CurrentLocation.Description + Environment.NewLine;

            if (_player.CurrentLocation.QuestAvailable != null)
            {
                rtbLocations.Text += "Quest: " + _player.CurrentLocation.QuestAvailable.Name + Environment.NewLine;
                if (_player.CurrentLocation.QuestAvailable.Completed == true)
                {
                    rtbLocations.Text += "Status: Completed" + Environment.NewLine;
                }
                else
                {
                    rtbLocations.Text += "Status: Not Completed" + Environment.NewLine;
                    rtbLocations.Text += "Items Required for this Quest:" + Environment.NewLine;
                    foreach (var entry in _player.CurrentLocation.QuestAvailable.RequiredItemsToComplete)
                    {
                        rtbLocations.Text += World.ItemByID(entry.Key).Name + ": " + entry.Value + Environment.NewLine;
                    }
                }
            }
            else
            {
                rtbLocations.Text += "Quest: No Quests Available" + Environment.NewLine;
            }

        }

        private void UpdateMonsterDetailsInCurrentLocation(Location location)
        {
            if (location.MonsterLivingHere != null)
            {
                _currentFightingMonster = location.MonsterLivingHere;
                if (location.MonsterLivingHere.Alive == false)
                {
                    rtbMonster.Clear();
                    rtbMonster.Text += "Found a dead" + location.MonsterLivingHere.Name + Environment.NewLine;
                }
                else
                {
                    rtbMonster.Clear();
                    rtbMonster.Text += "Monster Details: " + Environment.NewLine;
                    rtbMonster.Text += "----------------------------" + Environment.NewLine;
                    rtbMonster.Text += "Name: " + _currentFightingMonster.Name + Environment.NewLine;
                    rtbMonster.Text += "Max Hit Points: " + _currentFightingMonster.MaxHitPoints + Environment.NewLine;
                    rtbMonster.Text += "Currently Hit Points: " + _currentFightingMonster.CurrentHitPoints + Environment.NewLine;
                }
            }
            else
            {
                rtbMonster.Clear();
            }
        }

        private void UpdatePlayerPositionColors(Location location)
        {
            foreach (Control ctrl in tlpMapPanel.Controls)
            {
                if (ctrl.Name == "btn" + location.Name)
                {
                    ctrl.BackColor = CURRENT_POSITION_MAP_COLOR;
                }
                else
                {
                    ctrl.BackColor = MAIN_MAP_COLOR;
                }
            }
        }


        private void rtbMessages_TextChanged(object sender, EventArgs e)
        {
            rtbMessages.SelectionStart = rtbLocations.Text.Length;
            rtbMessages.ScrollToCaret();
        }

        private void UpdateInvnetoryListInUI()
        {
            dgvInventory.RowHeadersVisible = false;
            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Name";
            dgvInventory.Columns[0].Width = 225;
            dgvInventory.Columns[1].Name = "Quantity";
            dgvInventory.Columns[1].Width = 73;
            dgvInventory.Rows.Clear();

            foreach(var inventoryItem in _player.InventoryItems)
            {
                int itemID = inventoryItem.Key;
                int quantity = inventoryItem.Value;
                if (quantity > 0)
                {
                    dgvInventory.Rows.Add(new[] { World.ItemByID(itemID).Name, quantity.ToString() });
                }
            }
        }


        private void UpdateComboBoxesItemInUI_Helper<T>(ComboBox cbo, Button btnUse)
        {
            List<T> items = new List<T>();
            foreach (var inventoryItem in _player.InventoryItems)
            {
                int itemID = inventoryItem.Key;
                int quantity = inventoryItem.Value;

                if (World.ItemByID(itemID) is T item && quantity > 0)
                {
                    items.Add(item);
                }
            }

            if(items.Count > 0)
            {
                cbo.Visible = true;
                btnUse.Visible = true;
                cbo.DataSource = items;
                cbo.DisplayMember = "Name";
                cbo.ValueMember = "ID";
                cbo.SelectedIndex = 0;
            }
            else
            {
                cbo.Visible = false;
                btnUse.Visible = false;
            }
        }

        private void UpdatePotionAndWeaponsInUI()
        {
            UpdateComboBoxesItemInUI_Helper<Weapon>(cboWeapons, btnUseWeapon);
            UpdateComboBoxesItemInUI_Helper<HealingPotion>(cboPotions, btnUsePotion);
        }

        private void UpdatePlayerDetailsInUI()
        {
            lblPlayerName.Text = _player.Name;
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblLevel.Text = _player.Level.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
        }



       

        private void UseWeaponToAttackMonster(Weapon weapon)
        {
            int FightResult = MonsterFighting.FightMonster(_player, _currentFightingMonster, weapon);

            if (FightResult == 1)
            {
                rtbMessages.Text += "There is no monster to fight here" + Environment.NewLine;
            }
            else if(FightResult == 2)
            {
                rtbMessages.Text += "Fortunately, " + _currentFightingMonster.Name + " is found died" + Environment.NewLine;
            }
            else if(FightResult == 3)
            {
                rtbMessages.Text += "Sorry, you don't have " + weapon.Name + " or you lost it";
            }
            else if(FightResult > 3)
            {
                rtbMessages.Text += "Be Careful, the fight starts" + Environment.NewLine;
                rtbMessages.Text += "Nice, you used " + weapon.Name + " and attacked " + _currentFightingMonster.Name + Environment.NewLine;
                rtbMessages.Text += "You damaged the monster by " + _currentFightingMonster.LastDamageTaken + " hit points" + Environment.NewLine;

                if (FightResult == 4)
                {
                    rtbMessages.Text += "Wow, you killed " + _currentFightingMonster.Name;
                    rtbMessages.Text += "You got " + _currentFightingMonster.RewardExperiencePoints + " Experience Points" + Environment.NewLine;
                    rtbMessages.Text += "You got " + _currentFightingMonster.RewardGold + " Gold" + Environment.NewLine;
                    rtbMessages.Text += "You looted some items and added to your inventory" + Environment.NewLine;
                }
                else if(FightResult > 4)
                {
                    rtbMessages.Text += "Watch Out!! " + _currentFightingMonster.Name + " is running towards you " + Environment.NewLine;
                    rtbMessages.Text += _currentFightingMonster.Name + " attacked you " + Environment.NewLine;
                    rtbMessages.Text += "Your hit points decreased by " + _player.LastDamageTaken + Environment.NewLine;

                    if(FightResult == 5)
                    {
                        rtbMessages.Text += "You Are DEAD !! " + Environment.NewLine;
                    }
                    else if(FightResult == 6)
                    {
                        rtbMessages.Text += "Attack Again..." + Environment.NewLine;
                    }
                }

            }
        }


        private void UseHealingPotion(HealingPotion potion)
        {
            _player.UseHealingPotion(potion.ID);
            rtbMessages.Text += "you used " + potion.Name + " and got " + potion.AmountToHeal + " hit points" + Environment.NewLine;
        }
    }
}

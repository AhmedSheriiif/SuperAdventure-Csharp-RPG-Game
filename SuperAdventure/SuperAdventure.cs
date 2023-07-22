using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            _player = GameSavingAndLoading.LoadInitialState("Ahmed");
            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            UpdateInvnetoryListInUI();
            UpdateUI();
            AddDataBindingToUI();
            AddEventSubscribers();

        }

        private void AddEventSubscribers()
        {
            _player.OnInventoryItemUsed += InventoryItemUsed;
            _player.OnInventoryItemAdded += InventoryItemAdded;
        }

        private void InventoryItemAdded(int itemID)
        {
            UpdateInvnetoryListInUI();
            UpdateWeaponsItemsInUI();
            UpdatePotionsItemsInUI();
            rtbMessages.Text += "You found a: " + World.ItemByID(itemID).Name + Environment.NewLine;
        }

        private void InventoryItemUsed(int itemID)
        {
            UpdateInvnetoryListInUI();
            UpdateWeaponsItemsInUI();
            UpdatePotionsItemsInUI();
            rtbMessages.Text += "You used: " + World.ItemByID(itemID).Name + Environment.NewLine;
        }

        private void AddDataBindingToUI()
        {
            lblExperience.DataBindings.Add("Text", _player, "ExperiencePoints");
            lblLevel.DataBindings.Add("Text", _player, "Level");
            lblGold.DataBindings.Add("Text", _player, "Gold");
            lblMaxHitPoints.DataBindings.Add("Text", _player, "MaxHitPoints");
            lblHitPoints.DataBindings.Add("Text", _player, "CurrentHitPoints");
            tbUsername.DataBindings.Add("Text", _player, "Name");
        }

        

        private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(World.LocationByID(_player.CurrentLocationID).LocationToNorth);
            UpdateUI();
        }
        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(World.LocationByID(_player.CurrentLocationID).LocationToSouth);
            UpdateUI();
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(World.LocationByID(_player.CurrentLocationID).LocationToEast);
            UpdateUI();
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(World.LocationByID(_player.CurrentLocationID).LocationToWest);
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
            rtbLocations.Clear();
            rtbMonster.Clear();

            UpdateCurrentLocation(World.LocationByID(_player.CurrentLocationID));
            UpdatePlayerPositionColors(World.LocationByID(_player.CurrentLocationID));
            UpdateMonsterDetailsInCurrentLocation(World.LocationByID(_player.CurrentLocationID));
            UpdateWeaponsItemsInUI();
            UpdatePotionsItemsInUI();
        }

        private void MoveTo(Location location)
        {
            int MovementResult = PlayerMovement.MoveTo(_player, location);

            // Success Movement
            if (MovementResult == 0)
            {
                _player.CurrentLocationID = location.ID;
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
                _player.CurrentLocationID = location.ID;
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
            _player.CurrentLocationID = location.ID;

            // Show - Hide Moving Buttons
            btnNorth.Enabled = (location.LocationToNorth != null);
            btnSouth.Enabled = (location.LocationToSouth != null);
            btnEast.Enabled = (location.LocationToEast != null);
            btnWest.Enabled = (location.LocationToWest != null);

            rtbLocations.Clear();
            rtbLocations.Text = "Current Location: " + Environment.NewLine;
            rtbLocations.Text += "----------------------------------------" + Environment.NewLine;
            rtbLocations.Text += "Name: " + location.Name + Environment.NewLine;
            rtbLocations.Text += "Description: " + location.Description + Environment.NewLine;

            if (location.QuestAvailable != null)
            {
                rtbLocations.Text += "Quest: " + location.QuestAvailable.Name + Environment.NewLine;
                if (location.QuestAvailable.Completed == true)
                {
                    rtbLocations.Text += "Status: Completed" + Environment.NewLine;
                }
                else
                {
                    rtbLocations.Text += "Status: Not Completed" + Environment.NewLine;
                    rtbLocations.Text += "Items Required for this Quest:" + Environment.NewLine;
                    foreach (var entry in location.QuestAvailable.RequiredItemsToComplete)
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


        private void UpdateWeaponsItemsInUI()
        {
            List<Weapon> weapons = _player.InventoryItems
                .Where(inventoryItem => World.ItemByID(inventoryItem.Key) is Weapon && inventoryItem.Value > 0)
                .Select(inventoryItem => (Weapon)World.ItemByID(inventoryItem.Key))
                .ToList();

            if (weapons.Count > 0)
            {
                cboWeapons.Visible = true;
                btnUseWeapon.Visible = true;

                // Remove Event before binding to avoid calling it when binding weapons
                cboWeapons.SelectedIndexChanged -= cboWeapons_SelectedIndexChanged;
                cboWeapons.DataSource = weapons;
                cboWeapons.SelectedIndexChanged += cboWeapons_SelectedIndexChanged;

                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";

                if(_player.CurrentWeaponID > 0)
                {
                    cboWeapons.SelectedIndex = weapons.FindIndex(weapon => weapon.ID == _player.CurrentWeaponID);
                }


            }
            else
            {
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
        }

        private void UpdatePotionsItemsInUI()
        {
            List<HealingPotion> potions = _player.InventoryItems
                .Where(inventoryItem => World.ItemByID(inventoryItem.Key) is HealingPotion && inventoryItem.Value > 0)
                .Select(inventoryItem => (HealingPotion)World.ItemByID(inventoryItem.Key))
                .ToList();

            if (potions.Count > 0)
            {

                cboPotions.Visible = true;
                btnUsePotion.Visible = true;

                cboPotions.DataSource = potions;

                cboPotions.DisplayMember = "Name";
                cboPotions.ValueMember = "ID";
            }
            else
            {
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
            }
        }



       

        private void RestartGame()
        {
            _player = GameSavingAndLoading.LoadInitialState(_player.Name);
            UpdateUI();
        }

        private void CloseGame()
        {
            GameSavingAndLoading.SaveCurrentState(_player);
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
                        if(MessageBox.Show("You are DEAD!! Restart?", "Game Over", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            RestartGame();
                        }
                        else
                        {
                            Application.Exit();
                        }
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

        private void btnSaveGame_Click(object sender, EventArgs e)
        {
           GameSavingAndLoading.SaveCurrentState(_player);
        }

        private void btnLoadGame_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = "PlayersData",
                Title = "Browse Players Data",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "json",
                Filter = "json files (*.json)|*.json",
                FilterIndex = 2,
                RestoreDirectory = true,
                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _player = GameSavingAndLoading.LoadGame(openFileDialog1.FileName);
                UpdateUI();
            }
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void SuperAdventure_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseGame();
        }

        private void cboWeapons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboWeapons.SelectedIndex >= 0)
                _player.CurrentWeaponID = ((Weapon)cboWeapons.SelectedItem).ID;
        }

        private void btnChangeUsername_Click(object sender, EventArgs e)
        {
            tbUsername.ReadOnly = false;
            btnSaveUsername.Visible = true;
            btnChangeUsername.Visible = false;
        }

        private void btnSaveUsername_Click(object sender, EventArgs e)
        {
            if (tbUsername.Text == "")
                MessageBox.Show("Please, make sure you entered a name");
            else
            {
                tbUsername.Text = tbUsername.Text.Trim();
                tbUsername.ReadOnly = true;
                btnChangeUsername.Visible = true;
                btnSaveUsername.Visible = false;
                _player.Name = tbUsername.Text;
            }
        }
    }
}

using System;
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

        public SuperAdventure()
        {
            InitializeComponent();
            InitiateMap();
            _player = new Player(iD: 0, name: "Ahmed", maxHitPoints: 10, currentHitPoints: 10, gold: 20, experiencePoints: 0, level: 1);
            _player.InventoryItems.Add(4, 3); // Adding 3 Snake Fangs for Testing.

            lblPlayerName.Text = _player.Name;
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblLevel.Text = _player.Level.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));

        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToNorth);
        }
        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToSouth);
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToEast);
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToWest);
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

        private void MoveTo(Location location)
        {
            int MovementResult = PlayerMovement.MoveTo(_player, location);

            // Success Movement
            if (MovementResult == 0)
            {
                UpdateCurrentLocation(location);
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
                UpdateCurrentLocation(location);
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
            rtbMessages.Text += _player.Name + " moved to a new location" + Environment.NewLine;
            UpdateLocationsRichTextBox();

            // Show - Hide Moving Buttons
            btnNorth.Enabled = (location.LocationToNorth != null);
            btnSouth.Enabled = (location.LocationToSouth != null);
            btnEast.Enabled = (location.LocationToEast != null);
            btnWest.Enabled = (location.LocationToWest != null);

            // Update MapEntityLocation Colors
            UpdatePlayerPositionImage(location);
        }

        private void UpdatePlayerPositionImage(Location location)
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

        private void UpdateLocationsRichTextBox()
        {
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

        private void rtbMessages_TextChanged(object sender, EventArgs e)
        {
            rtbMessages.SelectionStart = rtbLocations.Text.Length;
            rtbMessages.ScrollToCaret();
        }
    }
}

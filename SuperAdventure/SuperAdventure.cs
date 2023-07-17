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
        private Player _player;
        public SuperAdventure()
        {
            InitializeComponent();

            _player = new Player(iD: 0, name: "Ahmed", maxHitPoints: 10, currentHitPoints: 10, gold: 20, experiencePoints: 0, level: 1);

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



        private void MoveTo(Location location)
        {
            int MovementResult = PlayerMovement.MoveTo(_player, location);

            // Success Movement
            if (MovementResult == 0)
            {
                UpdateCurrentLocation(location);
            }
            else if(MovementResult == 1)
            {
                rtbMessages.Text += "There is no location there" + Environment.NewLine;
            }
            else if (MovementResult == 2)
            {
                rtbMessages.Text += "Sorry, item " + location.ItemRequiredToEnter.Name + " is required to enter this location" + Environment.NewLine;
            }
            else if (MovementResult == 3)
            {
                rtbMessages.Text += "Sorry, quest " + location.QuestAvailable.Name + " is required to enter this location" + Environment.NewLine;
            }

        }

        private void UpdateCurrentLocation(Location location)
        {
            _player.CurrentLocation = location;
            rtbMessages.Text += _player.Name + " moved to a new location" + Environment.NewLine;
            UpdateLocationsRichTextBox();

            // Show - Hide Moving Buttons
            btnNorth.Visible = (location.LocationToNorth != null);
            btnSouth.Visible = (location.LocationToSouth != null);
            btnEast.Visible = (location.LocationToEast != null);
            btnWest.Visible = (location.LocationToWest != null);
        }

        private void UpdateLocationsRichTextBox()
        {
            rtbLocations.Clear();
            rtbLocations.Text = "Current Location: " + Environment.NewLine;
            rtbLocations.Text += "----------------------------------------" + Environment.NewLine;
            rtbLocations.Text += "Name: " + _player.CurrentLocation.Name + Environment.NewLine;
            rtbLocations.Text += "Description: " + _player.CurrentLocation.Description + Environment.NewLine;
            
            if(_player.CurrentLocation.QuestAvailable != null)
            {
                rtbLocations.Text += "Quest: " + _player.CurrentLocation.QuestAvailable.Name + Environment.NewLine;
                if(_player.CurrentLocation.QuestAvailable.Completed == true)
                {
                    rtbLocations.Text += "Status: Completed" + Environment.NewLine;
                }
                else
                {
                    rtbLocations.Text += "Status: Not Completed" + Environment.NewLine;
                    rtbLocations.Text += "Items Required for this Quest:" + Environment.NewLine;
                    foreach(var entry in _player.CurrentLocation.QuestAvailable.RequiredItemsToComplete)
                    {
                        rtbLocations.Text += entry.Key.Name + ": " + entry.Value + Environment.NewLine;
                    }
                }
            }
            else
            {
                rtbLocations.Text += "Quest: No Quests Available" + Environment.NewLine;
            }

            rtbLocations.ScrollToCaret();
        }

        private void rtbMessages_TextChanged(object sender, EventArgs e)
        {
            rtbMessages.SelectionStart = rtbLocations.Text.Length;
            rtbMessages.ScrollToCaret();
        }
    }
}

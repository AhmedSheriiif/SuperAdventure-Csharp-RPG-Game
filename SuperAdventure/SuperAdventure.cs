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

namespace SuperAdventure
{
    public partial class SuperAdventure : Form
    {
        private Player _player;
        private Location _location;
        public SuperAdventure()
        {
            InitializeComponent();

            _player = new Player(iD: "0", name: "Ahmed", maxHitPoints: 100, currentHitPoints: 100, gold: 500, experiencePoints: 0, level: 1);
            _location = new Location(iD: "0", name: "Home", description: "This is your house");

            lblPlayerName.Text = _player.Name;
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblLevel.Text = _player.Level.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
        }
    }
}

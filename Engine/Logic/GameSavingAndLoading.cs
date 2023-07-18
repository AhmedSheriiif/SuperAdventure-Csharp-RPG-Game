using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Engine.Logic
{
    public static class GameSavingAndLoading
    {

        public static void SaveInitialState(string name)
        {
            Player _initialPlayer = new Player();
            _initialPlayer.Name = name;
            _initialPlayer.ID = 0;
            _initialPlayer.Gold = 50;
            _initialPlayer.AddExperiencePoints(0);
            _initialPlayer.CurrentHitPoints = _initialPlayer.MaxHitPoints;
            _initialPlayer.CurrentLocationID = World.LOCATION_ID_HOME;
            _initialPlayer.AddItem(World.ITEM_ID_SNAKE_FANG);
            _initialPlayer.AddItem(World.ITEM_ID_SNAKE_FANG);
            _initialPlayer.AddItem(World.ITEM_ID_SNAKE_FANG);
            _initialPlayer.AddItem(World.ITEM_ID_CLUB);
            _initialPlayer.AddItem(World.ITEM_ID_CLUB);

            string jsonFileName = name + ".json";
            Directory.CreateDirectory("PlayersData");
            string jsonData = JsonConvert.SerializeObject(_initialPlayer, Formatting.Indented);
            File.WriteAllText(Path.Combine("PlayersData", jsonFileName), jsonData);

            Console.WriteLine("New Player: " + name + " added, Enjoy the Game.");

        }

        public static Player LoadInitialState(string name)
        {
            try
            {
                Player player = new Player();

                // Read file
                string jsonFileName = "newPlayer" + ".json";
                string data = File.ReadAllText(Path.Combine("PlayersData", jsonFileName));
                player = JsonConvert.DeserializeObject<Player>(data);
                player.Name = name;

                return player;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static void SaveCurrentState(Player player)
        {
            string jsonFileName = player.Name + ".json";
            Directory.CreateDirectory("PlayersData");
            string jsonData = JsonConvert.SerializeObject(player, Formatting.Indented);
            File.WriteAllText(Path.Combine("PlayersData", jsonFileName), jsonData);
        }

        public static Player LoadGame(string filePath)
        {
            try
            {
                Player player = new Player();

                // Read file
                string data = File.ReadAllText(filePath);
                player = JsonConvert.DeserializeObject<Player>(data);

                return player;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}

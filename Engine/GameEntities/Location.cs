using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Location : IEntity
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public Item ItemRequiredToEnter { get; set; }
        public Monster MonsterLivingHere { get; set; }
        public Quest QuestAvailable { get; set; }

        public Location LocationToNorth { get; set; }
        public Location LocationToEast { get; set; }
        public Location LocationToSouth { get; set; }
        public Location LocationToWest { get; set; }


        public Location() { }

        public Location(int iD, string name, string description, Item itemRequired = null, Monster monsterLivingHere = null, Quest questAvailable = null)
        {
            ID = iD;
            Name = name;
            Description = description;
            ItemRequiredToEnter = itemRequired;
            MonsterLivingHere = monsterLivingHere;
            QuestAvailable = questAvailable;

        }
    }
}

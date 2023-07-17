using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Interfaces
{
    public class Item : IEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string NamePlural { get; set; }
        public string Description { get; set; }

        public Item() { }

        public Item(int iD, string name, string namePlural = null, string description = null)
        {
            ID = iD;
            Name = name;
            NamePlural = namePlural;
            Description = description;
        }
    }
}

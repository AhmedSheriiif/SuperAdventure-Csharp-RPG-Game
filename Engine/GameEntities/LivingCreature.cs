using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class LivingCreature : IEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int CurrentHitPoints { get; set; }
        public int MaxHitPoints { get; set; }

        public int LastDamageTaken { get; set; }

        public bool Alive { get; set; }

        public LivingCreature() { }
        public LivingCreature(int iD, string name, int currentHitPoints, int maxHitPoints)
        {
            ID = iD;
            Name = name;
            CurrentHitPoints = currentHitPoints;
            MaxHitPoints = maxHitPoints;
            LastDamageTaken = 0;
            Alive = true;
        }
    }
}

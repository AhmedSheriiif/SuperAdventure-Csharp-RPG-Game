﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class LivingCreature : IEntity
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public int CurrentHitPoints { get; }
        public int MaxHitPoints { get; }

        public LivingCreature() { }
        public LivingCreature(string iD, string name, int currentHitPoints, int maxHitPoints)
        {
            ID = iD;
            Name = name;
            CurrentHitPoints = currentHitPoints;
            MaxHitPoints = maxHitPoints;
        }
    }
}
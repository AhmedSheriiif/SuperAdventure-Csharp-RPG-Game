using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class HealingPotion : Item
    {
        public int AmountToHeal { get; set; }

        public HealingPotion() { }

        public HealingPotion(string iD, string name, string namePlural, string description, int amountToHeal) : base(iD, name, namePlural, description)
        {
            AmountToHeal = amountToHeal;
        }

    }
}

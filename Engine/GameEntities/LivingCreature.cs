using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class LivingCreature : IEntity, INotifyPropertyChanged
    {

        private int _currentHitPoints;
        private int _maxHitPoints;
        private string _name;
        public int ID { get; set; }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }


        public int CurrentHitPoints
        {
            get { return _currentHitPoints; }
            set
            {
                _currentHitPoints = value;
                OnPropertyChanged("CurrentHitPoints");
            }
        }

        public int MaxHitPoints 
        {
            get { return _maxHitPoints; }
            set
            {
                _maxHitPoints = value;
                OnPropertyChanged("MaxHitPoints");
            }
        }

        public int LastDamageTaken { get; set; }

        public bool Alive { get; set; }

        public LivingCreature() { }
        public LivingCreature(int iD, string name, int currentHitPoints, int maxHitPoints)
        {
            ID = iD;
            Name = name;
            _currentHitPoints = currentHitPoints;
            MaxHitPoints = maxHitPoints;
            LastDamageTaken = 0;
            Alive = true;
        }

        // INotifyPropertyChanged Interface 
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

       
    }
}

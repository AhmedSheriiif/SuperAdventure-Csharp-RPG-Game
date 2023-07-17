using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class MapLocationEntity
    {
        public int LocationX;
        public int LocationY;
        public int LocationID;

        public MapLocationEntity() { }

        public MapLocationEntity(int locationX, int locationY, int locationID)
        {
            LocationX = locationX;
            LocationY = locationY;
            LocationID = locationID;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioio.Models
{
    public class Resource
    {
        public int Type;
				public List<Position> Distribution; // line for forest, gold,stone
				public Position Position; // point for food, alone wood
    }
}

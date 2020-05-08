using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioio.Models
{
    public class Building
    {
        public int Type;
				public string Score;// {time:count}
				public List<Position> Distribution;// vị trí các đơn vị nhà
    }
}

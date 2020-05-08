using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioio.Models
{
    public class Power
    {
        public string Citezen;
				public string Army;
				public string Building;
				public string Fund;
				public string Point;
				// {time {a} minute}:{tăng giảm};{next time {a} minute}:{tăng giảm}
				// Point cũng theo công thức này, và là tổng hợp cuối mỗi phút:
				// {time {a} minute}:{Point};{time {a} minute}:{Point}
    }
}

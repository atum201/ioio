using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ioio.Common;

namespace ioio.Models
{
    public class PersonalSetting
    {
        public int? MouseSpeed;
        public int? Voice;
        public int? Light;

        public int? AoeSpeed;
        public int? ScreenSize;
        public int? Music;
        public int? Sound;
        public int? Scroll;
        public PersonalSetting(int speed = 2, int screenSize = 2, int sound = 0, int mucic = 0, int scroll = 50)
        {
            MouseSpeed = WindowUtil.GetMouseSpeed();
            Voice = WindowUtil.GetVolume();
            Light = WindowUtil.GetLight();


            AoeSpeed = speed;
            ScreenSize = screenSize;
            Music = mucic;
            Sound = sound;
            Scroll = scroll;
        }
    }
}

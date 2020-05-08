using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ioio.Common;

namespace ioio.Models
{
    public class AoeSetting
    {
        public int? MapSize;
        public int? MapType;
        public int? DifficultlyLevel;
        public int? VictoryCondition;
        public int? StartingAge;
        public int? PathFinding;
        public bool EnableCheating;
        public int? Resources;
        public int? PopulationLimit;
        public bool FixedPosition;
        public bool FullTechTree;
        public bool RevealMap;

        public bool DeathMatch;
        public int? Empire;
        public int? TheLoai;

        public AoeSetting() { }
        public AoeSetting(int setting, bool deathMatch = false, int empire = (int)Cat_Empire.Random)
        {
            if (setting.Equals((int)Cat_TheLoai.Solo))
            {
                MapSize = deathMatch ? (int)Cat_MapSize.Small : (int)Cat_MapSize.Large;
                TheLoai = (int)Cat_TheLoai.Solo;
            }
            if (setting.Equals((int)Cat_TheLoai.Aoe22))
            {
                MapSize = deathMatch ? (int)Cat_MapSize.Medium : (int)Cat_MapSize.Large;
                TheLoai = (int)Cat_TheLoai.Aoe22;
            }
            if (setting.Equals((int)Cat_TheLoai.Aoe33))
            {
                MapSize = deathMatch ? (int)Cat_MapSize.Large : (int)Cat_MapSize.Huge;
                TheLoai = (int)Cat_TheLoai.Aoe33;
            }
            if (setting.Equals((int)Cat_TheLoai.Aoe44))
            {
                MapSize = deathMatch ? (int)Cat_MapSize.Huge : (int)Cat_MapSize.Gigiantic;
                TheLoai = (int)Cat_TheLoai.Aoe44;
            }

            MapType = (int)Cat_MapType.HillCountry;
            DifficultlyLevel = (int)Cat_DifficultlyLevel.Hardest;
            VictoryCondition = (int)Cat_VictoryCondition.Conquest;
            StartingAge = deathMatch ? (int)Cat_StartingAge.IronAge : (int)Cat_StartingAge.Default;
            PathFinding = (int)Cat_PathFinding.High;
            EnableCheating = false;
            Resources = (int)Cat_Resources.Default;
            PopulationLimit = (int)Cat_Population.Limit200;
            FixedPosition = true;
            FullTechTree = false;
            RevealMap = deathMatch ? true : false;

            
            DeathMatch = deathMatch;
            Empire = empire;
        }
        public static AoeSetting CombineSetting(List<AoeSetting> listSetting)
        {
            return new AoeSetting();
        }
    }
}

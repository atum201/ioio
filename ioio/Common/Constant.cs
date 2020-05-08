using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioio.Common
{
    class Constant
    {
        #region Text, String
        public static List<String> TheLoai = new List<string> { "Solo", "2 vs 2", "3 vs 3", "4 vs 4" };
        public static List<String> MapSize = new List<string> { "Tiny", "Small", "Medium", "Large", "Huge", "Gigiantic" };
        public static List<String> MapType = new List<string> { "Small Islands", "Large Islands", "Coastal", "Inland", "Highland", "Continental", "Mediterranean", "Hill Country", "Narrows" };
        public static List<String> DifficultlyLevel = new List<string> { "Easiest", "Easy", "Moderate", "Hard", "Hardest" };
        public static List<String> VictoryCondition = new List<string> { "Standard", "Conquest", "Time Limit", "Score" };
        public static List<String> StartingAge = new List<string> { "Default", "Nomad", "Stone Age", "Tool Age", "Bronze Age", "Iron Age" };
        public static List<String> PathFinding = new List<string> { "Default", "Medium", "High" };
        public static List<String> Resources = new List<string> { "Default", "Medium", "High" };
        public static List<String> Population = new List<string> { "25", "50", "75", "100", "125", "150", "175", "200" };
        public static List<String> Empire = new List<string> { "Egyptian","Greek","Babylonian","Assyrian", "Minoan", "Hittite","Phoenician",  "Summerian", "Persian",
            "Sang", "Yamoto", "Choson","Roman","Carthaginian","Palmyran","Macedonian", "Random" };

        public static List<String> AoeSpeed = new List<string> { "Normal", "Fast", "Very Fast" };
        public static List<String> ScreenSize = new List<string> { "640x480", "800x600", "1024x768" };
        public static List<String> AoeStatus = new List<string> { "None", "Aoe Running" };
        public static String btnPrimary = "btnPrimary";
        public static String Solo = "Solo";
        #endregion
        #region Offset

        public static List<uint> AddressBase = new List<uint>{
            0x3C4B18,
            0x585e88,
            };

        public static List<uint[]> Offset = new List<uint[]>{
            new uint[]{ 0x51C, 0x114, 0x40, 0x4, 0x50,  0x0},
            new uint[]{ 0x51C, 0x114, 0x40, 0x4, 0x224, 0x0, 0x334 }, // click doi 3
            new uint[]{ 0x51C, 0x114, 0x5C, 0x4, 0x10}, //Time of Match
            new uint[]{0x4,0x8,0xC,0x10,0x14,0x18,0x1c,0x20}
        };
        public static Dictionary<int, string[]> LabelStartFood = new Dictionary<int, string[]>()
        {
            { 0, new string[]{"Food" } },
            { 1, new string[]{"Wood"} },
            { 2, new string[]{"Stone" } },
            { 3, new string[]{"Gold" }},
            { 4, new string[]{"TL  " }},
            { 6, new string[]{"Age " }},
            { 9, new string[]{"T100" }},
            { 11, new string[]{"TL  " }},
            { 20, new string[]{"kill"}},
            { 19, new string[]{"Dan" }},
            { 21, new string[]{"Tech"}},
            { 22, new string[]{"MoMa" }},
            { 30, new string[]{"501 " }},
            { 32, new string[]{"Tmax" }},
            { 37, new string[]{"daht" }},
            { 40, new string[]{"liht" }},
            { 43, new string[]{"bdef" }},
            { 49, new string[]{"gget" }},
            { 51, new string[]{"BEBu" }},
            { 52, new string[]{"BpBu" } }
        };

        #endregion
    }
}

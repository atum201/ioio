using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioio.Category

{
    public class Address
    {

        public static Dictionary<string, UIntPtr> AOE = new Dictionary<string, UIntPtr>()
        {

        };
        public static List<UIntPtr> Base = new List<UIntPtr>() { };
        public static List<UIntPtr> EwE = new List<UIntPtr>();// Empire socket start with food.
        public static UIntPtr Time;

        public static void ReScoutAddress()
        {

        }
    }
    #region
    public enum TheLoai { Solo, Aoe22, Aoe33, Aoe44 }
    //public enum NoiDung { D3KT, DeathMatch }
    public enum MapSize { Tiny, Small, Medium, Large, Huge, Gigiantic }
    public enum MapType { SmallIslands, LargeIslands, Coastal, Inland, Highland, Continental, Mediterranean, HillCountry, Narrows }
    public enum DifficultlyLevel { Easiest, Easy, Moderate, Hard, Hardest }
    public enum VictoryCondition { Standard, Conquest, TimeLimit, Score }
    public enum StartingAge { Default, Nomad, StoneAge, ToolAge, BronzeAge, IronAge }
    public enum PathFinding { Default, Medium, High }
    public enum Resources { Default, Medium, High }
    public enum Population { Limit25, Limit50, Limit75, Limit100, Limit125, Limit150, Limit175, Limit200 }
    public enum Empire { Egyptian, Greek, Babylonian, Assyrian, Minoan, Hittite, Phoenician, Summerian, Persian, Sang, Yamoto, Choson, Roman, Carthaginian, Palmyran, Macedonian, Random }
    #endregion

    #region Game Setting
    public enum Speed { Normal, Fast, VeryFast }
    public enum ScreenSize { SS_640_480, SS_800_600, SS_1024_768 }

    #endregion

    #region Match
    public enum AoeStatus
    {
        None,
        Launcher,
        CreateGame,
        JoinGame,
        Initial,
        StartGame,
        //EarlyGame,
        //MidGame,
        //LateGame,
        Endgame, // F10 Enter.
        QuitGame // Endprocess, AOE shutdown ...
    }
    public enum AddressBase
    {
        Empire,

    }
    public enum MatchStatus
    {
        None,
        Initial,
        Start,
        Pause,
        Resume,
        End // Victory or Defeat, still in game to watching, timeline, review map.
    }
    public enum MapStatus
    {
        Loading,
        Start,
        Early, // 0-8
        Mid, // 8 - 20
        Late // 20 to end
    }
    public enum SocketType
    {
        Float, Byte, Byte2, Byte4, String
    }
    public enum Technologies
    {
        Doi2, Doi3, Doi4, ChatGo1, ChatGo2, ChatGo3, Da1, Da2, Vang1, Vang2, BanhXe, Ruong1, Ruong2, Ruong3, Cong1, Cong2, Cong3,
        GiapBB1, GiapBB2, GiapBB3, GiapL1, GiapL2, GiapL3, Khien1, Khien2, Khien3
    }
    public enum Events
    {
        BG, QuaDep, QuaXau, Huou5, Huou6, Huou7, Voi, Voi2, Voi3, Voi4, GapSuTu, ChetDan, BS, BB, BA, BL, BM, Doi2, Doi3, Doi4, BY, BatQuan, BiBatQuan, Defeat, Resign, VicTory
    }
    public enum Socket
    {
        Doi2, Doi3, Doi4, ClickDoi2, ClickDoi3, ClickDoi4, Food, Wood, Gold, Stone, Citizen, Army, Building
    }
    #endregion

    #region Address, Offset
    public enum Address1
    {
        Match = 0x3C4B18, // Dia chi lien quan den khoi tao cua Match. Tu dia chi nay de truy van toi cac dia chi khac cua Match
        Static = 0x585e88, // Dia chi tinh tro toi danh sach Empire
        Setting = 0x20930 // +4 de ra cac Address Setting.
    }
    public enum Offset
    {
        StartFood,
        Age2,
        Age3,
        Age4

    }
    public enum Offset_StartFood
    {
        Food,
        Wood = 0x4,
        Stone = 0x8,
        Gold = 0xC,
        TimeLineE = 0x10,

    }
    public enum Scout
    {
        Setting, // Setting1, Setting2 ...
        Launch
    }
    #endregion
}

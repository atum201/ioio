using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioio.Models
{
    public class CheatTable
    {
        public CheatEntries CheatEntries;
        public UserdefinedSymbols UserdefinedSymbols;
        public LuaScript LuaScript;
    }

    public class CheatEntries
    {
        public CheatEntry CheatEntry;
    }

    public class CheatEntry
    {
        public string ID;
        public string Description;
        public string VariableType;
        public string Address;
        public List<Offsets> Offsets;
    }
    public class Offsets
    {
        public string Offset;
    }
    public class UserdefinedSymbols
    {

    }
    public class LuaScript
    {

    }
}

using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ioio.Common;

namespace ioio.Models
{
    internal class Store : ViewModelBase
    {
        #region Variables
        public List<Match> match = new List<Match>();
        #endregion

        #region Properties
        public List<Match> Matchs
        {
            get => match;
            set { Set(ref match, value); }
        }
        #endregion

        #region Method
        public Match GetMatch(string id)
        {
            Match result = match.Find(x => x.ID == id);
            if (result == null)
                result = AWS_Util.Fetch(string.Empty) as Match;

            return result;
        }
        #endregion
    }
}

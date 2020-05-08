using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioio.Models
{
    public class Empire : ViewModelBase
    {
        #region Variables
        private string _king;
        private int _team;// team [1..4]
        private int _flag;// [1..8]
        private int _kind;
        private int _victory;// -1,0,1

        private List<History> _histories;
        private List<Technology> _technologies;

        private List<Position> _towns;
        private List<Citizen> _citizens;
        private List<Army> _armies;
        private List<Building> _buildings;

        private Power _power;

        private string _history;// {type}:{time};
        private string _announcement;

        #endregion

        #region Properties

        public string King
        {
            get => _king;
            set { Set(ref _king, value); }
        }

        #endregion

        public Empire(string empire)
        {

        }
        public Empire(int empire, string king, int flag, int team)
        {
            _kind = empire;
            _king = king;
            _flag = flag;
            _team = team;

            _histories = new List<History>();
            _technologies = new List<Technology>();

            _towns = new List<Position>();
            _citizens = new List<Citizen>();
            _armies = new List<Army>();
            _buildings = new List<Building>();

            _power = new Power();
        }

        #region Method

        public string Save()
        {
            // Serializable Empire to String;
            return _history;
        }
        public void Update(string info)
        {
            // Update empire when get announcement;
        }
        public string Announcement()
        {
            // for public empire's info for viewer
            return _announcement;
        }
        #endregion
    }
}

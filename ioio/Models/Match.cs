using GalaSoft.MvvmLight;
using ioio.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioio.Models
{
    public class Match : ViewModelBase
    {
        #region Variables
        private string _code;//{Created}:{Empires}:{Kings}:{Setting} Dùng để xác định cùng 1 Match
        private string _slogan;
        private int _status;
        private List<Empire> _empires;
        private string id;
        
        private Map _map; 
        private string victory = "Victory";
        private Models.Setting _setting;

        
        private float _time;
        private DateTime _created;

        private List<string> info;

        private string match;

        private string _announcement;
        #endregion

        #region Properties
        public string ID
        {
            get => id;
            set { Set(ref id, value); }
        }
        public string Victory
        {
            get => victory;
            set { Set(ref victory, value); }
        }
        public List<Empire> Empires
        {
            get => _empires;
            set { Set(ref _empires, value); }
        }
        public List<string> Info
        {
            get => info;
            set { Set(ref info, value); }
        }
        public float Time
        {
            get => _time;
            set { Set(ref _time, value); }
        }
        public int Status
        {
            get => _status;
            set { Set(ref _status, value); }
        }

        public string Test
        {
            get => "TEst";
        }
        #endregion
        public Match()
        {
            _status = (int)Cat_MatchStatus.Initial;
            _map = new Map();
            _empires = new List<Empire>();
            _setting = new Setting();
            _time = 0;
            _created = DateTime.Now;
            info = new List<string>();
            info.Add("Something5");
            info.Add("Something4");
            info.Add("Something3");
            info.Add("Something2");
        }
        public Match(string data)
        {
            var match = JObject.Parse(data);
            _code = (string)match["Code"];
            _slogan = (string)match["Match"];
            _status = (int)match["Status"];
            //...
        }

        #region Method

        public string Save()
        {
            string match = string.Empty;
            string empire = string.Join(";", _empires.Select(e => e.Save()).ToList());

            _empires.ForEach(e =>
            {
                empire += e.Save();
            });
            return match;
        }
        public void Update(string data) 
        {
            var m = JObject.Parse(data);
        }
        public string Announcement()
        {
            // for public empire's info for viewer
            return _announcement;
        }
        #endregion
    }
}

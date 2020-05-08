using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using ioio.Common;
using ioio.Models;
using System.Windows;
using GalaSoft.MvvmLight.Command;

namespace ioio.Views
{
    internal class Aoe : ViewModelBase
    {
        private AoeSetting _setting = null;// ioio.Common.Store.All.AoeSetting;
        private PersonalSetting _personal = null;// ioio.Common.Store.All.PersonalSetting;

        public ICommand SetTheLoaiCommand { get; }
        public ICommand SetEmpireCommand { get; }
        public ICommand SetDeathMatchCommand { get; }
        public ICommand SetAoeSpeed { get; }
        public Aoe()
        {

            SetTheLoaiCommand = new RelayCommand<String>(ChangeTheLoai);
            SetDeathMatchCommand = new RelayCommand(ChangeDeathMatch);
            SetEmpireCommand = new RelayCommand<String>(ChangeEmpire);
        }
        #region Set|Get Property
        public string Test
        {
            get => ioio.Common.Store.All.BackgroundReturn;
            set {  ioio.Common.Store.All.BackgroundReturn = value; }
        }
        public bool DeathMatch
        {
            get => _setting.DeathMatch;
            set { Set(ref _setting.DeathMatch, value); ioio.Common.Store.All.AoeSetting = _setting; }
        }
        public bool EnableCheating
        {
            get => _setting.EnableCheating;
            set { Set(ref _setting.EnableCheating, value); ioio.Common.Store.All.AoeSetting = _setting; }
        }
        public bool FixedPosition
        {
            get => _setting.FixedPosition;
            set { Set(ref _setting.FixedPosition, value); ioio.Common.Store.All.AoeSetting = _setting; }
        }
        public bool FullTechTree
        {
            get => _setting.FullTechTree;
            set { Set(ref _setting.FullTechTree, value); ioio.Common.Store.All.AoeSetting = _setting; }
        }
        public bool RevealMap
        {
            get => _setting.RevealMap;
            set { Set(ref _setting.RevealMap, value); ioio.Common.Store.All.AoeSetting = _setting; }
        }
        public string SelectTheLoai
        {
            get => Constant.TheLoai[_setting.TheLoai.Value];
            set { Set(ref _setting.TheLoai, Constant.TheLoai.IndexOf(value)); ioio.Common.Store.All.AoeSetting = _setting; }
        }
        public List<String> TheLoai
        {
            get => Constant.TheLoai;
        }
        public string SelectMapSize
        {
            get => Constant.MapSize[_setting.MapSize.Value];
            set { Set(ref _setting.MapSize, Constant.MapSize.IndexOf(value));  }
        }
        public List<String> MapSize
        {
            get => Constant.MapSize;
        }

        public string SelectMapType
        {
            get => Constant.MapType[_setting.MapType.Value];
            set { Set(ref _setting.MapType, Constant.MapType.IndexOf(value)); ioio.Common.Store.All.AoeSetting = _setting; }
        }
        public List<String> MapType
        {
            get => Constant.MapType;
        }
        
        public string SelectDifficultlyLevel
        {
            get => Constant.DifficultlyLevel[_setting.DifficultlyLevel.Value];
            set { Set(ref _setting.DifficultlyLevel, Constant.DifficultlyLevel.IndexOf(value)); ioio.Common.Store.All.AoeSetting = _setting; }
        }
        public List<String> DifficultlyLevel
        {
            get => Constant.DifficultlyLevel;
        }

        public string SelectVictoryCondition
        {
            get => Constant.VictoryCondition[_setting.VictoryCondition.Value];
            set { Set(ref _setting.VictoryCondition, Constant.VictoryCondition.IndexOf(value)); ioio.Common.Store.All.AoeSetting = _setting; }
        }
        public List<String> VictoryCondition
        {
            get => Constant.VictoryCondition;
        }

        public string SelectStartingAge
        {
            get => Constant.StartingAge[_setting.StartingAge.Value];
            set { Set(ref _setting.StartingAge, Constant.StartingAge.IndexOf(value)); ioio.Common.Store.All.AoeSetting = _setting; }
        }
        public List<String> StartingAge
        {
            get => Constant.StartingAge;
        }
        public string SelectPathFinding
        {
            get => Constant.PathFinding[_setting.PathFinding.Value];
            set { Set(ref _setting.PathFinding, Constant.PathFinding.IndexOf(value)); ioio.Common.Store.All.AoeSetting = _setting; }
        }
        public List<String> PathFinding
        {
            get => Constant.PathFinding;
        }
        public string SelectResources
        {
            get => Constant.Resources[_setting.Resources.Value];
            set { Set(ref _setting.Resources, Constant.Resources.IndexOf(value)); ioio.Common.Store.All.AoeSetting = _setting; }
        }
        public List<String> Resources
        {
            get => Constant.Resources;
        }
        public string SelectPopulation
        {
            get => Constant.Population[_setting.PopulationLimit.Value];
            set { Set(ref _setting.PopulationLimit, Constant.Population.IndexOf(value)); ioio.Common.Store.All.AoeSetting = _setting; }
        }
        public List<String> Population
        {
            get => Constant.Population;
        }
        public string SelectEmpire
        {
            get => Constant.Empire[_setting.Empire.Value];
            set { Set(ref _setting.Empire, Constant.Empire.IndexOf(value)); ioio.Common.Store.All.AoeSetting = _setting; }
        }
        public List<String> Empire
        {
            get => Constant.Empire;
        }
        #endregion

        public int MouseSpeed
        {
            get => _personal.MouseSpeed.HasValue ? _personal.MouseSpeed.Value : 10;
            set { }
        }

        public String GameSpeed
        {
            get => Constant.AoeSpeed[_personal.AoeSpeed.Value];
            set { Set(ref _personal.AoeSpeed, Constant.AoeSpeed.IndexOf(value)); ioio.Common.Store.All.PersonalSetting = _personal; }
        }

        public String ScreenSize
        {
            get => Constant.ScreenSize[_personal.ScreenSize.Value];
            set { Set(ref _personal.ScreenSize, Constant.ScreenSize.IndexOf(value)); ioio.Common.Store.All.PersonalSetting = _personal; }
        }
        

        #region Method
        private void ChangeTheLoai(String setting)
        {
            SelectTheLoai = setting;
            if (setting.Equals(Constant.TheLoai[(int)Cat_TheLoai.Solo]))
            {
                SelectMapSize = Constant.MapSize[DeathMatch ? (int)Cat_MapSize.Small : (int)Cat_MapSize.Large];
            }
            if (setting.Equals(Constant.TheLoai[(int)Cat_TheLoai.Aoe22]))
            {
                SelectMapSize = Constant.MapSize[DeathMatch ? (int)Cat_MapSize.Medium : (int)Cat_MapSize.Large];
            }
            if (setting.Equals(Constant.TheLoai[(int)Cat_TheLoai.Aoe33]))
            {
                SelectMapSize = Constant.MapSize[DeathMatch ? (int)Cat_MapSize.Large : (int)Cat_MapSize.Huge];
            }
            if (setting.Equals(Constant.TheLoai[(int)Cat_TheLoai.Aoe44]))
            {
                SelectMapSize = Constant.MapSize[DeathMatch ? (int)Cat_MapSize.Huge : (int)Cat_MapSize.Gigiantic];
            }
        }
        private void ChangeDeathMatch()
        {
            DeathMatch = DeathMatch == false;
            String setting = SelectTheLoai;
            if (setting.Equals(Constant.TheLoai[(int)Cat_TheLoai.Solo]))
            {
                SelectMapSize = Constant.MapSize[DeathMatch ? (int)Cat_MapSize.Small : (int)Cat_MapSize.Large];
            }
            if (setting.Equals(Constant.TheLoai[(int)Cat_TheLoai.Aoe22]))
            {
                SelectMapSize = Constant.MapSize[DeathMatch ? (int)Cat_MapSize.Medium : (int)Cat_MapSize.Large];
            }
            if (setting.Equals(Constant.TheLoai[(int)Cat_TheLoai.Aoe33]))
            {
                SelectMapSize = Constant.MapSize[DeathMatch ? (int)Cat_MapSize.Large : (int)Cat_MapSize.Huge];
            }
            if (setting.Equals(Constant.TheLoai[(int)Cat_TheLoai.Aoe44]))
            {
                SelectMapSize = Constant.MapSize[DeathMatch ? (int)Cat_MapSize.Huge : (int)Cat_MapSize.Gigiantic];
            }
            SelectStartingAge = Constant.StartingAge[DeathMatch ? (int)Cat_StartingAge.IronAge : (int)Cat_StartingAge.Default];
            RevealMap = DeathMatch;
        }
        private void ChangeEmpire(String empire)
        {
            SelectEmpire = empire;
        }
        private void ChangeAoeSpeed(String speed)
        {

        }
        #endregion
    }
}

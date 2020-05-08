using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ioio.Common;
using ioio.Views;
using ioio.Common.Util;

namespace ioio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Variables
        private Memory mem = new Memory();
        internal static MainApplication MainViewModel { get; set; }
        private static BackgroundWorker backgroundWorker;
        private int check = 0;
        #endregion
        #region Properties

        #endregion
        #region Events

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainViewModel = (MainApplication)FindResource("AppViewModel") ?? new MainApplication();
            MainViewModel.OpenHome.Execute(null);

            #region Thread AOE Background
            // start Thread fetch AOE
            backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            //Event creation.     

            //For the performing operation in the background.     

            backgroundWorker.DoWork += backgroundWorker_DoWork;

            //For the display of operation progress to UI.     

            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;

            //After the completation of operation.     

            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

            backgroundWorker.RunWorkerAsync("Press Enter in the next 5 seconds to Cancel operation:");

            Console.ReadLine();

            if (backgroundWorker.IsBusy)
            {
                Console.WriteLine("busysssssssssssssssssss");
                //backgroundWorker.CancelAsync();

                Console.ReadLine();

            }
            // start Thread fetch AOE
            #endregion
            return;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            //Store.Save();
            // end Thread fetch AOE

            // end Thread fetch AOE
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {

        }
        #endregion

        #region BackgroundWorker
        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Task for store AOE.

            bool launched = false;
            int c = -1;
            int c2 = -1;
            while (true)
            {
                switch (MainViewModel.Status)
                {
                    case (int)Category.AoeStatus.None:
                        if (c != (int)ioio.Category.AoeStatus.None)
                            Console.WriteLine("Aoe None");
                        MainViewModel.Status = Memory.IsGameAvailable("EMPIRESX") ? launched ? mem.IsStartGame() ? (int)Category.AoeStatus.Initial : (int)Category.AoeStatus.None : (int)Category.AoeStatus.Launcher : (int)Category.AoeStatus.None;
                        if (MainViewModel.Status == (int)Category.AoeStatus.None && !Memory.IsGameAvailable("EMPIRESX"))
                            launched = false;

                        c = (int)Category.AoeStatus.None;
                        break;
                    case (int)Category.AoeStatus.Launcher:
                        if (c != (int)Category.AoeStatus.Launcher)
                            Console.WriteLine("Aoe Laucher");
                        mem.OpenProcess("EMPIRESX");
                        launched = true;
                        //MainViewModel.Status = mem.IsCreateGame() ? (int)Category.AoeStatus.CreateGame : mem.IsJoinGame() ? (int)Category.AoeStatus.JoinGame : (int)Category.AoeStatus.Launcher;
                        MainViewModel.Status = mem.IsStartGame() ? (int)Category.AoeStatus.Initial : (int)Category.AoeStatus.None;
                        c = (int)Category.AoeStatus.Launcher;
                        break;
                    case (int)Category.AoeStatus.CreateGame:
                        if (c != (int)Category.AoeStatus.None)
                            Console.WriteLine("Aoe CreateGame");

                        MainViewModel.Status = mem.IsStartGame() ? (int)Category.AoeStatus.StartGame : MainViewModel.Status;
                        c = (int)Category.AoeStatus.CreateGame;
                        break;
                    case (int)Category.AoeStatus.JoinGame:
                        if (c != (int)Category.AoeStatus.JoinGame)
                            Console.WriteLine("Aoe JoinGame");

                        // Load Setting Game joined.

                        c = (int)Category.AoeStatus.JoinGame;
                        MainViewModel.Status = mem.IsStartGame() ? (int)Category.AoeStatus.Initial : MainViewModel.Status;
                        break;
                    case (int)Category.AoeStatus.Initial:
                        if (c != (int)Category.AoeStatus.Initial)
                            Console.WriteLine("Aoe Initial");

                        MainViewModel.Match = new Models.Match();
                        MainViewModel.Status = mem.StartGame(); // switch to time in game

                        c = (int)Category.AoeStatus.Initial;
                        break;
                    case (int)Category.AoeStatus.StartGame:
                        if (c != (int)Category.AoeStatus.StartGame)
                            Console.WriteLine("Aoe StartGame");

                        // Read Address for Scouter
                        Models.Match match = MainViewModel.Match;
                        mem.ScoutMatch(ref match);
                        MainViewModel.Match = match;

                        if (MainViewModel.Match.Status == (int)Category.MatchStatus.End && c2 != (int)Category.MatchStatus.End)
                        {
                            Console.WriteLine("Time = 0 => End game");
                            c2 = (int)Category.MatchStatus.End;
                            MainViewModel.Status = (int)Category.AoeStatus.Endgame;
                        }

                        if (MainViewModel.Match.Status == (int)Category.MatchStatus.Pause && c2 != (int)Category.MatchStatus.Pause)
                        {
                            Console.WriteLine("Paused game");
                            c2 = (int)Category.MatchStatus.Pause;
                        }

                        if (MainViewModel.Match.Status == (int)Category.MatchStatus.Resume && c2 != (int)Category.MatchStatus.Resume)
                        {
                            Console.WriteLine("Resume game");
                            c2 = (int)Category.MatchStatus.Resume;
                        }
                        c = (int)Category.AoeStatus.StartGame;
                        break;
                    case (int)Category.AoeStatus.Endgame: //quit or victory
                        if (c != (int)Category.AoeStatus.Endgame)
                            Console.WriteLine("Aoe Endgame");
                        MainViewModel.StoreAndRefreshMatch();
                        MainViewModel.Status = (int)Category.AoeStatus.None;
                        c = (int)Category.AoeStatus.Endgame;
                        break;
                    default:
                        //Console.WriteLine("default " + MainViewModel.Status);
                        break;
                }
            }
        }

        /// <summary>     

        /// Displays Progress changes to UI .     

        /// </summary>     

        /// <param name="sender"></param>     

        /// <param name="e"></param>     

        static void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        /// <summary>     

        /// Displays result of background performing operation.     

        /// </summary>     

        /// <param name="sender"></param>     

        /// <param name="e"></param>     

        static void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("333333333333333333333333333");
            if (e.Cancelled)
            {

                Console.WriteLine("Operation Cancelled");

            }

            else if (e.Error != null)
            {

                Console.WriteLine("Error in Process :" + e.Error);

            }

            else
            {

                Console.WriteLine("Operation Completed :" + e.Result);
            }

        }
        #endregion

        #region Load Match when running

        #endregion
    }
}

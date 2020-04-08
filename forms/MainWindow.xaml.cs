using Evade.Model;
using Evade.Util;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Evade
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CanvasAdapter adapter;

        private readonly DispatcherTimer gameTimer;
        private readonly DispatcherTimer playedTimer;

        private float gamespeed = 1;
        private int direction = 0;
        private float count = 0;
        private float timePlayed = -Constants.COUNTDOWNTIME;

        public MainWindow()
        {
            InitializeComponent();
            // prompt user for name
            WelcomeWindow ww = new WelcomeWindow();
            if (ww.ShowDialog() == true)
                lPlayerName.Content = ww.PlayerName;
            else
                this.Close();
            // setup game field
            adapter = new CanvasAdapter(canvas);
            adapter.AddPlayer();
            //get highscore
            string name = HighScoreManager.ReadAttribute("name");
            float time = float.Parse(HighScoreManager.ReadAttribute("time"));
            lHighscoreName.Content = name;
            lHighscoreTime.Content = time.ToString();
            // start timers
            gameTimer = new DispatcherTimer();
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 2);
            gameTimer.Start();
            playedTimer = new DispatcherTimer();
            playedTimer.Tick += PlayedTimer_Tick;
            playedTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            playedTimer.Start();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                direction = -1;
            else if (e.Key == Key.Right)
                direction = 1;
        }

        private void OnKeyUpHandler(object sender, KeyEventArgs e)
        {
            if (direction == -1 && e.Key == Key.Left || direction == 1 && e.Key == Key.Right)
            {
                direction = 0;
            }
        }

        private void PlayedTimer_Tick(object sender, EventArgs e)
        {
            gamespeed = 1 + ((int)timePlayed / 10) * Constants.GAMESPEEDFACTOR;
            timePlayed += 0.1F;
            lTime.Content = timePlayed.ToString("0.0");
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            adapter.UpdatePlayerPosition(direction);
            if (timePlayed >= 0) //countdown to game-start
            {
                bool gameOver = adapter.CheckBlockCollisions(gamespeed);
                if (gameOver)
                {
                    GameOver();
                }
                count += gamespeed;
                if (count >= Constants.BLOCKCREATETICKS)
                {
                    count = 0;
                    adapter.CreateBlock();
                }
            }
        }

        private void GameOver()
        {
            gameTimer.Stop();
            playedTimer.Stop();
            CheckHighscore();
            EndGame();
        }

        private void CheckHighscore()
        {
            //get previous highscore
            float score = float.Parse(lHighscoreTime.Content.ToString());
            if (timePlayed >= score)
            { //highscore beaten - replace it
                string newName = lPlayerName.Content.ToString();
                string newTime = lTime.Content.ToString();
                HighScoreManager.WriteAttribute("name", newName);
                HighScoreManager.WriteAttribute("time", newTime);
                lHighscoreName.Content = newName;
                lHighscoreTime.Content = newTime;
            }

        }

        private void EndGame()
        {
            GameOverWindow gow = new GameOverWindow(
                lPlayerName.Content.ToString(),
                lTime.Content.ToString(),
                lHighscoreName.Content.ToString(),
                lHighscoreTime.Content.ToString()
            );
            if (gow.ShowDialog() == true)
                RestartGame();
            else
                this.Close();
        }

        private void RestartGame()
        {
            //reset game variables
            gamespeed = 1;
            direction = 0;
            count = 0;
            timePlayed = -Constants.COUNTDOWNTIME;
            // reset game field
            adapter.Reset();
            //get highscore
            string name = HighScoreManager.ReadAttribute("name");
            float time = float.Parse(HighScoreManager.ReadAttribute("time"));
            lHighscoreName.Content = name;
            lHighscoreTime.Content = time.ToString();
            gameTimer.Start();
            playedTimer.Start();
        }
    }
}

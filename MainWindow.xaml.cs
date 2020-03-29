using Evade;
using Evade.Model;
using Evade.Util;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfEvade
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Player player;
        private List<Block> blocks = new List<Block>();

        private float gamespeed = 1;
        private int direction = 0;
        private float count = 0;
        private float timePlayed = -1;

        private readonly Random rand = new Random();
        private readonly DispatcherTimer gameTimer;
        private readonly DispatcherTimer playedTimer;

        public MainWindow()
        {
            InitializeComponent();
            WelcomeWindow ww = new WelcomeWindow();
            if (ww.ShowDialog() == true)
                lPlayerName.Content = ww.PlayerName;
            else
                this.Close();

            player = new Player();
            canvas.Children.Add(player.Rect);
            player.X = ((int)canvas.Width) / 2 - Constants.PLAYERSIZE / 2;
            player.Y = ((int)canvas.Height) - Constants.PLAYERSIZE * 5 / 3; // TODO: why?
            Canvas.SetLeft(player.Rect, player.X);
            Canvas.SetTop(player.Rect, player.Y);
            //get highscore
            string name = FileManager.ReadAttribute("name");
            float time = float.Parse(FileManager.ReadAttribute("time"));
            lHighscoreName.Content = name;
            lHighscoreTime.Content = time.ToString();
            gameTimer = new DispatcherTimer();
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 2);
            gameTimer.Start();
            playedTimer = new DispatcherTimer();
            playedTimer.Tick += PlayedTimer_Tick;
            playedTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            playedTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            UpdatePlayerPos();
            if (timePlayed >= 0)
            { //countdown to game-start
                UpdateBlockPos();
                count += gamespeed;
                if (count >= 15)
                { //#magicnumber
                    count = 0;
                    CreateBlock();
                }
            }
        }

        private void UpdatePlayerPos()
        {
            int movement = 0;

            switch (direction)
            {
                case -1:
                    movement = -Constants.PLAYERMOVESPEED;
                    break;
                case 1:
                    movement = Constants.PLAYERMOVESPEED;
                    break;
                default:
                    break;
            }
            if (movement != 0)
            {
                int newPos = player.X + movement;
                if (newPos >= 0 && newPos + Constants.PLAYERSIZE <= canvas.Width)
                {
                    player.X = newPos;
                    Canvas.SetLeft(player.Rect, newPos);
                }
            }
        }

        private void UpdateBlockPos()
        {
            foreach (Block b in blocks)
            {
                int y = b.Y;
                y += (int)(b.MoveSpeed * gamespeed);
                b.Y = y;
                Canvas.SetTop(b.Rect, y);
                if (b.Area.IntersectsWith(player.Area))
                    KillPlayer();
                if (y + b.Size >= canvas.Height)
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        blocks.Remove(b);
                        canvas.Children.Remove(b.Rect);
                    }
                    ));
                }
            }
        }

        private void KillPlayer()
        {
            gameTimer.Stop();
            playedTimer.Stop();
            CheckHighscore();
            EndGame();
        }

        private void CreateBlock()
        {
            int size = rand.Next(Constants.BLOCKMINSIZE, Constants.BLOCKMAXSIZE);
            int pos = rand.Next((int)canvas.Width - size);
            int speed = rand.Next(Constants.BLOCKMINSPEED, Constants.BLOCKMAXSPEED + 1);
            Block block = new Block(size, speed)
            {
                X = pos,
                Y = -size
            };
            blocks.Add(block);
            canvas.Children.Add(block.Rect);
            Canvas.SetLeft(block.Rect, block.X);
            Canvas.SetTop(block.Rect, block.Y);
        }

        private void CheckHighscore()
        {
            //get previous highscore
            string name = lHighscoreName.Content.ToString();
            float score = float.Parse(lHighscoreTime.Content.ToString());
            if (timePlayed >= score)
            { //highscore beaten - replace it
                string newName = lPlayerName.Content.ToString();
                string newTime = lTime.Content.ToString();
                FileManager.WriteAttribute("name", newName);
                FileManager.WriteAttribute("time", newTime);
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
            blocks = new List<Block>();
            canvas.Children.Clear();
            gamespeed = 1;
            direction = 0;
            count = 0;
            timePlayed = -1;
            player.X = (int)canvas.Width / 2 - Constants.PLAYERSIZE / 2;
            canvas.Children.Add(player.Rect);
            Canvas.SetLeft(player.Rect, player.X);
            //get highscore
            string name = FileManager.ReadAttribute("name");
            float time = float.Parse(FileManager.ReadAttribute("time"));
            lHighscoreName.Content = name;
            lHighscoreTime.Content = time.ToString();
            gameTimer.Start();
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
            if (direction == -1 && e.Key == Key.Left ||
                direction == 1 && e.Key == Key.Right)
                direction = 0;

        }

        private void PlayedTimer_Tick(object sender, EventArgs e)
        {
            gamespeed = 1 + ((int)timePlayed / 10) * Constants.GAMESPEEDFACTOR;
            timePlayed += 0.1F;
            lTime.Content = timePlayed.ToString("0.0");
        }
    }
}

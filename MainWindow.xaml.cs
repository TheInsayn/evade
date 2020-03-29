using Evade;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfEvade
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int PLAYERSIZE = 30;
        private const int PLAYERMOVESPEED = 8;

        private const int BLOCKMINSIZE = 30;
        private const int BLOCKMAXSIZE = 60;
        private const int BLOCKMINSPEED = 5;
        private const int BLOCKMAXSPEED = 8;

        private Color PLAYERCOLOR = Colors.Yellow;
        private Color BLOCKCOLOR = Colors.Red;

        private Player player;
        private List<Block> blocks = new List<Block>();
        private float gamespeed = 1;
        private int direction = 0;
        private float count = 0;
        private float timePlayed = -1;
        Random rand = new Random();
        System.Windows.Threading.DispatcherTimer gameTimer;
        System.Windows.Threading.DispatcherTimer playedTimer;
        Canvas canvas;

        public MainWindow()
        {
            InitializeComponent();
            WelcomeWindow ww = new WelcomeWindow();
            if (ww.ShowDialog() == true)
                lPlayerName.Content = ww.PlayerName;

            canvas = cnvGame;
            player = new Player(PLAYERSIZE, PLAYERCOLOR);
            canvas.Children.Add(player.Rect);
            player.X = (int)cnvGame.Width / 2 - PLAYERSIZE / 2;
            player.Y = (int)cnvGame.Height - PLAYERSIZE;
            Canvas.SetLeft(player.Rect, player.X);
            Canvas.SetTop(player.Rect, player.Y);
            //get highscore
            string name = FileManager.ReadAttribute("name");
            float time = 0;//float.Parse(FileManager.ReadAttribute("time"));
            lHighscoreName.Content = name;
            lHighscoreTime.Content = time.ToString();
            gameTimer = new System.Windows.Threading.DispatcherTimer();
            gameTimer.Tick += timer_Tick;
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            gameTimer.Start();
            playedTimer = new System.Windows.Threading.DispatcherTimer();
            playedTimer.Tick += tPlayed_Tick;
            playedTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            playedTimer.Start();
        }

        /*private void pnlField_Paint(object sender, PaintEventArgs e)
        {
            player.Draw(g);
            foreach (Block b in blocks)
            {
                b.Draw(g);
            }
        }*/

        private void timer_Tick(object sender, EventArgs e)
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
            canvas.InvalidateVisual();
        }

        private void UpdatePlayerPos()
        {
            int movement = 0;

            switch (direction)
            {
                case -1:
                    movement = -PLAYERMOVESPEED;
                    break;
                case 1:
                    movement = PLAYERMOVESPEED;
                    break;
                default:
                    break;
            }
            if (movement != 0)
            {
                int newPos = player.X + movement;
                if (newPos >= 0 && newPos + PLAYERSIZE <= canvas.Width)
                    player.X = newPos;
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
            int size = rand.Next(BLOCKMINSIZE, BLOCKMAXSIZE);
            int pos = rand.Next((int)canvas.Width - size);
            int speed = rand.Next(BLOCKMINSPEED, BLOCKMAXSPEED + 1);
            Block block = new Block(size, BLOCKCOLOR, speed, pos, -size);
            block.X = pos;
            block.Y = -size;
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
            /*FormEnd fe = new FormEnd(lPlayerName.Text, lGameTime.Text, lHighscoreName.Text, lHighscoreTime.Text);
            if (fe.ShowDialog() == DialogResult.Retry)
                RestartGame();
            else
                Application.Exit();*/
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
            player.X = (int)canvas.Width / 2 - PLAYERSIZE / 2;
            player.Y = (int)canvas.Height - PLAYERSIZE;
            canvas.Children.Add(player.Rect);
            //get highscore
            string name = FileManager.ReadAttribute("name");
            float time = float.Parse(FileManager.ReadAttribute("time"));
            lHighscoreName.Content = name;
            lHighscoreTime.Content = time.ToString();
            gameTimer.Start();
            playedTimer.Start();
        }

        /*private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                direction = -1;
            else if (e.KeyCode == Keys.Right)
                direction = 1;
        }*/

        /*private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (direction == -1 && e.KeyCode == Keys.Left ||
                direction == 1 && e.KeyCode == Keys.Right)
                direction = 0;

        }*/

        private void tPlayed_Tick(object sender, EventArgs e)
        {
            gamespeed = 1 + ((int)timePlayed / 10) / 5.0F; //#magicnumber
            timePlayed += 0.1F;
            lTime.Content = timePlayed.ToString("0.0");
        }
    }
}

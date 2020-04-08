using Evade.Util;
using System;
using System.Windows;
using System.Windows.Input;

namespace Evade
{
    /// <summary>
    /// Interaction logic for GameOverWindow.xaml
    /// </summary>
    public partial class GameOverWindow : Window
    {
        private const string FormatWon = "Well done, {0}. You have survived for {1} seconds.\nThe highscore is by {2} with {3} seconds.";
        private const string FormatHighscore = "Excellent {0}! You have beaten the highscore\nby surviving for {1} seconds.";

        public GameOverWindow(string playerName, string playerTime, string highscoreName, string highscoreTime)
        {
            InitializeComponent();
            if (float.Parse(playerTime) >= float.Parse(highscoreTime))
                lblResult.Content = String.Format(FormatHighscore, playerName, playerTime);
            else
                lblResult.Content = String.Format(FormatWon, playerName, playerTime, highscoreName, highscoreTime);
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.DialogResult = true;
                this.Close();
            }
            else if (e.Key == Key.Delete)
            {
                ResetScore();
            }
            else if (e.Key == Key.Escape)
            {
                this.DialogResult = false;
                this.Close();
            }
        }

        private void ResetScore()
        {
            HighScoreManager.WriteAttribute("name", "XXX");
            HighScoreManager.WriteAttribute("time", "00,0");
        }
    }
}

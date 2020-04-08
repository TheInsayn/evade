using System.Windows;
using System.Windows.Input;

namespace Evade
{
    /// <summary>
    /// Interaction logic for WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow()
        {
            InitializeComponent();
        }

        public string PlayerName
        {
            get
            {
                string text = txtName.Text;
                return text;
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}

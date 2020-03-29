using System.Windows;
using System.Windows.Input;

namespace WpfEvade
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
                //StringBuilder sb = new StringBuilder(text);
                //sb.Replace(" ", "");
                //text = sb.ToString();
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

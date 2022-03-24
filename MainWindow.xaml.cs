using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BowlingScoreboard
{
    static class Variables
    {
        public static int playerCount = 1;
        public static List<Player> players = new List<Player>();
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Variables.playerCount = 1;
            btnDecrease.IsEnabled = false;
        }

        private void btnIncrease_Click(object sender, RoutedEventArgs e)
        {
            if (!btnDecrease.IsEnabled)
                btnDecrease.IsEnabled = true;

            int count = Int32.Parse(tbPlayers.Text);
            count++;
            tbPlayers.Text = count.ToString();

            if (count >= 4)
                btnIncrease.IsEnabled = false;
        }

        private void btnDecrease_Click(object sender, RoutedEventArgs e)
        {
            if (!btnIncrease.IsEnabled)
                btnIncrease.IsEnabled = true;

            int count = Int32.Parse(tbPlayers.Text);
            count--;
            tbPlayers.Text = count.ToString();

            if(count <= 1)
                btnDecrease.IsEnabled = false;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Variables.playerCount= Int32.Parse(tbPlayers.Text);
            BoardWindow boardWindow = new BoardWindow();
            boardWindow.Show();
            this.Close();
        }
    }
}

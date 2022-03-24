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
using System.Windows.Shapes;

namespace BowlingScoreboard
{
    /// <summary>
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        private int frameIndex = 0;
        private int playerIndex = 0;
        private bool isFirstRoll = true;

        public BoardWindow()
        {
            InitializeComponent();

            for(int i = 0; i < Variables.playerCount; i++)
            {
                Player player = new Player((i + 1).ToString());
                Variables.players.Add(player);
            }

            buttonCheck();
        }

        private void roll(char roll)
        {
            Variables.players[playerIndex].frames[frameIndex].setRoll(isFirstRoll, roll); //stores the roll

            if (isFirstRoll && roll != 'X') //set buttons according to roll
                buttonCheck(roll);
            else if (Variables.players[playerIndex].frames[9].getRoll2() == 'X' && Variables.players[playerIndex].frames[9].getRoll2() == '/') //if there is a strike or spare in the last frame
                buttonCheck(roll);
            else
                buttonCheck();

            rollDisplay(roll, false); //display the roll

            if (Variables.players[playerIndex].getPrevRoll() == 'X' && Variables.players[playerIndex].getPrevRoll2() == 'X') //if 2 strikes in a row, calculate the score of the first
            {
                if(Variables.players[playerIndex].frames[9].getRoll2() != '0') //if this is the final roll, calculate the last frame
                {
                    setScore(frameIndex - 1);
                    scoreDisplay(frameIndex - 1);
                }
                else //calculate the score 2 frames ago
                {
                    setScore(frameIndex - 2);
                    scoreDisplay(frameIndex - 2);
                }
            }
            else if (Variables.players[playerIndex].getPrevRoll2() == 'X') //calculate a strike from the last frame
            {
                setScore(frameIndex - 1);
                scoreDisplay(frameIndex - 1);
            }

            if (Variables.players[playerIndex].getPrevRoll() == '/') //calculate a spare from the last frame
            {
                setScore(frameIndex - 1);
                scoreDisplay(frameIndex - 1);
            }

            if (roll != 'X' && roll != '/')  //calculate a normal frame
            {
                if (!isFirstRoll && Variables.players[playerIndex].frames[9].getRoll1() != 'X')
                {
                    setScore(frameIndex);
                    scoreDisplay(frameIndex);
                }
            }

            Variables.players[playerIndex].setPrevRoll2(Variables.players[playerIndex].getPrevRoll()); //move the previous roll back
            Variables.players[playerIndex].setPrevRoll(roll); //make this the previous roll

            if (playerIndex + 1 == Variables.playerCount) //if it's the last player
            {
                if (!isFirstRoll || roll == 'X') //if it's the last roll and a strike move to next frame and go back to player 1
                {
                    if(frameIndex < 9) //if it's not the final frame
                    {
                        frameIndex++;
                        playerIndex = 0;
                    }
                }
            }
            else if (!isFirstRoll || roll == 'X') //else if it's the last roll or a strike move to the next player 
            {
                if (Variables.players[playerIndex].frames[9].getRoll1() != 'X' && Variables.players[playerIndex].frames[9].getRoll2() != '/') //if the final frame a strike roll 1 or a spare roll 2
                    playerIndex++;
            }

            if (roll != 'X') //if not a strike switch value of isFirstRoll
                isFirstRoll = !isFirstRoll;
            if (Variables.players[playerIndex].frames[9].getRoll1() == 'X') //if strike in last frame roll 1, switch it back
                isFirstRoll = !isFirstRoll;

            if (Variables.players[playerIndex].frames[9].getRoll2() != '0' && Variables.players[playerIndex].getPrevRoll2() != 'X' && Variables.players[playerIndex].getPrevRoll() != '/') //if it's the final roll with no bonus
                buttonsOff();
        }

        private void finalRoll(char roll)
        {
            if (Variables.players[playerIndex].frames[9].getRoll2() == 'X') //if there were 2 strikes in the final frame
            {
                rollDisplay(roll, true);
                Variables.players[playerIndex].frames[frameIndex].setScore(10 + 10 + rollConvert(roll));
            }
            else if (Variables.players[playerIndex].frames[9].getRoll1() == 'X') //if there was a strike in the final frame
            {
                rollDisplay(roll, true);
                Variables.players[playerIndex].frames[frameIndex].setScore(10 + rollConvert(Variables.players[playerIndex].frames[frameIndex].getRoll2()) + rollConvert(roll));
            }
            else if (Variables.players[playerIndex].frames[9].getRoll2() == '/') //if there was a spare in the final frame
            {
                rollDisplay(roll, true);
                Variables.players[playerIndex].frames[frameIndex].setScore(10 + rollConvert(roll));
            }

            scoreDisplay(frameIndex);

            if (playerIndex + 1 == Variables.playerCount) //if it's the last player turn off the buttons
                buttonsOff();
            else
            {
                playerIndex++;
                isFirstRoll = true;
                buttonCheck();
            }
        }

        private void setScore(int frameIndex)
        {
            if (Variables.players[playerIndex].frames[frameIndex].getRoll1() == 'X') //if this frame was a strike
            {
                if(frameIndex == 8) //if this is the second to last frame
                {
                    if (Variables.players[playerIndex].frames[frameIndex + 1].getRoll1() == 'X') //if the next frame had a strike as well
                    {
                        Variables.players[playerIndex].frames[frameIndex].setScore(10 + 10 +
                        rollConvert(Variables.players[playerIndex].frames[frameIndex + 1].getRoll2()));
                    }
                    else
                    {
                        Variables.players[playerIndex].frames[frameIndex].setScore(10 +
                        rollConvert(Variables.players[playerIndex].frames[frameIndex + 1].getRoll1()) +
                        rollConvert(Variables.players[playerIndex].frames[frameIndex + 1].getRoll2()));
                    }
                }
                else if (frameIndex != 9) //else if not the last frame
                {
                    if (Variables.players[playerIndex].frames[frameIndex + 1].getRoll1() == 'X') //if the next frame had a strike as well
                    {
                        Variables.players[playerIndex].frames[frameIndex].setScore(10 + 10 +
                        rollConvert(Variables.players[playerIndex].frames[frameIndex + 2].getRoll1()));
                    }
                    else
                    {
                        Variables.players[playerIndex].frames[frameIndex].setScore(10 +
                        rollConvert(Variables.players[playerIndex].frames[frameIndex + 1].getRoll1()) +
                        rollConvert(Variables.players[playerIndex].frames[frameIndex + 1].getRoll2()));
                    }
                }  
            }
            else if (Variables.players[playerIndex].frames[frameIndex].getRoll2() == '/') //if this frame was a spare
            {
                Variables.players[playerIndex].frames[frameIndex].setScore(10 +
                    rollConvert(Variables.players[playerIndex].frames[frameIndex + 1].getRoll1()));
            }
            else //a normal frame
            {
                Variables.players[playerIndex].frames[frameIndex].setScore(rollConvert(Variables.players[playerIndex].frames[frameIndex].getRoll1()) +
                    rollConvert(Variables.players[playerIndex].frames[frameIndex].getRoll2()));
            }
        }

        private void scoreDisplay(int frameIndex)
        {
            int player = playerIndex + 1;
            int frame = frameIndex + 1;

            string name = "tbScore" + player + frame;
            TextBlock tb = (TextBlock)FindName(name);
            tb.Text = Variables.players[playerIndex].totalScore().ToString();

            string leaderboard = "\n\n";
            IEnumerable<Player> sortedPlayers = Variables.players.OrderByDescending(p => p.totalScore());
            foreach (Player p in sortedPlayers)
            {
                leaderboard += "Player " + p.getName() + ": " + p.totalScore() + "\n\n";
            }

            tbLeaderboard.Text = leaderboard;
        }

        private void rollDisplay(char roll, bool isFinal)
        {
            string name;
            int player = playerIndex + 1;
            int frame = frameIndex + 1;

            if (isFinal)
            {
                name = "tbRoll" + player + "10_3";
                TextBlock tb = (TextBlock)FindName(name);
                tb.Text = roll.ToString();
            }
            else
            {
                if (isFirstRoll)
                    name = "tbRoll" + player + frame + "_1";
                else
                    name = "tbRoll" + player + frame + "_2";

                TextBlock tb = (TextBlock)FindName(name);

                if (roll == '0')
                    tb.Text = "-";
                else
                    tb.Text = roll.ToString();
            }
        }

        private void buttonCheck()
        {
            btnSpare.IsEnabled = false;
            btnStrike.IsEnabled = true;
            btn1.IsEnabled = true;
            btn2.IsEnabled = true;
            btn3.IsEnabled = true;
            btn4.IsEnabled = true;
            btn5.IsEnabled = true;
            btn6.IsEnabled = true;
            btn7.IsEnabled = true;
            btn8.IsEnabled = true;
            btn9.IsEnabled = true;
        }

        private void buttonCheck(char roll)
        {
            btnSpare.IsEnabled = true;
            btnStrike.IsEnabled = false;

            if(roll == '1')
            {
                btn9.IsEnabled = false;
            }
            else if (roll == '2')
            {
                btn8.IsEnabled = false;
                btn9.IsEnabled = false;
            }
            else if (roll == '3')
            {
                btn7.IsEnabled = false;
                btn8.IsEnabled = false;
                btn9.IsEnabled = false;
            }
            else if (roll == '4')
            {
                btn6.IsEnabled = false;
                btn7.IsEnabled = false;
                btn8.IsEnabled = false;
                btn9.IsEnabled = false;
            }
            else if (roll == '5')
            {
                btn5.IsEnabled = false;
                btn6.IsEnabled = false;
                btn7.IsEnabled = false;
                btn8.IsEnabled = false;
                btn9.IsEnabled = false;
            }
            else if (roll == '6')
            {
                btn4.IsEnabled = false;
                btn5.IsEnabled = false;
                btn6.IsEnabled = false;
                btn7.IsEnabled = false;
                btn8.IsEnabled = false;
                btn9.IsEnabled = false;
                btn9.IsEnabled = false;
            }
            else if (roll == '7')
            {
                btn3.IsEnabled = false;
                btn4.IsEnabled = false;
                btn5.IsEnabled = false;
                btn6.IsEnabled = false;
                btn7.IsEnabled = false;
                btn8.IsEnabled = false;
                btn9.IsEnabled = false;
                btn9.IsEnabled = false;
            }
            else if (roll == '8')
            {
                btn2.IsEnabled = false;
                btn3.IsEnabled = false;
                btn4.IsEnabled = false;
                btn5.IsEnabled = false;
                btn6.IsEnabled = false;
                btn7.IsEnabled = false;
                btn8.IsEnabled = false;
                btn9.IsEnabled = false;
                btn9.IsEnabled = false;
            }
            else if (roll == '9')
            {
                btn1.IsEnabled = false;
                btn2.IsEnabled = false;
                btn3.IsEnabled = false;
                btn4.IsEnabled = false;
                btn5.IsEnabled = false;
                btn6.IsEnabled = false;
                btn7.IsEnabled = false;
                btn8.IsEnabled = false;
                btn9.IsEnabled = false;
                btn9.IsEnabled = false;
            }
        }

        private void buttonsOff()
        {
            btn0.IsEnabled = false;
            btnSpare.IsEnabled = false;
            btnStrike.IsEnabled = false;
            btn1.IsEnabled = false;
            btn2.IsEnabled = false;
            btn3.IsEnabled = false;
            btn4.IsEnabled = false;
            btn5.IsEnabled = false;
            btn6.IsEnabled = false;
            btn7.IsEnabled = false;
            btn8.IsEnabled = false;
            btn9.IsEnabled = false;
        }

        private int rollConvert(char roll)
        {
            if (roll == 'X')
                return 10;
            if (roll == '/')
                return 10;
            if (roll == '-')
                return 0;
            else
                return (int)Char.GetNumericValue(roll);
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            Variables.players.Clear();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            if (Variables.players[playerIndex].frames[9].getRoll2() != '0')
                finalRoll('1');
            else
                roll('1');
        }
        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            if (Variables.players[playerIndex].frames[9].getRoll2() != '0')
                finalRoll('2');
            else
                roll('2');
        }
        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            if (Variables.players[playerIndex].frames[9].getRoll2() != '0')
                finalRoll('3');
            else
                roll('3');
        }
        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            if (Variables.players[playerIndex].frames[9].getRoll2() != '0')
                finalRoll('4');
            else
                roll('4');
        }
        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            if (Variables.players[playerIndex].frames[9].getRoll2() != '0')
                finalRoll('5');
            else
                roll('5');
        }
        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            if (Variables.players[playerIndex].frames[9].getRoll2() != '0')
                finalRoll('6');
            else
                roll('6');
        }
        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            if (Variables.players[playerIndex].frames[9].getRoll2() != '0')
                finalRoll('7');
            else
                roll('7');
        }
        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            if (Variables.players[playerIndex].frames[9].getRoll2() != '0')
                finalRoll('8');
            else
                roll('8');
        }
        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            if (Variables.players[playerIndex].frames[9].getRoll2() != '0')
                finalRoll('9');
            else
                roll('9');
        }
        private void btnStrike_Click(object sender, RoutedEventArgs e)
        {
            if (Variables.players[playerIndex].frames[9].getRoll2() != '0')
                finalRoll('X');
            else
                roll('X');
        }
        private void btnSpare_Click(object sender, RoutedEventArgs e)
        {
            if (Variables.players[playerIndex].frames[9].getRoll2() != '0')
                finalRoll('/');
            else
                roll('/');
        }
        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            if (Variables.players[playerIndex].frames[9].getRoll2() != '0')
                finalRoll('-');
            else
                roll('-');
        }
    }
}

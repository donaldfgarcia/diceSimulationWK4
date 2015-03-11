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

namespace DiceSimulationWK4
{
    /// <summary>
    /// Dice Simulation
    /// Donald Garcia
    /// POS/409
    /// February 09, 2015
    /// John Becton
    /// Simulate 100 rolls of dice, display individual rolls, display all rolls, emphasis doubles (sequence + die value)
    /// Add total of each throw, IEnumerable, and LINQ functionality
    /// </summary>
    
    public partial class MainWindow : Window
    {
        // Inistialize Variables
        Random rnd = new Random();
        int rolls = 0;

        // Dictionay to hold throw sequence values
        Dictionary<int, int> sequence1 = new Dictionary<int, int>();
        Dictionary<int, int> sequence2 = new Dictionary<int, int>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Fill combobox displayChoice on form load
            displayChoice.Items.Add("Display Results");
            displayChoice.Items.Add("Total: Sequence 1");
            displayChoice.Items.Add("Total: Sequence 2");
        }

        // Clear All ListBoxes
        private void ClearResults()
        {
            sequence1Results.Items.Clear();
            sequence2Results.Items.Clear();
            sequence1Display.Items.Clear();
            sequence2Display.Items.Clear();
            sequence1.Clear();
            sequence2.Clear();
            searchValue.Clear();
            sequence1TotalDisplay.Items.Clear();
            sequence2TotalDisplay.Items.Clear();
        }

        // Roll dice 100 times
        private void RollDice(int rolls, Dictionary<int, int> sequence, ListBox displayRolls)
        {
            // Loop through rolls
            for (int i = 1; i <= rolls; i++)
            {
                // Declare variables
                int die1 = 0;
                int die2 = 0;
                int throwTotal = 0; // Total individual die for single throw

                die1 = rnd.Next(1, 7); // Generate random number between 1 and 7
                die2 = rnd.Next(1, 7);
                throwTotal = die1 + die2;

                // Display dice as they roll
                die1Display.Text = die1.ToString();
                die2Display.Text = die2.ToString();

                // Display results to user
                displayRolls.Items.Add(i.ToString() + "\t" + die1.ToString()
                                        + "\t" + die2.ToString() + "\t" + throwTotal.ToString());

                // Add throw total to Dictionary
                sequence.Add(i, throwTotal);
            }
        }

        // Throw dice once
        private void rollDice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearResults();
                rolls = 1;
                RollDice(rolls, sequence1, sequence1Results); // Pass parameters to RollDice Method
            }
            catch (Exception ex)
            {
                errorMessage.Text = ex.Message.ToString();
            }
        }

        // Throw dice 100 times
        private void roll100_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                rolls = 100;

                // Choose results display
                if (sequence1Results.Items.Count > 0 & sequence2Results.Items.Count > 0 || sequence1Results.Items.Count == 1)
                {
                    ClearResults();
                    RollDice(rolls, sequence1, sequence1Results); // Results in sequence 1 box
                }
                else if (sequence1Results.Items.Count > 0)
                {
                    RollDice(rolls, sequence2, sequence2Results); // Results in sequence 2 box
                }
                else
                {
                    RollDice(rolls, sequence1, sequence1Results);
                }
            }
            catch (Exception ex)
            {
                errorMessage.Text = ex.Message.ToString();
            }
        }

        // Select data to display from combobox displayChoice
        private void go_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sequence1Results.Items.Count == 0)
                {
                    errorMessage.Text = "Roll dice before totalling";
                }
                else if (displayChoice.SelectedIndex == 1)
                {
                    sequence1Display.Items.Clear();
                    
                    // Itterrate through sequence dictionary and display throw totals to user
                    foreach(KeyValuePair<int, int> pair in sequence1)
                    {
                        sequence1Display.Items.Add(string.Format("Throw {0} total = {1}",
                            pair.Key,
                            pair.Value));
                    }

                    // Total all throws in sequence 1
                    var sequence1Total = sequence1.Sum(i => i.Value);
                    sequence1TotalDisplay.Items.Add("Overall total: " + sequence1Total.ToString());

                }
                else if (displayChoice.SelectedIndex == 2)
                {
                    sequence2Display.Items.Clear();

                    // Itterrate through sequence dictionary and display throw totals to user
                    foreach (KeyValuePair<int, int> pair in sequence2)
                    {
                        sequence2Display.Items.Add(string.Format("Throw {0} total = {1}",
                            pair.Key,
                            pair.Value));
                    }

                    // Total all throws in sequence 2
                    var sequence2Total = sequence2.Sum(i => i.Value);
                    sequence2TotalDisplay.Items.Add("Overall total: " + sequence2Total.ToString());
                }
                else
                {
                    errorMessage.Text = "Select action from combobox";
                }
            }
            catch (Exception ex)
            {
                errorMessage.Text = ex.Message.ToString();
            }
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Clear user search display
                sequence1Display.Items.Clear();
                sequence2Display.Items.Clear();
                int searchfor = 0;
                
                if (int.TryParse(searchValue.Text, out searchfor))
                {
                    if (searchfor < 1 || searchfor > 12)
                    {
                        errorMessage.Text = "Search requires number 1 - 12";
                    }
                    else
                    {
                        // LINQ query dictionary for searchValue
                        var sequence1Match = from i in sequence1
                                             where i.Value.Equals(searchfor)
                                             select i;

                        // Itterate over query results to display to user
                        foreach (KeyValuePair<int, int> pair in sequence1Match)
                        {
                            sequence1Display.Items.Add(string.Format("Throw {0} total = {1}",
                                pair.Key,
                                pair.Value));
                        }

                        // Repeat above action for sequence2
                        var sequence2Match = from i in sequence2
                                             where i.Value.Equals(searchfor)
                                             select i;

                        foreach (KeyValuePair<int, int> pair in sequence2Match)
                        {
                            sequence2Display.Items.Add(string.Format("Throw {0} total = {1}",
                                pair.Key,
                                pair.Value));
                        }
                    }
                }
                else
                {
                    errorMessage.Text = "Search requires number 1 - 12";
                }
            }
            catch (Exception ex)
            {
                errorMessage.Text = ex.Message.ToString();
            }
        }

        private void clearResults_Click(object sender, RoutedEventArgs e)
        {
            ClearResults();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace ApplicationDevelopmentFinalProject
{
    public partial class Form1 : Form
    {
        String conn_string = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source = \\Programming_in_C#\\FarmInformation.accdb; Persist Security Info = False";
        String error_msg = "";

        OleDbConnection conn = null;

        public Form1()
        {
            InitializeComponent();
        }

        //List to hold IDs in database to help with iteration later
        private static List<int> cowIDs = new List<int>();
        private static List<int> dogIDs = new List<int>();
        private static List<int> goatIDs = new List<int>();
        private static List<int> sheepIDs = new List<int>();
        private static List<int> jersyCowIDs = new List<int>();


        //global variable to store the Animal before its added to the dictionary/hashtable
        Animal animal;

        //Dictionary to store all the animals in
        private static Dictionary<int, Animal> allAnimals = new Dictionary<int, Animal>();

        //Gets all animals and adds to dictionary
        public void get_animals()
        {
            //String with animal names in to help with iterating through the database tables
            string[] animalNamesArray = { "Cows", "Dogs", "Goats", "Sheep" };

            //
            for (int i = 0; i < animalNamesArray.Length; i++)
            {
                //Select query concatenated the table-name of the Database's animal tables to make
                //it easier to iterate through the tables when searching through all the rows
                String query = "SELECT * FROM " + animalNamesArray[i];

                //querying with the query using the conn string to the database
                OleDbCommand queryConn = new OleDbCommand(query, conn);
                                
                using (OleDbDataReader reader = queryConn.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //reads all of the values to strings first
                        //these are the attributes that ALL animals have in common
                        string ID = reader["ID"].ToString();
                        string DailyCost = reader["Daily cost"].ToString();
                        string Weight = reader["Weight"].ToString();
                        string AmtWater = reader["Amount of water"].ToString();
                        string Color = reader["Color"].ToString();
                        string Age = reader["Age"].ToString();
                        
                        //then converts them to their respective values
                        int id = Convert.ToInt32(ID);
                        double dailyCost = Convert.ToDouble(DailyCost);
                        double weight = Convert.ToDouble(Weight);
                        double amtwater = Convert.ToDouble(AmtWater);
                        string color = Convert.ToString(Color);
                        int age = Convert.ToInt32(Age);

                        //0 = Cows - So we also need to retrieve the attributes unique to cows
                        if (i == 0)
                        {
                            string AmtMilk = reader["Amount of Milk"].ToString();
                            double amtmilk = Convert.ToDouble(AmtMilk);
                            string IsJersy = reader["Is Jersy"].ToString();
                            bool isjersy = Convert.ToBoolean(IsJersy);

                            //we then determine whether the cow is a jersy cow or a regular cow and then, using all
                            //of the data from the variables above, create cow/jerseycow objects using them
                            if (isjersy == false)
                            {
                                animal = new Cow(id, amtwater, dailyCost, weight, age, color, amtmilk);
                                allAnimals.Add(id, animal);
                                cowIDs.Add(id);
                            }
                            else
                            {
                                animal = new JersyCow(id, amtwater, dailyCost, weight, age, color, amtmilk, isjersy);
                                allAnimals.Add(id, animal);
                                jersyCowIDs.Add(id);
                            }
                        }
                        //1 - Dogs. There aren't any attributes unique to dogs, so this is a short loop
                        else if (i == 1)
                        {
                            dogIDs.Add(id);
                            animal = new Dog(id, amtwater, dailyCost, weight, age, color);
                            allAnimals.Add(id, animal);
                        }
                        //2 = Goats
                        else if (i == 2)
                        {
                            goatIDs.Add(id);
                            string AmtMilk = reader["Amount of milk"].ToString();
                            double amtmilk = Convert.ToDouble(AmtMilk);
                            animal = new Goat(id, amtwater, dailyCost, weight, age, color, amtmilk);
                            allAnimals.Add(id, animal);
                        }
                        //Sheep catch-all-else: There will be no tables after '3', so there is no need 
                        //for an 'else-if (i==3)'
                        else
                        {
                            sheepIDs.Add(id);
                            string AmtWool = reader["Amount of wool"].ToString();
                            double amtwool = Convert.ToDouble(AmtWool);
                            animal = new Sheep(id, amtwater, dailyCost, weight, age, color, amtwool);
                            allAnimals.Add(id, animal);
                        }
                    }
                }
            }
        }

        public void get_prices()
        {
            //Query to select prices
            String SQL_Query = "SELECT * FROM COMMODITY";

            //Command to query prices
            OleDbCommand prices_query = new OleDbCommand(SQL_Query, conn);

            //lists to store the commodity names and price values
            List<double> prices = new List<double>();
            List<string> commodity = new List<string>();

            //reads data in sheep Table
            using (OleDbDataReader reader = prices_query.ExecuteReader())
            {
                //reader.Read will go through Access file and get all values
                while (reader.Read())
                {
                    string PriceType = reader["Commodity"].ToString();
                    string Price = reader["Price"].ToString();

                    //adds the prices/commodity names to the lists
                    prices.Add(Convert.ToDouble(Price));
                    commodity.Add(PriceType);
                }
            }

            //iterates through the prices list to assign the values to the keys
            //based on the order that they were read into the lists in
            for (int i = 0; i < prices.Capacity; i++)
            {
                //adds a new object of the 'Prices' class
                if (i == 0)
                    Prices.GoatMilkPrice = prices[0];
                if (i == 1)
                    Prices.SheepWoolPrice = prices[1];
                if (i == 2)
                    Prices.WaterPrice = prices[2];
                if (i == 3)
                    Prices.GovtTaxPerKg = prices[3];
                if (i == 4)
                    Prices.JersyCowTax = prices[4];
                if (i == 5)
                    Prices.CowMilkPrice = prices[5];
            }
        }

        //displayinfo for animal
        private static string displayInfo(int i, Dictionary<int, Animal> allAnimals)
        {
            Animal animal = allAnimals[i];
            return animal.ShowInf();
        }

        //Displays all dictionary values from input
        public void dictionary_run()
        {
            try
            {
                //Display all attributes for that ID
                //Convert userInput string to integer
                int ID = Convert.ToInt32(UserInput.Text);

                //String to store info in
                String mySTR = displayInfo(ID, allAnimals);

                //Displays the string in the textbox
                textBoxResults.Text = mySTR;
                
            }
            catch (Exception ex)
            {
                error_msg = ex.Message;
                MessageBox.Show(error_msg);
            }

        }
        
        private void label1_Click(object sender, EventArgs e)
        {

        }
                     
        //connect to the database and populate the dictionary on load
        private void Form1_Load(object sender, EventArgs e)
        {
            connectToolStripMenuItem.PerformClick();
        }

        //connect button
        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                conn = new OleDbConnection(conn_string);
                conn.Open();
                disconnectToolStripMenuItem.Enabled = true;
                connectToolStripMenuItem.Enabled = false;


            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        //disconnect button
        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Close();
                disconnectToolStripMenuItem.Enabled = false;
                connectToolStripMenuItem.Enabled = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        //exit button
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //disconnect from database on form close
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            disconnectToolStripMenuItem.PerformClick();
        }
                       
        private void Form1_Load_1(object sender, EventArgs e)
        {

            try
            {
                conn = new OleDbConnection(conn_string);
                conn.Open();
                disconnectToolStripMenuItem.Enabled = true;
                connectToolStripMenuItem.Enabled = false;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            try
            {
                //calls the methods that read the data from the database, creates the objects and puts them into <allAnimals>
                get_animals();
                get_prices();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }            
        }
        
        //search button
        private void Search_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            dictionary_run();
            this.Cursor = Cursors.Default;
        }

        //Generate Report 2
        private void button2_Click(object sender, EventArgs e)
        {
            //variable to store profit count
            double profit = 0.0;

            //iterates through the dictionary to call the method on each animal
            foreach (KeyValuePair<int, Animal> i in allAnimals)
            {
                profit += i.Value.GetDailyProfitability();
            }

            profit = Math.Round(profit, 2);
            textBoxResults.Text = "Daily profitability = $" + profit.ToString();
        }

        //Generate Report 3
        private void button3_Click(object sender, EventArgs e)
        {
            //variable to store total tax paid
            double taxPaid = 0.0;

            //iterates through the dictionary to call the method on each animal
            foreach (KeyValuePair<int, Animal> i in allAnimals)
            {
                taxPaid += i.Value.getDailyTax();
            }

            //Calculates taxPaid *31 which is equal to monthly tax, and then rounds it to 2 decimal places
            taxPaid = Math.Round(taxPaid * 31, 2);
            textBoxResults.Text = "Monthly tax paid = $" + taxPaid.ToString();
        }

        //Generate Report 4
        private void button4_Click(object sender, EventArgs e)
        {
            //variables to store total amount of Milk
            double totalCowMilk = 0;
            double totalGoatMilk = 0;

            //iterates through the dictionary to call the method on each cow, jersey cow and goat
            foreach (KeyValuePair<int, Animal> i in allAnimals)
            {
                if (i.Value.animalType.ToLower() == "cow")
                {
                    totalCowMilk += i.Value.getAmtMilk();
                }
                if (i.Value.animalType.ToLower() == "jersey cow")
                {
                    totalCowMilk += i.Value.getAmtMilk();
                }
                if (i.Value.animalType.ToLower() == "goat")
                {
                    totalGoatMilk += i.Value.getAmtMilk();
                }
            }

            textBoxResults.Text = "Total amount of milk from Cows per day: " + totalCowMilk + " litres\r\n" + "Total amount of milk from Goats per day: " + totalGoatMilk + " litres";
        }

        //Generate Report 5
        private void button5_Click(object sender, EventArgs e)
        {
            //variables to store cumulative age and total animal count
            int animalCount = 0;
            double totalAge = 0;

            //iterates through the dictionary to call the getAge() method on each animal, except dogs
            foreach (KeyValuePair<int, Animal> i in allAnimals)
            {
                if(i.Value.animalType.ToLower() != "dog")
                {
                    animalCount += 1;
                    totalAge += i.Value.getAge();
                }
            }
            
            //Divides total age by the number of animals to calculate the average, then prints it into the text box
            textBoxResults.Text = "Average age of all farm animals: " + (Math.Round((totalAge / animalCount),2)) + " years old";
        }

        //Generate Report 6
        private void button6_Click(object sender, EventArgs e)
        {
            //profit counters
            double goatAndCowprofit = 0.0;
            double sheepProfit = 0.0;
            //animal counters
            int goatAndCowCount = 0;
            int sheepCount = 0;

            //loops through the lists to access the animals from the dictionary
            //and then calculate the animals profitability to add to their profit counter

            foreach (KeyValuePair <int, Animal> i in allAnimals)
            {
                if (i.Value.animalType.ToLower() == "cow")
                {
                    goatAndCowCount += 1;
                    goatAndCowprofit += i.Value.GetDailyProfitability();
                }
                if (i.Value.animalType.ToLower() == "jersey cow")
                {
                    goatAndCowCount += 1;
                    goatAndCowprofit += i.Value.GetDailyProfitability();
                }
                if (i.Value.animalType.ToLower() == "goat")
                {
                    goatAndCowCount += 1;
                    goatAndCowprofit += i.Value.GetDailyProfitability();
                }
                if (i.Value.animalType.ToLower() == "sheep")
                {
                    sheepCount += 1;
                    sheepProfit += i.Value.GetDailyProfitability();
                }
            }

            //working out the average daily profit of each group
            double sheepAvgProfit = sheepProfit / sheepCount;
            double goatAndCowAvgProfit = goatAndCowprofit / goatAndCowCount;

            //printing the results to the text box
            textBoxResults.Text = "Average daily profit from sheep = $" + Math.Round(sheepAvgProfit, 2) +
                "\r\nAverage daily profit from Goats and Cows = $" + Math.Round(goatAndCowAvgProfit, 2);
        }

        //Generate Report 7
        private void button7_Click(object sender, EventArgs e)
        {
            //variables to store total losses and losses only from dogs
            double losses = 0.0;
            double dogLosses = 0.0;

            //iterates through the dictionary to call the getAnimalCosts() method on all animals
            foreach (KeyValuePair<int, Animal> i in allAnimals)
            {
                //first loop identifies only dogs to count dogLosses
                if (i.Value.animalType.ToLower() == "dog")
                {
                    dogLosses += i.Value.getAnimalCosts();
                    losses += i.Value.getAnimalCosts();
                }
                //next loop counts all losses from all other animals
                else
                {
                    losses += i.Value.getAnimalCosts();
                }
            }
            //Calculates the percentage of total losses by dividing dogLosses by the totalLosses and multiplying by 100,
            //rounded up to 2 decimal places
            double dogLossesPercentage = Math.Round((dogLosses/losses)*100, 2);
            textBoxResults.Text = "Percentage of total daily losses accounted by dogs: " + dogLossesPercentage + "%";
        }

        //Generate Report 8
        private void button8_Click(object sender, EventArgs e)
        {
            //List to store the IDs of animals
            List<int> idList = new List<int>();
            foreach (KeyValuePair<int, Animal> i in allAnimals)
            {
                if (i.Value.animalType.ToLower() != "dog")
                    idList.Add(i.Value.id);
            }

            //List to store the daily profit value of animals
            List<double> profitList = new List<double>();
            foreach (KeyValuePair<int, Animal> i in allAnimals)
            {
                if (i.Value.animalType.ToLower() != "dog")
                    profitList.Add(i.Value.GetDailyProfitability());
            }

            //bubble sort algorithm to sort the lists
            int n = profitList.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (profitList[j] > profitList[j + 1])
                    {
                        //temporary variables to store value of [j]
                        double profitTemp = profitList[j];
                        int idTemp = idList[j];
                        
                        //swap profitTemp and profitList[i] 
                        profitList[j] = profitList[j + 1];
                        profitList[j + 1] = profitTemp;
                        //swap idTemp and idList[i]
                        idList[j] = idList[j + 1];
                        idList[j + 1] = idTemp;
                    }            
                }
            }
            //string to store output
            string output = "ID \tProfitability\n--------------------\n";

            //for every index in the lists concatenate list elements into the string
            for (int i = 0; i < n; i++)
            {
                output += idList[i] + "\t" + Math.Round(profitList[i], 2) + "\n";
            }

            //read string to a text file
            System.IO.File.WriteAllText(@"E:\Programming_in_C#\Workspace\AppDevFinalProject\AppDevFinalProject\FarmAnimalProfitability.txt", output);
            MessageBox.Show("'FarmAnimalProfitability.txt' created in 'AppDevFinalProject' folder");
        }
        //Generate Report 9
        private void button9_Click(object sender, EventArgs e)
        {
            //variables to store total number of animals and number of animals who are red
            double redCount = 0;
            double allAnimalCount = 0;

            //iterates through the dictionary
            foreach (KeyValuePair<int, Animal> i in allAnimals)
            {
                //if animals have red color value add +1 to both count variables
                if (i.Value.color.ToLower() == "red")
                {
                    redCount += 1;
                    allAnimalCount += 1;
                }
                if (i.Value.color.ToLower() != "red")
                {
                    allAnimalCount += 1;
                }
            }

            //calculates the percentage of red animals out of the total
            textBoxResults.Text = "Percentage of animals that are red: " + Math.Round((redCount / allAnimalCount) * 100, 2) + "%";

        }

        //Generate Report 10
        private void button10_Click(object sender, EventArgs e)
        {
            //variable to store total amount of jersey tax paid
            double totalJerseyTax = 0;

            //iterates through dictionary to call getDailyTax() on all jersey cows
            foreach (KeyValuePair<int, Animal> i in allAnimals)
            {
                if (i.Value.animalType.ToLower() == "jersey cow")
                {
                    totalJerseyTax += i.Value.getDailyTax();
                }
            }
            //rounds total jersey tax to 2 decimal places and prints it to the text box
            textBoxResults.Text = "Total daily tax paid for Jersey Cows: $" + Math.Round(totalJerseyTax,2);
        }

        //Generate Report 11
        private void button1_Click(object sender, EventArgs e)
        {
            //variables to store...

            //the user-entered year
            string x = userYears.Text;
            //user-entered year when converted to integer
            int yearRange;

            //the count of all animals
            double allAnimalCount = 0;
            //the count of all animals over the threshold age       
            double overThresholdCount = 0;

            //if the user-entered number is a readable integer then...
            if (int.TryParse (x, out yearRange))
            {
                //for every animal in the dictionary...
                foreach (KeyValuePair<int, Animal> i in allAnimals)
                {
                    //if their age value is 6 or above add +1 to overThreshholdCount
                    if (i.Value.age > yearRange)
                    {
                        overThresholdCount += 1;
                    }
                    allAnimalCount += 1;
                }
                //calculates percentage of animals that are over the age threshhold and prints to the results text box
                textBoxResults.Text = "Percentage of animals over "+ x + " years of age: " + Math.Round((overThresholdCount/allAnimalCount)*100, 2) + "%";
            
                //else if the user-entered number is not an integer we produce a message popup telling them to check their input...
            } else
            {
                MessageBox.Show("Something went wrong. Please make sure you are entering an integer.");
            }
        }
        //Generate Report 12
        private void button11_Click(object sender, EventArgs e)
        {
            //variable to store total Jersey Profit
            double totalJerseyProfit = 0;
            //for every animal in the dictionary
            foreach (KeyValuePair<int, Animal> i in allAnimals)
            {
                //if their animalType is "jersey cow"...
                if (i.Value.animalType.ToLower() == "jersey cow")
                {
                    //call the GetDailyProfitability() method for that cow and add the value to the toal
                    totalJerseyProfit += i.Value.GetDailyProfitability();
                }
            }
            //prints out totalJerseyProfit rounded to 2 decimal places
            textBoxResults.Text = "Total daily profitability for Jersey Cows: $" + Math.Round(totalJerseyProfit, 2);
        }
    }
}

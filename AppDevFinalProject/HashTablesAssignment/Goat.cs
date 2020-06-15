using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDevelopmentFinalProject
{
    class Goat : Animal
    {
        public double amtMilk;
        public double goatMilkProfit;

        public Goat(int id, double amtWater, double dailyCost, double weight, int age, string color, double amtMilk) : base(id, amtWater, dailyCost, weight, age, color)
        {
            this.amtMilk = amtMilk;
            animalType = "goat";
        }

        public override String ShowInf()
        {
            string GoatString =
                "Animal type: " + animalType + "\r\n" +
                "ID: " + id + "\r\n" +
                "Amount of Water: " + amtWater + "\r\n" +
                "Daily Cost: " + dailyCost + "\r\n" +
                "Weight: " + weight + "\r\n" +
                "Age: " + age + "\r\n" +
                "Colour: " + color + "\r\n" +
                "Amount of Milk: " + amtMilk;
            return GoatString;
        }
        //Cow profitability is unique to Goats
        public override double GetDailyProfitability()
        {
            profit = getGoatMilkProfit() - (getWaterCost() + dailyCost + getDailyTax());
            return profit;
        }

        //Milk profit is unique to cows 
        public double getGoatMilkProfit()
        {
            goatMilkProfit = amtMilk * Prices.GoatMilkPrice;
            return goatMilkProfit;
        }

        //returns amtMilk
        public override double getAmtMilk()
        {
            return amtMilk;
        }
        //Goat costs are unique to cows, so must be calculated using an overriden method from the parent-class
        public override double getAnimalCosts()
        {
            double profit = GetDailyProfitability();
            double removeProfits = profit - getGoatMilkProfit();
            return removeProfits;
        }
    }
}

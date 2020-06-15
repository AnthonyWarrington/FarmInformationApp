using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDevelopmentFinalProject
{
    class Cow : Animal
    {
        //attributes unique to cows
        public double amtMilk;
        public double milkProfit;

        public Cow(int id, double amtWater, double dailyCost, double weight, int age, string color, double amtMilk) : base(id, amtWater, dailyCost, weight, age, color)
        {
            this.amtMilk = amtMilk;            
            animalType = "cow";
        }

        public override String ShowInf()
        {
            string CowString =
                "Animal type: " + animalType + "\r\n" +
                "ID: " + id + "\r\n" +
                "Amount of Water: " + amtWater + "\r\n" +
                "Daily Cost: " + dailyCost + "\r\n" +
                "Weight: " + weight + "\r\n" +
                "Age: " + age + "\r\n" +
                "Colour: " + color + "\r\n" +
                "Amount of Milk: " + amtMilk + "\r\n";
            return CowString;
        }

        //Cow profitability is unique to Cows and must be overridden from the parent-class
        public override double GetDailyProfitability()
        {
            profit = getMilkProfit() - (getWaterCost() + dailyCost + getDailyTax());
            return profit;
        }

        //Milk profit is unique to cows 
        public double getMilkProfit()
        {
            milkProfit = amtMilk * Prices.CowMilkPrice;
            return milkProfit;
        }

        //returns amtMilk
        public override double getAmtMilk()
        {
            return amtMilk;
        }

        //Cow costs are unique to cows, so must be calculated using an overriden method from the parent-class
        public override double getAnimalCosts()
        {
            double profit = GetDailyProfitability();
            double removeProfits = profit - getMilkProfit();
            return removeProfits;
        }
    }
}

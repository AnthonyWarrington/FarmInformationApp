using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDevelopmentFinalProject
{
    class JersyCow : Cow
    {
        public bool isJersy;

        public JersyCow(int id, double amtWater, double dailyCost, double weight, int age, string color, double amtMilk, bool isJersy) : base(id, amtWater, dailyCost, weight, age, color, amtMilk)
        {
            this.isJersy = isJersy;
            animalType = "jersey cow";
        }
        //Jersey Cow taxes are unique to jersey cows, so must be calculated using an overriden method from the parent-class
        public override double getDailyTax()
        {
            dailyTax = weight * Prices.JersyCowTax;
            return dailyTax;
        }

        //Jersey cow costs are unique to jersey cows, so must be calculated using an overriden method from the parent-class
        public override double getAnimalCosts()
        {
            double profit = GetDailyProfitability();
            double removeProfits = profit - getMilkProfit() -getDailyTax();
            return removeProfits;
        }
    }
}

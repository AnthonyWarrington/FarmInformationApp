using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDevelopmentFinalProject
{
    class Dog : Animal
    {
        public Dog(int id, double amtWater, double dailyCost, double weight, int age, string color) : base(id, amtWater, dailyCost, weight, age, color)
        {
            animalType = "dog";
        }

        public override String ShowInf()
        {
            string DogString =
                "Animal type: " + animalType + "\r\n" +
                "ID: " + id + "\r\n" +
                "Amount of Water: " + amtWater + "\r\n" +
                "Daily Cost: " + dailyCost + "\r\n" +
                "Weight: " + weight + "\r\n" +
                "Age: " + age + "\r\n" +
                "Colour: " + color;
            return DogString;
        }

        //Dog profitability is unique because dogs don't get taxed...
        public override double GetDailyProfitability()
        {
            profit = 0 - (getWaterCost() + dailyCost);
            return profit;
        }
    }
}

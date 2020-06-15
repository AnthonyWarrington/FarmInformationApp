using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationDevelopmentFinalProject
{
    //Animal superclass
    abstract class Animal
    {
        //variables common to ALL other animal subclasses
        public int id;
        public double amtWater;
        public double dailyCost;
        public double weight;
        public int age;
        public string color;
        public double waterCost;
        public double dailyTax;
        public string animalType;
        public double profit;

        //constructor
        public Animal(int id, double amtWater, double dailyCost, double weight, int age, string color)
        {
            this.id = id;
            this.amtWater = amtWater;
            this.dailyCost = dailyCost;
            this.weight = weight;
            this.age = age;
            this.color = color;
            animalType = "";
        }

        public virtual String ShowInf()
        {
            return ("No 'ShowInfo' method exists for this animal yet.");
        }

        public virtual double GetDailyProfitability()
        {
            return 0;
        }
        //water cost is common to all animals, so it can be inherited directly from Animal and has no need to be virtual
        public double getWaterCost()
        {
            waterCost = amtWater * Prices.WaterPrice;
            return waterCost;
        }

        //daily tax count is common to all animals except Jersey Cows, so it must be virtual, but can also be inhereted by all other animals
        public virtual double getDailyTax()
        {
            dailyTax = weight * Prices.GovtTaxPerKg;
            return dailyTax;
        }

        public virtual double getAmtMilk()
        {
            return 0;
        }
        //age is common to all animals, so it can be inherited directly from Animal and has no need to be virtual
        public double getAge()
        {
            return age;
        }

        public virtual double getAnimalCosts()
        {
            return GetDailyProfitability();
        }
    }
}

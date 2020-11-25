using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weekly_Meal_Planner
{
    [Serializable]
    public class Ingredient
    {
        public string Name { get; set; }
        public string Measurement { get; set; }
        public float Amount { get; set; }

        public float Calorie { get { return nutrition.Calorie; } set { nutrition.Calorie = value; } }
        public float Carb { get { return nutrition.Carb; } set { nutrition.Carb = value; } }
        public float Fat { get { return nutrition.Fat; } set { nutrition.Fat = value; } }
        public float Protein { get { return nutrition.Protein; } set { nutrition.Protein = value; } }

        public Nutrition nutrition;

        public Ingredient(string name, string measurement, float amount, float calorie = 0, float carb = 0, float fat = 0, float protein = 0)
        {
            this.Name = name;
            this.Measurement = measurement;
            this.Amount = amount;
            this.nutrition = new Nutrition(calorie, carb, fat, protein);
        }
    }
}

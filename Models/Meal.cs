using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weekly_Meal_Planner
{
    [Serializable]
    public class Meal
    {   
        public Nutrition nutrition;
        public List<Ingredient> ingredients;

        public string Name { get; set; }
        public MealType Type { get; set; }
        public DayOfWeek Day { get; set; }

        public Meal()
        {
            this.nutrition = new Nutrition();
            this.ingredients = new List<Ingredient>();
        }

        public Meal(string name, MealType mealType, DayOfWeek day, List<Ingredient> ingredients)
        {
            Name = name;
            Type = mealType;
            Day = day;
            nutrition = new Nutrition();
            this.ingredients = ingredients;
            CalculateNutrition();
        }

        public void CalculateNutrition()
        {
            // zero out nutrition details
            this.nutrition.ResetNutrition();

            // sum up nutrition from ingredients
            foreach(Ingredient ing in ingredients)
            {
                nutrition.Calorie += ing.nutrition.Calorie;
                nutrition.Carb += ing.nutrition.Carb;
                nutrition.Fat += ing.nutrition.Fat;
                nutrition.Protein += ing.nutrition.Protein;
            }
        }
    }
}

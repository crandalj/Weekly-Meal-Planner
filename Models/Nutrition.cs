using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Weekly_Meal_Planner
{
    public class Nutrition : INotifyPropertyChanged
    {
        private float _calorie = 0;
        private float _carb = 0;
        private float _fat = 0;
        private float _protein = 0;
        public float Calorie { get { return _calorie; } set { _calorie = value; OnPropertyChanged(); } }
        public float Carb { get { return _carb; } set { _carb = value; OnPropertyChanged(); } }
        public float Fat { get { return _fat; } set { _fat = value; OnPropertyChanged(); } }
        public float Protein { get { return _protein; } set { _protein = value; OnPropertyChanged(); } }

        public Nutrition(float calorie = 0, float carb = 0, float fat = 0, float protein = 0)
        {
            this.Calorie = calorie;
            this.Carb = carb;
            this.Fat = fat;
            this.Protein = protein;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        public void ResetNutrition()
        {
            this.Calorie = 0;
            this.Carb = 0;
            this.Fat = 0;
            this.Protein = 0;
        }
    }
}

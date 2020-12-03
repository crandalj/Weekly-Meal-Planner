using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Weekly_Meal_Planner
{
    public class Day : INotifyPropertyChanged
    {
        public ObservableCollection<Meal> Meals { get; set; }

        public DateTime Date { get; set; }

        private Nutrition _nutrition;

        public event PropertyChangedEventHandler PropertyChanged;

        public DayOfWeek DayOfWeek { get; set; }

        public Day(DayOfWeek day, DateTime date)
        {
            DayOfWeek = day;
            Meals = new ObservableCollection<Meal>();
            Date = date;
            this._nutrition = new Nutrition();
        }

        [NotifyParentProperty(true)]
        public Nutrition Nutrition
        {
            get
            {
                if (_nutrition == null)
                {
                    _nutrition = new Nutrition();
                    return _nutrition;
                }
                return _nutrition;
            }
            set
            {
                _nutrition = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        public void CalculateNutrition()
        {
            // zero out nutrition details
            _nutrition.ResetNutrition();

            foreach(Meal meal in Meals)
            {
                foreach(Ingredient ing in meal.ingredients)
                {
                    _nutrition.Calorie += ing.nutrition.Calorie;
                    _nutrition.Carb += ing.nutrition.Carb;
                    _nutrition.Fat += ing.nutrition.Fat;
                    _nutrition.Protein += ing.nutrition.Protein;
                }
            }

            Nutrition = _nutrition;
        }
    }
}

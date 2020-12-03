using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Weekly_Meal_Planner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public static DataController dataController;
        private Week _week;
        public MainWindow()
        {
            InitializeComponent();

            dataController = new DataController();

            // generate dates for current week
            List<DateTime> dates = new List<DateTime>();
            DateTime sunday = DateTime.Now.StartOfWeek(DayOfWeek.Sunday);

            for (int x = 0; x < 7; x++)
            {
                DayOfWeek day = (DayOfWeek)x;
                DateTime date = sunday.AddDays(x);
                Day newDay = new Day(day, date.Date);
                //Console.WriteLine(day + " " + date);
                Days.Add(newDay);
                RefreshMealsForDay(newDay);
            }

            DataContext = this;
        }

        public ObservableCollection<Day> Days { get; } = new ObservableCollection<Day>();

        private void NewMeal(object sender, RoutedEventArgs e)
        {
            // Instantiate meal dialog box
            var dlg = new MealEdit
            {
                Owner = this
            };

            // Open dialog box
            dlg.ShowDialog();

            // Process data from box if user clicks Save
            if(dlg.DialogResult == true)
            {
                int dayIndex = (int)dlg.NewMeal.Day;

                // Insert meal to db
                dlg.NewMeal.Date = Days[dayIndex].Date;
                Console.WriteLine("new meal added to " + dlg.NewMeal.Date + " " + dlg.NewMeal.Day);
                dataController.AddMeal(dlg.NewMeal);

                // Refresh meals in view for updated day
                RefreshMealsForDay(Days[dayIndex]);
            }
        }
        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Meal meal = (Meal)(sender as ListView).SelectedItem;
            
            if (meal == null)
            {
                Console.WriteLine("meal was null");
                return;
            }

            // Send Meal to MealView
            var dlg = new MealEdit(meal) { Owner = this };

            dlg.ShowDialog();
            
            // handle result
            if(dlg.DialogResult == true)
            {
                // update meal
                dataController.UpdateMeal(dlg.NewMeal);

                // Day can be changed during editing so may need to refresh where it came from as well
                if(dlg.NewMeal.Day != meal.Day)
                {
                    RefreshMealsForDay(Days[(int)meal.Day]);
                }

                RefreshMealsForDay(Days[(int)dlg.NewMeal.Day]);
            }
        }

        internal void DeleteMeal(long id, int day)
        {
            dataController.DeleteMealById(id);
            RefreshMealsForDay(Days[day]);
        }

        private void ResetWeek_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 0; x < 7; x++)
            {
                Days[x].Meals.Clear();
                Days[x].CalculateNutrition();
            }
        }

        private void RefreshMealsForDay(Day day)
        {
            day.Meals.Clear();

            // retrieve meals from DB
            List<Meal> meals = dataController.GetMealsForDay(day.Date);

            // Add to day's meals list
            foreach(Meal meal in meals)
            {
                day.Meals.Add(meal);
            }

            // Refresh day's nutrition
            day.CalculateNutrition();
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
    }
}

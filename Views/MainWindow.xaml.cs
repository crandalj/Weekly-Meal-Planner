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

            for (int x = 0; x < 7; x++)
            {
                DayOfWeek day = (DayOfWeek)x;
                Day newDay = new Day(day);
                Days.Add(newDay);
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

            // configure box

            // Open dialog box
            dlg.ShowDialog();

            // Process data from box if user clicks Save
            if(dlg.DialogResult == true)
            {
                // handle saving meal & updating UI
                int dayIndex = (int)dlg.NewMeal.Day;
                //_week.days[dayIndex].meals.Add(dlg.NewMeal);
                Days[dayIndex].Meals.Add(dlg.NewMeal);
                Days[dayIndex].CalculateNutrition();
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

            int oldDay = (int)meal.Day;
            int oldIndex = (sender as ListView).SelectedIndex;

            // Send Meal to MealView
            var dlg = new MealEdit(meal, oldIndex) { Owner = this };

            dlg.ShowDialog();
            
            // handle result
            if(dlg.DialogResult == true)
            {
                int day = (int)dlg.NewMeal.Day;

                // Day can be changed during editing
                if(day != oldDay)
                {
                    // remove old meal from old day meal list
                    Days[oldDay].Meals.RemoveAt(oldIndex);
                    Days[oldDay].CalculateNutrition();
                    // Add meal to Day's meal list
                    Days[day].Meals.Add(dlg.NewMeal);
                    Days[day].CalculateNutrition();
                }
                else
                {
                    // Add meal to Day's meal list
                    Days[day].Meals[oldIndex] = dlg.NewMeal;
                    Days[day].CalculateNutrition();
                }
            }
        }

        internal void DeleteMeal(int index, int day)
        {
            Days[day].Meals.RemoveAt(index);
            Days[day].CalculateNutrition();
        }

        private void SaveWeek_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        private void LoadWeek_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        private void ResetWeek_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 0; x < 7; x++)
            {
                Days[x].Meals.Clear();
                Days[x].CalculateNutrition();
            }
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

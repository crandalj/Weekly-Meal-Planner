using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;


namespace Weekly_Meal_Planner
{
    /// <summary>
    /// Interaction logic for MealEdit.xaml
    /// </summary>
    public partial class MealEdit : Window
    {
        public Meal NewMeal { get; set; }

        private int _originalDay;
        private long _meal_id;

        public MealEdit()
        {
            InitializeComponent();
            DataContext = this;

            DeleteMealButton.IsEnabled = false;
        }

        public MealEdit(Meal meal)
        {
            InitializeComponent();
            DataContext = this;
            
            _originalDay = (int)meal.Day;
            _meal_id = meal.Id;

            // prepare UI
            DaySelection.SelectedIndex = _originalDay;
            MealSelection.SelectedIndex = (int)meal.Type;
            Title.Content = "Editing a meal";
            MealName.Text = meal.Name;
            foreach(Ingredient ing in meal.ingredients)
            {
                Ingredients.Add(ing);
            }
            DeleteMealButton.IsEnabled = true;
        }

        public ObservableCollection<Ingredient> Ingredients { get; } = new ObservableCollection<Ingredient>();

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Meal edit canceled
            DialogResult = false;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (MealName.Text == "") return;
            if (MealSelection.SelectedIndex == -1 || MealSelection.SelectedItem == null) return;
            if (DaySelection.SelectedIndex == -1 || DaySelection.SelectedItem == null) return;
            
            string mealName = MealName.Text;
            MealType type = (MealType)MealSelection.SelectedItem;
            DayOfWeek day = (DayOfWeek)DaySelection.SelectedItem;
            List<Ingredient> ingredients = new List<Ingredient>(Ingredients);
            
            NewMeal = new Meal(mealName, type, day, ingredients);
 
            DialogResult = true;
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate ingredient dialog box
            var dlg = new IngredientEdit
            {
                Owner = this
            };

            // Open dialog box
            dlg.ShowDialog();

            // Process data from box if user clicks Save
            if (dlg.DialogResult == true)
            {
                // handle saving ingredient & updating UI
                Ingredients.Add(dlg.NewIngredient);
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            int index = IngredientList.SelectedIndex;
            Ingredient ingredient = (Ingredient)IngredientList.SelectedItem;

            if (ingredient == null) return;

            var dlg = new IngredientEdit(ingredient) { Owner = this };

            dlg.ShowDialog();

            if(dlg.DialogResult == true)
            {
                Ingredients[index] = dlg.NewIngredient;
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            int index = IngredientList.SelectedIndex;

            Ingredients.RemoveAt(index);
        }

        private void deleteMealButton_Click(object sender, RoutedEventArgs e)
        {
            var owner = this.Owner as MainWindow;

            owner.DeleteMeal(_meal_id, _originalDay);

            DialogResult = false;
        }
    }
}

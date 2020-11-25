using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Weekly_Meal_Planner
{
    /// <summary>
    /// Interaction logic for IngredientEdit.xaml
    /// </summary>
    public partial class IngredientEdit : Window
    {
        public Ingredient NewIngredient { get; set; }

        public IngredientEdit()
        {
            InitializeComponent();
        }

        public IngredientEdit(Ingredient ingredient)
        {
            InitializeComponent();
            IngName.Text = ingredient.Name;
            IngMeasurement.Text = ingredient.Measurement;
            IngAmount.Text = ingredient.Amount.ToString();
            IngCalorie.Text = ingredient.Calorie.ToString();
            IngCarb.Text = ingredient.Carb.ToString();
            IngFat.Text = ingredient.Fat.ToString();
            IngProtein.Text = ingredient.Protein.ToString();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Meal edit canceled
            DialogResult = null;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            // validate
            if (IngName.Text == "") return;
            if (IngMeasurement.Text == "") return;
            if (IngAmount.Text == "") return;
            if (IngCalorie.Text == "") return;
            if (IngCarb.Text == "") return;
            if (IngFat.Text == "") return;
            if (IngProtein.Text == "") return;

            string ingName = IngName.Text;       
            string ingMeasurement = IngMeasurement.Text;
            float ingAmount = Convert.ToSingle(IngAmount.Text);           
            float ingCalorie = Convert.ToSingle(IngCalorie.Text);           
            float ingCarb = Convert.ToSingle(IngCarb.Text);           
            float ingFat = Convert.ToSingle(IngFat.Text);           
            float ingProtein = Convert.ToSingle(IngProtein.Text);

            NewIngredient = new Ingredient(ingName, ingMeasurement, ingAmount, ingCalorie, ingCarb, ingFat, ingProtein);

            DialogResult = true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            // Validates for decimal values
            decimal result;
            e.Handled = !decimal.TryParse((sender as TextBox).Text + e.Text, out result);
        }
    }
}

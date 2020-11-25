using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weekly_Meal_Planner
{
    public enum MealType {
        [Description("Breakfast")]
        BREAKFAST,
        [Description("Lunch")]
        LUNCH,
        [Description("Dinner")]
        DINNER,
        [Description("Snack")]
        SNACK }
    public enum DayOfWeek {
        [Description("Sunday")]
        SUNDAY,
        [Description("Monday")]
        MONDAY,
        [Description("Tuesday")]
        TUESDAY,
        [Description("Wednesday")]
        WEDNESDAY,
        [Description("Thursday")]
        THURSDAY,
        [Description("Friday")]
        FRIDAY,
        [Description("Saturday")]
        SATURDAY }
}

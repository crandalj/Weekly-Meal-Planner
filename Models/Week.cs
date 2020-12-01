using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Weekly_Meal_Planner
{
    public class Week
    {
        public List<Day> Days { get; set; }

        public Week()
        {

        }

        public Week(List<Day> days)
        {
            Days = days;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Weekly_Meal_Planner
{
    public class DataController
    {
        private SQLiteConnection _connection;
        private string _connection_string = "Data Source=" + System.AppDomain.CurrentDomain.BaseDirectory + "data.db;Version=3;New=False;Compress=True";

        /* 
         DB SCHEMA
        Tables
        -- Meal
        -- Ingredient
        -- Nutrition

        CREATE TABLE "Meal" (
	        "id"	INTEGER NOT NULL UNIQUE,
	        "name"	TEXT NOT NULL,
	        "type"	TEXT NOT NULL CHECK(type IN ('BREAKFAST', 'LUNCH', 'DINNER', 'SNACK')),
	        "date"	NUMERIC NOT NULL,
	        PRIMARY KEY("id" AUTOINCREMENT)
        )

        CREATE TABLE "Ingredient" (
	        "id"	INTEGER NOT NULL UNIQUE,
	        "meal"	INTEGER NOT NULL,
	        "name"	TEXT NOT NULL,
	        "measurement"	TEXT NOT NULL,
	        "amount"	REAL NOT NULL,
            "calorie"   INTEGER NOT NULL DEFAULT 0,
            "protein"   REAL NOT NULL DEFAULT 0,
            "fat"       REAL NOT NULL DEFAULT 0,
            "carb"      REAL NOT NULL DEFAULT 0,
	        FOREIGN KEY("meal") REFERENCES "Meal"("id"),
	        PRIMARY KEY("id" AUTOINCREMENT)
        )

         */

        public DataController()
        {
            _connection = CreateConnection();
        }

        private SQLiteConnection CreateConnection()
        {
            // Create database connection
            SQLiteConnection sqlite_conn;
            sqlite_conn = new SQLiteConnection(_connection_string);

            // Open connection
            try
            {
                using (sqlite_conn) 
                {
                    sqlite_conn.Open();
                    // Meal table
                    SQLiteCommand _command = new SQLiteCommand();
                    _command.Connection = sqlite_conn;
                    _command.CommandText = @"CREATE TABLE IF NOT EXISTS [Meal] (
                        [id]    INTEGER NOT NULL UNIQUE PRIMARY KEY AUTOINCREMENT,
                        [name]  TEXT NOT NULL,
	                    [type]  TEXT NOT NULL CHECK(type IN ('BREAKFAST', 'LUNCH', 'DINNER', 'SNACK')),
	                    [date]  NUMERIC NOT NULL
                    )";
                    _command.ExecuteNonQuery();
                    // Ingredient table
                    _command.CommandText = @"CREATE TABLE IF NOT EXISTS [Ingredient] (
                        [id]    INTEGER NOT NULL UNIQUE,
                        [meal]  INTEGER NOT NULL,
	                    [name]  TEXT NOT NULL,
	                    [measurement]   TEXT NOT NULL,
	                    [amount]    REAL NOT NULL,
                        [calorie]   INTEGER NOT NULL DEFAULT 0,
                        [protein]   REAL NOT NULL DEFAULT 0,
                        [fat]       REAL NOT NULL DEFAULT 0,
                        [carb]      REAL NOT NULL DEFAULT 0,
	                    FOREIGN KEY([meal]) REFERENCES [Meal]([id]),
	                    PRIMARY KEY([id] AUTOINCREMENT)
                    )";
                    _command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return sqlite_conn;
        }

        public void AddMeal(Meal meal)
        {
            try
            {
                // Create database connection
                _connection = new SQLiteConnection(_connection_string);
                using (_connection)
                {
                    _connection.Open();

                    // begin transaction
                    SQLiteTransaction transaction = null;
                    transaction = _connection.BeginTransaction();

                    SQLiteCommand _command = new SQLiteCommand();
                    _command.Connection = _connection;
                    
                    // Insert meal into database
                    _command.CommandText = "INSERT INTO [Meal](name, type, date) VALUES(@name, @type, @date)";
                    _command.Parameters.AddWithValue("@name", meal.Name);
                    _command.Parameters.AddWithValue("@type", meal.Type.ToString());
                    _command.Parameters.AddWithValue("@date", meal.Date);
                    _command.Prepare();
                    _command.ExecuteNonQuery();

                    long meal_id = _connection.LastInsertRowId;

                    // Insert each new ingredient
                    foreach(Ingredient ing in meal.ingredients)
                    {
                        // Insert Ingredient
                        _command.CommandText = "INSERT INTO [Ingredient](meal, name, measurement, amount, calorie, protein, fat, carb) VALUES(@meal, @name, @measurement, @amount, @calorie, @protein, @fat, @carb)";
                        _command.Parameters.AddWithValue("@meal", meal_id);
                        _command.Parameters.AddWithValue("@name", ing.Name);
                        _command.Parameters.AddWithValue("@measurement", ing.Measurement);
                        _command.Parameters.AddWithValue("@amount", ing.Amount);
                        _command.Parameters.AddWithValue("@calorie", ing.Calorie);
                        _command.Parameters.AddWithValue("@protein", ing.Protein);
                        _command.Parameters.AddWithValue("@fat", ing.Fat);
                        _command.Parameters.AddWithValue("@carb", ing.Carb);
                        _command.Prepare();
                        _command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        public List<Meal> GetMealsForDay(DateTime date)
        {
            List<Meal> meals = new List<Meal>();

            // get meals with matching date
            try
            {
                // Create database connection
                _connection = new SQLiteConnection(_connection_string);
                using (_connection)
                {
                    _connection.Open();

                    SQLiteCommand _command = new SQLiteCommand();
                    _command.Connection = _connection;

                    _command.CommandText = "SELECT * FROM [Meal] WHERE date = @date";
                    _command.Parameters.AddWithValue("@date", date);
                    _command.Prepare();
                    
                    SQLiteDataReader reader = _command.ExecuteReader();
                    // Read will process row by row of the returnset, ie each meal row
                    while (reader.Read())
                    {
                        Meal newMeal = new Meal();
                        //Console.WriteLine(reader.GetInt32(0));
                        newMeal.Id = reader.GetInt32(0);
                        //Console.WriteLine(reader.GetString(1));
                        newMeal.Name = reader.GetString(1);
                        //Console.WriteLine(reader.GetString(2));
                        newMeal.Type = (MealType)Enum.Parse(typeof(MealType), reader.GetString(2));
                        //Console.WriteLine(reader.GetDateTime(3));
                        newMeal.Date = reader.GetDateTime(3);
                        newMeal.Day = newMeal.Date.DayOfWeek;
                        meals.Add(newMeal);
                    }
                    reader.Close();

                    foreach(Meal meal in meals)
                    {
                        // get ingredients for each meal
                        _command.CommandText = "SELECT * FROM [Ingredient] WHERE meal = @meal";
                        _command.Parameters.AddWithValue("@meal", meal.Id);
                        _command.Prepare();
                        reader = _command.ExecuteReader();
                        while (reader.Read())
                        {
                            Ingredient newIng = new Ingredient();
                            newIng.Id = reader.GetInt32(0);
                            newIng.Meal = reader.GetInt32(1);
                            newIng.Name = reader.GetString(2);
                            newIng.Measurement = reader.GetString(3);
                            newIng.Amount = reader.GetFloat(4);
                            newIng.Calorie = reader.GetInt32(5);
                            newIng.Protein = reader.GetFloat(6);
                            newIng.Fat = reader.GetFloat(7);
                            newIng.Carb = reader.GetFloat(8);
                            meal.ingredients.Add(newIng);
                        }
                        // calculate meal nutrition
                        meal.CalculateNutrition();
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            // for each meal, get ingredients then build Meal object to add to meals

            return meals;
        }

        public void UpdateMeal(Meal meal)
        {

        }

        public void DeleteMealById(long id)
        {
            try
            {
                // Create database connection
                _connection = new SQLiteConnection(_connection_string);
                using (_connection)
                {
                    _connection.Open();

                    // begin transaction
                    SQLiteTransaction transaction = null;
                    transaction = _connection.BeginTransaction();

                    SQLiteCommand _command = new SQLiteCommand();
                    _command.Connection = _connection;

                    _command.CommandText = "DELETE FROM [Meal] WHERE id = @id";
                    _command.Parameters.AddWithValue("@id", id);
                    _command.Prepare();
                    _command.ExecuteNonQuery();

                    _command.CommandText = "DELETE FROM [Ingredient] WHERE meal = @meal";
                    _command.Parameters.AddWithValue("@meal", id);
                    _command.Prepare();
                    _command.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public Meal GetMealById(int id)
        {
            SQLiteCommand _command = new SQLiteCommand();
            _command.CommandText = "Select * FROM meals WHERE id = " + id;
            
            try
            {
                SQLiteDataReader reader = _command.ExecuteReader();
                // reader["column_name"] to access meal columns
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }


    }
}


    


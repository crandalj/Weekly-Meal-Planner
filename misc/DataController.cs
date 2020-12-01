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
        private SQLiteDataAdapter _DB;
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
	        FOREIGN KEY("meal") REFERENCES "Meal"("id"),
	        PRIMARY KEY("id" AUTOINCREMENT)
        )
        
        CREATE TABLE "Nutrition" (
	        "id"	INTEGER NOT NULL UNIQUE,
	        "meal"	INTEGER NOT NULL,
	        "ingredient"	INTEGER NOT NULL,
	        "calorie"	INTEGER NOT NULL DEFAULT 0,
	        "protein"	REAL NOT NULL DEFAULT 0,
	        "fat"	REAL NOT NULL DEFAULT 0,
	        "carb"	REAL NOT NULL DEFAULT 0,
	        PRIMARY KEY("id" AUTOINCREMENT),
	        FOREIGN KEY("meal") REFERENCES "Meal"("id"),
	        FOREIGN KEY("ingredient") REFERENCES "Ingredient"("id")
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
            Console.WriteLine(_connection_string);
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
	                    FOREIGN KEY([meal]) REFERENCES [Meal]([id]),
	                    PRIMARY KEY([id] AUTOINCREMENT)
                    )";
                    _command.ExecuteNonQuery();
                    // Nutrition table
                    _command.CommandText = @"CREATE TABLE IF NOT EXISTS [Nutrition] (
                        [id]    INTEGER NOT NULL UNIQUE,
                        [meal]  INTEGER NOT NULL,
	                    [ingredient]    INTEGER NOT NULL,
	                    [calorie]   INTEGER NOT NULL DEFAULT 0,
	                    [protein]   REAL NOT NULL DEFAULT 0,
	                    [fat]   REAL NOT NULL DEFAULT 0,
	                    [carb]  REAL NOT NULL DEFAULT 0,
	                    PRIMARY KEY([id] AUTOINCREMENT),
	                    FOREIGN KEY([meal]) REFERENCES [Meal]([id]),
	                    FOREIGN KEY([ingredient]) REFERENCES [Ingredient]([id])
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
            SQLiteCommand _command = new SQLiteCommand();
            _command.CommandText = @"";
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


    


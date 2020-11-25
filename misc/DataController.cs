using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

public class DataController
{
    public SQLiteConnection connection;
    public DataController(){
        
        connection = CreateConnection();
    }

    static SQLiteConnection CreateConnection()
    {
        // Create database connection
        SQLiteConnection sqlite_conn;
        sqlite_conn = new SQLiteConnection("Data Source=database.db;Version=3;New=True;Compress=True");

        // Open connection
        try
        {
            sqlite_conn.Open();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }

        return sqlite_conn;
    }

    
}

    


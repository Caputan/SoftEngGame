using System;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class Database : MonoBehaviour
{
    private void Start()
    {
        // Create database
        string connection = $"URI=file:{Application.persistentDataPath}/data";
        Debug.Log($"Database path: {Application.persistentDataPath}/data");

        // Open connection
        IDbConnection dbConnection = new SqliteConnection(connection);
        dbConnection.Open();

        // Used to send SQL queries
        IDbCommand dbCommand;
        
        // Create tables
        string[] createTableQueries =
        {
            "CREATE TABLE IF NOT EXISTS Characters (" +
            "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
            "name TEXT NOT NULL UNIQUE" +
            ")",
            "CREATE TABLE IF NOT EXISTS Locations (" +
            "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE," +
            "name TEXT NOT NULL," +
            "parentLocation INTEGER NOT NULL" +
            ")",
            "CREATE TABLE IF NOT EXISTS Progress (" +
            "character_id INTEGER NOT NULL," +
            "location_id INTEGER NOT NULL" +
            "FOREIGN KEY(\"character_id\") REFERENCES \"Characters\"(\"id\")," +
            "FOREIGN KEY(\"location_id\") REFERENCES \"Locations\"(\"id\")" +
            ")"
        };
        foreach (var createTableQuery in createTableQueries)
        {
            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = createTableQuery;
            dbCommand.ExecuteReader();
        }

        // Insert values in table
        try // Value must be unique so we should check for that
        {
            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = "INSERT INTO Characters ('name') VALUES ('Geralt')";
            dbCommand.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        // Read and print all values in table
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "SELECT * FROM Characters";
        IDataReader reader = dbCommand.ExecuteReader();
        while (reader.Read())
        {
            Debug.Log("id: " + reader[0]);
            Debug.Log("name: " + reader[1]);
        }

        // Close connection
        dbConnection.Close();
    }
}

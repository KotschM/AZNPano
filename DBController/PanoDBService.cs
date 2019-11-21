using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AZNPano.DBController
{
    public class PanoDBService : IDBService
    {
        private SqliteConnection sqliteconnection;
        public void Init(string dataName){
            sqliteconnection = new SqliteConnection($"Data Source={dataName}");
            sqliteconnection.Open();

            CreateIfNotExist("Typ");
            CreateIfNotExist("Bereich");
            CreateIfNotExist("Gehalt");
            CreateIfNotExist("GehaltFaktor");
            CreateIfNotExist("Schicht");
            CreateIfNotExist("Zeitraum");

            insertDemoData();

            sqliteconnection.Close();
        
        }
        private void CreateIfNotExist(string relation)
        {
            string sql = $"SELECT name FROM sqlite_master WHERE name='{relation}'";
            object r = QueryScalar(sql);
            
            Console.WriteLine(relation);
            if (!(r != null && r.ToString() == relation))
            {
                sql = "";
                switch (relation)
                {
                    case "Typ":
                        sql = $"CREATE TABLE {relation} (T_ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Bezeichnung TEXT NOT NULL)";
                    break;
                    case "Bereich":
                        sql = $"CREATE TABLE {relation} (B_ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Bezeichnung TEXT NOT NULL)";
                    break;
                    case "Gehalt":
                        sql = $"CREATE TABLE {relation} (G_ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,"+
                                                        " Betrag REAL NOT NULL, Beginn TIMESTAMP NOT NULL, Ende TIMESTAMP)";
                    break;
                    case "GehaltFaktor":
                        sql = $"CREATE TABLE {relation} (GF_ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,"+
                                                        " Bezeichnung TEXT NOT NULL, Faktor REAL NOT NULL,"+
                                                        " Beginn TIMESTAMP NOT NULL, Ende TIMESTAMP)";
                    break;
                    case "Schicht":
                        sql = $"CREATE TABLE {relation} (S_ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,"+
                                                        " T_ID INTEGER NOT NULL, B_ID INTEGER NOT NULL,"+
                                                        " G_ID INTEGER NOT NULL, Beginn TIMESTAMP NOT NULL,"+
                                                        " Ende TIMESTAMP, FOREIGN KEY(T_ID) REFERENCES Typ(T_ID),"+
                                                        " FOREIGN KEY(B_ID) REFERENCES Bereich(B_ID), FOREIGN KEY(G_ID) REFERENCES Gehalt(G_ID))";
                    break;
                    case "Zeitraum":
                        sql = $"CREATE TABLE {relation} (S_ID INTEGER, GF_ID INTEGER, Beginn TIMESTAMP NOT NULL,"+
                                                        " Ende TIMESTAMP, PRIMARY KEY (S_ID, GF_ID))";
                    break;
                }
                Update(sql);                   
                
            }
        }

        private void insertDemoData(){
            string sql = "INSERT INTO GehaltFaktor (Bezeichnung, Faktor, Beginn) values ('Pause', 0.0, '2010-08-28T13:40:02.200');";
            Update(sql);
        }

        public void Update(string sql){
            SqliteCommand insertCommand = new SqliteCommand(sql, sqliteconnection);
            insertCommand.ExecuteNonQuery();
        }

        public object QueryScalar(string sql){
            using (SqliteCommand command = sqliteconnection.CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                return command.ExecuteScalar();
            }
        }

        public SqliteDataReader QueryList(string sql){
            using (SqliteCommand command = sqliteconnection.CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                return command.ExecuteReader();
            }
        }
        private void Test(){
            using (SqliteCommand command = sqliteconnection.CreateCommand())
            {
                command.CommandText = @"SELECT DISTINCT Key, Value FROM test";
                command.CommandType = CommandType.Text;
                SqliteDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    Console.WriteLine(r["value"] + " " + r["key"]);
                }
            }
        }
    }
}
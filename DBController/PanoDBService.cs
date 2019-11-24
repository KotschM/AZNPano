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
                        sql = $"CREATE TABLE {relation} (T_ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Bezeichnung INTEGER NOT NULL)";
                    break;
                    case "Bereich":
                        sql = $"CREATE TABLE {relation} (B_ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Bezeichnung INTEGER NOT NULL)";
                    break;
                    case "Gehalt":
                        sql = $"CREATE TABLE {relation} (G_ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,"+
                                                        " Betrag REAL NOT NULL, Beginn TIMESTAMP NOT NULL, Ende TIMESTAMP)";
                    break;
                    case "GehaltFaktor":
                        sql = $"CREATE TABLE {relation} (GF_ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,"+
                                                        " Bezeichnung INTEGER NOT NULL, Faktor REAL NOT NULL,"+
                                                        " Beginn TIMESTAMP NOT NULL, Ende TIMESTAMP)";
                    break;
                    case "Schicht":
                        sql = $"CREATE TABLE {relation} (S_ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,"+
                                                        " T_ID INTEGER NOT NULL, B_ID INTEGER NOT NULL,"+
                                                        " G_ID INTEGER NOT NULL, Beginn TIMESTAMP NOT NULL,"+
                                                        " Ende TIMESTAMP NOT NULL, FOREIGN KEY(T_ID) REFERENCES Typ(T_ID),"+
                                                        " FOREIGN KEY(B_ID) REFERENCES Bereich(B_ID), FOREIGN KEY(G_ID) REFERENCES Gehalt(G_ID))";
                    break;
                    case "Zeitraum":
                        sql = $"CREATE TABLE {relation} (Z_ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, S_ID INTEGER NOT NULL, GF_ID INTEGER NOT NULL, Beginn TIMESTAMP NOT NULL,"+
                                                        " Ende TIMESTAMP)";
                    break;
                }
                Update(sql);                   
                
            }
        }

        private void insertDemoData(){
            string sql = "INSERT INTO Typ (Bezeichnung) values (1);"+
                        "INSERT INTO Typ (Bezeichnung) values (2);"+
                        "INSERT INTO Typ (Bezeichnung) values (3);"+
                        "INSERT INTO Typ (Bezeichnung) values (4);"+

                        "INSERT INTO Bereich (Bezeichnung) values (1);"+
                        "INSERT INTO Bereich (Bezeichnung) values (2);"+
                        "INSERT INTO Bereich (Bezeichnung) values (3);"+
                        "INSERT INTO Bereich (Bezeichnung) values (4);"+
                        "INSERT INTO Bereich (Bezeichnung) values (5);"+
                        "INSERT INTO Bereich (Bezeichnung) values (6);"+
                        "INSERT INTO Bereich (Bezeichnung) values (7);"+

                        "INSERT INTO Gehalt (Betrag, Beginn, Ende) values (12.50, '2018-01-01T00:00:00.000', '2018-12-31T00:00:00.000');" +
                        "INSERT INTO Gehalt (Betrag, Beginn) values (12.94, '2019-01-01T00:00:00.000');" +

                        "INSERT INTO GehaltFaktor (Bezeichnung, Faktor, Beginn, Ende) values (1, 1.25, '2018-01-01T00:00:00.000', '2018-12-31T00:00:00.000');" +
                        "INSERT INTO GehaltFaktor (Bezeichnung, Faktor, Beginn) values (1, 1.35, '2019-01-01T00:00:00.000');" +
                        "INSERT INTO GehaltFaktor (Bezeichnung, Faktor, Beginn, Ende) values (2, 1.30, '2018-01-01T00:00:00.000', '2018-12-31T00:00:00.000');" +
                        "INSERT INTO GehaltFaktor (Bezeichnung, Faktor, Beginn) values (2, 1.40, '2019-01-01T00:00:00.000');" +
                        "INSERT INTO GehaltFaktor (Bezeichnung, Faktor, Beginn, Ende) values (3, 1.35, '2018-01-01T00:00:00.000', '2018-12-31T00:00:00.000');" +
                        "INSERT INTO GehaltFaktor (Bezeichnung, Faktor, Beginn) values (3, 1.45, '2019-01-01T00:00:00.000');" +
                        "INSERT INTO GehaltFaktor (Bezeichnung, Faktor, Beginn, Ende) values (4, 0.0, '2018-01-01T00:00:00.000', '2018-12-31T00:00:00.000');" +
                        "INSERT INTO GehaltFaktor (Bezeichnung, Faktor, Beginn) values (4, 0.0, '2019-01-01T00:00:00.000');" +
                        "INSERT INTO GehaltFaktor (Bezeichnung, Faktor, Beginn, Ende) values (5, 1.0, '2018-01-01T00:00:00.000', '2018-12-31T00:00:00.000');" +
                        "INSERT INTO GehaltFaktor (Bezeichnung, Faktor, Beginn) values (5, 1.0, '2019-01-01T00:00:00.000');" +
                        "INSERT INTO GehaltFaktor (Bezeichnung, Faktor, Beginn, Ende) values (6, 1.0, '2018-01-01T00:00:00.000', '2018-12-31T00:00:00.000');" +
                        "INSERT INTO GehaltFaktor (Bezeichnung, Faktor, Beginn) values (6, 1.0, '2019-01-01T00:00:00.000');" +

                        "INSERT INTO Schicht (T_ID, B_ID, G_ID, Beginn, Ende) values (1, 1, 2, '2019-11-19T06:45:00.000', '2019-11-19T15:15:00.000');" +
                        "INSERT INTO Schicht (T_ID, B_ID, G_ID, Beginn, Ende) values (2, 1, 2, '2019-11-20T15:00:00.000', '2019-11-20T23:30:00.000');" +

                        "INSERT INTO Zeitraum (S_ID, GF_ID, Beginn, Ende) values (1, 12, '2019-11-19T06:45:00.000', '2019-11-19T09:00:00.000');" +
                        "INSERT INTO Zeitraum (S_ID, GF_ID, Beginn, Ende) values (1, 8, '2019-11-19T09:00:00.000', '2019-11-19T09:30:00.000');" +
                        "INSERT INTO Zeitraum (S_ID, GF_ID, Beginn, Ende) values (1, 12, '2019-11-19T09:30:00.000', '2019-11-19T15:15:00.000');" +
                        "INSERT INTO Zeitraum (S_ID, GF_ID, Beginn, Ende) values (2, 12, '2019-11-20T15:00:00.000', '2019-11-20T19:00:00.000');" +
                        "INSERT INTO Zeitraum (S_ID, GF_ID, Beginn, Ende) values (2, 8, '2019-11-19T09:00:00.000', '2019-11-19T09:30:00.000');" +
                        "INSERT INTO Zeitraum (S_ID, GF_ID, Beginn, Ende) values (2, 12, '2019-11-19T09:30:00.000', '2019-11-19T15:15:00.000');";
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
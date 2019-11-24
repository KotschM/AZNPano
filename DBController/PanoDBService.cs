using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AZNPano.DBModel;

namespace AZNPano.DBController
{
    public class PanoDBService : IDBService
    {
        private SqliteConnection sqliteconnection;
        public void Init(string dataName){
            sqliteconnection = new SqliteConnection($"Data Source={dataName}");
            sqliteconnection.Open();

            //Achtung hier wird die Datenbank gelöscht.
            DeleteAll();
            //Achtung hier wird die Datenbank gelöscht.

            CreateIfNotExist("Typ");
            CreateIfNotExist("Bereich");
            CreateIfNotExist("Gehalt");
            CreateIfNotExist("GehaltFaktor");
            CreateIfNotExist("Schicht");
            CreateIfNotExist("Zeitraum");

            InsertDemoData();

            CreateGehaltModels();
            List<GehaltFaktorModel> test = CreateGehaltFaktorModels();
            /*
            foreach (var item in test)
            {
                Console.WriteLine(item.Key);
            }*/

            sqliteconnection.Close();
        
        }

        private void DeleteAll(){
            string sql = "DROP TABLE Zeitraum;"+
                        " DROP TABLE Schicht;"+
                        " DROP TABLE GehaltFaktor;"+
                        " DROP TABLE Gehalt;"+
                        " DROP TABLE Bereich;"+
                        " DROP TABLE Typ;";
            Update(sql);
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

        private void InsertDemoData(){
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

        public List<GehaltFaktorModel> CreateGehaltFaktorModels(){
            List<GehaltFaktorModel> allGehaltFaktorModels = new List<GehaltFaktorModel>();
            List<(double Faktor, DateTime Start, DateTime End)> faktoren = new List<(double Faktor, DateTime Start, DateTime End)>();
            List<string> loop = new List<string> {
                "Nacht", "Feiertag", "Sonntag", "Pause", "Bereitschaftspause", "Normal"
            };
            foreach (string item in loop)
            {
                using (SqliteCommand command = sqliteconnection.CreateCommand())
                {
                    //Initialisiere die Klasen GehaltFaktorModel.cs
                    command.CommandText = $"SELECT Faktor, Beginn, Ende FROM GehaltFaktor WHERE Bezeichnung = '{item}'";
                    command.CommandType = CommandType.Text;
                    SqliteDataReader r = command.ExecuteReader();
                    while (r.Read())
                    {
                        DateTime beginn = DateTime.ParseExact(r["Beginn"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime ende;
                        if (r["Ende"].ToString() != "")
                        {
                            ende = DateTime.ParseExact(r["Ende"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                        }else
                        {
                            ende = new DateTime();
                        }
                        double faktor = Convert.ToDouble(r["Faktor"]);
                        faktoren.Add((faktor, beginn, ende));
                    }
                    switch (item)
                    {
                        case "Nacht":
                            allGehaltFaktorModels.Add(new GehaltFaktorModel(faktoren, ESchichtGehaltsfaktoren.Nacht));
                        break;
                        case "Feiertag":
                            allGehaltFaktorModels.Add(new GehaltFaktorModel(faktoren, ESchichtGehaltsfaktoren.Feiertag));
                        break;
                        case "Sonntag":
                            allGehaltFaktorModels.Add(new GehaltFaktorModel(faktoren, ESchichtGehaltsfaktoren.Sonntag));
                        break;
                        case "Pause":
                            allGehaltFaktorModels.Add(new GehaltFaktorModel(faktoren, ESchichtGehaltsfaktoren.Pause));
                        break;
                        case "Bereitschaftspause":
                            allGehaltFaktorModels.Add(new GehaltFaktorModel(faktoren, ESchichtGehaltsfaktoren.Bereitschaftspause));
                        break;
                        case "Normal":
                            allGehaltFaktorModels.Add(new GehaltFaktorModel(faktoren, ESchichtGehaltsfaktoren.Normal));
                        break;
                    }
                }
            }
            return allGehaltFaktorModels;
        }
        public GehaltModel CreateGehaltModels(){
            List<(double Faktor, DateTime Start, DateTime End)> Gehaelter = new List<(double Faktor, DateTime Start, DateTime End)>();
            using (SqliteCommand command = sqliteconnection.CreateCommand())
            {
                //Initialisiere die Klasen GehaltFaktorModel.cs
                command.CommandText = "SELECT * FROM Gehalt";
                command.CommandType = CommandType.Text;
                SqliteDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    DateTime beginn = DateTime.ParseExact(r["Beginn"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime ende;
                    if (r["Ende"].ToString() != "")
                    {
                        ende = DateTime.ParseExact(r["Ende"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                    }else
                    {
                        ende = new DateTime();
                    }
                    double betrag = Convert.ToDouble(r["Betrag"]);
                    Gehaelter.Add((betrag, beginn, ende));
                }
            }
            return new GehaltModel(Gehaelter);
        }

        public List<SchichtModel> CreateSchichtModels(){
            List<(double Faktor, DateTime Start, DateTime End)> Gehaelter = new List<(double Faktor, DateTime Start, DateTime End)>();
            using (SqliteCommand command = sqliteconnection.CreateCommand())
            {
                //Initialisiere die Klasen GehaltFaktorModel.cs
                command.CommandText = "SELECT * FROM Gehalt";
                command.CommandType = CommandType.Text;
                SqliteDataReader r = command.ExecuteReader();
                while (r.Read())
                {
                    DateTime beginn = DateTime.ParseExact(r["Beginn"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime ende;
                    if (r["Ende"].ToString() != "")
                    {
                        ende = DateTime.ParseExact(r["Ende"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                    }else
                    {
                        ende = new DateTime();
                    }
                    double betrag = Convert.ToDouble(r["Betrag"]);
                    Gehaelter.Add((betrag, beginn, ende));
                }
            }
            return null;
        }
    }
}
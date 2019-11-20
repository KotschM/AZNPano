using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace AZNPano
{
    class DBService
    {
        private static SqliteConnection sqliteconnection;
        public static void Main2(string[] args)
        {
            sqliteconnection = new SqliteConnection("Data Source=mytest.db");
            sqliteconnection.Open();
            using (SqliteCommand command = sqliteconnection.CreateCommand())
            {
                command.CommandText = "SELECT name FROM sqlite_master WHERE name='Test'";
                var name = command.ExecuteScalar();

                // check test table exist or not 
                // if exist do nothing 
                if (!(name != null && name.ToString() == "Test"))
                {

                    string sql = "create table Test (key varchar(20), value int)";
                    SqliteCommand createCommand = new SqliteCommand(sql, sqliteconnection);
                    createCommand.ExecuteNonQuery();

                    sql = "insert into Test (key, value) values ('key1', 9001);" +
                        "insert into Test (key, value) values ('key2', 9002);" +
                        "insert into Test (key, value) values ('key3', 9003);" +
                        "insert into Test (key, value) values ('key4', 9004);" +
                        "insert into Test (key, value) values ('key5', 9005);" +
                        "insert into Test (key, value) values ('key6', 9006);" +
                        "insert into Test (key, value) values ('key7', 9007);";
                    SqliteCommand insertCommand = new SqliteCommand(sql, sqliteconnection);
                    insertCommand.ExecuteNonQuery();
                }else
                {
                    string sql = "insert into Test (key, value) values ('key2', 9008)";
                    SqliteCommand insertCommand = new SqliteCommand(sql, sqliteconnection);
                    insertCommand.ExecuteNonQuery();
                }
            }


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
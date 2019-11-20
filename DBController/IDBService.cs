using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace AZNPano
{
    public interface IDBService
    {
        void init();

        void update(SqliteCommand sc);

        SqliteDataReader query(SqliteCommand sc);
    }
}
using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace AZNPano.DBController
{
    public interface IDBService
    {
        void Init(string dataName);

        void Update(string sql);

        object QueryScalar(string sql);

        SqliteDataReader QueryList(string sql);
    }
}
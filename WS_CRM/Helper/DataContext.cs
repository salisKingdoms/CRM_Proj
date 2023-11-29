﻿using System.Data;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
namespace WS_CRM.Helper
{
    public class DataContext
    {
        private DbSettings _dbSettings;
        private DbSettingsAct _dbSettingsAct;
        private DbCatalogue _dbCatalogue;   

        public DataContext(IOptions<DbSettings> dbSettings, IOptions<DbSettingsAct> dbSetAct,IOptions<DbCatalogue> dbCatalogue)
        {
            _dbSettings = dbSettings.Value;
            _dbSettingsAct = dbSetAct.Value;
            _dbCatalogue = dbCatalogue.Value;
        }

        public IDbConnection CreateConnection()
        {
            var connectionString = $"Host={_dbSettings.Server}; Database={_dbSettings.Database}; Username={_dbSettings.UserId}; Password={_dbSettings.Password};";
            return new NpgsqlConnection(connectionString);
        }

        public IDbConnection ConnectionActivity()
        {
            var connectionString = $"Host={_dbSettingsAct.Server}; Database={_dbSettingsAct.Database}; Username={_dbSettingsAct.UserId}; Password={_dbSettingsAct.Password};";
            return new NpgsqlConnection(connectionString);
        }
        public IDbConnection ConnectionCatalogue()
        {
            var connectionString = $"Host={_dbCatalogue.Server}; Database={_dbCatalogue.Database}; Username={_dbCatalogue.UserId}; Password={_dbCatalogue.Password};";
            return new NpgsqlConnection(connectionString);
        }

        public async Task Init()
        {
            await _initDatabase();
            await _initTables();
        }

        private async Task _initDatabase()
        {
            // create database if it doesn't exist
            var connectionString = $"Host={_dbSettings.Server}; Database=postgres; Username={_dbSettings.UserId}; Password={_dbSettings.Password};";
            using var connection = new NpgsqlConnection(connectionString);
            var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{_dbSettings.Database}';";
            var dbCount = await connection.ExecuteScalarAsync<int>(sqlDbCount);
            if (dbCount == 0)
            {
                var sql = $"CREATE DATABASE \"{_dbSettings.Database}\"";
                await connection.ExecuteAsync(sql);
            }
        }

        private async Task _initTables()
        {
            // create tables if they don't exist
            using var connection = CreateConnection();
           //wait _initCustomers();

            //async Task _initCustomers()
            //{
            //    var sql = """
            //    CREATE TABLE IF NOT EXISTS customers (
            //        id SERIAL PRIMARY KEY,
            //        name VARCHAR,
            //        FirstName VARCHAR,
            //        LastName VARCHAR,
            //        Email VARCHAR,
            //        Role INTEGER,
            //        PasswordHash VARCHAR
            //    );
            //""";
            //    await connection.ExecuteAsync(sql);
            //}
        }
    }
}

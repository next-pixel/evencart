﻿using EvenCart.Core.Infrastructure;
using EvenCart.Data.Database;
using EvenCart.Services.Installation;

namespace EvenCart.Services.Tests
{
    public class TestDbInit
    {
        public static void SqlServer(string connectionString)
        {
            Db(connectionString, "SqlServer");
        }

        public static void MySql(string connectionString)
        {
            Db(connectionString, "MySql");
        }

        public static void Db(string connectionString, string providerName)
        {
            //seed data
            var dbSettings = DependencyResolver.Resolve<IDatabaseSettings>() as TestDatabaseSettings;
            dbSettings.SetSettings(connectionString, providerName);

            var installationService = new InstallationService(dbSettings);
            installationService.Install();
            installationService.FillRequiredSeedData("admin@store.com", "@#$%^&*", "localhost", "Test Store");
        }
        public class TestDatabaseSettings : IDatabaseSettings
        {
            public string ConnectionString { get; private set; }
            public string ProviderName { get; private set; }

            public void SetSettings(string connectionString, string providerName)
            {
                ConnectionString = connectionString;
                ProviderName = providerName;
            }
            public void LoadSettings()
            {
                //do nothing
            }

            public void WriteSettings(string connectionString, string providerName)
            {
                //do nothing
            }

            public bool HasSettings()
            {
                return true;
            }
        }
    }
}
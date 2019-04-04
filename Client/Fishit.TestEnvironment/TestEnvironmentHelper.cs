﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Fishit.Dal;
using Fishit.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Fishit.TestEnvironment
{
    public static class TestEnvironmentHelper
    {
        private const string InitializationError = "Error while re-initializing database entries.";

        private static List<FishingTrip> FishingTrips =>
            new List<FishingTrip>
            {
                new FishingTrip {Id = 1, Name = "FishingTrip Number 1", Location = "Zurich"},
                new FishingTrip {Id = 2, Name = "FishingTrip Number 2", Location = "Wil"},
                new FishingTrip {Id = 3, Name = "FishingTrip Number 3", Location = "Zurich"},
                new FishingTrip {Id = 4, Name = "FishingTrip Number 4", Location = "Geneva"}
            };

        public static void InitializeTestData(this FishitContext context)
        {
            string fishingTripTableName = context.GetTableName<FishingTrip>();

            try
            {
                context.DeleteAllRecords(fishingTripTableName);

                SeedFishingTrip(context, fishingTripTableName);
            }
            catch (Exception exception)
            {
                throw new ApplicationException(InitializationError, exception);
            }
        }

        private static void SeedFishingTrip(FishitContext context, string fishingTripTableName)
        {
            try
            {
                // Reset the identity seed (Id's will start again from 1)
                context.ResetEntitySeed(fishingTripTableName);

                // Temporarily allow insertion of identity columns (Id)
                context.SetAutoIncrementOnTable(fishingTripTableName, true);

                // Insert test data
                context.FishingTrips.AddRange(FishingTrips);
                context.SaveChanges();
            }
            finally
            {
                // Disable insertion of identity columns (Id)
                context.SetAutoIncrementOnTable(fishingTripTableName, false);
            }
        }

        public static string GetTableName<T>(this DbContext context)
        {
            IRelationalEntityTypeAnnotations entityTypeAnnotations = context.Model
                .FindEntityType(typeof(T))
                .Relational();

            string schema = entityTypeAnnotations.Schema;
            string table = entityTypeAnnotations.TableName;

            return string.IsNullOrWhiteSpace(schema)
                ? $"[{table}]"
                : $"[{schema}].[{table}]";
        }

        private static void DeleteAllRecords(this DbContext context, string table)
        {
            string statement = $"DELETE FROM {table}";
            context?.Database?.ExecuteSqlCommand(statement);
        }

        private static void ResetEntitySeed(this DbContext context, string table)
        {
            if (context.TableHasIdentityColumn(table))
            {
                string statement = $"DBCC CHECKIDENT('{table}', RESEED, 0)"; // Must be a separate variable
                context?.Database?.ExecuteSqlCommand(statement);
            }
        }

        private static void SetAutoIncrementOnTable(
            this DbContext context,
            string table, bool
                isAutoIncrementOn)
        {
            if (context.TableHasIdentityColumn(table))
            {
                string statement =
                    $"SET IDENTITY_INSERT {table} {(isAutoIncrementOn ? "ON" : "OFF")}"; // Must be a separate variable
                context?.Database?.ExecuteSqlCommand(statement);
            }
        }

        private static bool TableHasIdentityColumn(
            this DbContext context,
            string table)
        {
            bool hasIdentityColumn = false;
            DbCommand command = context.Database.GetDbConnection().CreateCommand();
            try
            {
                command.CommandText = $"SELECT OBJECTPROPERTY(OBJECT_ID('{table}'), 'TableHasIdentity')";
                command.CommandType = CommandType.Text;

                if (command.Connection.State != ConnectionState.Open) command.Connection.Open();

                using (DbDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        hasIdentityColumn = reader.GetInt32(0) == 1;
                    }
                }
            }
            catch
            {
                hasIdentityColumn = false;
            }
            finally
            {
                command?.Dispose();
            }

            return hasIdentityColumn;
        }
    }
}
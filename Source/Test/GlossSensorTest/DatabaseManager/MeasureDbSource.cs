using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Windows.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlossSensorTest.Properties;

namespace GlossSensorTest.DatabaseManager
{
    public interface IMeasureSource
    {
        IEnumerable<MeasureInfo> Measures { get; }

        IMeasureSource LoadMeasures(ICollection<MeasureInfo> measures = null);
        IMeasureSource SaveMeasures(IEnumerable<MeasureInfo> measures);

    }

    public class MeasureService
    {
        IMeasureSource measureSource;
        ILoggerFacade<MeasureService> _logger;

        ICollection<MeasureInfo> _meausres { get; } = new ObservableCollection<MeasureInfo>();
        public IEnumerable<MeasureInfo> Meausres => _meausres;

        public MeasureService(ILoggerFacade<MeasureService> logger)
        {
            _logger = logger;

            BindingOperations.EnableCollectionSynchronization(_meausres, new object());

            Load();

            _logger.Info("Initialized");
        }

        public bool Load()
        {
            try
            {
                var connection = MeasureDbConnectionFactory.Create(

                    new DbConnectionInfo(
                        Settings.Default.DbSource,
                        Settings.Default.DbPort,
                        Settings.Default.DbPath,
                        Settings.Default.DbUserID,
                        Settings.Default.DbPassword));
                measureSource = new MeasureDbSource(connection)
                            .LoadMeasures(_meausres);

                _logger.Info("Db Loading Success");

                return true;
            }
            catch (Exception e)
            {
                _logger.Exception($"Db Loading Fail ( exception - {e.Message}");

                return false;
            }
        }
    }

    public class MeasureDbConnectionFactory
    {
        public static DbConnection Create(DbConnectionInfo info)
        {
            var builder = new DbConnectionStringBuilder();

            if (string.IsNullOrEmpty(info.DataSource) == false)
            {
                builder["DataSource"] = info.DataSource;
            }

            builder["Port"] = info.Port;

            if (string.IsNullOrEmpty(info.Database) == false)
                builder["Database"] = info.Database;

            builder["UserID"] = info.UserID;
            builder["Password"] = info.Password;

            return new FbConnection(builder.ConnectionString);
        }
    }

    public sealed class MeasureConfiguration : DbMigrationsConfiguration<MeasureDbContext>
    {
        public MeasureConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }

    public class MeasureDbContext : DbContext
    {
        private static DbConnection _connection;

        public MeasureDbContext() : base(_connection, false)
        {

        }

        public MeasureDbContext(DbConnection connection) : base(connection, true)
        {
            _connection = connection;
        }

        public DbSet<MeasureInfo> Measures { get; set; }
    }

    public class MeasureDbSource : IMeasureSource
    {
        MeasureDbContext measureDbContext;

        public IEnumerable<MeasureInfo> Measures => measureDbContext.Measures;

        public MeasureDbSource(DbConnection connection)
        {
            measureDbContext = new MeasureDbContext(connection);
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<MeasureDbContext, MeasureConfiguration>());
        }

        public IMeasureSource LoadMeasures(ICollection<MeasureInfo> measures = null)
        {
            measures?.Clear();
            foreach (var measure in measureDbContext.Measures)
                measures?.Add(measure.Clone() as MeasureInfo);

            return this;
        }

        public IMeasureSource SaveMeasures(IEnumerable<MeasureInfo> measures)
        {
            measureDbContext.Measures.RemoveRange(measureDbContext.Measures);
            measureDbContext.SaveChanges();

            measureDbContext.Measures.AddRange(measures.Reverse());
            measureDbContext.SaveChanges();

            return this;
        }

    }
}

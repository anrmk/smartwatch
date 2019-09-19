using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Base;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Core.Context {
    public class ApplicationContext: DbContext, IApplicationContext {
        /**
         * Первоначальное создание базы. Из консоли исполнить команды:
         * 1. Enable-Migrations
         * 2. Add-Migration InitialMigration
         * 2. Update-Database
         * Последующие миграции:
         * Аналогично, главное не затирать старые миграции и заново не включать миграции
         * Версия миграции: 1.0.1
         */
        //Database IApplicationContext.ApplicationDatabase => throw new NotImplementedException();

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {

        }

        public Database ApplicationDatabase { get; private set; }
        //public DbContextConfiguration ApplicationDatabaseConfiguration { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql("Host=localhost;Database=smartwatch;Username=postgres;Password=postgres");
        }

        // public string Error { get; private set; }

        //public ApplicationContext() : base(DbContextOptions options) {
        //    //ApplicationDatabaseConfiguration = Configuration;
        //    //Database.CommandTimeout = 180;
        //    ApplicationDatabase = Database;
        //}

        //public static ApplicationContext Create() {
        //    return new ApplicationContext();
        //}

        #region ENTITIES
        public DbSet<DeviceEntity> Devices { get; set; }
        public DbSet<DeviceLastLocationEntity> DeviceLastLocations { get; set; }
        public DbSet<DeviceLocationEntity> DeviceLocations { get; set; }
        public DbSet<ProfileCardEntity> ProfileCards { get; set; }
        public DbSet<ProfileCardMediaEntity> ProfileCardMedias { get; set; }
        #endregion

        //protected override void OnModelCreating(DbModelBuilder modelBuilder) {
        //    modelBuilder.HasDefaultSchema("public");
        //    base.OnModelCreating(modelBuilder);
        //}

        //public override int SaveChanges() {
        //    var modifiedEntries = ChangeTracker.Entries()
        //        .Where(x => x.Entity is IAuditableEntity
        //            && (x.State == EntityState.Added || x.State == EntityState.Modified));

        //    foreach (var entry in modifiedEntries) {
        //        IAuditableEntity entity = entry.Entity as IAuditableEntity;
        //        if (entity != null) {
        //            string identityName = Thread.CurrentPrincipal.Identity.Name;
        //            DateTime now = DateTime.UtcNow;

        //            if (entry.State == EntityState.Added) {
        //                entity.CreatedBy = identityName;
        //                entity.CreatedDate = now;
        //            } else {
        //                Entry(entity).Property(x => x.CreatedBy).IsModified = false;
        //                Entry(entity).Property(x => x.CreatedDate).IsModified = false;
        //            }
        //            entity.UpdatedBy = identityName;
        //            entity.UpdatedDate = now;
        //        }
        //    }
        //    bool saveFailed;
        //    do {
        //        saveFailed = false;
        //        try {
        //            return base.SaveChanges();
        //        } catch (DbUpdateConcurrencyException ex) {
        //            saveFailed = true;
        //            //if (!ex.Entries.Any(p => p.State == EntityState.Added))
        //            {
        //                if (ex.Entries.Any())
        //                    ex.Entries.Single().Reload();
        //            }
        //        } catch (DbUpdateException ex) {
        //            saveFailed = true;
        //            /*if (!ex.Entries.Any(p => p.State == EntityState.Added))
        //            {
        //                if (ex.Entries.Count() > 0)
        //                    ex.Entries.Single().Reload();
        //            }*/
        //            var exception = HandleDbUpdateException(ex);
        //            throw exception;
        //        } catch (Exception) {
        //            saveFailed = true;
        //        }
        //    } while (saveFailed);
        //    return -1;
        //}

        public async Task<int> SaveChangesAsync() {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity
                    && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach(var entry in modifiedEntries) {
                IAuditableEntity entity = entry.Entity as IAuditableEntity;
                if(entity != null) {
                    string identityName = "";// Thread.CurrentPrincipal.Identity.Name;
                    DateTime now = DateTime.UtcNow;

                    if(entry.State == EntityState.Added) {
                        entity.CreatedBy = identityName;
                        entity.CreatedDate = now;
                    } else {
                        Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    }
                    entity.UpdatedBy = identityName;
                    entity.UpdatedDate = now;
                }
            }
            bool saveFailed;
            do {
                saveFailed = false;
                try {
                    return await base.SaveChangesAsync();
                } catch(DbUpdateConcurrencyException) {
                    saveFailed = true;
                    return -1001;
                } catch(DbUpdateException) {
                    saveFailed = true;
                    return -1002;
                } catch(Exception) {
                    saveFailed = true;
                    return -1003;
                }
            } while(saveFailed);
        }

        //private static Exception HandleDbUpdateException(DbUpdateException dbu) {
        //    var builder = new StringBuilder("A DbUpdateException was caught while saving changes. ");

        //    try {
        //        foreach (var result in dbu.Entries) {
        //            builder.AppendFormat("Type: {0} was part of the problem. ", result.Entity.GetType().Name);
        //        }
        //    } catch (Exception e) {
        //        builder.Append("Error parsing DbUpdateException: " + e.ToString());
        //    }

        //    string message = builder.ToString();
        //    return new Exception(message, dbu);
        //}
    }
}
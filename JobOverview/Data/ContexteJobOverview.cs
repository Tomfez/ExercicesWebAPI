using JobOverview.Entities;
using Microsoft.EntityFrameworkCore;
using Version = JobOverview.Entities.Version;

namespace JobOverview.Data
{
    public class ContexteJobOverview : DbContext
    {
        public ContexteJobOverview(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Filiere> Filieres { get; set; }
        public virtual DbSet<Logiciel> Logiciels { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Release> Releases { get; set; }
        public virtual DbSet<Version> Versions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Filiere>(entity =>
            {
                entity.HasKey(e => e.Code);
                entity.Property(e => e.Code).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.Nom).HasMaxLength(60);
            });

            modelBuilder.Entity<Logiciel>(entity =>
            {
                entity.HasKey(e => e.Code);
                entity.HasOne<Filiere>().WithMany().HasForeignKey(l => l.CodeFiliere).OnDelete(DeleteBehavior.NoAction);

                entity.Property(e => e.Code).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.CodeFiliere).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.Nom).HasMaxLength(60).IsUnicode(false);
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.HasKey(e => new { e.Code, e.CodeLogiciel });
                entity.HasOne<Module>().WithMany(m => m.SousModules).HasForeignKey(m => new { m.CodeModuleParent, m.CodeLogicielParent }).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne<Logiciel>().WithMany(l => l.Modules).HasForeignKey(m => m.CodeLogiciel).OnDelete(DeleteBehavior.NoAction);

                entity.Property(e => e.Code).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.CodeLogiciel).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.Nom).HasMaxLength(60).IsUnicode(false);
                entity.Property(e => e.CodeModuleParent).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.CodeLogicielParent).HasMaxLength(20).IsUnicode(false);
            });

            modelBuilder.Entity<Release>(entity =>
            {
                entity.HasKey(e => new { e.Numero, e.NumeroVersion, e.CodeLogiciel });
                //entity.HasOne<Version>().WithMany().HasForeignKey(r => new { r.NumeroVersion, r.CodeLogiciel});
                entity.HasOne<Version>().WithMany(v => v.Releases).HasForeignKey(r => new { r.NumeroVersion, r.CodeLogiciel});

                entity.Property(e => e.CodeLogiciel).HasMaxLength(20).IsUnicode(false);
            });

            modelBuilder.Entity<Version>(entity =>
            {
                entity.HasKey(e => new { e.Numero, e.CodeLogiciel });
                //entity.HasOne<Logiciel>().WithMany().HasForeignKey(v => v.CodeLogiciel).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne<Logiciel>().WithMany().HasForeignKey(v => v.CodeLogiciel).OnDelete(DeleteBehavior.NoAction);

                entity.Property(e => e.CodeLogiciel).HasMaxLength(20).IsUnicode(false);
            });
        }
    }
}

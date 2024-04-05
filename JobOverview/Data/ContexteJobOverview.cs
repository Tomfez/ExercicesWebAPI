using JobOverview.Entities;
using Microsoft.EntityFrameworkCore;
using Version = JobOverview.Entities.Version;
using Service = JobOverview.Entities.Service;

namespace JobOverview.Data
{
    public class ContexteJobOverview : DbContext
    {
        public ContexteJobOverview(DbContextOptions options)
            : base(options)
        {
        }

        #region props
        public virtual DbSet<Filiere> Filieres { get; set; }
        public virtual DbSet<Logiciel> Logiciels { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Release> Releases { get; set; }
        public virtual DbSet<Version> Versions { get; set; }
        public virtual DbSet<Equipe> Equipes { get; set; }
        public virtual DbSet<Entities.Service> Services { get; set; }
        public virtual DbSet<Metier> Metiers { get; set; }
        public virtual DbSet<Personne> Personnes { get; set; }
        public virtual DbSet<Travail> Travaux { get; set; }
        public virtual DbSet<Tache> Taches { get; set; }
        public virtual DbSet<Activite> Activites { get; set; }
        public virtual DbSet<ActiviteMetier> ActivitesMetiers { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Logiciels
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
                entity.HasOne<Version>().WithMany(v => v.Releases).HasForeignKey(r => new { r.NumeroVersion, r.CodeLogiciel });

                entity.Property(e => e.CodeLogiciel).HasMaxLength(20).IsUnicode(false);
            });

            modelBuilder.Entity<Version>(entity =>
            {
                entity.HasKey(e => new { e.Numero, e.CodeLogiciel });
                entity.HasOne<Logiciel>().WithMany(l => l.Versions).HasForeignKey(v => v.CodeLogiciel).OnDelete(DeleteBehavior.NoAction);

                entity.Property(e => e.CodeLogiciel).HasMaxLength(20).IsUnicode(false);
            });
            #endregion

            #region Equipes
            modelBuilder.Entity<Equipe>(entity =>
            {
                entity.HasKey(e => e.Code);
                entity.HasOne(e => e.Service).WithMany().HasForeignKey(s => s.CodeService).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne<Filiere>().WithMany().HasForeignKey(f => f.CodeFiliere).OnDelete(DeleteBehavior.NoAction);

                entity.Property(e => e.Code).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.Nom).HasMaxLength(60).IsUnicode(false);
                entity.Property(e => e.CodeService).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.CodeFiliere).HasMaxLength(20).IsUnicode(false);
            });

            modelBuilder.Entity<Entities.Service>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.Property(e => e.Code).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.Nom).HasMaxLength(60);
            });

            modelBuilder.Entity<Metier>(entity =>
            {
                entity.HasKey(e => e.Code);
                entity.HasOne<Entities.Service>().WithMany().HasForeignKey(m => m.CodeService).OnDelete(DeleteBehavior.NoAction);

                entity.Property(e => e.Code).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.Titre).HasMaxLength(60);
                entity.Property(e => e.CodeService).HasMaxLength(20).IsUnicode(false);
            });

            modelBuilder.Entity<Personne>(entity =>
            {
                entity.HasKey(e => e.Pseudo);
                entity.HasOne<Equipe>().WithMany(e => e.Personnes).HasForeignKey(p => p.CodeEquipe).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne<Personne>().WithMany().HasForeignKey(p => p.Manager).OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(p => p.Metier).WithMany().HasForeignKey(p => p.CodeMetier).OnDelete(DeleteBehavior.NoAction);

                entity.Property(e => e.Pseudo).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.Nom).HasMaxLength(60);
                entity.Property(e => e.Prenom).HasMaxLength(60);
                entity.Property(e => e.TauxProductivite).HasColumnType("decimal(3,2)").HasDefaultValue(1);
                entity.Property(e => e.CodeEquipe).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.CodeMetier).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.Manager).HasMaxLength(20).IsUnicode(false);
            });
            #endregion

            #region Taches
            modelBuilder.Entity<Activite>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.Property(e => e.Code).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.Titre).HasMaxLength(60);

                entity.HasMany<Metier>().WithMany(m => m.Activites).UsingEntity<ActiviteMetier>(
                   l => l.HasOne<Metier>().WithMany().HasForeignKey(am => am.CodeMetier),
                   r => r.HasOne<Activite>().WithMany().HasForeignKey(am => am.CodeActivite));
            });

            modelBuilder.Entity<Tache>(entity =>
            {
                entity.Property(e => e.Titre).HasMaxLength(60);
                entity.Property(e => e.DureePrevue).HasColumnType("decimal(3, 1)");
                entity.Property(e => e.DureeRestante).HasColumnType("decimal(3, 1)");
                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.HasOne<Activite>().WithMany().HasForeignKey(d => d.CodeActivite)
                   .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne<Personne>().WithMany().HasForeignKey(d => d.Personne)
                   .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne<Module>().WithMany().HasForeignKey(d => new { d.CodeModule, d.CodeLogiciel })
                   .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne<Version>().WithMany().HasForeignKey(d => new { d.NumVersion, d.CodeLogiciel })
                   .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Travail>(entity =>
            {
                entity.HasKey(e => new { e.DateTravail, e.IdTache });

                entity.Property(e => e.Heures).HasColumnType("decimal(3, 1)");
                entity.Property(e => e.TauxProductivite).HasColumnType("decimal(3, 2)").HasDefaultValue(1m);

                entity.HasOne<Tache>().WithMany(t => t.Travaux).HasForeignKey(d => d.IdTache);
            });
            #endregion

        }
    }
}

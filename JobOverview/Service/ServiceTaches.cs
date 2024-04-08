using JobOverview.Data;
using JobOverview.Entities;
using JobOverview.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating.CodeCompilation;

namespace JobOverview.Service
{
    public interface IServiceTaches
    {
        public Task<List<Tache>> GetTaches(string? personne, string? logiciel, float? version);
        public Task<Tache?> GetTache(int id);
        public Task<List<Travail>> GetTravaux(int idTache);
        public Task<Personne?> GetPersonne(string pseudo);
        public Task<Tache> PostTache(Tache tache);
        public Task<Travail> PostTravail(int idTache, Travail travail);
        public Task DeleteTravail(int idTache, DateOnly date);
        public Task<int> DeleteTaches(string? personne, string? logiciel, float? version);
    }

    public class ServiceTaches : IServiceTaches
    {
        private readonly ContexteJobOverview _context;
        public ServiceTaches(ContexteJobOverview context)
        {
            _context = context;
        }

        #region GET
        public async Task<List<Tache>> GetTaches(string? personne, string? logiciel, float? version)
        {
            var req = from t in _context.Taches
                      where (personne == null || t.Personne == personne)
                      && (logiciel == null || t.CodeLogiciel == logiciel)
                      && (version == null || t.NumVersion == version)
                      orderby t.CodeLogiciel, t.NumVersion
                      select t;

            return await req.ToListAsync();
        }

        public async Task<Tache?> GetTache(int id)
        {
            var req = from t in _context.Taches
                      .Include(tr => tr.Travaux.OrderBy(x => x.DateTravail))
                      where t.Id == id
                      select t;

            return await req.FirstOrDefaultAsync();
        }

        public async Task<Personne?> GetPersonne(string pseudo)
        {
            var req = from p in _context.Personnes
                      .Include(p => p.Metier)
                      .ThenInclude(m => m.Activites)
                      where p.Pseudo == pseudo
                      select p;

            return await req.FirstOrDefaultAsync();
        }

        public async Task<List<Travail>> GetTravaux(int idTache)
        {
            var req = from t in _context.Travaux
                      where t.IdTache == idTache
                      select t;

            return await req.ToListAsync();
        }
        #endregion

        #region POST
        public async Task<Tache> PostTache(Tache tache)
        {
            tache.Travaux = null!;
            Tache? findTache = await _context.Taches.FindAsync(tache.Id);

            ValidationRulesException vre = new ValidationRulesException();

            if (findTache != null)
            {
                vre.Errors.Add("Id", ["La tache existe déjà."]);
                throw vre;
            }

            Personne? p = await GetPersonne(tache.Personne);
            if (p == null)
                throw new ValidationRulesException("Personne", $"Personne {tache.Personne} non trouvée");

            // Vérifie si le code activité de la tâche fait partie de ceux de la personne
            if (p.Metier!.Activites.Find(a => a.Code == tache.CodeActivite) == null)
                throw new ValidationRulesException("CodeActivite", "L'activité ne correspond pas au métier de la personne.");

            _context.Taches.Add(tache);
            await _context.SaveChangesAsync();
            return tache;
        }

        public async Task<Travail> PostTravail(int idTache, Travail travail)
        {
            ValidationRulesException vre = new();

            if (travail.Heures < 0.5m || travail.Heures > 8)
                vre.Errors.Add("Heures", new string[] { "Le nombre d'heures doit être compris entre 0.5 et 8" });

            if (vre.Errors.Any())
                throw vre;

            // Récupère la tâche
            Tache? tache = await _context.Taches.FindAsync(idTache);

            if (tache == null)
                throw new ValidationRulesException("IdTache", $"Tache {idTache} non trouvée");

            // Récupère la personne associée à la tâche et ses activités
            Personne? p = await GetPersonne(tache.Personne);

            travail.IdTache = idTache;
            travail.TauxProductivite = p!.TauxProductivite;

            // Met à jour la durée de travail restante sur la tâche
            tache.DureeRestante -= travail.Heures;
            if (tache.DureeRestante < 0) tache.DureeRestante = 0;

            _context.Travaux.Add(travail);
            await _context.SaveChangesAsync();
            return travail;
        }
        #endregion

        #region DELETE
        public async Task DeleteTravail(int idTache, DateOnly date)
        {
            Tache? tache = await GetTache(idTache);

            if (tache == null)
                throw new ValidationRulesException("IdTache", $"Tache {idTache} non trouvée");

            Travail? travail = tache.Travaux.Where(t => t.DateTravail == date).FirstOrDefault();

            if (travail == null)
                throw new ValidationRulesException("Date", "Aucun travail trouvé à la date donnée");

            tache.DureeRestante += travail.Heures;

            _context.Remove(travail);

            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteTaches(string? personne, string? logiciel, float? version)
        {
            var req = _context.Taches.Where(t =>
                        (personne == null || t.Personne == personne) &&
                        (logiciel == null || t.CodeLogiciel == logiciel) &&
                        (version == null || t.NumVersion == version));

            return await req.ExecuteDeleteAsync();

            //List<Tache> taches = await GetTaches(personne, logiciel, version);

            //if (!taches.Any())
            //    throw new ValidationRulesException("Personne", "Aucune info trouvée");

            //using (var transaction = _context.Database.BeginTransaction())
            //{
            //    int nbSuppr = 0;

            //    foreach (var tache in taches)
            //    {
            //        _context.Entry(tache).State = EntityState.Deleted;

            //        //await _context.Taches.Where(x => x.Id == tache.Id).ExecuteDeleteAsync();

            //        // On supprime le travail
            //        Travail travail = new() { IdTache = tache.Id };
            //        nbSuppr++;
            //    }

            //    await _context.SaveChangesAsync();

            //    transaction.Commit();

            //    return nbSuppr;
            //}
        }
        #endregion
    }
}

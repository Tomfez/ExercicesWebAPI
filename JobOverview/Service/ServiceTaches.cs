using JobOverview.Data;
using JobOverview.Entities;
using JobOverview.Exceptions;
using JobOverview.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JobOverview.Service
{
    public interface IServiceTaches
    {
        public Task<ServiceResult<List<Tache>>> GetTaches(string? personne, string? logiciel, float? version);
        public Task<ServiceResult<Tache?>> GetTache(int id);
        public Task<ServiceResult<List<Travail>>> GetTravaux(int idTache);
        public Task<ServiceResult<Personne?>> GetPersonne(string pseudo);
        public Task<ServiceResult<Tache?>> PostTache(Tache tache);
        public Task<ServiceResult<Travail?>> PostTravail(int idTache, Travail travail);
        public Task<ServiceResult<int>> DeleteTravail(int idTache, DateOnly date);
        public Task<ServiceResult<int>> DeleteTaches(string? personne, string? logiciel, float? version);
        public Task<ServiceResult<Tache?>> PutPostTache(Tache tache);
    }

    public class ServiceTaches(ContexteJobOverview context) : ServiceBase(context), IServiceTaches
    {
        private readonly ContexteJobOverview _context = context;

        #region GET
        public async Task<ServiceResult<List<Tache>>> GetTaches(string? personne, string? logiciel, float? version)
        {
            var req = from t in _context.Taches
                      where (personne == null || t.Personne == personne)
                      && (logiciel == null || t.CodeLogiciel == logiciel)
                      && (version == null || t.NumVersion == version)
                      orderby t.CodeLogiciel, t.NumVersion
                      select t;

            var taches = await req.ToListAsync();
            return ResultOk(taches);
        }

        public async Task<ServiceResult<Tache?>> GetTache(int id)
        {
            var req = from t in _context.Taches
                      .Include(tr => tr.Travaux.OrderBy(x => x.DateTravail))
                      where t.Id == id
                      select t;

            var tache = await req.FirstOrDefaultAsync();
            return ResultOkOrNotFound(id, tache);
        }

        public async Task<ServiceResult<Personne?>> GetPersonne(string pseudo)
        {
            var req = from p in _context.Personnes
                      .Include(p => p.Metier)
                      .ThenInclude(m => m.Activites)
                      where p.Pseudo == pseudo
                      select p;

            var personne = await req.FirstOrDefaultAsync();
            return ResultOkOrNotFound(pseudo, personne);
        }

        public async Task<ServiceResult<List<Travail>>> GetTravaux(int idTache)
        {
            var req = from t in _context.Travaux
                      where t.IdTache == idTache
                      select t;

            var travaux = await req.ToListAsync();
            return ResultOk(travaux);
        }
        #endregion

        #region POST
        public async Task<ServiceResult<Tache?>> PostTache(Tache tache)
        {
            tache.Travaux = null!;
            Tache? findTache = await _context.Taches.FindAsync(tache.Id);

            ValidationRulesException vre = new ValidationRulesException();

            if (findTache != null)
            {
                vre.Errors.Add("Id", ["La tache existe déjà."]);
                throw vre;
            }

            var p = await GetPersonne(tache.Personne);
            if (p.ResultKind != ResultKinds.Ok)
                return ResultNotFound<Tache?>($"Personne {tache.Personne} non trouvée");

            // Vérifie si le code activité de la tâche fait partie de ceux de la personne
            if (p.Data?.Metier?.Activites.Find(a => a.Code == tache.CodeActivite) == null)
                throw new ValidationRulesException("CodeActivite", "L'activité ne correspond pas au métier de la personne.");

            _context.Taches.Add(tache);
            return await SaveAndResultOkAsync(tache);
        }

        public async Task<ServiceResult<Travail?>> PostTravail(int idTache, Travail travail)
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
            var p = await GetPersonne(tache.Personne);

            travail.IdTache = idTache;
            travail.TauxProductivite = p.Data!.TauxProductivite;

            // Met à jour la durée de travail restante sur la tâche
            tache.DureeRestante -= travail.Heures;
            if (tache.DureeRestante < 0) tache.DureeRestante = 0;

            _context.Travaux.Add(travail);
            return await SaveAndResultOkAsync<Travail?>(travail);
        }
        #endregion

        #region DELETE
        public async Task<ServiceResult<int>> DeleteTravail(int idTache, DateOnly date)
        {
            var tache = await GetTache(idTache);

            if (tache.ResultKind != ResultKinds.Ok)
                return ResultNotFound<int>($"Tache {idTache} non trouvée");

            Travail? travail = tache.Data?.Travaux.Where(t => t.DateTravail == date).FirstOrDefault();

            if (travail == null)
                return ResultNotFound<int>($"Aucun travail trouvé à la date du {date} sur la tâche {idTache}.");

            tache.Data!.DureeRestante += travail.Heures;

            // Rattache l'entité au suivi, sans ses filles, en passant son état à Modified
            EntityEntry<Tache> ent = _context.Entry(tache.Data);
            ent.State = EntityState.Modified;

            _context.Remove(travail);

            return await SaveAndResultOkAsync(idTache);
        }

        public async Task<ServiceResult<int>> DeleteTaches(string? personne, string? logiciel, float? version)
        {
            var tache = _context.Taches.Where(t =>
                        (personne == null || t.Personne == personne) &&
                        (logiciel == null || t.CodeLogiciel == logiciel) &&
                        (version == null || t.NumVersion == version));

            _context.Remove(tache);
            return await SaveAndResultOkAsync();
        }
        #endregion

        public async Task<ServiceResult<Tache?>> PutPostTache(Tache tache)
        {
            // Dans le cas d'une modification
            if (tache.Id != 0)
            {
                var req = from t in _context.Taches.AsNoTracking()
                          where t.Id == tache.Id
                          select t.Id;

                if (await req.FirstOrDefaultAsync() == 0)
                    return ResultNotFound<Tache?>(tache.Id);
            }

            tache.Travaux = null!;

            _context.Taches.Update(tache);

            // Génère une nouvelle valeur de jeton d'accès concurrentiel
            tache.Vers = Guid.NewGuid();

            return await SaveAndResultOkAsync<Tache?>(tache);
        }
    }
}

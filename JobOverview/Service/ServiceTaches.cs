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

            _context.Taches.Add(tache);
            await _context.SaveChangesAsync();
            return tache;
        }

        public async Task<Travail> PostTravail(int idTache, Travail travail)
        {
            travail.IdTache = idTache;
            _context.Travaux.Add(travail);
            await _context.SaveChangesAsync();
            return travail;
        }
        #endregion
    }
}

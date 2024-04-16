using JobOverview.Data;
using JobOverview.Entities;
using JobOverview.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace JobOverview.V1.Services
{
    public interface IServiceEquipes
    {
        public Task<List<Equipe>?> GetEquipes(string codeFiliere);
        public Task<Equipe?> GetEquipe(string codeFiliere, string nomEquipe);
        public Task<Equipe> PostEquipe(string codeFiliere, Equipe equipe);
        public Task<Personne> PostPersonne(string nomEquipe, Personne personne);
        public Task<int> PutPersonne(string codeEquipe, string pseudo);
    }

    public class ServiceEquipes : IServiceEquipes
    {
        private readonly ContexteJobOverview _context;
        public ServiceEquipes(ContexteJobOverview context)
        {
            _context = context;
        }

        #region GET
        public async Task<List<Equipe>?> GetEquipes(string codeFiliere)
        {
            if (await _context.Filieres.FindAsync(codeFiliere) == null)
                return null;

            var req = from e in _context.Equipes
                      .Include(e => e.Service)
                      where e.CodeFiliere == codeFiliere
                      select e;

            return await req.ToListAsync();
        }

        public async Task<Equipe?> GetEquipe(string codeFiliere, string nomEquipe)
        {
            var req2 = from e in _context.Equipes
                       .Include(e => e.Service)
                       .Include(e => e.Personnes)
                       .ThenInclude(p => p.Metier)
                       where e.Code == nomEquipe
                       select e;

            return await req2.FirstOrDefaultAsync();
        }
        #endregion

        #region POST
        public async Task<Equipe> PostEquipe(string codeFiliere, Equipe equipe)
        {
            equipe.Service = null;
            equipe.CodeFiliere = codeFiliere;

            foreach (Personne personne in equipe.Personnes)
            {
                personne.Metier = null;
            }

            _context.Equipes.Add(equipe);
            await _context.SaveChangesAsync();
            return equipe;
        }

        public async Task<Personne> PostPersonne(string nomEquipe, Personne personne)
        {
            personne.CodeEquipe = nomEquipe;

            _context.Personnes.Add(personne);
            await _context.SaveChangesAsync();
            return personne;
        }
        #endregion

        #region PUT
        public async Task<int> PutPersonne(string codeEquipe, string pseudo)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                // Modifie le manager de toute l'équipe
                int nbModifs = await _context.Personnes
                    .Where(p => p.CodeEquipe == codeEquipe)
                    .ExecuteUpdateAsync(setter => setter.SetProperty(p => p.Manager, pseudo));

                // Remet à null le champ manager pour lui-même
                await _context.Personnes
                    .Where(p => p.Pseudo == pseudo)
                    .ExecuteUpdateAsync(setter => setter.SetProperty(p => p.Manager, (string?)null));

                transaction.Commit();
                return nbModifs;
            }
        }
        #endregion
    }
}

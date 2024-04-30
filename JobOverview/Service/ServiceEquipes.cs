using JobOverview.Data;
using JobOverview.Entities;
using JobOverview.Tools;
using Microsoft.EntityFrameworkCore;

namespace JobOverview.Service
{
    public interface IServiceEquipes
    {
        public Task<ServiceResult<List<Equipe>?>> GetEquipes(string codeFiliere);
        public Task<ServiceResult<Equipe?>> GetEquipe(string codeFiliere, string nomEquipe);
        public Task<ServiceResult<Equipe?>> PostEquipe(string codeFiliere, Equipe equipe);
        public Task<ServiceResult<Personne?>> PostPersonne(string nomEquipe, Personne personne);
        public Task<ServiceResult<int>> PutPersonne(string codeEquipe, string pseudo);
    }

    public class ServiceEquipes(ContexteJobOverview context) : ServiceBase(context), IServiceEquipes
    {
        private readonly ContexteJobOverview _context = context;

        #region GET
        public async Task<ServiceResult<List<Equipe>?>> GetEquipes(string codeFiliere)
        {
            var req = from e in _context.Equipes
                      .Include(e => e.Service)
                      where e.CodeFiliere == codeFiliere
                      select e;

            var equipes = await req.ToListAsync();
            return ResultOkOrNotFound(codeFiliere, equipes);
        }

        public async Task<ServiceResult<Equipe?>> GetEquipe(string codeFiliere, string nomEquipe)
        {
            var req2 = from e in _context.Equipes
                       .Include(e => e.Service)
                       .Include(e => e.Personnes)
                       .ThenInclude(p => p.Metier)
                       where e.Code == nomEquipe
                       select e;

            var equipe = await req2.FirstOrDefaultAsync();
            return ResultOk(equipe);
        }
        #endregion

        #region POST
        public async Task<ServiceResult<Equipe?>> PostEquipe(string codeFiliere, Equipe equipe)
        {
            equipe.Service = null;
            equipe.CodeFiliere = codeFiliere;

            foreach (Personne personne in equipe.Personnes)
            {
                personne.Metier = null;
            }

            _context.Equipes.Add(equipe);
            return await SaveAndResultCreatedAsync(equipe);
        }

        public async Task<ServiceResult<Personne?>> PostPersonne(string nomEquipe, Personne personne)
        {
            personne.CodeEquipe = nomEquipe;

            _context.Personnes.Add(personne);

            return await SaveAndResultCreatedAsync(personne);
        }
        #endregion

        #region PUT
        public async Task<ServiceResult<int>> PutPersonne(string codeEquipe, string pseudo)
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
                return ResultOk(nbModifs);
            }
        }
        #endregion
    }
}

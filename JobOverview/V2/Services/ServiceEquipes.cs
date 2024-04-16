using JobOverview.Data;
using JobOverview.Entities;
using JobOverview.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace JobOverview.V2.Services
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

        private void ControlerPersonne(Personne pers)
        {
            ValidationRulesException vre = new();

            if (string.IsNullOrWhiteSpace(pers.Pseudo) ||
                string.IsNullOrWhiteSpace(pers.Nom) ||
                string.IsNullOrWhiteSpace(pers.Prenom) ||
                string.IsNullOrWhiteSpace(pers.Email))
                vre.Errors.Add("Proprité non renseignée", new string[] { "Le pseudo, le nom, le prénom et l'email de la personne sont obligatoires." });

            if (!Regex.Match(pers.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase).Success)
                vre.Errors.Add("Format d'email incorrect", new string[] { $"Le format de l'adresse {pers.Email} est incorrect." });

            if (vre.Errors.Any()) throw vre;
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
        // Ajoute une équipe avec des personnes dans une filière donnée
        public async Task<Equipe> PostEquipe(string codeFilière, Equipe équipe)
        {
            équipe.CodeFiliere = codeFilière;
            équipe.Service = null!;
            foreach (Personne p in équipe.Personnes)
            {
                ControlerPersonne(p);
                p.Metier = null!;
            }
            _context.Equipes.Add(équipe);

            await _context.SaveChangesAsync();

            return équipe;
        }

        // Ajoute une personne dans une équipe donnée
        public async Task<Personne> PostPersonne(string codeEquipe, Personne personne)
        {
            personne.CodeEquipe = codeEquipe;
            personne.Metier = null!;

            ControlerPersonne(personne);

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

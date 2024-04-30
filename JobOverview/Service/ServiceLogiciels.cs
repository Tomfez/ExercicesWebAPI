using JobOverview.Data;
using JobOverview.Entities;
using JobOverview.Exceptions;
using JobOverview.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Version = JobOverview.Entities.Version;

namespace JobOverview.Service
{
    public interface IServiceLogiciels
    {
        Task<ServiceResult<List<Logiciel>>> GetLogiciels(string? codeFiliere);
        Task<ServiceResult<Logiciel?>> GetLogiciel(string code);
        Task<ServiceResult<List<Version>?>> GetVersionsLogiciel(string code, short? millesime);
        Task<ServiceResult<Release?>> GetRelease(string codeLogiciel, float numVersion, short numRelease);
        Task<ServiceResult<Release?>> PostRelease(string codeLogiciel, float numVersion, Release release);
        Task<ServiceResult<Version?>> PostVersion(string codeLogiciel, Version version);
    }

    public class ServiceLogiciels(ContexteJobOverview context) : ServiceBase(context), IServiceLogiciels
    {
        private readonly ContexteJobOverview _context = context;

        #region GET
        public async Task<ServiceResult<List<Logiciel>>> GetLogiciels(string? codeFiliere)
        {
            IQueryable<Logiciel> req = from l in _context.Logiciels
                                       where l.CodeFiliere == codeFiliere
                                       select l;

            var res = await req.ToListAsync();

            return ResultOk(res);
        }

        public async Task<ServiceResult<Logiciel?>> GetLogiciel(string code)
        {
            // Récupère le logiciel et ses données à plat
            var req = from l in _context.Logiciels
                      .Include(l => l.Modules)
                      .ThenInclude(m => m.SousModules)
                      where l.Code == code
                      select l;

            Logiciel? logiciel = await req.FirstOrDefaultAsync();

            if (logiciel == null)
                return ResultNotFound<Logiciel?>(code);

            var req2 = from m in logiciel.Modules
                       where m.CodeModuleParent == null
                       select new Module
                       {
                           Code = m.Code,
                           Nom = m.Nom,
                           CodeLogicielParent = m.CodeLogiciel,
                           SousModules = (from sm in m.SousModules select sm).ToList()
                       };

            logiciel.Modules = req2.ToList();
            return ResultOk<Logiciel?>(logiciel);
        }

        public async Task<ServiceResult<List<Version>?>> GetVersionsLogiciel(string code, short? millesime)
        {
            if (await _context.Logiciels.FindAsync(code) == null)
                return ResultNotFound<List<Version>?>(code);

            IQueryable<Version> req = from v in _context.Versions
                       .Include(v => v.Releases)
                                      where v.CodeLogiciel == code &&
                                      (millesime == null || v.Millesime == millesime)
                                      select v;

            var res = await req.ToListAsync();
            return ResultOk<List<Version>?>(res);
        }

        public async Task<ServiceResult<Release?>> GetRelease(string codeLogiciel, float numVersion, short numRelease)
        {
            var res = await _context.Releases.FindAsync(numRelease, numVersion, codeLogiciel);
            return ResultOkOrNotFound(numRelease, res);
        }
        #endregion

        #region POST
        public async Task<ServiceResult<Release?>> PostRelease(string codeLogiciel, float numVersion, Release release)
        {
            var req = from r in _context.Releases
                      where r.CodeLogiciel == codeLogiciel
                      && r.NumeroVersion == numVersion
                      orderby r.Numero
                      select r.Numero;

            short relMax = await req.LastOrDefaultAsync(); //renvoie 0 si pas de résultat
            ValidationRulesException vre = new ValidationRulesException();

            if (relMax > 0)
            {
                if (release.Numero <= relMax)
                {
                    vre.Errors.Add("NumeroVersion", [$"Le numéro de version ({numVersion}) doit être supérieur à la dernière version."]);
                    throw vre;
                }

                var req2 = from r in _context.Releases
                           where r.CodeLogiciel == codeLogiciel
                           && r.NumeroVersion == numVersion
                           && r.Numero == relMax
                           select r.DatePubli;

                DateOnly datePubliPrec = await req2.LastOrDefaultAsync();

                if (release.DatePubli < datePubliPrec)
                {
                    vre.Errors.Add("DatePubli", [$"La date de publication ({release.DatePubli}) doit être supérieure à la dernière version."]);
                    throw vre;
                }
            }

            _context.Releases.Add(release);
            return await SaveAndResultCreatedAsync<Release?>(release);
        }

        public async Task<ServiceResult<Version?>> PostVersion(string codeLogiciel, Version version)
        {
            ValidationRulesException vre = new ValidationRulesException();
            Regex regex = new Regex(@"^\d{1,3}(.\d{1,2})?$");

            if (!regex.IsMatch(version.Numero.ToString()))
                vre.Errors.Add("Numero", new string[] { $"Le numéro de version ({version.Numero}) doit avoir au maximum 3 chiffres avant la virgule et 2 après." });

            if (version.Millesime < 2020 || version.Millesime > 2100)
                vre.Errors.Add("Millesime", new string[] { $"Le millésime ({version.Millesime}) doit être compris entre 2020 et 2100 inclus" });

            if ((version.DateOuverture > version.DateSortiePrevue) ||
                (version.DateSortieReelle != null && version.DateSortieReelle < version.DateOuverture))
                vre.Errors.Add("DateOuverture", ["La date d'ouverture doit être inférieur à la date de sortie."]);

            if (vre.Errors.Count > 0)
                throw vre;

            version.CodeLogiciel = codeLogiciel;

            _context.Versions.Add(version);
            return await SaveAndResultCreatedAsync<Version?>(version);
        }
        #endregion
    }
}

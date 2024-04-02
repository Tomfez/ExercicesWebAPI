using JobOverview.Data;
using JobOverview.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Version = JobOverview.Entities.Version;

namespace JobOverview.Service
{
    public interface IServiceLogiciels
    {
        Task<List<Logiciel>> GetLogiciels(string? codeFiliere);
        Task<Logiciel?> GetLogiciel(string code);
        Task<List<Version>?> GetVersionsLogiciel(string code, short? millesime);
    }

    public class ServiceLogiciels : IServiceLogiciels
    {
        private readonly ContexteJobOverview _context;
        public ServiceLogiciels(ContexteJobOverview context)
        {
            _context = context;
        }

        public async Task<List<Logiciel>> GetLogiciels(string? codeFiliere)
        {
            IQueryable<Logiciel> req = from l in _context.Logiciels
                                       where l.CodeFiliere == codeFiliere
                                       select l;

            return await req.ToListAsync();
        }

        public async Task<Logiciel?> GetLogiciel(string code)
        {
            // Récupère le logiciel et ses données à plat
            var req = from l in _context.Logiciels
                      .Include(l => l.Modules)
                      .ThenInclude(m => m.SousModules)
                      where l.Code == code
                      select l;

            Logiciel? logiciel = await req.FirstOrDefaultAsync();

            if (logiciel == null)
                return null;

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
            return logiciel;

            //return await _context.Logiciels.FindAsync(code);
        }

        public async Task<List<Version>?> GetVersionsLogiciel(string code, short? millesime)
        {
            if (await _context.Logiciels.FindAsync(code) == null)
                return null;

            IQueryable<Version> req = from v in _context.Versions
                       .Include(v => v.Releases)
                                      where v.CodeLogiciel == code &&
                                      (millesime == null || v.Millesime == millesime)
                                      select v;

            return await req.ToListAsync();
        }
    }
}

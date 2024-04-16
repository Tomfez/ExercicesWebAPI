using Microsoft.AspNetCore.Mvc;
using JobOverview.Entities;
using Microsoft.AspNetCore.Authorization;
using JobOverview.Controllers;
using Asp.Versioning;
using JobOverview.V1.Services;
using JobOverview.V1.Entities;

namespace JobOverview.V1.Controllers
{
    // api/Filieres/BIOV/Equipes
    [Route("api/Filieres/{codeFiliere}/[controller]")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(1.0)]
    [Authorize(Policy = "GererEquipes")]
    public class EquipesController : ControllerBase
    {
        private readonly IServiceEquipes _service;
        private readonly ILogger<EquipesController> _logger;

        public EquipesController(IServiceEquipes service, ILogger<EquipesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        #region GET
        // GET: api/Filieres/BIOH/Equipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipe>?>> GetEquipes(string codeFiliere)
        {
            return Ok(await _service.GetEquipes(codeFiliere));
        }

        // GET: api/Filieres/BIOH/Equipes/BIOH_DEV
        [HttpGet("{nomEquipe}")]
        public async Task<ActionResult<Equipe?>> GetEquipe(string codeFiliere, string nomEquipe)
        {
            var equipe = await _service.GetEquipe(codeFiliere, nomEquipe);

            if (equipe == null)
            {
                return NotFound();
            }

            return Ok(equipe);
        }
        #endregion

        #region POST
        // POST: api/Filieres/BIOH/Equipes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // POST: api/Filieres/BIOV/Equipes
        [HttpPost]
        public async Task<ActionResult<Equipe>> PostEquipe(string codeFiliere, EquipeDTO eqDTO)
        {
            Equipe eq = new Equipe
            {
                Code = eqDTO.Code,
                CodeFiliere = eqDTO.CodeFiliere,
                CodeService = eqDTO.CodeService,
                Nom = eqDTO.Nom,
                Service = eqDTO.Service,
                Personnes = new()
            };

            foreach (PersonneDTO p in eqDTO.Personnes)
                eq.Personnes.Add(GetPersonneFromDTO(p));

            try
            {
                Equipe res = await _service.PostEquipe(codeFiliere, eq);

                // Renvoie une réponse de code 201 avec l'en-tête
                // "location: <url d'accès à l’équipe>" et un corps contenant l’équipe
                return CreatedAtAction(nameof(GetEquipe), new { codeFiliere = res.CodeFiliere, codeEquipe = res.Code }, res);
            }
            catch (Exception e)
            {
                // Journalise des détails sur l'erreur et renvoie la réponse HTTP
                return this.CustomResponseForError(e, eq, _logger);
            }
        }

        private Personne GetPersonneFromDTO(PersonneDTO persDTO)
        {
            return new Personne
            {
                Pseudo = persDTO.Pseudo,
                Nom = persDTO.Nom,
                Prenom = persDTO.Prenom,
                Email = string.Empty,
                CodeEquipe = persDTO.CodeEquipe,
                CodeMetier = persDTO.CodeMetier,
                Manager = persDTO.Manager,
                Metier = persDTO.Métier,
                TauxProductivite = persDTO.TauxProductivite
            };
        }

        // POST: api/Filieres/BIOV/Equipes/BIOV_MKT
        [HttpPost("{codeEquipe}")]
        public async Task<ActionResult<Equipe>> PostPersonne(string codeFiliere, string codeEquipe, PersonneDTO persDTO)
        {
            Personne pers = GetPersonneFromDTO(persDTO);
            try
            {
                Personne res = await _service.PostPersonne(codeEquipe, pers);

                // Renvoie une réponse de code 201 avec l'en-tête
                // "location: <url d'accès à l’équipe de la personne>" et un corps contenant l’équipe
                return CreatedAtAction(nameof(GetEquipe), new { codeFiliere, codeEquipe }, res);
            }
            catch (Exception e)
            {
                // Journalise des détails sur l'erreur et renvoie la réponse HTTP
                return this.CustomResponseForError(e, pers, _logger);
            }
        }
        #endregion


        // PUT: api/Equipes?manager=x
        [HttpPut("{codeEquipe}")]
        public async Task<IActionResult> PutEquipe(string codeEquipe, [FromQuery] string manager)
        {
            try
            {
                int res = await _service.PutPersonne(codeEquipe, manager);

                return Ok(res + " personnes modifiées");
            }
            catch (Exception ex)
            {
                return this.CustomResponseForError(ex);
            }
            //    if (id != equipe.Code)
            //    {
            //        return BadRequest();
            //    }

            //    _context.Entry(equipe).State = EntityState.Modified;

            //    try
            //    {
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!EquipeExists(id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }

            //    return NoContent();
        }



        //// DELETE: api/Equipes/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteEquipe(string id)
        //{
        //    var equipe = await _context.Equipes.FindAsync(id);
        //    if (equipe == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Equipes.Remove(equipe);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool EquipeExists(string id)
        //{
        //    return _context.Equipes.Any(e => e.Code == id);
        //}
    }
}

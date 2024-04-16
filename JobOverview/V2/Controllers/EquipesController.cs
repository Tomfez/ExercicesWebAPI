using Microsoft.AspNetCore.Mvc;
using JobOverview.Entities;
using Microsoft.AspNetCore.Authorization;
using JobOverview.Controllers;
using Asp.Versioning;
using JobOverview.V2.Services;

namespace JobOverview.V2.Controllers
{
    // api/Filieres/BIOV/Equipes
    [Route("api/Filieres/{codeFiliere}/[controller]")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(2.0)]
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
        [HttpPost]
        public async Task<ActionResult<Equipe>> PostEquipe(string codeFiliere, Equipe equipe)
        {
            try
            {
                Equipe res = await _service.PostEquipe(codeFiliere, equipe);

                return CreatedAtAction("GetEquipe", new { codeFiliere = res.CodeFiliere, nomEquipe = res.Nom }, res);
            }
            catch (Exception ex)
            {
                return this.CustomResponseForError(ex, equipe, _logger);
            }
        }

        // POST: api/Filieres/BIOV/Equipes/BIOV_MKT
        [HttpPost("{nomEquipe}")]
        public async Task<ActionResult<Personne>> PostPersonne(string codeFiliere, string nomEquipe, Personne personne)
        {
            try
            {
                Personne res = await _service.PostPersonne(nomEquipe, personne);
                object key = new { codeFiliere, nomEquipe };

                return CreatedAtAction(nameof(GetEquipe), key, res);
            }
            catch (Exception ex)
            {
                return this.CustomResponseForError(ex, personne, _logger);
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

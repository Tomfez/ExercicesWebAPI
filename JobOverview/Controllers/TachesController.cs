using Microsoft.AspNetCore.Mvc;
using JobOverview.Entities;
using JobOverview.Service;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Asp.Versioning;

namespace JobOverview.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiController]
    public class TachesController : ControllerBase
    {
        private readonly IServiceTaches _service;

        public TachesController(IServiceTaches service)
        {
            _service = service;
        }

        #region GET
        // GET: api/Taches?personne=x&logiciel=y&version=z
        /// <summary>Liste de tâches filtrable</summary>
        /// <remarks>
        /// Obtient les tâches de l'utilisateur courant si aucun filtre de personne n'est appliqué,  
        /// ou bien les tâches de la personne définie en filtre, si l'utilisateur courant est son manager.  
        /// 
        /// La liste est filtrable sur un logiciel et une version de logiciel.
        /// </remarks>
        /// <param name="personne">Pseudo de la personne dont on souhaite obtenir les tâches</param>
        /// <param name="logiciel">Code du logciel concerné</param>
        /// <param name="version">N° de version du logiciel</param>
        /// <response code="200">Liste des tâches</response>
        /// <response code="403">L'utilisateur courant n'est pas autorisé à obtenir la liste des tâches, car il n'est pas le manager de la personne spécifiée</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<Tache>>> GetTaches(
            [FromQuery] string? personne, [FromQuery] string? logiciel, [FromQuery] float? version)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Si aucun filtre sur la personne, on filtre sur l'utilisateur courant
            if (string.IsNullOrEmpty(personne))
            {
                personne = userId;
            }
            // Sinon, on vérifie que l'utilisateur courant est le manager de la personne
            // Si ce n'est pas le cas, on renvoie une réponse de code 403
            else
            {
                Personne? pers = await _service.GetPersonne(personne);
                if (pers == null || pers.Manager != userId)
                    return Forbid();
            }

            List<Tache> res = await _service.GetTaches(personne, logiciel, version);

            //if (res.Count == 0)
            //    return NotFound();

            return Ok(res);
        }

        // GET: api/Taches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tache?>> GetTache(int id)
        {
            Tache? tache = await _service.GetTache(id);

            if (tache == null)
                return NotFound();

            return Ok(tache);
        }

        // GET: api/Taches/44/Travaux
        [HttpGet("{idTache}/Travaux")]
        public async Task<ActionResult<IEnumerable<Travail>>> GetTravaux(int idTache)
        {
            return await _service.GetTravaux(idTache);
        }


        // GET: api/Personnes/RBEAUMONT
        [HttpGet("/api/personnes/{pseudo}")]
        public async Task<ActionResult<Personne?>> GetPersonne(string pseudo)
        {
            var personne = await _service.GetPersonne(pseudo);

            if (personne == null)
                return NotFound();

            return Ok(personne);
        }
        #endregion

        #region POST
        // POST: api/Taches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Tache>> PostTache(Tache tache)
        //{
        //    try
        //    {
        //        await _service.PostTache(tache);
        //        return CreatedAtAction(nameof(GetTache), new { id = tache.Id }, tache);
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.CustomResponseForError(ex);
        //    }
        //}

        // POST: api/Taches/44/Travaux
        [HttpPost("{idTache}/Travaux")]
        public async Task<ActionResult<Tache>> PostTravail([FromRoute] int idTache, Travail travail)
        {
            try
            {
                await _service.PostTravail(idTache, travail);
                return CreatedAtAction(nameof(GetTravaux), new { idTache }, travail);
            }
            catch (Exception ex)
            {
                return this.CustomResponseForError(ex);
            }
        }
        #endregion

        #region DELETE
        // DELETE: api/Taches/45/Travaux/2023-11-23
        [HttpDelete("{idTache}/Travaux/{date}")]
        public async Task<IActionResult> DeleteTravail(int idTache, DateOnly date)
        {
            try
            {
                await _service.DeleteTravail(idTache, date);
                return NoContent();
            }
            catch (Exception ex)
            {
                return this.CustomResponseForError(ex);
            }
        }

        // DELETE: api/Taches?personne=x&logiciel=y&version=z
        [HttpDelete]
        [Authorize(Policy = "GererTaches")]
        public async Task<IActionResult> DeleteTaches(
            [FromQuery] string? personne, [FromQuery] string? logiciel, [FromQuery] float? version)
        {
            try
            {
                int nbSupr = await _service.DeleteTaches(personne, logiciel, version);
                return Ok(nbSupr + " tâches supprimées");
            }
            catch (Exception ex)
            {
                return this.CustomResponseForError(ex);
            }
        }
        #endregion

        #region PUT
        // PUT: api/Taches/5
        /// <summary>Crée ou modifie une tâche</summary>
        /// <remarks>
        /// Si la tâche passée en paramètre à un id = 0, ajoute cette tâche dans la base,  
        /// sinon, modifie la tâche existante en base 
        /// </remarks>
        /// <param name="tache">Tâche à créer ou à modifier</param>
        /// <response code="200">Renvoie la tâche modifiée</response>
        /// <response code="201">Renvoie la tâche créée</response>
        /// <response code="400">Erreur si la personne spécifiée n'existe pas, ou si l'activité de la tâche ne fait pas partie de celles de la personne</response>
        /// <response code="403">L'utilisateur courant n'est pas autorisé à créer ou modifier des tâches</response>
        [HttpPut]
        [Authorize(Policy = "GererTaches")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Tache>> PutTache(Tache tache)
        {
            try
            {
                Tache result = await _service.PutPostTache(tache);

                // Si elle est ajoutée, EF renvoie 0
                if (result.Id == 0)
                    return CreatedAtAction(nameof(GetTache), new { result.Id }, result);
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                return this.CustomResponseForError(ex);
            }
        }
        #endregion
    }
}

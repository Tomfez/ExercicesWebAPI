﻿using JobOverview.Data;
using JobOverview.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace JobOverview.Controllers
{
    public static class ControllerBaseExtensions
    {
        // Renvoie une réponse HTTP personnalisée pour les erreurs
        public static ActionResult CustomResponseForError(this ControllerBase controller, Exception e)
        {
            if(e is DbUpdateConcurrencyException)
            {
                return controller.Problem("L'entité ou au moins l'une de ses entités filles n'existe pas en base.",
                    null, (int)HttpStatusCode.NotFound, "Aucune modification enregistrée en base.");
            }
            else if (e is DbUpdateException dbe)
            {
                ProblemDetails pb = dbe.ConvertToProblemDetails();
                return controller.Problem(pb.Detail, null, pb.Status, pb.Title);
            }
            else if (e is ValidationRulesException vre)
            {
                ValidationProblemDetails vpd = new(vre.Errors);
                return controller.ValidationProblem(vpd);
            }
            else throw e;
        }

        // Journalise une erreur avec le détail de l'action et de l'entité concernées,
        // puis renvoie une réponse HTTP personnalisée
        public static ActionResult CustomResponseForError<T>(this ControllerBase controller,
            Exception e, T entity, ILogger logger, [CallerMemberName] string? action = null)
        {
            if (e is DbUpdateException dbe)
            {
                ProblemDetails pb = dbe.ConvertToProblemDetails();
                logger.LogWarning("Action {action}, entité de type {type}\n{détail}\n{entity}",
                    action,
                    entity?.GetType().Name,
                    pb.Detail,
                    JsonSerializer.Serialize(entity, new JsonSerializerOptions { WriteIndented = true }));

                return controller.Problem(pb.Detail, null, pb.Status, pb.Title);
            }
            else if (e is ValidationRulesException vre)
            {
                ValidationProblemDetails vpd = new(vre.Errors);
                return controller.ValidationProblem(vpd);
            }
            else throw e;
        }
    }
}

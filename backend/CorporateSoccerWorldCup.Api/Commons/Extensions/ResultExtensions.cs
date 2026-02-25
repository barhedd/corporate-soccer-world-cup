using CorporateSoccerWorldCup.Application.Common.Errors;
using CorporateSoccerWorldCup.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace CorporateSoccerWorldCup.Api.Commons.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToFailureResult(this Result result)
    {
        if (result.Success)
            throw new InvalidOperationException(
                "Cannot map a successful result to failure response.");

        return result.ErrorCode switch
        {
            ErrorCodes.DeleteError =>
                new ConflictObjectResult(result.ErrorMessage),

            ErrorCodes.DuplicateError =>
                new ConflictObjectResult(result.ErrorMessage),

            ErrorCodes.FieldTooLong =>
                new BadRequestObjectResult(result.ErrorMessage),

            ErrorCodes.InvalidRange =>
                new BadRequestObjectResult(result.ErrorMessage),

            ErrorCodes.NotAuthorized =>
                new UnauthorizedObjectResult(result.ErrorMessage),
         
            ErrorCodes.NotEmptyField =>
                new BadRequestObjectResult(result.ErrorMessage),

            ErrorCodes.NotFound =>
                new NotFoundObjectResult(result.ErrorMessage),

            ErrorCodes.ReadOnlyError =>
                new ConflictObjectResult(result.ErrorMessage),

            _ => new BadRequestObjectResult(result.ErrorMessage)
        };
    }
}

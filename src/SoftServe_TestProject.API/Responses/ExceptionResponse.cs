using System.Net;

namespace SoftServe_TestProject.API.Responses
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Description) { }
}

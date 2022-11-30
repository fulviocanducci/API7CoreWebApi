using Microsoft.AspNetCore.Mvc;
using WebApplication4.Extensions;
using WebApplication4.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.GetWebApplication();

var group = app.MapGroup("/api");

group.MapGet("/home", () => { return new { Name = "Fulvio" }; }).RequireAuthorization();

group.MapGet("/jwt", ([FromServices] ITokenService tokenService) =>
{
    return tokenService.GetToken();
});

app.Run();
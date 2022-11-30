using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("jwt").AddJwtBearer("jwt", o =>
{

});
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();

app.MapGet("/home", () => "Home Page");

app.MapGet("/jwt", () =>
{
    var handler = new JsonWebTokenHandler();
    DateTime createdAt = DateTime.UtcNow;
    DateTime expiresAt = createdAt.AddDays(7);
    string token = handler.CreateToken(new SecurityTokenDescriptor()
    {
        Subject = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, "fulviocanducci@hotmail.com"),
            new Claim(ClaimTypes.Email, "fulviocanducci@hotmail.com")
        }),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Secret.Bytes), SecurityAlgorithms.HmacSha256Signature),
        Expires = expiresAt
    });
    return token;
});

app.Run();

public static class Secret
{
    public static string Value
    {
        get
        {
            return "43e4dbf0-52ed-4203-895d-42b586496bd4";
        }
    }
    public static byte[] Bytes
    {
        get
        {
            return Encoding.ASCII.GetBytes(Value);
        }
    }
}


using System.ComponentModel.DataAnnotations;
using System.Globalization;
using DotnetInterview.Data;
using DotnetInterview.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<UserRepository>();

const string CorsPolicy = "AllowAngular";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
    {
        var allowedOrigins = builder.Configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? new[] { "http://localhost:4200" };

        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(CorsPolicy);

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapGet("/", () => Results.Ok(new { service = "dotnetinterview", status = "ok" }));

app.MapPost("/api/users", async (UserCreateRequest req, UserRepository repo, CancellationToken ct) =>
{
    var validationResults = new List<ValidationResult>();
    var validationContext = new ValidationContext(req);
    if (!Validator.TryValidateObject(req, validationContext, validationResults, validateAllProperties: true))
    {
        return Results.ValidationProblem(validationResults.ToDictionary(
            r => r.MemberNames.FirstOrDefault() ?? "request",
            r => new[] { r.ErrorMessage ?? "Invalid value" }));
    }

    if (!DateOnly.TryParseExact(req.BirthDay, "dd/MM/yyyy", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var birthDay))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["BirthDay"] = new[] { "BirthDay must be a valid date in DD/MM/YYYY format" }
        });
    }

    var user = new User
    {
        FirstName = req.FirstName.Trim(),
        LastName = req.LastName.Trim(),
        Email = req.Email.Trim(),
        Phone = req.Phone.Trim(),
        ProfileBase64 = req.ProfileBase64,
        BirthDay = birthDay,
        Occupation = req.Occupation.Trim(),
        Gender = req.Gender.Trim()
    };

    var id = await repo.InsertAsync(user, ct);
    return Results.Ok(new { id });
})
.WithName("CreateUser");

app.Run();

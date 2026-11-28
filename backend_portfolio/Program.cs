using Microsoft.EntityFrameworkCore;
using main.db;
using functions.Endpoint;
using Microsoft.AspNetCore.Identity;
using Models.user;
using data.DbSeeder;
using data.Policies;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi



builder.Services.AddOpenApi();
builder.Services.AddValidation();
builder.Services.AddIdentityApiEndpoints<User>()
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<PortDB>();


string editor = "Editor";
string admin = "Admin";

builder.Services.AddAuthorizationBuilder()
.AddPolicy(Policies.CanEditItem, policy => policy.RequireRole(editor, admin))
.AddPolicy(Policies.CanCreateItem, policy => policy.RequireRole(editor, admin))
.AddPolicy(Policies.CanDeleteItem, policy => policy.RequireRole(editor, admin));

var policyVersion = "2";


builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero;

    options.OnRefreshingPrincipal = context =>
{
    var roleInCookie = context.CurrentPrincipal?.FindFirstValue(ClaimTypes.Role);
    var roleInDatabase = context.NewPrincipal?.FindFirstValue(ClaimTypes.Role);
    var policyInCookie = context.CurrentPrincipal?.FindFirstValue("policy_version");

    if (roleInCookie != roleInDatabase ||
        (policyInCookie is not null && policyInCookie != policyVersion))
        context.NewPrincipal = null;

    context.NewPrincipal?.Identities.First().AddClaim(new Claim("policy_version", policyVersion));

    return Task.CompletedTask;
};
});

builder.Configuration.AddUserSecrets<Program>(optional: true);


var connectionString = builder.Environment.IsEnvironment("Testing")
    ? builder.Configuration.GetConnectionString("TestConnection")
    : builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PortDB>(options =>
    options.UseNpgsql(connectionString));


var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
 await DbSeeder.SeedAsync(app.Services);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "PortDB"));

}






app.MapAllEndpoints();


app.Run();

//zoek naar de goede 

public partial class Program { }
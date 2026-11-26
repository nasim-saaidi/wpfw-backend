using Microsoft.EntityFrameworkCore;
using main.db;
using functions.Endpoint;
using Microsoft.AspNetCore.Identity;
using Models.user;
using data.DbSeeder;
using data.Policies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddOpenApi();
builder.Services.AddValidation();
builder.Services.AddIdentityApiEndpoints<User>()
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<PortDB>();

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero;
});




// builder.Services.AddAuthentication();
// builder.Services.AddAuthorization();
builder.Services.AddAuthorizationBuilder()
.AddPolicy(Policies.CanEditItem, policy => policy.RequireRole("Editor", "Admin"))
.AddPolicy(Policies.CanCreateItem, policy => policy.RequireRole("Editor", "Admin"))
.AddPolicy(Policies.CanDeleteItem, policy => policy.RequireRole("Editor", "Admin"));


builder.Services.AddDbContext<PortDB>((sp, options) =>
    options.UseNpgsql(sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")));


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
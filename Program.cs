using WebApi.Helpers;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// ajouter les services au conteneur DI
{
    var services = builder.Services;
    services.AddCors();
    services.AddControllers();

    // configurer l'objet de paramètres de type fortement typé
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    // configurer DI pour les services de l'application
    services.AddScoped<IUserService, UserService>();
}

var app = builder.Build();

// configurer le pipeline des requêtes HTTP
{
    // politique cors globale
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    // middleware d'authentification jwt personnalisé
    app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();
}

app.Run("http://localhost:4000");

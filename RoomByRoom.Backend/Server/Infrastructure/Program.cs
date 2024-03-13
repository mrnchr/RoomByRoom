using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Server.Authentication;
using Server.Database;
using Server.Profile;
using SharedData.Checkers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("Database/PrivateDbConfig.json");

builder.Services.AddControllers()
    .AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

IConfigurationSection? hostDb = builder.Configuration.GetSection("SelectedDb");

var webglPath = "";
string? webglDir = Directory.GetDirectories("wwwroot", "RoomByRoom.Web").SingleOrDefault();
if (Directory.Exists(webglDir))
    webglPath = Path.GetFileName(webglDir);

builder.Services
    .Configure<DbConnectionInfo>(builder.Configuration.GetSection($"{hostDb.Value}:ConnectionInfo"))
    .AddDbContext<ApplicationContext>()
    .AddScoped<IAuthDataProvider, AuthDbProvider>()
    .AddScoped<IAuthenticationService, AuthenticationService>()
    .AddScoped<IUserInputChecker, UserInputChecker>()
    .AddScoped<ICheckingService, CheckingService>()
    .AddScoped<IProfileDataProvider, ProfileDataProvider>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = AuthenticationOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateIssuer = false
        };
    });

WebApplication app = builder.Build();

app.UseDefaultFiles();
var provider = new FileExtensionContentTypeProvider
{
    Mappings =
    {
        [".data"] = "application/octet-stream",
        [".wasm"] = "application/wasm",
        [".br"] = "application/octet-stream",
        [".js"] = "application/javascript"
    }
};

app.UseStaticFiles(new StaticFileOptions()
{
    ContentTypeProvider = provider,
    OnPrepareResponse = context =>
    {
        var path = context.Context.Request.Path.Value;
        var extension = Path.GetExtension(path);
        
        if (extension is ".gz" or ".br")
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path) ?? "";
            if (provider.TryGetContentType(fileNameWithoutExtension, out string? contentType))
            {
                context.Context.Response.ContentType = contentType;
                context.Context.Response.Headers.Add("Content-Encoding", extension == ".gz" ? "gzip" : "br");
            }
        }
    },
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Map("/", (HttpContext context) => context.Response.Redirect(webglPath));
app.Map("/alive", () => Results.Ok());
app.Map("/authorize", [Authorize]() => Results.Ok());

app.Run();

return;

void PrintBody(HttpContext context)
{
    string bodyStr;
    HttpRequest req = context.Request;

    // Allows using several time the stream in ASP.Net Core
    req.EnableBuffering();

    // Arguments: Stream, Encoding, detect encoding, buffer size 
    // AND, the most important: keep stream opened
    using (StreamReader reader
        = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
    {
        bodyStr = reader.ReadToEndAsync().Result;
    }

    Console.WriteLine("Body:" + bodyStr);

    // Rewind, so the core is not lost when it looks at the body for the request
    req.Body.Position = 0;
}

void PrintHeader(HttpContext context)
{
    Console.WriteLine(context.Request.Headers.Authorization);
}


using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Ardalis.GuardClauses;
using Ardalis.ListStartupServices;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using PomoziAuctions.Abstractions.Web;
using PomoziAuctions.Core;
using PomoziAuctions.Core.Abstractions;
using PomoziAuctions.Core.Aggregates.BlobAggregate.Interfaces;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Interfaces;
using PomoziAuctions.Core.Aggregates.CompanyAggregate.Models;
using PomoziAuctions.Core.Auth.Identity.Models;
using PomoziAuctions.Core.Auth.Identity.Services;
using PomoziAuctions.Core.Auth.Security.Interfaces;
using PomoziAuctions.Core.Auth.Security.Services;
using PomoziAuctions.Infrastructure;
using PomoziAuctions.Infrastructure.AzureStorage;
using PomoziAuctions.Infrastructure.Data;
using PomoziAuctions.Infrastructure.Emailing;
using PomoziAuctions.Infrastructure.Encryption;
using PomoziAuctions.Infrastructure.Identity;
using PomoziAuctions.SharedKernel.DataFilters;
using PomoziAuctions.Web.Configuration;
using PomoziAuctions.Web.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Extensions.Logging;
using IdentitySeedData = PomoziAuctions.Infrastructure.Identity.SeedData;

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting web host");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));
var microsoftLogger = new SerilogLoggerFactory(logger)
    .CreateLogger<Program>();

// Configure Web Behavior
builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.CheckConsentNeeded = context => true;
  options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddSingleton<IStringEncryptor, StringEncryptor>()
  .AddDataProtection().PersistKeysToDbContext<AppDbContext>();

builder.Services
  .AddIdentity<PomoziAuctions.Core.Auth.Identity.Models.IdentityUser, PomoziAuctions.Core.Auth.Identity.Models.IdentityRole>(options =>
  {
    options.Tokens.EmailConfirmationTokenProvider = "Email";
    options.Tokens.ChangeEmailTokenProvider = "Email";
    options.Tokens.PasswordResetTokenProvider = "Email";
    options.User.RequireUniqueEmail = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
  })
  .AddEntityFrameworkStores<IdentityDbContext>()
  .AddDefaultTokenProviders();

builder.Services.AddAuthentication(o =>
{
  o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
  .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
  {
    o.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateAudience = false,
      ValidateIssuer = false,
      ValidateLifetime = false,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Authentication:AccessTokenSecret")))
    };
  });

builder.Services
  .Configure<PomoziAuctions.Core.Auth.Security.Models.TokenOptions>(builder.Configuration.GetSection("Authentication"))
  .AddScoped<ITokenService, TokenService>()
  .AddCurrentUserServices();

builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
       o.TokenLifespan = TimeSpan.FromHours(1));

// Add CORS policy
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowSpecificOrigins",
      policy =>
      {
        policy.WithOrigins("https://white-grass-031220f03.5.azurestaticapps.net")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
      });
});

// Add services
builder.Services.AddControllers().AddJsonOptions(options =>
{
  options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
  options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddResponseCaching();


// Add DbContext and more.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Guard.Against.Null(connectionString);

builder.Services.AddDbContext<AppDbContext>(o =>
{
  o.UseSqlServer(connectionString);
});

builder.Services.AddDbContext<IdentityDbContext>(o =>
{
  o.EnableSensitiveDataLogging(true);
  o.UseSqlServer(connectionString);
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IdentityService>();
builder.Services.AddScoped<IDataFilter, DataFilter>()
  .AddScoped<ICurrentCompany, CurrentCompany>()
  .AddSingleton<ICurrentCompanyAccessor, CurrentCompanyAccessor>();




builder.Services.AddScoped<IBlobStorage, AzureStorage>();
builder.Services.AddSingleton<IAppPath, AppPath>();

builder.Services.Configure<IdentityEmailOptions>(builder.Configuration.GetSection("IdentityEmail"));
if (builder.Environment.IsDevelopment())
{
  //builder.Services.AddSingleton<IEmailSender, SmtpEmailSender>();
  builder.Services.AddSingleton<IEmailSender, SendGridEmailSender>().Configure<SendGridOptions>(builder.Configuration.GetSection("SendGrid"));
}
else
{
  builder.Services.AddSingleton<IEmailSender, SendGridEmailSender>().Configure<SendGridOptions>(builder.Configuration.GetSection("SendGrid"));
}

// TO DO: Try to split adding services into a separate module using Autofac.
//builder.Services.AddInfrastructureServices(builder.Configuration, microsoftLogger);

builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new OpenApiInfo { Title = "Sajamska Aplikacija API", Version = "v1" });
  options.EnableAnnotations();
});

var devCorsPolicy = "devCorsPolicy";
builder.Services.AddCors(options =>
{
  options.AddPolicy(devCorsPolicy, builder =>
  {
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
  });
});



if (builder.Environment.IsDevelopment())
{
  // Use a local test email server
  // See: https://ardalis.com/configuring-a-local-test-email-server/
  //builder.Services.AddScoped<IEmailSender, MimeKitEmailSender>();

  // Otherwise use this:
  //builder.Services.AddScoped<IEmailSender, FakeEmailSender>();
  AddShowAllServicesSupport();
}
else
{
  //builder.Services.AddScoped<IEmailSender, MimeKitEmailSender>();
}

var mappingConfig = new MapperConfiguration(
  mc => mc.AddProfile(new AutoMappingProfile()));
var mapper = mappingConfig.CreateMapper();

builder.Services.AddSingleton(mapper);

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
  containerBuilder.RegisterModule(new DefaultCoreModule());
  containerBuilder.RegisterModule(new DefaultInfrastructureModule(builder.Environment.EnvironmentName == "Development"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseShowAllServicesMiddleware(); // see https://github.com/ardalis/AspNetCoreStartupServices
  app.UseCors(devCorsPolicy);
}
else
{
  app.UseExceptionHandler("/Home/Error"); // from FastEndpoints                                         
  app.UseCors("AllowSpecificOrigins");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
app.UseSwaggerUI(options =>
{
  options.SwaggerEndpoint("swagger/v1/swagger.json", "Sajamska Aplikacija API V1");
  options.RoutePrefix = string.Empty;
});


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Set up your endpoints
app.MapDefaultControllerRoute();
app.MapFallbackToFile("index.html");


await SeedDatabase(app);

app.Run();

static async Task SeedDatabase(WebApplication app)
{
  using var scope = app.Services.CreateScope();
  var services = scope.ServiceProvider;

  try
  {
    var appContext = services.GetRequiredService<AppDbContext>();
    var identityContext = services.GetRequiredService<IdentityDbContext>();

    if (app.Environment.IsDevelopment())
    {
      // Seed Companies
      if (!appContext.Companies.Any())
      {
        //appContext.Companies.AddRange(SeedData.GetCompanies());
        await appContext.SaveChangesAsync();
      }

      // Seed Users
      if (!identityContext.Users.Any())
      {
        var userManager = services.GetRequiredService<UserManager<PomoziAuctions.Core.Auth.Identity.Models.IdentityUser>>();
        foreach (var user in IdentitySeedData.GetUsers())
        {
          // Log user properties before creating
          Console.WriteLine($"Creating user: {user.UserName}, CompanyId: {user.CompanyId}, CandidateId: {user.CandidateId}");
          var createResult = await userManager.CreateAsync(user, "Password1");
          if (!createResult.Succeeded)
          {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError("Failed to create user {userName}: {errors}", user.UserName, string.Join(", ", createResult.Errors.Select(e => e.Description)));
          }
        }
      }

      if (!identityContext.UserRoles.Any())
      {
        identityContext.UserRoles.AddRange(IdentitySeedData.GetUserRoles());
        await identityContext.SaveChangesAsync();
      }
    }
    else
    {
      // Seed Users
      if (!identityContext.Users.Any())
      {
        var userManager = services.GetRequiredService<UserManager<PomoziAuctions.Core.Auth.Identity.Models.IdentityUser>>();
        foreach (var user in IdentitySeedData.GetProdUsers())
        {
          // Log user properties before creating
          Console.WriteLine($"Creating user: {user.UserName}, CompanyId: {user.CompanyId}, CandidateId: {user.CandidateId}");
          var createResult = await userManager.CreateAsync(user, "Password1");
          if (!createResult.Succeeded)
          {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError("Failed to create user {userName}: {errors}", user.UserName, string.Join(", ", createResult.Errors.Select(e => e.Description)));
          }
        }
      }

      if (!identityContext.UserRoles.Any())
      {
        identityContext.UserRoles.AddRange(IdentitySeedData.GetProdUserRoles());
        await identityContext.SaveChangesAsync();
      }
    }
  }
  catch (Exception ex)
  {
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
  }
}

void AddShowAllServicesSupport()
{
  // add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
  builder.Services.Configure<ServiceConfig>(config =>
  {
    config.Services = new List<ServiceDescriptor>(builder.Services);

    // optional - default path to view services is /listallservices - recommended to choose your own path
    config.Path = "/listservices";
  });
}

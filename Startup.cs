using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TaskApplicationApi.Clients;
using TaskApplicationApi.Repositories;
using TaskApplicationApi.Repositories.Interfaces;
using TaskApplicationApi.Services;
using TaskApplicationApi.Services.Interfaces;

namespace TaskApplicationApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter JWT Token in bearer format: 'bearer {token}'"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });
            });
            services.AddTransient<IUserTasksService, UserTasksService>();
            services.AddTransient<IUserTasksRepository, AzureCosmosUserTasksRepository>();

            services.AddTransient<IUserPreferencesService, UserPreferencesService>();
            services.AddTransient<IUserPreferencesRepository, AzureCosmosUserPreferencesRepository>();

            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IUsersRepository, AzureCosmosUsersRepository>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration.GetValue<string>("Auth0:Domain");
                    options.Audience = Configuration.GetValue<string>("Auth0:Audience");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            ConfigureAzureCosmosDb(services);

            services.AddMvc(config =>
            {
                config.Filters.Add(typeof(ResponseStatusExceptionFilter));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
        }

        private void ConfigureAzureCosmosDb(IServiceCollection services)
        {
            var client = new CosmosClient(
                Configuration.GetValue<string>("CosmosDB:Endpoint"),
                Configuration.GetValue<string>("CosmosDB:PrimaryKey"));

            services.AddSingleton<CosmosClient>(x => client);

            var azureCosmosDbClient = new AzureCosmosDbClient(client, Configuration);

            services.AddSingleton<IAzureCosmosDbClient>(x => azureCosmosDbClient);
        }
    }
}

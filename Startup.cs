using Microsoft.Azure.Cosmos;
using TaskApplicationApi.Clients;
using TaskApplicationApi.Repositories;
using TaskApplicationApi.Services;

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
            services.AddSwaggerGen();
            services.AddTransient<IUserTasksService, UserTasksService>();
            services.AddTransient<IUserTasksRepository, AzureCosmosUserTasksRepository>();

            ConfigureAzureCosmosDb(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
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

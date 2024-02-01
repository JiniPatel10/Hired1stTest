using backendApi.Infrastructure;
using core.Data;
using core.Data.Repositories;
using core.Domain.InterfaceRepositories;
using core.Domain.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;

namespace backendApi
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add cors Policy, allow all because is Dev
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("ApiCorsPolicy",
            //        s => s.AllowAnyOrigin()
            //              .AllowAnyMethod()
            //              .AllowAnyHeader());
            //});

            services.AddCors(options =>
            {
                //TODO : Use a App setting instead of hardcoded URL
                options.AddPolicy("ApiCorsPolicy",
                    s => s.AllowAnyOrigin()      
                    .AllowAnyMethod()
                          .AllowAnyHeader()
                         );
            });

            SetConnectionStringForMongoDBContext();
            SetDependencyInjectionDefinitions(services);
            SetGmailCredentials();
            // Add framework services.
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
            });
            services.AddHttpClient();
            services.AddControllers().AddNewtonsoftJson();
            
  
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors("ApiCorsPolicy");
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

      
   
        private void SetConnectionStringForMongoDBContext()
        {
            MongoDBContext.ConnString = Configuration.GetSection("MongoConnection:ConnString").Value;
            MongoDBContext.Database = Configuration.GetSection("MongoConnection:Database").Value;
        }
        private void SetGmailCredentials()
        {
            MailService.Username = Configuration.GetSection("Gmail:Username").Value;
            MailService.Password = Configuration.GetSection("Gmail:Password").Value;
            MailService.SMTPPort = Configuration.GetSection("Gmail:SMTPPort").Value;
            MailService.SMTPServer = Configuration.GetSection("Gmail:SMTPServer").Value;
        }

        private void SetDependencyInjectionDefinitions(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<IMongoDBContext, MongoDBContext>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IMailService, MailService>();
        }



    }
}

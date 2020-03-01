using ErrorLog.Common;
using ErrorLog.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace ErrorLog.API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public string[] ApiAllowedCorsOrigins => _config.GetValue<string>("AllowedCorsOrigins").Split(",");
        public string ApiAuthorityUrl => _config.GetValue<string>("ApiAuthorityUrl");
        public string ApiName => _config.GetValue<string>("ApiName");

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("1.0.0", new OpenApiInfo { Title = "ErrorLogAPI", Version = "1.0.0" });
                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey
                //});
            });

            services.AddControllers();
            services.AddTransient<ILogAccess, LogsService>();
            services.AddTransient<ILogger, LogsService>();
            services.AddTransient<IBackupLogger, FileSystemLogger>();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = ApiAuthorityUrl;
                    options.RequireHttpsMetadata = false;
                    options.Audience = ApiName;
                });

            services.AddCors();

            services.AddMvcCore()
                .AddApiExplorer()
                .AddMvcOptions(opt => opt.EnableEndpointRouting = false)
                .AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/1.0.0/swagger.json", "ErrorLogAPI 1.0.0");
            });

            app.UseCors(
                options => options
                    .WithOrigins(ApiAllowedCorsOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );

            //app.UseRouting();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}

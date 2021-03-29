using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JWTSandbox.Audience.API.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JWTSandbox.Audience.API
{
    public class Startup
    {
        // Example
        // https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api

        // Explanation
        // https://medium.com/marzouk/jwt-authentication-to-authenticate-many-parties-asp-net-example-4d4af924419d

        // https://github.com/jignesht24/Aspnetcore/tree/master/JWTAuthentication/JWTAuthentication

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // configure strongly typed settings objects
            var authenticationSettingsSection = Configuration.GetSection("Authentication");
            services.Configure<AuthenticationSettings>(authenticationSettingsSection);

            ConfigureAuthentication(services, authenticationSettingsSection);
        }

        private void ConfigureAuthentication(IServiceCollection services, IConfigurationSection authenticationSettingsSection)
        {
            var authSettings = authenticationSettingsSection.Get<AuthenticationSettings>();

            var key = Convert.FromBase64String(authSettings.AudienceSecret);

            services.AddAuthentication(
                x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            )
            .AddJwtBearer(
                x =>
                {
                    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuerSigningKey = true,

                        ValidateAudience = true,
                        ValidAudience = authSettings.AudienceName,

                        ValidateIssuer = true,
                        ValidIssuer = authSettings.Issuer,

                        ValidateLifetime = true
                        
                    };
                }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

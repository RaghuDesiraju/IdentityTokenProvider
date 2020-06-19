using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityTokenProvider.Models;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace IdentityTokenProvider
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddNewtonsoftJson();

            services.AddTransient<ITokenProvider, TokenProvider>();
            //services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            ClientStore.AuthURL = Configuration["AuthURL"];
            ClientStore.AuthUserURL = Configuration["AuthUserURL"];


            services.AddIdentityServer(options => options.IssuerUri = "localhost")
            .AddInMemoryApiResources(ClientStore.GetApiResources())
            .AddInMemoryIdentityResources(ClientStore.GetIdentityResources())
            .AddInMemoryClients(ClientStore.GetClients())
            .AddDeveloperSigningCredential();

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateAudience = false,
            //        ValidateLifetime = true,
            //        LifetimeValidator = (notBefore, expires, securityToken, validationParameter) =>
            //    expires >= DateTime.UtcNow
            //    };

            //    options.RequireHttpsMetadata = false;
            //    options.Authority = "http://localhost:5000";
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseIdentityServer();
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

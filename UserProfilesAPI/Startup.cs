using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserProfilesAPI.Data;
using UserProfilesAPI.Helper;

namespace UserProfilesAPI
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
            services.Configure<KestrelServerOptions>(
                Configuration.GetSection("Kestrel"));

            //services.AddDbContext<AuthDbContext>(options =>
            //     options.UseSqlServer(Configuration.GetConnectionString("AuthDB")));
            //if (Configuration["SourceType"] != "db")
            //{
            //services.AddDbContext<AuthDbContext>(options =>
            // options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            //}
            //else
            //{
            //    services.AddDbContext<AuthDbContext>(options =>
            // options.UseSqlServer(Configuration.GetConnectionString("DefaultSqlServerConnection")));

            //}

            services.AddDbContext<AuthDbContext>(options =>
             options.UseSqlServer(UserProfilesAPI.Helper.EncryptDecrypt.DecryptString(
                 Configuration.GetConnectionString("DefaultSqlServerConnection"))));

            services.AddControllers().AddNewtonsoftJson();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });

            services.AddAutoMapper(typeof(Startup));
            services.AddTransient<ICredentialRepository, ClientCredentialRepository>();
            services.AddTransient<IAuthUserRepository, AuthUserRepository>();

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseExceptionHandler(builder => {
                builder.Run(async context => {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        context.Response.AddApplicationError(error.Error.Message);
                        await context.Response.WriteAsync(error.Error.Message);
                    }
                });
            });

            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

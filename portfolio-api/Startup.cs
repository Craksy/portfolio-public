using System;
using System.IO;
using System.Reflection;
using auth_api.AuthRequirements;
using auth_api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using auth_api.Services;

namespace auth_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            //Auth stuff
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.Authority = "https://sw-auth0-test.eu.auth0.com/";
                options.Audience = "https://sw-auth.org/api";
            });

            services.AddAuthorization(options =>{
                foreach(var perm in Configuration.GetSection("Scopes").GetChildren()){
                    Console.WriteLine($"perm: {perm.Value}");
                    options.AddPolicy(perm.Value, p => {
                        p.RequireClaim("permissions", perm.Value);
                    });
                }
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();


            services.AddDbContext<BlogDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("foo"));
            });
            
            services.AddSingleton<IStorageService>(new StorageService(Configuration.GetConnectionString("blob-storage")));
            services.AddScoped<IBlogService, BlogService>();
            services.AddControllers();
            services.AddCors(opts =>{
                opts.AddDefaultPolicy(builder =>{
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("*");
                });
            });
            services.AddSwaggerGen(c => {
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { 
                    { securityScheme, new string[] { } } 
                    });
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "auth_api", Version = "v1" });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "auth_api v1"));
            }

            app.UseHttpsRedirection();
            app.UseCors();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

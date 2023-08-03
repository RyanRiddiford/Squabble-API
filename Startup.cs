using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Squabble.Data;
using Squabble.Helpers;
using Squabble.Managers;
using Squabble.Models;

using System.Text;

namespace Squabble
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //Add database context
            services.AddDbContext<SquabbleContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(nameof(SquabbleContext)));

            }
          );

            //Cookies configuration
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            }
            );

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    {
                        builder
                            .WithOrigins(
                                "http://localhost:4200",
                                "https://*.azurestaticapps.net",
                                "https://*.eastasia.azurestaticapps.net"
                            )
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddScoped<AccountManager>();
            services.AddScoped<LoginManager>();
            services.AddScoped<PostManager>();
            services.AddScoped<ServerManager>();
            services.AddScoped<CommunicationTokenManager>();
            services.AddScoped<ChannelManager>();
            services.AddScoped<FriendshipManager>();
            services.AddScoped<FriendRequestManager>();
            services.AddScoped<KanbanManager>();


            //Manual compatibility setting
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest);

            services.AddAuthentication().AddCookie(cfg => cfg.SlidingExpiration = true);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(cfg =>
                     {
                         cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                         {
                             ValidIssuer = JwtHelpers.Issuer,
                             ValidAudience = JwtHelpers.Audience,
                             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtHelpers.Key))
                         };
                     });

            //Add Web Api controllers
            services.AddControllers();

            //Generate Swagger UI
            services.AddSwaggerGen();

            services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //Use Swagger UI for API development
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Squabble V1.2");
                    c.RoutePrefix = string.Empty;
                });

            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Author}/{action=GetAuthor}/{id?}");
            });
        }
    }
}

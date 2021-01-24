using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Ecco.Web.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ecco.Web.Areas.Identity;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Ecco.Web.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Westwind.AspNetCore.LiveReload;

namespace Ecco.Web
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //services.AddIdentity<EccoUser, IdentityRole>()
            services.AddDefaultIdentity<EccoUser>()    
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<UserManager<EccoUser>>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireDigit = false;
                options.SignIn.RequireConfirmedEmail = false;
            });

            var secret = Configuration["Secret"];

            var builder = services.AddIdentityServer()
              .AddDeveloperSigningCredential()
              .AddInMemoryApiScopes(AuthConfig.ApiScopes)
              .AddInMemoryClients(AuthConfig.GetClients(secret));

            services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://ecco-space.azurewebsites.net";
                    //options.Authority = "https://localhost:44376";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                    };
                });

            string storageConnectionString = Configuration.GetConnectionString("StorageConnection");
            services.AddSingleton(typeof(StorageService), new StorageService(storageConnectionString));

            string sendGridApiKey = Configuration["SendGridKey"];
            services.AddSingleton(typeof(IEmailSender), new EccoEmailSender(sendGridApiKey));

            string notificationHubConnectionString = Configuration.GetConnectionString("NotificationHub");
            services.AddSingleton(typeof(NotificationService), new NotificationService("Ecco-Space", notificationHubConnectionString));

            string eventsHubConnectionString = Configuration.GetConnectionString("EventsHub");
            services.AddSingleton(typeof(EventsHubService), new EventsHubService(eventsHubConnectionString));

            services.AddSession();

            services.AddLiveReload(config => 
            {
                config.LiveReloadEnabled = true;
                config.ClientFileExtensions = ".cshtml,.css,.js,.htm,.html,.ts,.razor,.custom";
            });

            services.AddControllersWithViews();
            //services.AddRazorPages();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddMvc().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseLiveReload();

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "application/json"
            });

            app.UseRouting();

            app.UseSession();

            app.UseIdentityServer();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
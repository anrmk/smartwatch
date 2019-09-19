using Core.Context;
using Core.Managers;
using Core.Services.Business;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartWatch.App_Config;
using SmartWatch.Hubs;

namespace SmartWatch {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Registering Automapper
            MapperConfig.Register();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR();

            // Additional configuration for Npgsql
            services.AddDbContext<ApplicationContext>(options => {
                options.UseNpgsql("Host=localhost;Database=smartwatch;Username=postgres;Password=postgres");
            });
            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationContext>().BuildServiceProvider();

            services.AddScoped<IApplicationContext, ApplicationContext>();

            services.AddTransient<IDeviceManager, DeviceManager>();
            services.AddTransient<IDeviceLocationManager, DeviceLocationManager>();
            services.AddTransient<IDeviceLastLocationManager, DeviceLastLocationManager>();
            services.AddTransient<IProfileCardManager, ProfileCardManager>();
            services.AddTransient<IProfileCardMediaManager, ProfileCardMediaManager>();
            services.AddTransient<IIndicatorManager, IndicatorManager>();

            services.AddTransient<ILocationBusinessService, LocationBusinessService>();
            services.AddTransient<IProfileCardBusinessService, ProfileCardBusinessService>();
            services.AddTransient<IIndicatorBusinessService, IndicatorBusinessService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSignalR(routes => {
                routes.MapHub<DeviceLocationHub>("/deviceLocationHub");
            });

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "area",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "api",
                    template: "api/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

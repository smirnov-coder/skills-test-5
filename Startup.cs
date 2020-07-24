using System;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SkillsTest.Data;
using SkillsTest.Development;
using SkillsTest.Mappings;

namespace SkillsTest
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string
                connectionString = Configuration.GetConnectionString("SqliteConnection"),
                dataFolder = Path.Combine(Environment.ContentRootPath, "Data");
            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);
            connectionString = connectionString.Replace("{DataDirectory}", dataFolder);

            // Добавить EntityFrameworkCore с SQLite.
            services.AddDbContext<MoviesDbContext>(options => options.UseSqlite(connectionString));
            // Добавить Identity.
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<MoviesDbContext>();

            // Настройка cookie.
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                // Время хранения куки в браузере = 10 дней.
                options.ExpireTimeSpan = TimeSpan.FromDays(10);
                options.SlidingExpiration = true;

                // Редирект на страницу логина при формировании ответа 401.
                options.LoginPath = "/account/login";
                // Редирект на страницу "Доступ запрещён" при формировании ответа 403.
                options.AccessDeniedPath = "/error/403";
            });

            // Настройка маппинга Automapper.
            var mappingConfig = new MapperConfiguration(config => config.AddProfile(new MappingProfile()));
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Добавить пользовательские сервисы в контейнер внедрения зависимостей.
            services.AddTransient<FakeDataInitializer>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Включить автообновление страницы браузера при сохранении файлов .cshtml.
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error/Index");
                // The default HSTS value is 30 days. You may want to change this for production scenarios,
                // see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            // Включить редиректы на соответствующие страницы при формировании ответов 401, 403, 404.
            app.UseStatusCodePagesWithRedirects("/Error/{0}");

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Movies}/{action=Index}/{id?}");
            });
        }
    }
}

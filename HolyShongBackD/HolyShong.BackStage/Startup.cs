using HolyShong.BackStage.Entity;
using HolyShong.BackStage.Helpers;
using HolyShong.BackStage.Repositories;
using HolyShong.BackStage.Repositories.Interfaces;
using HolyShong.BackStage.Services;
using HolyShong.BackStage.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolyShong.BackStage
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

            services.AddControllersWithViews();
            services.AddSwaggerDocument();
            //註冊資料庫
            services.AddDbContext<HolyShongContext>(options => options.UseSqlServer(Configuration.GetConnectionString("HolyShongContext")));
            //注入Repository
            services.AddTransient<IDbRepository, DbRepository>();
            //注入Service
            services.AddTransient<IDiscountAService, DiscountAService>();
            services.AddTransient<IDiscountService, DiscountService>();
            services.AddTransient<IDashboardService, DashboardService>();

            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<ILinebotService, LinebotService>();

            //認證
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
            {
                option.IncludeErrorDetails = true;

                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidIssuer = JwtHelper.Issuer,
                    IssuerSigningKey = JwtHelper.SecurityKey,
                    ValidateAudience = false
                };
            });
            services.AddTransient<IMemberService, MemberService>();
            services.AddTransient<IStoreService, StoreService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseRouting();

            //先驗證
            app.UseAuthentication();
            //再授權
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

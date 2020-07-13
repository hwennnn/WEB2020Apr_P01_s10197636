using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication;

namespace web_s10197636
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
            // Add a default in-memory implementation of distributed cache
            services.AddDistributedMemoryCache();
            // Add the session service
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();

            // Adds the authentication services
            services.AddAuthentication(options =>
            {
                // Use a cookie to locally sign-in the user
                options.DefaultScheme =
                CookieAuthenticationDefaults.AuthenticationScheme;
                // Use OpenID Connect protocol to login
                options.DefaultChallengeScheme = "oidc";
            })
            //Add a handler to process cookies
            .AddCookie()
            //Configure the handler that perform the OpenID Connect protocol
            .AddOpenIdConnect("oidc", options =>
            {
                //The server to process the authentication
                options.Authority = "https://ictonejourney.com";
                //To identify the client
                options.ClientId = "ojweb-practical";
                        options.ClientSecret =
                "SG54frqHvy6K6wk2+C7bOngKp++TmGRV//NVq93c3ik=";
                //To require server to return authorization code
                options.ResponseType = "code";
                //To persist the tokens from Identity Server in the cookie
                options.SaveTokens = true;
                //The resource scopes (parts of the API) that you are requesting
                //permission for to access
                options.Scope.Add("IdentityServerApi");
            });
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

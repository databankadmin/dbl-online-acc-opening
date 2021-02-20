using System;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AppMain.Startup))]

namespace AppMain
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireCon");

            app.UseHangfireDashboard();
            app.UseHangfireServer();

        }
    }
}

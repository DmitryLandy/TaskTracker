using System;
using BugTracker.Areas.Identity.Data;
using BugTracker.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(BugTracker.Areas.Identity.IdentityHostingStartup))]
namespace BugTracker.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<BTAuthContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("TTConnection")));

                services.AddDefaultIdentity<BugTrackerUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<BTAuthContext>();
            });
        }
    }
}
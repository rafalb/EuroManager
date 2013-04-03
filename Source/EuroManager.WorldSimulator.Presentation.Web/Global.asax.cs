using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using EuroManager.WorldSimulator.Presentation.Web.Models;
using EuroManager.WorldSimulator.Services;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;

namespace EuroManager.WorldSimulator.Presentation.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            var bootstrapper = new Bootstrapper();
            bootstrapper.Initialize();

            InitializeSimpleMembership();
            SeedUsersAndRoles();
        }

        private void InitializeSimpleMembership()
        {
            Database.SetInitializer<UsersContext>(null);

            using (var context = new UsersContext())
            {
                if (!context.Database.Exists())
                {
                    // Create the SimpleMembership database without Entity Framework migration schema
                    ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                }
            }

            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
        }

        private void SeedUsersAndRoles()
        {
            if (!Roles.RoleExists(UserRole.Administrator))
            {
                Roles.CreateRole(UserRole.Administrator);
            }

            string defaultAdminUserName = WebConfigurationManager.AppSettings["DefaultAdminUserName"];
            string defaultAdminFacebookId = WebConfigurationManager.AppSettings["DefaultAdminFacebookId"];

            if (!WebSecurity.UserExists(defaultAdminUserName))
            {
                using (var context = new UsersContext())
                {
                    context.UserProfiles.Add(new UserProfile { UserId = 1, UserName = defaultAdminUserName });
                    context.SaveChanges();
                }

                OAuthWebSecurity.CreateOrUpdateAccount("facebook", defaultAdminFacebookId, defaultAdminUserName);
                Roles.AddUserToRole(defaultAdminUserName, UserRole.Administrator);
            }
        }
    }
}

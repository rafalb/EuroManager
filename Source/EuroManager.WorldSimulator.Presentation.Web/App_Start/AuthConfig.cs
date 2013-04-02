using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using EuroManager.WorldSimulator.Presentation.Web.Models;
using Microsoft.Web.WebPages.OAuth;

namespace EuroManager.WorldSimulator.Presentation.Web
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            OAuthWebSecurity.RegisterFacebookClient(
                ConfigurationManager.AppSettings["FacebookAppId"],
                ConfigurationManager.AppSettings["FacebookAppSecret"]);
        }
    }
}

using System;
using System.Web.Http;
using System.Web.Routing;
using HedgehogDevelopment.Scaas.Web.Framework.Controllers;

namespace HedgehogDevelopment.Scaas.Web
{
    public class Global : Sitecore.Web.Application
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Create our ASP.NET Web API route
            // The route would be: http://[server]/api/{action}/{key}
            // Working actions are:
            //  - item
            //  - parent
            //  - ancestors
            //  - children
            //  - descendants
            //  - referrers
            //  - items
            // Supported 'keys' would be
            //  - GUID of Sitecore item
            //  - path to Sitecore item
            // Example:
            //  http://[server]/api/item/sitecore/content/home
            //  Would return a JSON object of the out-of-the-box home page item.

            RouteTable.Routes.MapHttpRoute(
                name: "ContentApi",
                routeTemplate: "api/{action}/{*key}",
                defaults: new { controller = "ContentApi", key = RouteParameter.Optional }
            );

            // Setup JSON serialization
            ConfigureJsonSerialization();

            // Globally add Cross Origin support
            GlobalConfiguration.Configuration.MessageHandlers.Add(new CorsHandler()); 
        }
        
        /// <summary>
        /// Configures the JSON serializer to work nicely with our Content Items
        /// </summary>
        private static void ConfigureJsonSerialization()
        {
            // configure Json.Net to handle our ContentItems properly
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            json.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            // Configure Json.Net to produce nice Json for demo purposes
            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            // remove the Xml formatter
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);            
        }

    }
}
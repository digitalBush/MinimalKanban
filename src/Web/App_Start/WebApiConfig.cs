using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Kanban
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}

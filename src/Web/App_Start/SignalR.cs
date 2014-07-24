using System;
using System.Reflection;
using Kanban;
using Kanban.Controllers.events;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;

[assembly: OwinStartup(typeof(SignalR))]
namespace Kanban
{
    public class SignalR
    {
        public void Configuration(IAppBuilder app)
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();
            var serializer = JsonSerializer.Create(settings);
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);
            app.MapSignalR<Aggregator>("/events");
        }
    }

    public class SignalRContractResolver : IContractResolver
    {
        private readonly Assembly _assembly;
        private readonly IContractResolver _camelCaseContractResolver;
        private readonly IContractResolver _defaultContractSerializer;

        public SignalRContractResolver()
        {
            _defaultContractSerializer = new DefaultContractResolver();
            _camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
            _assembly = typeof(Connection).Assembly;
        }

        #region IContractResolver Members

        public JsonContract ResolveContract(Type type)
        {
            if (type.Assembly.Equals(_assembly))
                return _defaultContractSerializer.ResolveContract(type);

            return _camelCaseContractResolver.ResolveContract(type);
        }

        #endregion
    }
}

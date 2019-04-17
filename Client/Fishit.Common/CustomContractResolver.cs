using System.Collections.Generic;
using Newtonsoft.Json.Serialization;

namespace Fishit.Common
{
    public class CustomContractResolver : DefaultContractResolver
    {
        public CustomContractResolver()
        {
            PropertyMappings = new Dictionary<string, string>
            {
                {"Id", "_id"}
            };
        }

        private Dictionary<string, string> PropertyMappings { get; }

        protected override string ResolvePropertyName(string propertyName)
        {
            string resolvedName = null;
            bool resolved = PropertyMappings.TryGetValue(propertyName, out resolvedName);
            return resolved ? resolvedName : base.ResolvePropertyName(propertyName);
        }
    }
}
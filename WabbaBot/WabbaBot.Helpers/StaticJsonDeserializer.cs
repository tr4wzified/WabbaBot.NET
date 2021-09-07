using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WabbaBot.Helpers
{
    public static class StaticJsonDeserializer
    {
        // Credit to https://stackoverflow.com/questions/60373210/newtonsoft-json-serialize-deserialize-static-class
        public static void Deserialize(string json, Type staticClassType)
        {
            if (!staticClassType.IsClass)
                throw new ArgumentException("Type must be a class", nameof(staticClassType));

            if (!staticClassType.IsAbstract || !staticClassType.IsSealed)
                throw new ArgumentException("Type must be static", nameof(staticClassType));

            var document = JObject.Parse(json);
            var classFields = staticClassType.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in classFields)
            {
                var documentField = document[field.Name];
                if (documentField == null)
                    throw new JsonSerializationException($"Not found in JSON: {field.Name}");
                field.SetValue(null, documentField.ToObject(field.FieldType));
            }
        }
    }
}

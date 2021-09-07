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
                {
                    dynamic value = field.GetValue(staticClassType);
                    if (value != null)
                    {
                        document[field.Name] = JsonConvert.SerializeObject(value, field.FieldType.BaseType, null);
                        field.SetValue(null, value);
                    }
                    else
                    {
                        throw new JsonSerializationException($"No value found for {field.Name} in either JSON or static class.");
                    }
                }
                else
                {
                    field.SetValue(null, documentField.ToObject(field.FieldType));
                }
            }
        }
    }
}

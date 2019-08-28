using System;
using System.Linq;
using Assessment2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Assessment2.JsonConverters
{
    public class PolymorphTypesJsonConverter : JsonConverter
    {
        private static readonly Type[] _knowTypes = { typeof(ApartmentData) };

        public override bool CanWrite => false;
        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return _knowTypes.Any(x => x.IsAssignableFrom(objectType));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object result = null;
            var obj = JObject.Load(reader);

            if (obj["propertyID"] != null)
            {
                result = new RealProperty();
            }
            else if(obj["mgmtID"] != null)
            {
                result = new ManagementCompany();
            }
            if(result == null)
            {
                throw new OperationCanceledException("Unable to recognize type in JSON file");
            }           
            
            serializer.Populate(obj.CreateReader(), result);
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

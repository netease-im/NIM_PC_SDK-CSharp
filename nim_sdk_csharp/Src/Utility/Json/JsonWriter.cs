using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NimUtility.Json
{

    public class JsonField
    {
        public string Key { get; set; }
        public string PropertyName { get; set; }
        public Type PropertyType { get; set; }
    }

    public class JsonObjectBase
    {
        public virtual List<JsonField> JsonPropertyDefines { get; set; }
    }
    public class NimJsonWriter
    {
        public void Serialization(object obj, Newtonsoft.Json.JsonWriter writer)
        {

            if (obj == null)
                return;
            var objType = obj.GetType();

            if (objType == typeof(string))
            {
                writer.WriteValue(obj as string);
            }
            else if (objType.IsArray || typeof(System.Collections.ICollection).IsAssignableFrom(objType))
            {
                writer.WriteStartArray();
                var list = obj as System.Collections.IList;
                foreach (var element in list)
                    Serialization(element, writer);
                writer.WriteEndArray();
            }
            else if (objType.IsEnum)
            {
                var e = Enum.Parse(objType, obj.ToString());
                writer.WriteValue((int)e);
            }
            else if (objType == typeof(bool))
            {
                var b = (bool)obj;
                int v = b ? 1 : 0;
                writer.WriteValue(v);
            }
            else if (objType.IsClass)
            {
                var properties = objType.GetProperties();
                writer.WriteStartObject();
                foreach (var p in properties)
                {
                    var type = p.PropertyType;
                    var value = p.GetValue(obj, null);
                    if (value != null)
                    {
                        writer.WritePropertyName(p.Name);
                        Serialization(value, writer);
                    }
                }
                writer.WriteEndObject();
            }
            else
            {
                writer.WriteValue(obj);
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ClientData
{
    internal class JsonSerializer : Serializer
    {
        public override T Deserialize<T>(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(json);
        }

        public override string? GetCommandHeader(string command)
        {
            JsonObject jsonObj = JsonObject.Parse(command).AsObject();
            if(jsonObj.ContainsKey("Header"))
            {
                return (string)jsonObj["Header"];
            }
            return null;
        }

        public override string Serialize<T>(T toSerialize)
        {
            return System.Text.Json.JsonSerializer.Serialize<T>(toSerialize);
        }
    }
}

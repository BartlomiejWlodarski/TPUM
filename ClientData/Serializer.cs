using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientData
{
    public abstract class Serializer
    {
        public abstract string Serialize<T>(T toSerialize);
        public abstract T Deserialize<T>(string json);

        public abstract string? GetCommandHeader(string command);

        public static Serializer Create()
        {
            return new JsonSerializer();
        }

    }
}

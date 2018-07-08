using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Hakaton_Service
{
    public class JsonManager
    {
        public static string GetJsonString<T>(T obj)
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(T));
            var stream = new MemoryStream();
            jsonSerializer.WriteObject(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);
            return new StreamReader(stream).ReadToEnd();
        }

        public static T FromJson<T>(string obj)where T : new()
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(T));
            var stream = new MemoryStream();
            new StreamWriter(stream).Write(obj);
            stream.Seek(0, SeekOrigin.Begin);
            //return (T) jsonSerializer.ReadObject(stream);
            return new T();
        }

        public static string JsonError(string message) => GetJsonString(new ErrorClass {Message = message});

        [DataContract]
        private class ErrorClass
        {
            [DataMember]
            public string Message { get; set; }
        }
    }
}
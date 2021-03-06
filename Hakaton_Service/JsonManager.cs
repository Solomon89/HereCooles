﻿using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Hakaton_Service
{
    public class JsonManager
    {
        public static string GetJsonString<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T FromJson<T>(string obj) where T : new()
        {
            if (string.IsNullOrWhiteSpace(obj)) return default;
            return JsonConvert.DeserializeObject<T>(obj);
        }

        public static string JsonError(string message)
        {
            return GetJsonString(new ErrorClass {Message = message});
        }

        [DataContract]
        private class ErrorClass
        {
            [DataMember]
            public string Message { get; set; }
        }
    }
}
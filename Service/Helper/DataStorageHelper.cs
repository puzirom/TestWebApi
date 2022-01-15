using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace TestWebApi.Service.Helper
{
    [CollectionDataContract(Namespace = "http://serialize")]
    public static class DataStorageHelper
    {      
        /// <summary>
        /// Object deserialization
        /// </summary>
        public static T Deserialize<T>(string xmlFile)
        {
            if (!File.Exists(xmlFile)) return (T) Activator.CreateInstance(typeof(T));

            var xml = XElement.Load(xmlFile);
            var dcs = new DataContractSerializer(typeof(T));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml.ToString())))
                return (T)dcs.ReadObject(stream);
        }

        /// <summary>
        /// Object serialization
        /// </summary>
        public static XElement Serialize(Type t, object o)
        {
            var xs = new DataContractSerializer(t);
            using (var ms = new MemoryStream())
            {
                xs.WriteObject(ms, o);
                ms.Position = 0;
                return XElement.Load(ms);
            }
        }
    }
}
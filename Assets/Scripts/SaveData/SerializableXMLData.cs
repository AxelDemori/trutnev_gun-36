using System;
using System.IO;
using System.Xml.Serialization;

namespace SaveData
{
    public class SerializableXMLData<T> : IData<T>
    {
        private XmlSerializer _serializer;

        public SerializableXMLData()
        {
            _serializer = new XmlSerializer(typeof(T));
        }

        public void OnSave(T data, string path = null)
        {
            if (data == null) return;
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                string directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var writer = new StreamWriter(path))
                {
                    _serializer.Serialize(writer, data);
                }
            }
            catch { }
        }

        public T OnLoad(string path)
        {
            if (string.IsNullOrEmpty(path)) return default;
            if (!File.Exists(path)) return default;

            try
            {
                using (var reader = new StreamReader(path))
                {
                    return (T)_serializer.Deserialize(reader);
                }
            }
            catch
            {
                return default;
            }
        }
    }
}

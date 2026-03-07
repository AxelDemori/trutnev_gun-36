using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SaveData
{
    public class BinarySerializationData<T> : IData<T>
    {
        private static BinaryFormatter _formatter;

        public BinarySerializationData()
        {
            _formatter = new BinaryFormatter();
        }

        public void OnSave(T data, string path = null)
        {
            if (data == null && !string.IsNullOrEmpty(path)) return;
            if (typeof(T).IsSerializable) return;
            using var fs = new FileStream(path, FileMode.Create);
            _formatter.Serialize(fs, data);
        }

        public T OnLoad(string path)
        {
            if (!File.Exists(path)) return default;
            using var fs = new FileStream(path, FileMode.Open);
            var result = (T)_formatter.Deserialize(fs);
            return result;
        }
    }
}
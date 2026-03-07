using System.IO;
using System.Model;
using UnityEngine;

namespace SaveData
{
    public sealed class SaveDataServiceXML : ISaveDataService<PlayerBase>
    {
        private readonly IData<SavedData> _data;

        private const string _folderName = "dataSave";
        private const string _fileName = "data.xml"; 
        private readonly string _path;

        public SaveDataServiceXML(IData<SavedData> data)
        {
            _data = data;
            _path = Path.Combine(Application.dataPath, _folderName);
        }

        public void Save(PlayerBase player)
        {
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            var savePlayer = new SavedData
            {
                Position = (Vector3Serializable)player.transform.position,
                Name = "Player",
                IsEnabled = player.gameObject.activeSelf
            };

            _data.OnSave(savePlayer, Path.Combine(_path, _fileName));
        }

        public void Load(PlayerBase player)
        {
            var file = Path.Combine(_path, _fileName);
            if (!File.Exists(file)) return;

            var loadedData = _data.OnLoad(file);
            if (loadedData != null)
            {
                player.transform.position = loadedData.Position;
                player.gameObject.SetActive(loadedData.IsEnabled);
            }
        }
    }
}
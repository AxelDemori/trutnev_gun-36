using System.IO;
using System.Model;
using UnityEngine;
using Zenject.SpaceFighter;

namespace SaveData
{
    public sealed class SaveDataService : ISaveDataService<PlayerBase>
    {
        private readonly IData<SavedData> _data;

        private const string _folderName = "dataSave";
        private const string _fileName = "data.bat";
        private readonly string _path;
        public SaveDataService(IData<SavedData> data)
        {
            _data = data;
            _path = Path.Combine(Application.dataPath, _folderName);
        }

        public void Save(PlayerBase player)
        {

            if (!Directory.Exists(Path.Combine(_path)))
            {
                Directory.CreateDirectory(_path);
            }
            var savePlayer = new SavedData
            {
                Position = (Vector3Serializable)player.transform.position,
                Name = "Name",
                IsEnabled = true
            };

            _data.OnSave(savePlayer, Path.Combine(_path, _fileName));
        }

        public void Load(PlayerBase player)
        {
            var file = Path.Combine(_path, _fileName);
            if (!File.Exists(file)) return;
            var newPlayer = _data.OnLoad(file);
        }
    }
}
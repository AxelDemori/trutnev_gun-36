using SaveData;
using System.Model;
using UniRx;
using Zenject;
using UnityEngine;

namespace System.Presenter
{
    public sealed class SavePresenter : IInitializable, IDisposable
    {
        private PlayerBase _player;
        private readonly ISaveDataService<PlayerBase> _saveDataService;
        private readonly ISaveLoadInputValues _inputModel;

        private CompositeDisposable _compositeDisposable = new();
        public SavePresenter([Inject(Id = "Player")] Transform player, ISaveDataService<PlayerBase> saveDataService, ISaveLoadInputValues input)
        {
            _player = player.GetComponent<PlayerBase>();
            _saveDataService = saveDataService;
            _inputModel = input;
        }

        public void Initialize()
        {
            _inputModel.LoadClicked.Subscribe(OnLoad).AddTo(_compositeDisposable);
            _inputModel.SaveClicked.Subscribe(OnSave).AddTo(_compositeDisposable);
        }
        private void OnLoad(bool clicked)
        {
            if (clicked)
            {
                _saveDataService.Load(_player);
            }

        }

        private void OnSave(bool clicked)
        {
            if (clicked)
            {
                _saveDataService.Save(_player);
            }
        }

        public void Dispose() => _compositeDisposable.Dispose();
    }
}


       
    
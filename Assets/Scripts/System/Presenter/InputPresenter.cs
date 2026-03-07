using SaveData;
using System.Model;
using UniRx;
using UnityEngine;
using Zenject;

namespace System.Presenter
{
    public class InputPresenter : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _inputDisposable = new();
        private readonly CompositeDisposable _gameOverInputDisposable = new();

        private readonly KeyCode _savePlayer = KeyCode.C;
        private readonly KeyCode _loadPlayer = KeyCode.V;
        private IVectorSet _vectorSet;
        private ISaveLoadInputValues _saveLoadInputValues;
        private readonly ISaveDataService<PlayerBase> _saveService;

        [Inject]
        public InputPresenter(IVectorSet vectorSet, ISaveLoadInputValues saveLoadInputValues, ISaveDataService<PlayerBase> saveService)
        {
            _vectorSet = vectorSet;
            _saveLoadInputValues = saveLoadInputValues;
            _saveService = saveService;
        }

        public void Initialize()
        {
            Observable
                .EveryUpdate()
                .Where(_ => Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                .Subscribe(OnNext).AddTo(_gameOverInputDisposable);

            Observable.EveryUpdate().Subscribe(SaveData).AddTo(_inputDisposable);
            Observable.EveryUpdate().Subscribe(LoadData).AddTo(_gameOverInputDisposable);

        }

        private void SaveData(long _) => _saveLoadInputValues.SaveClicked.Value = Input.GetKeyDown(KeyCode.C);
        private void LoadData(long _) => _saveLoadInputValues.LoadClicked.Value = Input.GetKeyDown(KeyCode.V);


        private void OnNext(long obj) => _vectorSet.SetVector(new Vector3(x:Input.GetAxis("Horizontal"), y: 0, z: Input.GetAxis("Vertical")));
       
        public void Dispose() => _inputDisposable.Dispose();
    }
}

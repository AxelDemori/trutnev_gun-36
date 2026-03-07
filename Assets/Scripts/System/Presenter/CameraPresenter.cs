using UnityEngine;
using Zenject;

namespace System.Presenter
{
    public class CameraPresenter : ILateTickable, IInitializable
    {
        private Transform _player;
        private Transform _mainCamera;
        private Vector3 _offset;
        public CameraPresenter([Inject(Id = "Player")] Transform player, [Inject(Id = "Camera")] Transform mainCamera)
        {
            _player = player;
            _mainCamera = mainCamera;
        }

        public void Initialize()
        {
            _mainCamera.LookAt(_player);
            _offset = _mainCamera.position - _player.position;
        }
        public void LateTick() => _mainCamera.position = _player.position + _offset;
    }
}
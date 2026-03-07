using System;
using UnityEditor.VersionControl;
using UniRx;
using UnityEngine;
using Zenject;

namespace MiniMap
{
    public sealed class MiniMap : MonoBehaviour
    {
        private Transform _player;

        [Inject]
        private void Inject([Inject(Id = "Camera")] Transform mainCamera)
        {
            _player = mainCamera;
            transform.parent = null;
            transform.rotation = Quaternion.Euler(90.0f, 0, 0);
            transform.position = _player.position + new Vector3(0, 5.0f, 0);

            Resources.LoadAsync<RenderTexture>(path: "MiniMap/MiniMapTexture").AsAsyncOperationObservable().Subscribe(OnTextureLoaded).AddTo(this);
        }

            private void OnTextureLoaded(ResourceRequest obj)
        {
            GetComponent<Camera>().targetTexture = obj.asset as RenderTexture;
            Observable.EveryLateUpdate().Subscribe(OnLateUpdate).AddTo(this);
        }
        
        private void OnLateUpdate(long param)
        {
            var newPosition = _player.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
            transform.rotation = Quaternion.Euler(90, _player.eulerAngles.y, 0);
        }
    }
}
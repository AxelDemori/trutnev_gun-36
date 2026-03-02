using UnityEditor.VersionControl;
using UnityEngine;

namespace MiniMap
{
    public sealed class MiniMap : MonoBehaviour
    {
        private Transform _player;
        private void Start()
        {
            _player = Camera.main.transform;
            transform.parent = null;
            transform.rotation = Quaternion.Euler(x:90.0f, y:0, z:0);
            transform.position = _player.position + new Vector3(x:0, y:5.0f, z:0);

            var rt = Resources.Load<RenderTexture>(path:"MiniMap/MiniMapTexture");

            GetComponent<Camera>().targetTexture = rt;
        }
        private void LateUpdate()
        {
            var newPosition = _player.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
            transform.rotation = Quaternion.Euler(x: 90, _player.eulerAngles.y, z: 0);
        }
    }
}
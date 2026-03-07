using Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MiniMap
{
    public sealed class Radar : MonoBehaviour, IRadar
    {
        private readonly float _mapScale = 2;
        private Transform _playerPos;
        public List<RadarObject> RadObjects = new();

        [Inject]
        private void Inject([Inject(Id = "Camera")]Transform mainCamera)
        {
            _playerPos = mainCamera;

            Observable
                .EveryUpdate()
                .Where(_ => Time.frameCount % 2 == 0)
                .Subscribe(_ => DrawRadarDots())
                .AddTo(this);
        }

        public void RegisterRadarObject(GameObject o, Image i)
        {
            var image = Instantiate(i);
            RadObjects.Add( new RadarObject { Owner = o, Icon = image });

        }
        public void RemoveRadarObject(GameObject o)
        {
            List<RadarObject> newList = new List<RadarObject>();
            foreach (RadarObject t in RadObjects)
            {
                if (t.Owner == o)
                {
                    Destroy(t.Icon);
                    continue;
                }
                newList.Add(t);
            }
            RadObjects.RemoveRange(index: 0, RadObjects.Count);
            RadObjects.AddRange(newList);

        }

        private void DrawRadarDots()
        {
            foreach (RadarObject rad0bject in RadObjects)
            {
                Vector3 radarPos = (rad0bject.Owner.transform.position - _playerPos.position);
                float distToObject = Vector3.Distance(a: _playerPos.position, b: rad0bject.Owner.transform.position) * _mapScale;
                float deltay = Mathf.Atan2(y: radarPos.x, x: radarPos.z) * Mathf.Rad2Deg - 270 - _playerPos.eulerAngles.y;
                radarPos.x = distToObject * Mathf.Cos(f: deltay * Mathf.Deg2Rad) * -1;
                radarPos.z = distToObject * Mathf.Sin(f: deltay * Mathf.Deg2Rad);
                rad0bject.Icon.transform.SetParent(transform);
                rad0bject.Icon.transform.position = new Vector3(radarPos.x, y: radarPos.z, z: 0) + transform.position;
            }
        }
    }


    public sealed class RadarObject
    {
        public Image Icon;
        public GameObject Owner;
    }
}
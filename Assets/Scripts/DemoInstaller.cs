using Interface;
using MiniMap;
using SaveData;
using System;
using System.ComponentModel;
using System.Model;
using System.Presenter;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public sealed class DemoInstaller : MonoInstaller
    {
        [SerializeField] private Radar _radar;

        public override void InstallBindings()
        {
            BindModels();
            BindPresenters();
            BindView();
            BindServices();
        }

        private void BindModels()
        {
            Container.BindInterfacesAndSelfTo<InputModel>().AsSingle();
            Container.Bind<PlayerHealth>().AsSingle().NonLazy();
            Container.Bind<BonusCount>().AsSingle();
            Container.Bind<PlayerSpeed>().FromInstance(new PlayerSpeed(3));
        }

        private void BindPresenters()
        {
            Container.BindInterfacesAndSelfTo<CameraPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<SavePresenter>().AsSingle();
        }

        private void BindView()
        {
            Container.Bind<IRadar>().To<Radar>().FromInstance(_radar);
        }

        private void BindServices()
        {
            Container.Bind<IData<SavedData>>().To<BinarySerializationData<SavedData>>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveDataService>().AsSingle();
        }
    }
}
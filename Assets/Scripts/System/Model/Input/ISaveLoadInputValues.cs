using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace System.Model
{
    public interface ISaveLoadInputValues
    {
        public ReactiveProperty<bool> SaveClicked { get; }

        public ReactiveProperty<bool> LoadClicked { get; }
    }
}
      

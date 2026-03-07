using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveData
{
    public interface ISaveDataService<T>
    {
        void Load(T player);

        void Save(T player);
    }   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveData
{
    public interface IData<T>
    {
        void OnSave(T data, string path = null);
        
        T OnLoad(string path = null);
    }
}

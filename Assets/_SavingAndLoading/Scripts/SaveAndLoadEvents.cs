using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveAndLoadEvents : MonoBehaviour
{
    public static UnityAction OnClearData;
    public static  UnityAction OnSaveData;
    public static UnityAction OnLoadData;

    public void ClearData() 
    {
        OnClearData?.Invoke();
    }

    public void SaveData() 
    {
        OnSaveData?.Invoke();
    }

    public void LoadData() 
    {
        OnLoadData?.Invoke();
    }
}

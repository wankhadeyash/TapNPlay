using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static RoomSO;
using Unity.VisualScripting;

[System.Serializable]
public class MoneyJsonData : SaveLoadBase 
{
    [SerializeField]
    public SaveLoadBase m_SaveLoadBase;

    public int m_CurrentMoneyCount;

    public void Save(int currentMoneyCount) 
    {
        m_CurrentMoneyCount = currentMoneyCount;
        Serializer.SaveJsonData<MoneyJsonData>(this,false);
    }

    public MoneyJsonData Load() 
    {
        return Serializer.LoadJsonData<MoneyJsonData>(this);
    }

    public void Clear() 
    {
        Serializer.DeleteFile<MoneyJsonData>(this);
    }
}

public class MoneyManager :  Singleton<MoneyManager>, ISaveAble
{
    [SerializeField]
    public MoneyJsonData m_JsonData = new MoneyJsonData();

    public List<MoneySpawnPoint> m_TableMoneySlotPoints;
    public Transform m_TableMoneySpawn;
    public MoneyEffect m_MoneyPrefab;

    public TextMeshProUGUI m_MoneyCountTMP;
    int m_CurrentMoneyCount =  0;

    public int CurrentMoneyCount => m_CurrentMoneyCount;

    
    private void OnEnable()
    {
        Load();

        SaveAndLoadEvents.OnLoadData += Load;
        SaveAndLoadEvents.OnSaveData += Save;
        SaveAndLoadEvents.OnClearData += Clear;

        MoneyEffect.OnMoneyCollected += OnMoneyCollected;
        QueueManager.OnCustomerAllotedRoom += OnCustomerAllotedRoom;
        RoomUpgradeController.OnRoomUpgrade += OnRoomUpgrade;
        m_MoneyCountTMP.text = m_CurrentMoneyCount.ToString();

    }

    private void OnDisable()
    {

        SaveAndLoadEvents.OnLoadData -= Load;
        SaveAndLoadEvents.OnSaveData -= Save;
        SaveAndLoadEvents.OnClearData -= Clear;
        MoneyEffect.OnMoneyCollected -= OnMoneyCollected;

        QueueManager.OnCustomerAllotedRoom -= OnCustomerAllotedRoom;

    }

    private void OnRoomUpgrade(int cost)
    {
        m_CurrentMoneyCount -= cost;
        Save();
        m_MoneyCountTMP.text = m_CurrentMoneyCount.ToString();

    }

    private void OnMoneyCollected(int amount)
    {
        m_CurrentMoneyCount += amount;
        m_MoneyCountTMP.text = m_CurrentMoneyCount.ToString();
        Save();
    }

    private void OnCustomerAllotedRoom()
    {
        MoneySpawnPoint freeSpawnPoint = m_TableMoneySlotPoints.FirstOrDefault(x => !x.IsEuipped);
        MoneyEffect money = Instantiate(m_MoneyPrefab, m_TableMoneySpawn.transform.position, m_MoneyPrefab.transform.rotation);
        freeSpawnPoint.EquipSlot(money);
    }

    

    #region Save and Load
    public void Save()
    {
        m_JsonData.Save(m_CurrentMoneyCount);
        m_MoneyCountTMP.text = m_CurrentMoneyCount.ToString();

    }

    public void Load()
    {
        MoneyJsonData data = m_JsonData.Load();
        if (data == null)
        {
            Save();
            return;
        }
        m_JsonData = data;
        m_CurrentMoneyCount = data.m_CurrentMoneyCount;
    }

    public void Clear() 
    {
        m_CurrentMoneyCount = 0;
        Save();
    }

    #endregion
}

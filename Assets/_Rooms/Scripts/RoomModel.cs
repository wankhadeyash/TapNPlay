using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[System.Serializable]
public class RoomModelJsonData : SaveLoadBase 
{
    [SerializeField]
    public SaveLoadBase m_SaveLoadBase;

    public int m_CurrentRoomLevel;

    public void Save(int currentRoomLevel)
    {
        m_CurrentRoomLevel = currentRoomLevel;
        Serializer.SaveJsonData<RoomModelJsonData>(this, false);
    }

    public RoomModelJsonData Load()
    {
        return Serializer.LoadJsonData<RoomModelJsonData>(this);
    }

    public void Clear()
    {
        Serializer.DeleteFile<RoomModelJsonData>(this);
    }
} 
public class RoomModel : MonoBehaviour
{
    [SerializeField]
    public RoomModelJsonData m_JsonData = new RoomModelJsonData();

    public RoomSO m_Data;

    [SerializeField]  private  int m_CurrentRoomLevel;

     private int m_MaxRoomLevel;

    public float fillDuration = 10f;

    public GameObject m_MoneySpendPrefab;

    public PlayerMovementManager m_PlayerManager;

    public Transform m_MoneyCollectTransform;

    public GameObject m_UpgradeUI;

    public TextMeshProUGUI m_UpgradeCostTMP;

    public TextMeshProUGUI m_UpgradeDiscriptionTMP;

    public int GetCurrentRoomLevel() => m_CurrentRoomLevel;

    public int GetMaxRoomLevel()=> m_MaxRoomLevel;


    private void Start()
    {
        m_MaxRoomLevel = m_Data.maxLevel;
        m_PlayerManager = GameObject.FindAnyObjectByType<PlayerMovementManager>();
    }
    public void UpgradeRoom() 
    {
        m_CurrentRoomLevel++;
    }

    public void MaxRoomLevelReached()
    {

    }

    public void OnTriggerPlayerStay(Action OnFillImageCompleted = null)
    { 
        
    }

    public void OnPlayerExitedTrigger() 
    {
        
    }

    public void Save() 
    {
        m_JsonData.Save(GetCurrentRoomLevel());
    }

    public void Load() 
    {
        RoomModelJsonData data = m_JsonData.Load();
        if (data == null)
        {
            Save();
            return;
        }
        m_JsonData = data;
        m_CurrentRoomLevel = data.m_CurrentRoomLevel;
    }

    public void Clear() 
    {
        m_CurrentRoomLevel = 0;
        Save();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static RoomSO;

public class RoomUpgradeController : MonoBehaviour, ISaveAble
{
    public RoomModel m_RoomModel;
    public RoomView m_RoomView;

    private bool m_CanUpgradeRoom = false;

    public static UnityAction<int> OnRoomUpgrade;

    void OnEnable() 
    {
        Load();
        SaveAndLoadEvents.OnLoadData += Load;
        SaveAndLoadEvents.OnSaveData += Save;
        SaveAndLoadEvents.OnClearData += Clear;
    }

    void OnDisable() 
    {
        SaveAndLoadEvents.OnLoadData -= Load;
        SaveAndLoadEvents.OnSaveData -= Save;
        SaveAndLoadEvents.OnClearData -= Clear;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //    UpgradeRoom();
    }

    public void UpgradeRoom() 
    {
        m_RoomModel.UpgradeRoom();
        m_RoomView.UpgradeRoom();
        OnRoomUpgrade?.Invoke(m_RoomModel.m_Data.upgrade[m_RoomModel.GetCurrentRoomLevel()].cost);

        if (m_RoomModel.GetCurrentRoomLevel() == m_RoomModel.GetMaxRoomLevel() - 1) {
            m_RoomModel.MaxRoomLevelReached();
            m_RoomView.MaxRoomLevelReached();
        }
        Save();
    }

    public void MaxRoomLevelReached() 
    {
        Debug.Log("Max Room level reached");
        m_RoomModel.MaxRoomLevelReached();
        m_RoomView.MaxRoomLevelReached();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.PlayerTag) && (m_RoomModel.GetCurrentRoomLevel() + 1 < m_RoomModel.GetMaxRoomLevel()))
        {
            if( MoneyManager.Instance.CurrentMoneyCount >= m_RoomModel.m_Data.upgrade[m_RoomModel.GetCurrentRoomLevel() + 1].cost)
            {
                m_CanUpgradeRoom = true;
                m_RoomView.OnTriggerEnterPlayer();
                return;
            }
        }
            m_CanUpgradeRoom = false;   
    }
    private void OnTriggerStay(Collider other)
    {
        if(m_CanUpgradeRoom)
        {
            m_RoomView.OnTriggerPlayerStay(() => {
                Vibration.Vibrate(100);
                UpgradeRoom();
                m_CanUpgradeRoom =false;    
                });
            m_RoomModel.OnTriggerPlayerStay();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.PlayerTag))
        {
            m_RoomModel.OnPlayerExitedTrigger();
            m_RoomView.OnPlayerExitedTrigger();
            m_CanUpgradeRoom = false;
        }
    }

    #region Save and load
    public void Save()
    {
        m_RoomModel.Save();
        m_RoomView.Refresh();

    }

    public void Load()
    {
        m_RoomModel.Load();
        m_RoomView.Load();
        m_RoomView.Refresh();


    }

    public void Clear()
    {
        m_RoomModel.Clear();
        m_RoomView.Clear();
        m_RoomView.Refresh();

    }
    #endregion
}

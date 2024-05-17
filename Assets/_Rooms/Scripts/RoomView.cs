using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomView : MonoBehaviour
{
    public RoomModel m_RoomModel;
    public List<Furniture> m_Furniture;

    public bool m_IsSpendingMoney;

    public Image fillImage;
    // Total duration in seconds for fill completion
    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
       Invoke(nameof(Refresh),1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpgradeRoom() 
    {
        for (int i = 0; i < m_Furniture.Count; i++) 
        {
            m_Furniture[i].UpgradeFurniture();
        }
        if (m_RoomModel.GetCurrentRoomLevel() + 1 < m_RoomModel.GetMaxRoomLevel())
            Refresh();

    }

    public void MaxRoomLevelReached()
    {
        m_RoomModel.m_UpgradeUI.gameObject.SetActive(false);
    }

    public void EnableCanvas() 
    {
        m_RoomModel.m_UpgradeUI.gameObject.SetActive(true);

    }
    public void OnTriggerEnterPlayer() 
    {
        m_IsSpendingMoney = true;
        StartCoroutine(Co_SpendMoney());
    }

    IEnumerator Co_SpendMoney() 
    {
        while (m_IsSpendingMoney) 
        {
            Vibration.Vibrate(100);
            GameObject money = Instantiate(m_RoomModel.m_MoneySpendPrefab, m_RoomModel.m_PlayerManager.m_PlayerChestTransform.position,
                m_RoomModel.m_MoneySpendPrefab.transform.rotation);

            money.transform.DOMove(m_RoomModel.m_MoneyCollectTransform.position, 0.5f);
            yield return new WaitForSeconds(0.15f);
        }
    }
    public  void OnTriggerPlayerStay(Action OnFillImageCompleted = null) 
    {
        // Update timer
        timer += Time.deltaTime;

        // Calculate fill amount based on timer progress
        float fillAmount = Mathf.Clamp01(timer / m_RoomModel.fillDuration);

        // Apply fill amount to the radial fill image
        fillImage.fillAmount = fillAmount;

        // Optionally reset timer when fill amount reaches 1 (full)
        if (fillAmount >= 1f)
        {
            fillImage.fillAmount = 0;
            OnFillImageCompleted?.Invoke();
            m_IsSpendingMoney = false;
            // Add logic here for when the fill operation is complete
        }
    }

    public void OnPlayerExitedTrigger() 
    {
        fillImage.fillAmount = 0;
        timer = 0;
        m_IsSpendingMoney = false;
    }

    public void Load()
    {
        for (int i = 0; i < m_Furniture.Count; i++)
        {
            m_Furniture[i].EnableFurnitureObjectBasedOnLevel(m_RoomModel.GetCurrentRoomLevel());
        }
        m_RoomModel.m_UpgradeDiscriptionTMP.text = m_RoomModel.m_Data.m_UpgradeDiscription;
    }

    public void Refresh() 
    {
        if (m_RoomModel.GetCurrentRoomLevel() + 1 < m_RoomModel.GetMaxRoomLevel())
        {
            EnableCanvas();
            m_RoomModel.m_UpgradeCostTMP.text = m_RoomModel.m_Data.upgrade[m_RoomModel.GetCurrentRoomLevel() + 1].cost.ToString();

        }
        else
            MaxRoomLevelReached();

    }

    public void Clear() 
    {
        for (int i = 0; i < m_Furniture.Count; i++)
        {
            m_Furniture[i].EnableFurnitureObjectBasedOnLevel(0);
        }
    }
}



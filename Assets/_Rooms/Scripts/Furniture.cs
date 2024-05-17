using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains;
using MoreMountains.Feedbacks;
using Cinemachine;

public class Furniture : MonoBehaviour
{
    [SerializeField] FurnitureSO m_Data;
    [SerializeField] private int m_CurrentFurnitureLevel = 0;
    private int m_MaxFurnitureLevel;

    public List<GameObject> m_LevelObjects;

    public MMF_Player m_UpgradeFeedback_FirstHalf;
    public MMF_Player m_UpgradeFeedback_CameraZoom;


    private CinemachineVirtualCameraBase m_CurrentEnableCMV;

    public CinemachineVirtualCamera m_UpgradeCMV;
    // Start is called before the first frame update
    void Start()
    {
        m_MaxFurnitureLevel = m_Data.maxLevel;
        EnableFurnitureObjectBasedOnCurrentlevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpgradeFurniture() 
    {
        if (m_CurrentFurnitureLevel == m_MaxFurnitureLevel - 1)
        {
            Debug.Log("Max furniture level reached");
            return;
        }

        ICinemachineCamera activeCinemachineCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
        m_CurrentEnableCMV = activeCinemachineCamera as CinemachineVirtualCameraBase;
        m_CurrentEnableCMV.Priority = -1;
        m_UpgradeCMV.Priority = 11;

        m_CurrentFurnitureLevel++;
        m_UpgradeFeedback_CameraZoom.PlayFeedbacks();
        m_UpgradeFeedback_FirstHalf.PlayFeedbacks();
    }

    public void EnableFurnitureObjectBasedOnCurrentlevel() 
    {
        for (int i = 0; i < m_LevelObjects.Count; i++) 
        {
            if (i == m_CurrentFurnitureLevel)
                m_LevelObjects[i].gameObject.SetActive(true);
            else
                m_LevelObjects[i].gameObject.SetActive(false);

        }
    }

    public void EnableFurnitureObjectBasedOnLevel(int level) 
    {
        m_CurrentFurnitureLevel = level;
        EnableFurnitureObjectBasedOnCurrentlevel();
    }

    public void OnFeedbackCompleted() 
    {
        m_UpgradeCMV.Priority = -1;
        m_CurrentEnableCMV.Priority = 11;
    }
}

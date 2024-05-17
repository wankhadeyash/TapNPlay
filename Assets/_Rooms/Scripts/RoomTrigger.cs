using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class RoomTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera m_PlayerFollowCMV;
    public CinemachineVirtualCamera m_RoomCMV;
    private CinemachineBrain m_CinemachineBrain;

    private void Start()
    {
        m_CinemachineBrain = Camera.main.transform.GetComponent<CinemachineBrain>();  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            m_PlayerFollowCMV.Priority = -1;
            m_RoomCMV.Priority = 10;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_RoomCMV.Priority = -1;
            m_PlayerFollowCMV.Priority = 10;
        }
    }



}

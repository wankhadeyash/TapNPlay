using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Transform m_RoomSpot;
    private CustomerManager m_CurrentCustomer;
    public CustomerManager CurrrentCustomer => m_CurrentCustomer;

    [SerializeField] private bool m_IsRoomAvailable = true;

    public bool IsRoomAvailable =>  m_IsRoomAvailable;

    public void EquipeRoom(CustomerManager CurrrentCustomer) 
    {
        m_IsRoomAvailable = false;
        m_CurrentCustomer = CurrrentCustomer;
    }

    public void UnEquipeRoom() 
    {
        m_IsRoomAvailable = true;
        m_CurrentCustomer = null;   
    }
}

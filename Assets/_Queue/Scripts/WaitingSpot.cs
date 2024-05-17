using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingSpot : MonoBehaviour
{
    [SerializeField] private bool m_IsSpotAvailable;
    private CustomerManager m_Customer;
    public CustomerManager CurrentCustomer => m_Customer;

    private bool m_HasCustomerReached;

    public bool HasCustomerReached => m_HasCustomerReached; 
    private void Start()
    {
         m_IsSpotAvailable = true;
        m_HasCustomerReached = false;
    }
    public bool IsSpotAvailable() 
    {
        return m_IsSpotAvailable;
    }

    public void EquipeSpot(CustomerManager customer)
    {
        m_IsSpotAvailable = false;
        m_Customer = customer;
    }

    public void UnEquipeSpot() 
    {
        m_IsSpotAvailable = true;
        m_HasCustomerReached = false;

        m_Customer = null;
    }
    
    public void CustomerReached() 
    {
        m_HasCustomerReached = true;
    }
}

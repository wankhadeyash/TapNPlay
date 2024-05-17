using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySpawnPoint : MonoBehaviour
{
    private bool m_IsEuiped = false;

    MoneyEffect m_CurrentMoneyEffect;

    public MoneyEffect CurrentMoneyEffect => m_CurrentMoneyEffect;

    public bool IsEuipped => m_IsEuiped;

    public void EquipSlot(MoneyEffect moneyEffect) 
    {
        m_IsEuiped = true;
        m_CurrentMoneyEffect=moneyEffect;
        moneyEffect.EquipSlot(this);
    }

    public void UnEquipSlot() 
    {
        m_IsEuiped = false;
        m_CurrentMoneyEffect=null;
          
    }
}

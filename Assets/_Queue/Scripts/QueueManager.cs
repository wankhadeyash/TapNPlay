using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public class QueueManager : Singleton<QueueManager>
{
    public CustomerManager m_CustomerPrefab;
    public Transform m_OffScreenPosition;
    public Transform m_ExitHotelPosition;

    public List<WaitingSpot> m_QueueWaitingSpots;
    [HideInInspector] public List<RoomManager> m_RoomManagers;
    public List<CustomerManager> m_CustomersQueue;

    public static UnityAction OnCustomerAllotedRoom;

    private void OnEnable()
    {
        TableManager.OnAllotRoom += Dequeue;
    }

    private void OnDisable()
    {
        TableManager.OnAllotRoom -= Dequeue;

    }
    // Start is called before the first frame update
    void Start()
    {
        m_RoomManagers = GameObject.FindObjectsOfType<RoomManager>().ToList();
        StartCoroutine(Co_SpawnCustomer());
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //    Dequeue();
    }

    public void Enqueue()
    {
        m_CustomersQueue.Add(Instantiate(m_CustomerPrefab,m_OffScreenPosition.position,Quaternion.identity));

        if (GetWaitingSpot(out WaitingSpot freeSpot)) 
        {
            CustomerManager manager = m_CustomersQueue[m_CustomersQueue.Count - 1];
            manager.MoveToWaitingSpot(freeSpot);
        }
    }

    public void Dequeue() 
    {
        Debug.Log("Dequiign");
        if (m_CustomersQueue.Count <= 0)
            return;
        if (GetFreeRoom(out RoomManager roomManager))
        {
            CustomerManager manager = m_CustomersQueue[0];
            manager.MoveToRoomSpot(roomManager);
            m_CustomersQueue.RemoveAt(0);
            MoveQueue();
            OnCustomerAllotedRoom?.Invoke();
        }
    }


    private void MoveQueue() 
    {
        for (int i = 0; i <= m_CustomersQueue.Count-1; i++) 
        {
            if (GetWaitingSpot(out WaitingSpot waitingSpot)) 
            {
                m_CustomersQueue[i].MoveToWaitingSpot(waitingSpot);
            }
        }

        Enqueue();
    }

    public bool IsLastWaitingSpotAvailable() 
    {
        return m_QueueWaitingSpots[m_QueueWaitingSpots.Count - 1].IsSpotAvailable();
    }

    public bool GetWaitingSpot(out WaitingSpot freeWaitingSpot) 
    {
        for (int i = 0; i < m_QueueWaitingSpots.Count; i++) 
        {
            if (m_QueueWaitingSpots[i].IsSpotAvailable())
            {
                freeWaitingSpot = m_QueueWaitingSpots[i];
                return true;    
            }
        }
        freeWaitingSpot = null;
        return false;   
    }

    public bool GetFreeRoom(out RoomManager roomManager) 
    {
        for (int i = 0; i < m_RoomManagers.Count; i++) 
        {
            if (m_RoomManagers[i].IsRoomAvailable) 
            {
                roomManager = m_RoomManagers[i];
                return true;
            }
        }
        roomManager = null;
        return false;
    }

    public bool GetFreeRoom() 
    {
        for (int i = 0; i < m_RoomManagers.Count; i++)
        {
            if (m_RoomManagers[i].IsRoomAvailable)
            {
                return true;
            }
        }
        return false;
    }
    IEnumerator Co_SpawnCustomer()
    {
        int i = 0;
        while (i<2) 
        {
            yield return new WaitForSeconds(3);
            SpawnCustomer();
            i++;
        }
    }   
    public void SpawnCustomer() 
    {
        if (!GetWaitingSpot(out WaitingSpot spot))
            return;

        Enqueue();
    }

    public bool IsCustomerAtTable() 
    {
       return m_QueueWaitingSpots[0].HasCustomerReached;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerManager : MonoBehaviour
{
    public enum State 
    {
        WaitingInLine,
        MovingInLine,
        GoingToRoom,
        ReachedRoom,
        ExitingHotel
    }

    public State m_CurrentState = State.WaitingInLine;
    public NavMeshAgent m_NavAgent;
    private WaitingSpot m_CurrentWaitingSpot;
    private RoomManager m_CurrentRoomManager;
    public WaitingSpot CurrentWatingSpot => m_CurrentWaitingSpot;
    public RoomManager CurrentRoomManager => m_CurrentRoomManager;

    public Animator m_Animator;

    

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (m_CurrentState == State.GoingToRoom) 
        {
            if (m_NavAgent.remainingDistance <= 1) 
            {
                StartCoroutine(Co_ExitHotel());
            }
        }

        if (m_CurrentState == State.MovingInLine) 
        {
            if (m_NavAgent.remainingDistance <= 0)
            {
                m_CurrentState = State.WaitingInLine;
                m_CurrentWaitingSpot.CustomerReached();
            }


        }
        if (m_NavAgent.remainingDistance <= 0)
        {
            if (m_Animator.GetBool("Running"))
                m_Animator.SetBool("Running", false);
        }
        else 
        {
            if (!m_Animator.GetBool("Running"))
                m_Animator.SetBool("Running", true);
        }
        
    }

    

    public void MoveToWaitingSpot(WaitingSpot waitingSpot) 
    {
        SetAIDestination(new Vector3(waitingSpot.transform.position.x, transform.position.y, waitingSpot.transform.position.z));
        if (m_CurrentWaitingSpot)
            m_CurrentWaitingSpot.UnEquipeSpot();

        m_CurrentWaitingSpot = waitingSpot;
        waitingSpot.EquipeSpot(this);
        m_CurrentState = State.MovingInLine;
        
    }

    public void MoveToRoomSpot(RoomManager roomManager)
    {
        SetAIDestination(new Vector3(roomManager.m_RoomSpot.transform.position.x, transform.position.y, roomManager.m_RoomSpot.transform.position.z));
        m_CurrentRoomManager = roomManager;
        roomManager.EquipeRoom(this);
        m_CurrentWaitingSpot.UnEquipeSpot();
        m_CurrentState = State.GoingToRoom;
    }

    IEnumerator Co_ExitHotel() 
    {
      

        m_CurrentState = State.ExitingHotel;

        yield return new WaitForSeconds(3f);
        SetAIDestination(new Vector3(QueueManager.Instance.m_ExitHotelPosition.transform.position.x, transform.position.y
          , QueueManager.Instance.m_ExitHotelPosition.transform.position.z));

        m_CurrentRoomManager.UnEquipeRoom();

    }
    private void SetAIDestination(Vector3 destination) 
    {
        m_NavAgent.SetDestination(destination);
        
    }
}

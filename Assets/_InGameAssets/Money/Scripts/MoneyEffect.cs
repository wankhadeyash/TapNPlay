using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains;
using MoreMountains.Feedbacks;
using DG.Tweening;
using UnityEngine.Events;

public class MoneyEffect : MonoBehaviour
{
    public MMF_Player m_WiggleFeedback;
    public float m_LiftUpDuration;
    public float m_GoToPlayerSpeed;
    public float m_MoveToSlotDuration;
    private bool m_IsMovingToPlayer = false;
    PlayerMovementManager m_Player;

    private MoneySpawnPoint m_CurrentSpawnPoint;

    public int m_MoneyCount;
    public static UnityAction<int> OnMoneyCollected;

    public MoneySpawnPoint CurrentSpawnPoint => m_CurrentSpawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        m_MoneyCount = Random.Range(2, 10);
        m_Player = GameObject.FindGameObjectWithTag(Tags.PlayerTag).GetComponent<PlayerMovementManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsMovingToPlayer && m_Player != null)
        {
            // Calculate the direction towards the player
            Vector3 targetPosition = m_Player.m_PlayerChestTransform.transform.position;

            // Move towards the player using Vector3.MoveTowards
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * m_GoToPlayerSpeed);

            // Check if the object has reached the player's position
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Debug.Log("Reached player's position");
                OnMoneyCollected?.Invoke(m_MoneyCount);
                Destroy(gameObject); // Destroy the object when it reaches the player
            }
        }

    }
    private float CalculateMoveSpeed()
    {
        // Calculate the move speed based on the total distance and the duration set for DOTween movement
        float distanceToPlayer = Vector3.Distance(transform.position, m_Player.transform.position);
        return distanceToPlayer / m_GoToPlayerSpeed; // Speed = Distance / Time
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.PlayerTag))
        {
            GetComponent<Collider>().enabled = false;
            Debug.Log("Lifting up");
            Vibration.Vibrate(100);

            m_WiggleFeedback.StopFeedbacks();
            transform.DOMoveY(transform.position.y + 1, m_LiftUpDuration).
                OnComplete(() =>
                {
                    transform.DOLocalRotate(new Vector3(0f, 360, 0f), 0.05f)
                    .SetEase(Ease.Linear)  // Set the easing type (e.g., linear)
                    .SetLoops(6, LoopType.Incremental)
                    .OnComplete(() => {

                        UnEquipSlot();
                        m_IsMovingToPlayer = true;
                    });
                    
                });


        }
    }

    public void EquipSlot(MoneySpawnPoint moneySpawnPoint)
    {
        m_CurrentSpawnPoint = moneySpawnPoint;

        MoveToSlot();
    }

    public void MoveToSlot()
    {
        transform.DOMove(m_CurrentSpawnPoint.transform.position, m_MoveToSlotDuration)
            .OnComplete(() => StartFeedBacks());
    }

    public void StartFeedBacks() 
    {
        m_WiggleFeedback.PlayFeedbacks();

    }
    public void UnEquipSlot()
    {
        m_CurrentSpawnPoint.UnEquipSlot();
        m_CurrentSpawnPoint = null;
    }
}

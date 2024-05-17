using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TableManager : MonoBehaviour
{
    
    public Image fillImage;
    public float fillDuration = 10f; // Total duration in seconds for fill completion
    private float timer = 0f;
    public static UnityAction OnAllotRoom;
    bool CanAllotSlot = true;
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.PlayerTag) && QueueManager.Instance.GetFreeRoom() && QueueManager.Instance.IsCustomerAtTable())
        {
            CanAllotSlot = true;
            Vibration.Vibrate(100); 
        }
        else
            CanAllotSlot = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (CanAllotSlot) 
        {
            // Update timer
            timer += Time.deltaTime;

            // Calculate fill amount based on timer progress
            float fillAmount = Mathf.Clamp01(timer / fillDuration);

            // Apply fill amount to the radial fill image
            fillImage.fillAmount = fillAmount;

            // Optionally reset timer when fill amount reaches 1 (full)
            if (fillAmount >= 1f)
            {
                CanAllotSlot = false;
                fillImage.fillAmount = 0;
                Vibration.Vibrate(100);

                Invoke(nameof(ResetTimer), 1f);
                OnAllotRoom?.Invoke();
                // Add logic here for when the fill operation is complete
            }
        }
    }

    void ResetTimer() 
    {
       timer = 0f;
       CanAllotSlot = true;

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            ResetTimer();

            fillImage.fillAmount = 0;
        }
    }
}

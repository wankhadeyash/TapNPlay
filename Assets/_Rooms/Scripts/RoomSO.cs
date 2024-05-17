using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RoomData")]

public class RoomSO : ScriptableObject
{
    [System.Serializable]
    public struct RoomUpgrade
    {
        [SerializeField] public int level;
        [SerializeField] public int cost;
    }
    public string m_UpgradeDiscription;
    public int maxLevel;

    public List<RoomUpgrade> upgrade;

    
}

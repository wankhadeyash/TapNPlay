using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Furniture")]
public class FurnitureSO : ScriptableObject
{
    [System.Serializable]
    public struct FurnitureUpgrade
    {
        [SerializeField] int level;
        [SerializeField] int cost;
    }
    public string furnitureName;
    public Image icon;

    public int maxLevel;

    public List<FurnitureUpgrade> upgrade;
}

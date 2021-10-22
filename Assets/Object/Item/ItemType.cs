using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollisionItemInfo.asset", menuName = "Item/CollisionItem")]
public class ItemType : ScriptableObject
{
    public GameObject prefab;
    public float ItemValue1;
    public float ItemValue2;
    public ItemList type;
}

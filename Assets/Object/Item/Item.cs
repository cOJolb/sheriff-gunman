using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : isCollision, ICollisionAble
{
    public SetPos setPos;
    public ItemType itemType;
    public void nowCollision(GameObject go)
    {
        switch (itemType.type)
        {
            case ItemList.SpeedUp:
                var player = go.GetComponent<Player>();
                player.GetItem(itemType.type, itemType.ItemValue1, itemType.ItemValue2);
                GameManager.instance.ItemCollision();
                Destroy(gameObject);
                break;
            case ItemList.SizeUp:
                break;
            case ItemList.None:
                break;
            default:
                break;
        }
    }

    void Start()
    {
        PosSetting(setPos);
    }
    
}

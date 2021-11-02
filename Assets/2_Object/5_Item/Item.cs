using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : isCollision, ICollisionAble
{
    public SetPos setPos;
    public ItemType itemType;
    public float distance;
    public float rotateSpeed = 100f;

    Dreamteck.Splines.SplinePositioner positioner;
    public void nowCollision(GameObject go)
    {
        switch (itemType.type)
        {
            case ItemList.SpeedUp:
                var player = go.GetComponent<Horse>();
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
        positioner = GetComponent<Dreamteck.Splines.SplinePositioner>();
        positioner.spline = GameManager.instance.road.GetComponent<Dreamteck.Splines.SplineComputer>();
        positioner.SetDistance(distance);
        PosSetting(setPos);

        //������ ���� �����ּ���
        positioner.motion.offset = new Vector2(positioner.motion.offset.x, 0.8f);
    }
    private void Update()
    {
        positioner.motion.rotationOffset = new Vector3(0, positioner.motion.rotationOffset.y + Time.deltaTime * rotateSpeed, 0f);
    }
}
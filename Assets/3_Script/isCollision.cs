using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class isCollision : MonoBehaviour
{
    public void PosSetting(SetPos pos)
    {
        if (GetComponent<Dreamteck.Splines.SplinePositioner>() != null)
        {
            var splinePositioner = GetComponent<Dreamteck.Splines.SplinePositioner>();
            switch (pos)
            {
                case SetPos.Left:
                    splinePositioner.motion.offset = new Vector2(splinePositioner.motion.offset.x - 3, splinePositioner.motion.offset.y);
                    break;
                case SetPos.Center:
                    break;
                case SetPos.Right:
                    splinePositioner.motion.offset = new Vector2(splinePositioner.motion.offset.x + 3, splinePositioner.motion.offset.y);
                    break;
                default:
                    break;
            }
        }
        else if (GetComponent<Dreamteck.Splines.SplineFollower>() != null)
        {
            var splinefollower = GetComponent<Dreamteck.Splines.SplineFollower>();
            switch (pos)
            {
                case SetPos.Left:
                    splinefollower.motion.offset = new Vector2(splinefollower.motion.offset.x - 3, splinefollower.motion.offset.y);
                    break;
                case SetPos.Center:
                    break;
                case SetPos.Right:
                    splinefollower.motion.offset = new Vector2(splinefollower.motion.offset.x + 3, splinefollower.motion.offset.y);
                    break;
                default:
                    break;
            }
        }
    }
}
public enum SetPos
{
    Left,
    Center,
    Right
}
public interface ICollisionAble
{
    void nowCollision(GameObject go);
}

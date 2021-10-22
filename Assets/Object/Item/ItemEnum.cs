using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Flags]
public enum ItemList
{
    SpeedUp = 1,
    SizeUp = 1<<1,
    None
}
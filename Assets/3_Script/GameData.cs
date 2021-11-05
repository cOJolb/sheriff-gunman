using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int stageSave;
    public int totalEnemy;
    public int horseSkin;
    public bool bossRun;
    public bool soundSave;
    public GameData (GameManager game)
    {
        totalEnemy = game.totalEnemyCatch;
        horseSkin = game.HorseSkin;
        bossRun = game.bossRun;
        stageSave = game.stageSave;
        soundSave = game.soundSave;
    }
}

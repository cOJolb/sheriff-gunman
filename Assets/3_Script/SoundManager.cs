using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip onbuttonSound;
    public AudioClip[] enemyCatchSound;
    public AudioClip getItemSound;
    public AudioClip stageClearSound;
    public AudioClip vsBossLose;
    public AudioClip playerShoot;
    public AudioClip playerDamage;
    public AudioClip bossShoot;
    public AudioClip onStart;
    public AudioClip onClear;
    public AudioClip usedGun;
    public AudioClip bgm;

    public AudioSource MainSound;
    public AudioSource BGM;
    public static SoundManager instance;

    private void Start()
    {
        instance = this;
    }
    public void OnButtonSound()
    {
        MainSound.PlayOneShot(onbuttonSound);
    }
    public void GameStart()
    {
        MainSound.PlayOneShot(onStart);
    }
    public void GameClear()
    {
        MainSound.PlayOneShot(onClear);
    }
    public void GetItemSound()
    {
        //cheak
        MainSound.PlayOneShot(getItemSound);
    }
    public void CatchEnemySound()
    {
        //cheak
        int index = Random.Range(0, 4);
        MainSound.PlayOneShot(enemyCatchSound[index]);
    }
    public void GameOverSound()
    {
        MainSound.PlayOneShot(vsBossLose);
    }
    public void PlayerShootSound()
    {
        //cheak
        MainSound.PlayOneShot(playerShoot);
    }
    public void BossShootSound()
    {
        //cheak
        MainSound.PlayOneShot(bossShoot);
    }
    public void PlayerDamageSound()
    {
        //cheak
        MainSound.PlayOneShot(playerDamage);
    }
    public void StageClearSound()
    {
        MainSound.PlayOneShot(stageClearSound);
    }
    public void UsedGunSound()
    {
        //cheak
        MainSound.PlayOneShot(usedGun);
    }
    public void BgmStart()
    {
        BGM.Play();
    }
    public void BgmStop()
    {
        BGM.Stop();
    }
    public void SoundOnOff(bool value)
    {
        if (value)
        {
            BGM.volume = 0.5f;
            MainSound.volume = 1f;
        }
        else
        {
            BGM.volume = 0f;
            MainSound.volume = 0f;
        }
    }
}
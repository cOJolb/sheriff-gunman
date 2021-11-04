using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    Animator ani;
    public MalbersAnimations.Controller.MAnimal animal;
    public float Timing;

    public Transform weaponPos;
    public Transform gunPos;
    public Weapon weapon;

    GameObject muzzle;
    GameObject shootingParticle;
    public enum BossAnimation
    {
        finish,
        Death,
        Stand
    }
    bool isMount;
    private void Start()
    {
        ani = GetComponent<Animator>();

        Instantiate(weapon.GunposPrefab, gunPos);
        Instantiate(weapon.WeaponPrefab, weaponPos);
        muzzle = Instantiate(weapon.Muzzle, weaponPos);

        shootingParticle = Instantiate(weapon.ShootParticle);
        shootingParticle.gameObject.SetActive(false);
        weaponPos.gameObject.SetActive(false);
    }
    private void Update()
    {
        switch (GameManager.instance.state)
        {
            case GameManager.GameState.Idle:
                break;
            case GameManager.GameState.Start:
                if (!isMount)
                {
                    ani.SetInteger("State", 1);
                    animal.SetFloatParameter(animal.hash_Vertical, 3f);
                }
                break;
            case GameManager.GameState.Play:
                break;
            case GameManager.GameState.RunOver:
                break;
            case GameManager.GameState.Boss:
                break;
            case GameManager.GameState.finish:
                break;
            case GameManager.GameState.GameOver:
                break;
            case GameManager.GameState.ReStart:
                if (GameManager.instance.PrevState == GameManager.GameState.Boss)
                {
                    LookCowboy();
                }
                break;
            default:
                break;
        }
    }

    public void PlzStop()
    {
        animal.SetFloatParameter(animal.hash_Vertical, 0f);
        isMount = true;
    }
    public void SetAnimation(BossAnimation setAni)
    {
        switch (setAni)
        {
            case BossAnimation.finish:
                ani.SetTrigger("GameOver");
                break;
            case BossAnimation.Death:
                ani.SetTrigger("Death");
                break;
            case BossAnimation.Stand:
                ani.SetTrigger("Stand");
                break;
            default:
                break;
        }
    }
    public void StateInit(GameManager.GameState state)
    {
        
        switch (state)
        {
            case GameManager.GameState.Start:

                var horse = GameManager.instance.bossHorse.transform;
                transform.position = horse.position;
                transform.position += /*horse.transform.right * 2f*/ -horse.transform.forward * 3f;

                transform.LookAt(horse);
                break;
            case GameManager.GameState.Play:
                break;
            case GameManager.GameState.RunOver:
                transform.position =
                    new Vector3(transform.position.x, 0, transform.position.z);
                animal.State_Activate(10);
                break;
            case GameManager.GameState.Boss:
                SetAnimation(BossAnimation.Stand);
                LookCowboy();
                transform.position = GameManager.instance.cowboy.transform.position +
GameManager.instance.cowboy.transform.forward * 6f;
                break;
            case GameManager.GameState.finish:
                break;
            case GameManager.GameState.GameOver:
                break;
            case GameManager.GameState.ReStart:
                switch (GameManager.instance.PrevState)
                {
                    case GameManager.GameState.Play:
                        break;
                    case GameManager.GameState.Trace:
                        break;
                    case GameManager.GameState.Boss:
                        LookCowboy();
                        SetAnimation(BossAnimation.Stand);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    public void LookCowboy()
    {
        //var cowboy = GameObject.FindGameObjectWithTag("Player");
        transform.LookAt(GameManager.instance.cowboy.transform.position);
    }
    public void BossShoot()
    {
        LookCowboy();
        transform.Rotate(new Vector3(0, 90f, 0));

        SetAnimation(BossAnimation.finish);
    }
    public void UsingWeapon()
    {
        gunPos.gameObject.SetActive(false);
        weaponPos.gameObject.SetActive(true);
    }
    public void UsedWeapon()
    {
        gunPos.gameObject.SetActive(true);
        weaponPos.gameObject.SetActive(false);
        SoundManager.instance.UsedGunSound();
    }
    public void shooting()
    {
        var cowboy = GameManager.instance.cowboy;
        var cowboyScript = cowboy.GetComponent<Cowboy>();


        shootingParticle.gameObject.SetActive(true);
        shootingParticle.transform.position = muzzle.transform.position;
        shootingParticle.transform.LookAt(cowboy.GetComponent<Collider>().bounds.center);
        SoundManager.instance.BossShootSound();
        SoundManager.instance.PlayerDamageSound();
        cowboyScript.SetAnimation(Cowboy.PlayerAnimation.Death);
    }
    public void FinishDirecting()
    {
        GameManager.instance.FinishDirecting();
    }
}

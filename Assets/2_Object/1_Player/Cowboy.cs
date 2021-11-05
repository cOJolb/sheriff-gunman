using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MalbersAnimations.Utilities;
public class Cowboy : MonoBehaviour
{
    Animator ani;
    public MalbersAnimations.Controller.MAnimal animal;
    public MalbersAnimations.HAP.MRider rider;

    public Transform weaponPos;
    public Transform gunPos;
    public Weapon weapon;
    public GameObject ClearCamera;

    GameObject muzzle;
    public GameObject shootingParticle;

    Touch touch;
    bool isMount;
    Vector3 prevPos;
    public enum PlayerAnimation
    {
        Idle,
        finish,
        Death,
        Sad,
        MountShoot,
        DisMount
    }
    private void Start()
    {
        ani = GetComponent<Animator>();

        Instantiate(weapon.GunposPrefab, gunPos);
        Instantiate(weapon.WeaponPrefab, weaponPos);
        muzzle = Instantiate(weapon.Muzzle, weaponPos);

        shootingParticle = Instantiate(weapon.ShootParticle);
        shootingParticle.gameObject.SetActive(false);
        weaponPos.gameObject.SetActive(false);

        var horse = GameObject.FindGameObjectWithTag("Horse").transform;
        transform.position = horse.position;
        transform.position += horse.transform.right * 2f + horse.transform.forward * 5f;

        transform.LookAt(horse);
    }
    private void Update()
    {
        //총쏘는 애니메이션 테스트
        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    ani.SetInteger("WeaponAction", 101);
        //    ani.SetInteger("WeaponType", 300);
        //    ani.SetBool("WeaponHand", false);
        //}
        //if(Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    ani.SetInteger("WeaponAction", 0);
        //    ani.SetInteger("WeaponType", 0);
        //    ani.SetBool("WeaponHand", false);
        //}
    }

    public void StopShooting()
    {
        ani.SetInteger("WeaponAction", 0);
        ani.SetInteger("WeaponType", 0);
        ani.SetBool("WeaponHand", false);

        SetAnimation(PlayerAnimation.DisMount);
    }
    
    private void FixedUpdate()
    {
        if (!isMount && GameManager.instance.state != GameManager.GameState.Idle)
        {
            ani.SetInteger("State", 1);
            animal.SetFloatParameter(animal.hash_Vertical, 3f);
        }
    }
    public void PlzStop()
    {
        animal.SetFloatParameter(animal.hash_Vertical, 0f);
        isMount = true;
    }
    public void SetAnimation(PlayerAnimation setAni)
    {
        switch (setAni)
        {
            case PlayerAnimation.Idle:
                ani.SetTrigger("Stand");
                break;
            case PlayerAnimation.finish:
                ani.SetTrigger("finish");
                break;
            case PlayerAnimation.Death:
                ani.SetTrigger("Death");
                //ani.SetInteger("State", 10);
                break;
            case PlayerAnimation.Sad:
                ani.SetTrigger("PlayerSad");
                break;
            case PlayerAnimation.MountShoot:
                ani.SetInteger("WeaponAction", 0);
                ani.SetInteger("WeaponType", 0);
                ani.SetBool("WeaponHand", false);
                break;
            case PlayerAnimation.DisMount:
                var rider = GetComponent<MalbersAnimations.HAP.MRider>();
                rider.DismountAnimal();
                break;

            default:
                break;
        }
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
        //if (GameManager.instance.tooEarly)
        //{
        //    GameManager.instance.boss.GetComponent<Boss>().BossShoot();
        //}
    }
    public void shooting()
    {
        var boss = GameObject.FindGameObjectWithTag("Boss");
        var bossScript = boss.GetComponent<Boss>();
        shootingParticle.gameObject.SetActive(true);
        shootingParticle.transform.position = muzzle.transform.position;
        SoundManager.instance.PlayerShootSound();
        SoundManager.instance.PlayerDamageSound();
        if (GameManager.instance.tooEarly)
        {
            shootingParticle.transform.LookAt(boss.transform.position
                - boss.transform.right * 1.5f);
            GameManager.instance.boss.GetComponent<Boss>().BossShoot();
        }
        else
        {
            shootingParticle.transform.LookAt(boss.GetComponent<Collider>().bounds.center);
            bossScript.SetAnimation(Boss.BossAnimation.Death);
        }

    }

    public void StateInit(GameManager.GameState state)
    {
        var horse = GameManager.instance.horse;
        var boss = GameManager.instance.boss;
        switch (state)
        {
            case GameManager.GameState.Start:
                transform.LookAt(horse.transform.position);
                ani.SetInteger("State", 1);
                animal.SetFloatParameter(animal.hash_Vertical, 3f);
                break;
            case GameManager.GameState.Play:
                break;
            case GameManager.GameState.RunOver:
                var rider = GetComponent<MalbersAnimations.HAP.MRider>();
                rider.DismountAnimal();
                break;
            case GameManager.GameState.Boss:
                //보스전 포지션 저장 애니메이션때문에 이동됨
                prevPos = transform.position;
                break;
            case GameManager.GameState.finish:
                transform.LookAt(boss.transform.position);
                transform.Rotate(new Vector3(0, 90f, 0));

                SetAnimation(PlayerAnimation.finish);
                break;
            case GameManager.GameState.GameOver:
                if(GameManager.instance.tooEarly)
                {
                    transform.LookAt(boss.transform.position);
                    transform.Rotate(new Vector3(0, 90f, 0));
                    SetAnimation(PlayerAnimation.finish);
                }
                break;
            case GameManager.GameState.ReStart:
                switch (GameManager.instance.PrevState)
                {
                    case GameManager.GameState.Play:
                        break;
                    case GameManager.GameState.Trace:
                        break;
                    case GameManager.GameState.Boss:
                        SetAnimation(PlayerAnimation.Idle);
                        transform.LookAt(boss.transform.position);
                        transform.position = prevPos;
                        FinishDirecting();
                        break;
                    default:
                        break;
                }
                break;
                
            default:
                break;
        }
    }
    public void disMount()
    {
        var horse = GameManager.instance.horse;
        horse.GetComponent<Horse>().disMount();
    }
    public void ClearDirecting(int value)
    {
        ani.SetTrigger("Clear");
        ani.SetInteger("ClearState", value);
        ClearCamera.SetActive(true);
    }
    public void FinishDirecting()
    {
        GameManager.instance.FinishDirecting();
    }

}

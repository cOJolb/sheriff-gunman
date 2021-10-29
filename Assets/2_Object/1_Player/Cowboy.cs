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
    
    GameObject muzzle;
    GameObject shootingParticle;

    Touch touch;
    bool isMount;
    public enum PlayerAnimation
    {
        Idle,
        finish,
        Death,
        Sad,
        MountShoot
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
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //animal.State_Activate(2);
            ani.SetInteger("WeaponAction", 101);
            ani.SetInteger("WeaponType", 300);
            ani.SetBool("WeaponHand", false);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            ani.SetInteger("WeaponAction", 0);
            ani.SetInteger("WeaponType", 0);
            ani.SetBool("WeaponHand", false);
        }
    }
    public void StopShooting()
    {
        Debug.Log("test");
        ani.SetInteger("WeaponAction", 0);
        ani.SetInteger("WeaponType", 0);
        ani.SetBool("WeaponHand", false);
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
                ani.SetInteger("State", 0);
                break;
            case PlayerAnimation.finish:
                ani.SetTrigger("finish");
                break;
            case PlayerAnimation.Death:
                ani.SetInteger("State", 10);
                break;
            case PlayerAnimation.Sad:
                ani.SetTrigger("PlayerSad");
                break;
            case PlayerAnimation.MountShoot:
                ani.SetInteger("WeaponAction", 0);
                ani.SetInteger("WeaponType", 0);
                ani.SetBool("WeaponHand", false);
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
    }
    public void shooting()
    {
        var boss = GameObject.FindGameObjectWithTag("Boss");

        shootingParticle.gameObject.SetActive(true);
        shootingParticle.transform.position = muzzle.transform.position;
        shootingParticle.transform.LookAt(boss.GetComponent<Collider>().bounds.center);

        var bossScript = boss.GetComponent<Boss>();
        bossScript.SetAnimation(Boss.BossAnimation.Death);
    }

    public void StateInit(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.Start:
                break;
            case GameManager.GameState.Play:
                break;
            case GameManager.GameState.RunOver:
                break;
            case GameManager.GameState.Boss:
                rider.DismountAnimal();
                break;
            case GameManager.GameState.finish:
                var boss = GameObject.FindGameObjectWithTag("Boss");
               
                transform.LookAt(boss.transform.position);
                transform.Rotate(new Vector3(0, 90f, 0));

                SetAnimation(PlayerAnimation.finish);
                break;
            case GameManager.GameState.GameOver:
                
                break;
            default:
                break;
        }
    }
    public void disMount()
    {
        var horse = GameObject.FindGameObjectWithTag("Horse");
        horse.GetComponent<Horse>().disMount();
    }
    public void HorseSleepSoWhat()
    {
        var rider = GetComponent<MalbersAnimations.HAP.MRider>();
        rider.DismountAnimal();
    }
    public void FinishDirecting()
    {
        GameManager.instance.FinishDirecting();
    }
}

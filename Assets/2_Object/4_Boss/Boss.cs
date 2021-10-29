using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Death
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

        var horse = GameObject.FindGameObjectWithTag("BossHorse").transform;
        transform.position = horse.position;
        transform.position += horse.transform.right * 2f - horse.transform.forward * 5f;

        transform.LookAt(horse);
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
            default:
                break;
        }
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
                break;
            case GameManager.GameState.finish:
                break;
            case GameManager.GameState.GameOver:
                break;
            default:
                break;
        }
    }
    public void BossShoot()
    {
        var cowboy = GameObject.FindGameObjectWithTag("Player");

        transform.LookAt(cowboy.transform.position);
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
    }
    public void shooting()
    {
        shootingParticle.gameObject.SetActive(true);
        shootingParticle.transform.position = muzzle.transform.position;
        var cowboy = GameObject.FindWithTag("Player");
        var cowboyScript = cowboy.GetComponent<Cowboy>();
        cowboyScript.SetAnimation(Cowboy.PlayerAnimation.Death);
    }
    public void FinishDirecting()
    {
        GameManager.instance.FinishDirecting();
    }
}

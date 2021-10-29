using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon.asset", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public GameObject WeaponPrefab;
    public GameObject GunposPrefab;
    public GameObject ShootParticle;
    public GameObject Muzzle;
    public WeaponType.weaponType type;
    
}

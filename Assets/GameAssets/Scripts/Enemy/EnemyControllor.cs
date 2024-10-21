using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyControllor : MonoBehaviour
{
    public WeaponType weaponType;
    public abstract void Aim();
    public abstract void Shoot();
    public abstract void Move();
    public abstract void Dead();

}

public enum WeaponType
{
    laser,
    dash,
}

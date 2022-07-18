using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {

    }

    public void Shoot()
    {

    }
}

public interface IWeapon
{
    void Shoot();
    string GetAnimationName();
}

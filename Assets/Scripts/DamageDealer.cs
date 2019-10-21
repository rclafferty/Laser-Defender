using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    int damage = 100;

    private void Start()
    {
        if (name == "Enemy Bomb")
        {
            damage = 200;
        }
    }

    public int Damage
    {
        get
        {
            return damage;
        }
    }

    public void Hit()
    {
        Destroy(gameObject);
    }
}

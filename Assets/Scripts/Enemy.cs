using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float health;
    public float defense;
    public float attack;
    public float moveSpeed;
    public float startYPos;

    protected Room roomIn;
    protected bool isDead;
    //because there will be death behavior, we need to store this bool 
    //so there arent extra Die() calls

    protected AudioSource audioSource;

    public abstract void Move();
    public abstract void Hit(float damage, Collider hit);
    public abstract void Die();

    void Awake()
    {
        health *= GameController.enemHealthMultiplier;
        attack *= GameController.enemAttackMultiplier;
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (roomIn.IsActive())
            Move();

        if (health <= 0 && !isDead)
            Die();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Weapon w = other.gameObject.GetComponent<Weapon>();
            Hit(w.damage * PowerController.strengthMultiplier * GameController.enemDefenseMultiplier, other);
        }
    }

    public void SetRoom(Room room)
    {
        roomIn = room;
    }

}

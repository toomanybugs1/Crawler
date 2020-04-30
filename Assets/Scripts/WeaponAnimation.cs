using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    Animator anim;
    public bool isInSecondSwingRange;
    public bool canHit = true;
    public bool isSwinging;

    Weapon currentWeap;
    AudioSource source;

    void Start()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        //should grab the initial weapon
        currentWeap = GetComponentInChildren<Weapon>();
        currentWeap.Equip(this);
    }

    public void Clicked()
    {
        if (!canHit)
            return;

        if (isInSecondSwingRange)
        {
            anim.SetTrigger("secondHit");
        }
        else
        {
            anim.SetTrigger("hit");
        }
    }

    public void PlaySound()
    {
        source.pitch = Random.Range(0.8f, 1.2f);
        source.Play();
    }

    public void SetWeapon(Weapon weap)
    {
        currentWeap = weap;
    }

    public Weapon GetWeap()
    {
        return currentWeap;
    }
}

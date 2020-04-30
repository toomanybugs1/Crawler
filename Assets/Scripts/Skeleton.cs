using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    bool stagger, inRange;
    public bool isSwinging;
    Animator anim;

    Transform target;
    public override void Die()
    {
        isDead = true;
        anim.SetTrigger("death");
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        roomIn.subtractEnemy();
        Destroy(gameObject, 2.0f);
    }

    public override void Hit(float damage, Collider hit)
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
        StartCoroutine(KnockBack());
        health -= damage - defense;
    }

    public override void Move()
    {
        if (target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }

        float dist = Vector3.Distance(target.position, transform.position);

        if (!isDead && !stagger)
        {
            if (dist >= 1.8f && !isSwinging)
            {
                anim.SetInteger("moveState", 1);
                Vector3 newTarget = new Vector3(target.position.x, startYPos, target.position.z);

                transform.LookAt(newTarget);
                transform.position = Vector3.MoveTowards(transform.position, newTarget, moveSpeed * Time.deltaTime);
            } else
            {
                Vector3 newTarget = new Vector3(target.position.x, startYPos, target.position.z);

                transform.LookAt(newTarget);
                if (!isSwinging)
                    StartCoroutine(Swing());
            }
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("moveState", 0);
        //make sure their feet are on the ground
        transform.position = new Vector3(transform.position.x, startYPos, transform.position.z);
    }

    public IEnumerator KnockBack()
    {
        anim.SetTrigger("hit");
        stagger = true;
        yield return new WaitForSeconds(0.75f);
        stagger = false;
    }

    public IEnumerator Swing()
    {
        isSwinging = true;
        anim.SetTrigger("swing");
        yield return new WaitForSeconds(0.8f);
        isSwinging = false;
    }
}

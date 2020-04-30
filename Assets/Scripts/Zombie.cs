using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    bool stagger;
    Animator anim;

    Transform target;

    public override void Die()
    {
        isDead = true;
        anim.SetTrigger("death");
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<CapsuleCollider>().enabled = false;
        roomIn.subtractEnemy();
        Destroy(gameObject, 2.292f);
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

        Debug.Log(target);

        if (!isDead && !stagger)
        {
            anim.SetInteger("moveState", 1);
            Vector3 newTarget = new Vector3(target.position.x, 0, target.position.z);

            transform.LookAt(newTarget);
            transform.position = Vector3.MoveTowards(transform.position, newTarget, moveSpeed * Time.deltaTime);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("moveState", 0);
        //make sure their feet are on the ground
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    public IEnumerator KnockBack()
    {
        anim.SetTrigger("hit");
        stagger = true;
        yield return new WaitForSeconds(0.75f);
        stagger = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : Enemy
{
    bool canFly = true;

    public override void Hit(float damage, Collider hit)
    {
        health -= damage - defense;

        Vector3 dir = hit.transform.position - transform.position;
        transform.rotation = Quaternion.Euler(dir);
    }

    public override void Move()
    {
        if (canFly)
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
    public override void Die()
    {
        isDead = true;
        roomIn.subtractEnemy();
        canFly = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;
        Destroy(gameObject, 2);
    }

    void OnCollisionEnter(Collision other)
    {
        if (canFly)
            transform.rotation = Quaternion.Euler(Random.Range(0.0f, 50.0f), Random.Range(0.0f, 360.0f), 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Weapon : MonoBehaviour
{
    public int damage;
    bool isEquipped;
    public string weapName;

    BoxCollider col;
    WeaponAnimation weapAnim = null;
    Rigidbody rigid;

    void Start()
    {
        col = GetComponent<BoxCollider>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isEquipped && weapAnim != null)
        {
            col.isTrigger = weapAnim.isSwinging;
        }

        else
        {
            col.isTrigger = false;
        }
    }

    public void Equip(WeaponAnimation newWeapAnim)
    {
        weapAnim = newWeapAnim;

        //this conditional is really only for the initial equip
        if (weapAnim.GetWeap() != this)
            weapAnim.GetWeap().Dequip();

        newWeapAnim.SetWeapon(this);

        isEquipped = true;

        rigid.constraints = RigidbodyConstraints.FreezeAll;
        transform.parent = weapAnim.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
    public void Dequip()
    {
        isEquipped = false;
        weapAnim = null;
        transform.parent = null;
        rigid.constraints = RigidbodyConstraints.None;
        rigid.AddForce(Vector3.forward * 3);
    }
}

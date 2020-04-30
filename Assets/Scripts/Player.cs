using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Animator anim;
    Camera cam;
    Vector3 impact = Vector3.zero;
    AudioSource source;

    public WeaponAnimation weaponHold;
    public TextMeshProUGUI itemDesc;
    public float health;
    public float defense;
    public Slider healthBar;
    public AudioClip bonk;

    float maxHealth;
    UnityEngine.CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        cam = Camera.main;
        controller = GetComponent<UnityEngine.CharacterController>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            Die();

        if (impact.magnitude > 0.2)
            controller.Move(impact * Time.deltaTime);

        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);

        HandleInput();
        ItemDescription();

        if (health > maxHealth)
            health = maxHealth;

        healthBar.value = health / maxHealth; 
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            weaponHold.Clicked();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log(hit.transform.name);

                if (hit.transform.tag == "Weapon")
                {
                    hit.transform.GetComponent<Weapon>().Equip(weaponHold);
                }
                else if (hit.transform.tag == "Item")
                {
                    if (hit.transform.name == "CrystalBall")
                        GameController.NextLevel();

                    Inventory.AddItem(hit.transform.GetComponent<Item>().Clone());
                }

                Interactable interactable = hit.transform.GetComponent<Interactable>();

                if (interactable == null) { return; }
                interactable.OnInteract();
            }
        }
    }

    void ItemDescription()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.tag == "Weapon")
            {
                Weapon lookWeap = hit.transform.GetComponent<Weapon>();

                if (lookWeap == weaponHold.GetWeap())
                    return;

                int attackDif = lookWeap.damage - weaponHold.GetWeap().damage;

                if (attackDif >= 0)
                    itemDesc.text = lookWeap.weapName + " +" + attackDif;

                else
                    itemDesc.text = lookWeap.weapName + " " + attackDif;
            }
            else if (hit.transform.tag == "Item")
            {
                itemDesc.text = hit.transform.GetComponent<Item>().itemName;
            }
            else
            {
                itemDesc.text = "";
            }
        }
    }

    void Die()
    {
        Inventory.EmptyInventory();
        GameController.ResetNumbers();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Enemy e = other.gameObject.GetComponent<Enemy>();
            health -= e.attack - (defense * PowerController.defenseMultiplier);

            Vector3 dir = transform.position - e.transform.position;
            dir.Normalize();

            if (dir.y < 0)
                dir.y = -dir.y;

            impact += dir.normalized * 20;
            source.clip = bonk;
            source.Play();
        } 
        else if (other.tag == "EnemySword")
        {
            Enemy e = other.gameObject.GetComponentInParent<Enemy>();
            health -= e.attack - (defense * PowerController.defenseMultiplier);

            Vector3 dir = transform.position - e.transform.position;
            dir.Normalize();

            if (dir.y < 0)
                dir.y = -dir.y;

            impact += dir.normalized * 20;
            source.clip = bonk;
            source.Play();
        }
    }
}

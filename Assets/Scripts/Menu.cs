using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    public GameObject menu;
    public AudioMixer master;
    FirstPersonController fpsControl;

    void Start()
    {
        fpsControl = GetComponent<FirstPersonController>();
        menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void SetVolume (float sliderVal)
    {
        master.SetFloat("MasterVol", Mathf.Log10(sliderVal) * 20);
    }

    public void ToggleMenu()
    {
        if (menu.activeInHierarchy)
        {
            fpsControl.enabled = true;
            menu.SetActive(false);
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } 
        else
        {
            fpsControl.enabled = false;
            menu.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void QuitGame()
    {
        GameController.ResetNumbers();
        Inventory.EmptyInventory();
        fpsControl.enabled = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelNum : MonoBehaviour
{
    TextMeshProUGUI levelNumText;
    // Start is called before the first frame update
    void Start()
    {
        levelNumText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        levelNumText.text = "Level: " + GameController.roomNumber;
    }
}

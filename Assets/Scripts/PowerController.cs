using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerController : MonoBehaviour
{
    /* POWERS -
     * 0 - Strength Boost
     * 1 - Defense Boost
     */

    static bool isEnabled;
    static float timeLeft;

    public static float strengthMultiplier = 1;
    public static float defenseMultiplier = 1;
    static string boostUI;
    static public string boostUIComplete;

    TextMeshProUGUI tm;

    // Start is called before the first frame update
    void Start()
    {
        tm = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {

        tm.text = boostUIComplete;

        if (isEnabled)
        {
            Debug.Log("enabled");
            timeLeft -= Time.deltaTime;
            boostUIComplete = boostUI + timeLeft.ToString("F");
            if (timeLeft <= 0)
            {
                isEnabled = false;
                timeLeft = 0;
                ClearPowers();
            }
        }
    }

    static public void PowerUp(int powerType)
    {
        ClearPowers();
        isEnabled = true;
        timeLeft = 60;

        switch (powerType)
        {
            case 0:
                strengthMultiplier = 1.5f;
                boostUI = "Strength x1.5: ";
                break;
            case 1:
                defenseMultiplier = 1.5f;
                boostUI = "Defense x1.5: ";
                break;
            default:
                break;
        }
    }

    static void ClearPowers()
    {
        strengthMultiplier = 1;
        defenseMultiplier = 1;

        boostUI = "";
        boostUIComplete = "";
    }
}

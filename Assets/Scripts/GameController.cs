using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static float enemHealthMultiplier = 1;
    public static float enemAttackMultiplier = 1;
    public static float enemDefenseMultiplier = 1;
    public static int roomNumber = 1;

    public static void NextLevel() {
        roomNumber++;
        enemHealthMultiplier += 0.1f;
        enemAttackMultiplier += 0.1f;
        enemDefenseMultiplier += 0.1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void ResetNumbers()
    {
        enemHealthMultiplier = 1;
        enemAttackMultiplier = 1;
        enemDefenseMultiplier = 1;
        roomNumber = 1;
    }
}

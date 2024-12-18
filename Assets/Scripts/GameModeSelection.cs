using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeSelection: MonoBehaviour
{
    public void StartUnrankedGame()
    {
        SceneManager.LoadScene("UnrankedLevelSelection"); // Load the unranked play scene
    }
    public void StartRankedGame()
    {
        SceneManager.LoadScene("RankedPlay"); // Load the unranked play scene
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Return to main menu
    }
}

using UnityEngine;
using UnityEngine.UI;

public class DifficultyButtonHandler : MonoBehaviour
{
    public GridManager gridManager;

    private void Start()
    {
        // Attach difficulty buttons
        Button rookieButton = GameObject.Find("Rookie").GetComponent<Button>();
        Button challengerButton = GameObject.Find("Challenger").GetComponent<Button>();
        Button aceButton = GameObject.Find("Ace").GetComponent<Button>();
        Button eliteButton = GameObject.Find("Elite").GetComponent<Button>();
        Button legendButton = GameObject.Find("Legend").GetComponent<Button>();

        rookieButton.onClick.AddListener(() => gridManager.SetGridSize(4));
        challengerButton.onClick.AddListener(() => gridManager.SetGridSize(6));
        aceButton.onClick.AddListener(() => gridManager.SetGridSize(9));
        eliteButton.onClick.AddListener(() => gridManager.SetGridSize(12));
        legendButton.onClick.AddListener(() => gridManager.SetGridSize(16));
    }
}

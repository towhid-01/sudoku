using UnityEngine;
using UnityEngine.UI;

public class SudokuCellPrefab2 : MonoBehaviour
{
    public int row;
    public int col;
    private GridManager gridManager;
    private Button cellButton;
    public Text cellText;

    // Initialize cell with row, column, and GridManager reference
    public void SetupCell(int row, int col, GridManager gridManager)
    {
        this.row = row;
        this.col = col;
        this.gridManager = gridManager;

        // Set up Button component and assign click event
        cellButton = GetComponent<Button>();
        cellButton.onClick.AddListener(OnCellClicked);

        // Get Text component and set initial value
        cellText = GetComponentInChildren<Text>();
        cellText.text = "";
    }

    // Trigger GridManager action on cell click
    private void OnCellClicked()
    {
        gridManager.OnCellSelected(this);
    }

    // Sets the value to display in the cell
    public void SetValue(string value)
    {
        if (cellText != null)
        {
            cellText.text = value;
        }
    }
}

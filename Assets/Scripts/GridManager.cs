using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public GameObject sudokuCellPrefab; // Cell Prefab to instantiate
    public Transform gridParent; // Parent object to hold the grid
    public SudokuCellPrefab2[,] gridCells; // To hold references to all the cells

    private GridLayoutGroup gridLayoutGroup; // Reference to the Grid Layout Group component

    void Start()
    {
        // Get Grid Layout Group component on gridParent
        gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
    }

    public void SetGridSize(int gridSize)
    {
        ClearGrid(); // Clears any previous grid cells
        ConfigureGridLayout(gridSize); // Adjust grid layout for the chosen size
        CreateGrid(gridSize); // Create the grid cells
        PopulateGrid(gridSize); // Populate cells with numbers
    }

    // Configure Grid Layout based on grid size
    void ConfigureGridLayout(int gridSize)
    {
        float cellSize;

        // Set cell sizes and spacing based on grid size
        if (gridSize == 4)
            cellSize = 120f; // Larger cell size for smaller grid
        else if (gridSize == 6)
            cellSize = 90f;
        else if (gridSize == 9)
            cellSize = 60f;
        else if (gridSize == 12)
            cellSize = 50f;
        else // For gridSize == 16
            cellSize = 40f;

        // Update Grid Layout Group with calculated cell size and spacing
        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
        gridLayoutGroup.spacing = new Vector2(5, 5);
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = gridSize;
    }

    // Create the grid with cells
    void CreateGrid(int gridSize)
    {
        gridCells = new SudokuCellPrefab2[gridSize, gridSize];

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                GameObject cell = Instantiate(sudokuCellPrefab, gridParent);
                SudokuCellPrefab2 cellScript = cell.GetComponent<SudokuCellPrefab2>();
                cellScript.SetupCell(row, col, this);
                gridCells[row, col] = cellScript;
            }
        }
    }

    // Populate the grid with numbers based on difficulty
    void PopulateGrid(int gridSize)
    {
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                string value = Random.Range(1, Mathf.Min(gridSize + 1, 10)).ToString(); // Random number for now
                gridCells[row, col].SetValue(value); // Set value in cell
            }
        }
    }

    // Clear grid before creating a new one
    public void ClearGrid()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnCellSelected(SudokuCellPrefab2 selectedCell)
    {
        Debug.Log("Cell Selected: " + selectedCell.row + "," + selectedCell.col);
    }
}

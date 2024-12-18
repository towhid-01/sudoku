using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board4x4 : MonoBehaviour
{
    // Create the initial 4x4 Sudoku Grid
    protected int[,] grid = new int[4, 4];
    protected int[,] puzzle = new int[4, 4];

    // Default numbers removed
    protected int difficulty = 4;

    public Transform square00, square01,
                     square10, square11;

    public GameObject SudokuCell_Prefab;
    public GameObject winMenu;
    [SerializeField] GameObject loseText;

    // Start is called before the first frame update
    void Start()
    {
        winMenu.SetActive(false);
        difficulty = 8; // Number of cells to remove for puzzle
        CreateGrid();
        CreatePuzzle();
        CreateButtons();
    }

    // Console output for debugging the grid
    void ConsoleOutputGrid(int[,] g)
    {
        string output = "";
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                output += g[i, j];
            }
            output += "\n";
        }
        // Debug.Log(output);  // Uncomment to debug grid
    }

    // Validation functions for rows, columns, and subgrids
    bool ColumnContainsValue(int col, int value)
    {
        for (int i = 0; i < 4; i++)
        {
            if (grid[i, col] == value)
            {
                return true;
            }
        }
        return false;
    }

    bool RowContainsValue(int row, int value)
    {
        for (int i = 0; i < 4; i++)
        {
            if (grid[row, i] == value)
            {
                return true;
            }
        }
        return false;
    }

    bool SquareContainsValue(int row, int col, int value)
    {
        for (int i = 0; i < 2; i++) // For 2x2 subgrids
        {
            for (int j = 0; j < 2; j++)
            {
                if (grid[row / 2 * 2 + i, col / 2 * 2 + j] == value)
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool CheckAll(int row, int col, int value)
    {
        if (ColumnContainsValue(col, value)) return false;
        if (RowContainsValue(row, value)) return false;
        if (SquareContainsValue(row, col, value)) return false;
        return true;
    }

    bool IsValid()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (grid[i, j] == 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    // Create the Sudoku grid with random values while ensuring they are valid
    void CreateGrid()
    {
        List<int> rowList = new List<int>() { 1, 2, 3, 4 };
        List<int> colList = new List<int>() { 1, 2, 3, 4 };

        int value = rowList[Random.Range(0, rowList.Count)];
        grid[0, 0] = value;
        rowList.Remove(value);
        colList.Remove(value);

        for (int i = 1; i < 4; i++)
        {
            value = rowList[Random.Range(0, rowList.Count)];
            grid[i, 0] = value;
            rowList.Remove(value);
        }

        for (int i = 1; i < 4; i++)
        {
            value = colList[Random.Range(0, colList.Count)];
            grid[0, i] = value;
            colList.Remove(value);
        }

        for (int i = 2; i < 4; i++)
        {
            value = Random.Range(1, 5);
            while (SquareContainsValue(0, 2, value) || SquareContainsValue(2, 0, value))
            {
                value = Random.Range(1, 5);
            }
            grid[i, i] = value;
        }

        ConsoleOutputGrid(grid);
        SolveSudoku();
    }

    // Solve the Sudoku puzzle using a backtracking algorithm
    bool SolveSudoku()
    {
        int row = 0;
        int col = 0;

        if (IsValid())
        {
            return true;
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (grid[i, j] == 0)
                {
                    row = i;
                    col = j;
                }
            }
        }

        for (int i = 1; i <= 4; i++)
        {
            if (CheckAll(row, col, i))
            {
                grid[row, col] = i;

                if (SolveSudoku())
                {
                    return true;
                }
                else
                {
                    grid[row, col] = 0;
                }
            }
        }
        return false;
    }

    // Create the puzzle by removing numbers from the grid
    void CreatePuzzle()
    {
        System.Array.Copy(grid, puzzle, grid.Length);

        for (int i = 0; i < difficulty; i++)
        {
            int row = Random.Range(0, 4);
            int col = Random.Range(0, 4);

            while (puzzle[row, col] == 0)
            {
                row = Random.Range(0, 4);
                col = Random.Range(0, 4);
            }

            puzzle[row, col] = 0;
        }

        ConsoleOutputGrid(puzzle);
    }

    // Function to create the UI buttons
    void CreateButtons()
    {
        if (SudokuCell_Prefab == null)
        {
            Debug.LogError("SudokuCell_Prefab is not assigned in the Inspector!");
            return;
        }

        if (square00 == null || square01 == null ||
            square10 == null || square11 == null)
        {
            Debug.LogError("One or more square containers are not assigned in the Inspector!");
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                GameObject newButton = Instantiate(SudokuCell_Prefab);
                SudokuCell sudokuCell = newButton.GetComponent<SudokuCell>();

                if (sudokuCell == null)
                {
                    Debug.LogError("SudokuCell component not found on the prefab!");
                    return;
                }

                sudokuCell.SetValues(i, j, puzzle[i, j], i + "," + j, this);
                newButton.name = i.ToString() + j.ToString();

                Transform parentTransform = null;

                // Decide on the parent square for each button based on the position in the grid
                if (i < 2)
                {
                    if (j < 2) parentTransform = square00;
                    else parentTransform = square01;
                }
                else
                {
                    if (j < 2) parentTransform = square10;
                    else parentTransform = square11;
                }

                if (parentTransform != null)
                {
                    newButton.transform.SetParent(parentTransform, false);
                }
                else
                {
                    Debug.LogError($"Parent transform for cell ({i}, {j}) is null.");
                }
            }
        }
    }

    // Update the puzzle with the player's move
    public void UpdatePuzzle(int row, int col, int value)
    {
        puzzle[row, col] = value;
    }

    // Check if the puzzle is complete
    public void CheckComplete()
    {
        if (CheckGrid())
        {
            winMenu.SetActive(true);
        }
        else
        {
            loseText.SetActive(true);
        }
    }

    // Function to check if the Sudoku grid is filled and valid
    bool CheckGrid()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (puzzle[i, j] != grid[i, j])
                {
                    return false;
                }
            }
        }
        return true;
    }
}

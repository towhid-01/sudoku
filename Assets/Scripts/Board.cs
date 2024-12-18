using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    // Create the initial Sudoku Grid
    protected int[,] grid = new int[9, 9];
    protected int[,] puzzle = new int[9, 9];

    // Default numbers removed
    protected int difficulty = 15;

    public Transform square00, square01, square02,
                     square10, square11, square12,
                     square20, square21, square22;

    public GameObject SudokuCell_Prefab;
    public GameObject winMenu;
    [SerializeField] GameObject loseText;

    // Start is called before the first frame update
    void Start()
    {
        winMenu.SetActive(false);
        difficulty = 65;
        CreateGrid();
        CreatePuzzle();
        CreateButtons();
    }

    // Console output for debugging the grid
    void ConsoleOutputGrid(int[,] g)
    {
        string output = "";
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
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
        for (int i = 0; i < 9; i++)
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
        for (int i = 0; i < 9; i++)
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
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (grid[row / 3 * 3 + i, col / 3 * 3 + j] == value)
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
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
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
        List<int> rowList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        List<int> colList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        int value = rowList[Random.Range(0, rowList.Count)];
        grid[0, 0] = value;
        rowList.Remove(value);
        colList.Remove(value);

        for (int i = 1; i < 9; i++)
        {
            value = rowList[Random.Range(0, rowList.Count)];
            grid[i, 0] = value;
            rowList.Remove(value);
        }

        for (int i = 1; i < 9; i++)
        {
            value = colList[Random.Range(0, colList.Count)];
            if (i < 3)
            {
                while (SquareContainsValue(0, 0, value))
                {
                    value = colList[Random.Range(0, colList.Count)];
                }
            }
            grid[0, i] = value;
            colList.Remove(value);
        }

        for (int i = 6; i < 9; i++)
        {
            value = Random.Range(1, 10);
            while (SquareContainsValue(0, 8, value) || SquareContainsValue(8, 0, value) || SquareContainsValue(8, 8, value))
            {
                value = Random.Range(1, 10);
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

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid[i, j] == 0)
                {
                    row = i;
                    col = j;
                }
            }
        }

        for (int i = 1; i <= 9; i++)
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
            int row = Random.Range(0, 9);
            int col = Random.Range(0, 9);

            while (puzzle[row, col] == 0)
            {
                row = Random.Range(0, 9);
                col = Random.Range(0, 9);
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

        if (square00 == null || square01 == null || square02 == null ||
            square10 == null || square11 == null || square12 == null ||
            square20 == null || square21 == null || square22 == null)
        {
            Debug.LogError("One or more square containers are not assigned in the Inspector!");
            return;
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
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
                if (i < 3)
                {
                    if (j < 3) parentTransform = square00;
                    else if (j >= 3 && j < 6) parentTransform = square01;
                    else parentTransform = square02;
                }
                else if (i >= 3 && i < 6)
                {
                    if (j < 3) parentTransform = square10;
                    else if (j >= 3 && j < 6) parentTransform = square11;
                    else parentTransform = square12;
                }
                else if (i >= 6)
                {
                    if (j < 3) parentTransform = square20;
                    else if (j >= 3 && j < 6) parentTransform = square21;
                    else parentTransform = square22;
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
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
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

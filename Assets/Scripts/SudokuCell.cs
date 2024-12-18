using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SudokuCell : MonoBehaviour
{
    private Board board;
    private int row;
    private int col;
    private int value;
    private string id;

    public Text t; // Ensure this is assigned in the Inspector

    // Set initial values for the Sudoku cell
    public void SetValues(int _row, int _col, int _value, string _id, Board _board)
    {
        row = _row;
        col = _col;
        id = _id;
        board = _board;
        value = _value;

        if (t == null)
        {
            Debug.LogError("Text component is not assigned to SudokuCell.");
            return;
        }

        if (value != 0)
        {
            t.text = value.ToString();
            GetComponent<Button>().interactable = false; // Disable button for preset values
        }
        else
        {
            t.text = "";
            GetComponent<Button>().interactable = true; // Enable button for empty cells
        }
    }

    public void OnCellClicked()
    {
        Debug.Log("Cell clicked at (" + row + ", " + col + ")");

        if (InputButton.instance != null)
        {
            // Activate the InputButton UI and pass the current cell reference
            InputButton.instance.ActivateInputButton(this);
        }
    }

    public void UpdateValue(int newValue)
    {
        value = newValue;
        t.text = newValue.ToString(); // Display the new value
        Debug.Log("Updated cell value to: " + newValue);

        if (board != null)
        {
            board.UpdatePuzzle(row, col, newValue); // Notify the board of the new value
        }
    }

    internal void SetValues(int i, int j, int v1, string v2, Board4x4 board4x4)
    {
        throw new NotImplementedException();
    }
}

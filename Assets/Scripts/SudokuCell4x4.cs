using UnityEngine;
using UnityEngine.UI;

public class SudokuCell4x4 : MonoBehaviour
{
    public Text cellText;
    private int row, col, value;
    private Board board;

    public void SetValues(int row, int col, int value, string name, Board board)
    {
        this.row = row;
        this.col = col;
        this.value = value;
        this.board = board;
        this.name = name;

        if (value == 0)
        {
            cellText.text = "";
        }
        else
        {
            cellText.text = value.ToString();
        }
    }

    public void OnValueChanged(string input)
    {
        if (int.TryParse(input, out int newValue))
        {
            board.UpdatePuzzle(row, col, newValue);
        }
        else
        {
            Debug.LogError("Invalid input in Sudoku cell!");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputButton : MonoBehaviour
{
    public static InputButton instance;
    SudokuCell lastCell;
    [SerializeField] GameObject wrongText;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    public void ActivateInputButton(SudokuCell cell)
    {
        lastCell = cell;
    }

    public void ClickedButton(int num)
    {
        lastCell.UpdateValue(num);
        wrongText.SetActive(false);
    }
}

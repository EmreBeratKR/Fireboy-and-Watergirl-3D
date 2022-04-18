using UnityEngine;
using TMPro;

public class CharacterTouchKey : TouchKey
{
    [SerializeField] protected TextMeshProUGUI placeholder; 
    [SerializeField] private char input;


    public override void OnPressed()
    {
        TouchKeyboard.AppendChar(input);
    }


    private void Start()
    {
        SetPlaceHolder();
    }

    private void SetPlaceHolder()
    {
        this.placeholder.text = input.ToString();
    }

    
    public void ToUpper()
    {
        if (char.IsLetter(input))
        {
            input = char.ToUpper(input);
            SetPlaceHolder();
        }
    }

    public void ToLower()
    {
        if (char.IsLetter(input))
        {
            input = char.ToLower(input);
            SetPlaceHolder();
        }
    }
}

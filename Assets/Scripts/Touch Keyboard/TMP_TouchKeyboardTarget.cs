using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class TMP_TouchKeyboardTarget : TouchKeyboardTarget
{
    private TMP_InputField inputField;

    public override string Content => inputField.text;

    public override bool IsFull
    {
        get
        {
            if (inputField.characterLimit == 0) return false;

            if (inputField.text.Length >= inputField.characterLimit) return true;

            return false;
        }
    }


    public override bool TryAppendChar(char input)
    {
        if (IsFull) return false;

        if (!validation.IsValid(input)) return false;
        
        inputField.text += input;

        return true;
    }

    public override bool TryRemoveChar()
    {
        if (Content.Length == 0) return false;

        inputField.text = Content.Substring(0, Content.Length-1);

        return true;
    }


    private void Start()
    {
        inputField = this.GetComponent<TMP_InputField>();
        inputField.onSelect.AddListener(OnSelect);
    }

    
}

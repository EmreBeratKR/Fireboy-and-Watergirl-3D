using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class TouchKeyboard : Singleton<TouchKeyboard>
{
    [SerializeField] private Transform[] characterKeyParents;
    [SerializeField] private GameObject mainGameObject;
    [SerializeField] private ShiftFunctionalKey shiftKey;
    private List<CharacterTouchKey> characterTouchKeys;
    private EventSystem eventSystem;
    private TMP_InputField lastFocusedInputField;


    public static EventSystem CurrentEventSystem
    {
        get
        {
            if (Instance.eventSystem == null)
            {
                Instance.eventSystem = FindObjectOfType<EventSystem>();
            }

            return Instance.eventSystem;
        }
    }


    private void Start()
    {
        CacheCharacterKeys();
    }

    private void Update()
    {
        CheckFocus();
    }

    private void CacheCharacterKeys()
    {
        characterTouchKeys = new List<CharacterTouchKey>();

        foreach (var parent in characterKeyParents)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i).TryGetComponent(out CharacterTouchKey key))
                {
                    characterTouchKeys.Add(key);
                }
            }
        }
    }

    private void CheckFocus()
    {
        var eventSystem = CurrentEventSystem;

        if (eventSystem == null) return;

        var focus = eventSystem.currentSelectedGameObject;

        if (focus == null) return;

        if (focus.TryGetComponent(out TMP_InputField inputField))
        {
            lastFocusedInputField = inputField;
        }
    }


    public static void AppendChar(char input)
    {
        if (!char.IsLetterOrDigit(input)) return;

        var focus = Instance.lastFocusedInputField;

        if (focus == null) return;

        if (focus.text.Length >= focus.characterLimit) return;

        focus.text += input;

        Instance.shiftKey.OnCharAppended();
    }

    public static void RemoveChar()
    {
        var focus = Instance.lastFocusedInputField;

        if (focus == null) return;

        if (focus.text.Length == 0) return;

        focus.text = focus.text.Substring(0, focus.text.Length-1);
    }

    public static void AllUpper()
    {
        foreach (var key in Instance.characterTouchKeys)
        {
            key.ToUpper();
        }
    }

    public static void AllLower()
    {
        foreach (var key in Instance.characterTouchKeys)
        {
            key.ToLower();
        }
    }

    public static void Show()
    {
        if (Instance == null) return;

        Instance.mainGameObject.SetActive(true);
    }

    public static void Hide()
    {
        if (Instance == null) return;

        Instance.mainGameObject.SetActive(false);
    }
}

using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class TouchKeyboard : Singleton<TouchKeyboard>
{
    [SerializeField] private Transform[] characterKeyParents;
    [SerializeField] private GameObject mainGameObject;
    [SerializeField] private TextMeshProUGUI contentHolder;
    [SerializeField] private ShiftFunctionalKey shiftKey;
    [SerializeField] private float caretInterval;
    private List<CharacterTouchKey> characterTouchKeys;
    private EventSystem eventSystem;
    private TouchKeyboardTarget lastFocus;
    private float caretTimer;
    private bool showCaret;


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
        caretTimer = 0;
        showCaret = true;
        
        CacheCharacterKeys();
    }

    private void Update()
    {
        CheckFocus();
        UpdateCaretState();
        UpdateFocusContent();
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

        if (focus.TryGetComponent(out TouchKeyboardTarget inputField))
        {
            lastFocus = inputField;
        }
    }

    private void UpdateFocusContent()
    {
        var caret = showCaret ? "<color=#c0c0c0ff>|</color>" : "";

        var focus = lastFocus;

        if (focus == null)
        {
            contentHolder.text = caret;
            return;
        }

        contentHolder.text = focus.Content + caret;
    }

    private void UpdateCaretState()
    {
        caretTimer += Time.deltaTime;

        if (caretTimer >= caretInterval)
        {
            caretTimer = 0;
            showCaret = !showCaret;
        }
    }

    private static void ResetCaretInterval()
    {
        Instance.caretTimer = 0;
        Instance.showCaret = true;
    }


    public static void AppendChar(char input)
    {
        var focus = Instance.lastFocus;

        if (focus == null) return;

        if (!focus.TryAppendChar(input)) return;

        ResetCaretInterval();

        Instance.shiftKey.OnCharAppended();
    }

    public static void RemoveChar()
    {
        var focus = Instance.lastFocus;

        if (focus == null) return;

        if (!focus.TryRemoveChar()) return;

        ResetCaretInterval();
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

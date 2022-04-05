using System;
using UnityEngine;
using NaughtyAttributes;

public class LevelTree : Scenegleton<LevelTree>
{
    [SerializeField] private Transform root;
    [SerializeField] private Transform linkRoot;
    [SerializeField] private NodeSprites sprites;
    [SerializeField] private LinkColors linkColor;

    public static NodeSprites Sprites => LevelTree.Instance.sprites;
    public static LinkColors linkColors => LevelTree.Instance.linkColor;


    private void Start()
    {
        ResetTree();

        SetLevelNumberTexts();
        ToggleLevelNumberTexts(false);

        SetTree();
    }

    private void OnEnable()
    {
        MyEventSystem.OnLevelNumberToggled += ToggleLevelNumberTexts;
    }

    private void OnDisable()
    {
        MyEventSystem.OnLevelNumberToggled -= ToggleLevelNumberTexts;
    }

    private void ToggleLevelNumberTexts(bool isOn)
    {
        foreach (Transform child in root)
        {
            if (isOn)
            {
                child.GetComponent<LevelNode>().ShowNumberText();
                continue;
            }

            child.GetComponent<LevelNode>().HideNumberText();
        }
    }

    private void SetTree()
    {
        foreach (Transform child in root)
        {
            child.GetComponent<LevelNode>().SetStatus();
        }
    }


    [Button]
    private void SetLevelNumberTexts()
    {
        foreach (Transform child in root)
        {
            child.GetComponent<LevelNode>().SetNumberText();
        }
    }

    [Button]
    private void SetNames()
    {
        for (int i = 0; i < root.childCount; i++)
        {
            root.GetChild(i).name = "Level " + (i+1);
        }
    }

    [Button(null, EButtonEnableMode.Playmode)]
    private void ResetTree()
    {
        for (int i = 0; i < root.childCount; i++)
        {
            var node = root.GetChild(i).GetComponent<LevelNode>();

            node.SetSprite(LevelStatus.NotFinished);

            if (i == 0)
            {
                node.Activate(false);
            }
            else
            {
                node.DeActivate();
            }
        }
    }



    [Serializable]
    public struct NodeSprites
    {
        [field:SerializeField] public Sprite triangleNotFinished { get; private set; }
        [field:SerializeField] public Sprite triangleSemiFinished { get; private set; }
        [field:SerializeField] public Sprite triangleFullFinished { get; private set; }
        [field:SerializeField] public Sprite hexagonNotFinished { get; private set; }
        [field:SerializeField] public Sprite hexagonSemiFinished { get; private set; }
        [field:SerializeField] public Sprite hexagonFullFinished { get; private set; }
        [field:SerializeField] public Sprite diamondNotFinished { get; private set; }
        [field:SerializeField] public Sprite diamondFullFinished { get; private set; }
    }

    [Serializable]
    public struct LinkColors
    {
        [field:SerializeField] public Color enabled { get; private set; }
        [field:SerializeField] public Color disabled { get; private set; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelNode : MonoBehaviour
{
    [SerializeField] private LevelNode[] linkedLevels;
    [SerializeField] private Image[] links;
    [SerializeField] private GameObject fill;
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private Text number;

    [field:SerializeField] public Type type { get; private set; }

    public int levelNumber => this.transform.GetSiblingIndex() + 1;


    public void SetNumberText()
    {
        this.number.text = levelNumber.ToString();
    }

    public void ShowNumberText()
    {
        this.number.gameObject.SetActive(true);
    }

    public void HideNumberText()
    {
        this.number.gameObject.SetActive(false);
    }

    public void SetSprite(LevelStatus levelStatus)
    {
        if (this.type == Type.Triangle)
        {
            switch (levelStatus)
            {
                case LevelStatus.NotFinished:
                    this.image.sprite = LevelTree.Sprites.triangleNotFinished;
                    break;
                case LevelStatus.SemiFinished:
                    this.image.sprite = LevelTree.Sprites.triangleSemiFinished;
                    break;
                case LevelStatus.FullFinished:
                    this.image.sprite = LevelTree.Sprites.triangleFullFinished;
                    break;
            }
            
            return;
        }

        if (this.type == Type.Hexagon)
        {
            switch (levelStatus)
            {
                case LevelStatus.NotFinished:
                    this.image.sprite = LevelTree.Sprites.hexagonNotFinished;
                    break;
                case LevelStatus.SemiFinished:
                    this.image.sprite = LevelTree.Sprites.hexagonSemiFinished;
                    break;
                case LevelStatus.FullFinished:
                    this.image.sprite = LevelTree.Sprites.hexagonFullFinished;
                    break;
            }

            return;
        }

        if (this.type == Type.Diamond)
        {
            switch (levelStatus)
            {
                case LevelStatus.NotFinished:
                    this.image.sprite = LevelTree.Sprites.diamondNotFinished;
                    break;
                case LevelStatus.FullFinished:
                    this.image.sprite = LevelTree.Sprites.diamondFullFinished;
                    break;
            }

            return;
        }
    }

    public void Activate(bool isCompleted)
    {
        var linkColor = isCompleted ? LevelTree.linkColors.enabled : LevelTree.linkColors.disabled;

        foreach (var link in links)
        {
            link.color = linkColor;
        }

        this.fill.SetActive(true);
        button.enabled = true;
    }

    public void DeActivate()
    {
        foreach (var link in links)
        {
            link.color = LevelTree.linkColors.disabled;
        }

        this.fill.SetActive(false);
        button.enabled = false;
    }

    public void SetStatus()
    {
        LevelStatus levelStatus = DataManager.LoadLevelStatus(this.levelNumber);

        if (levelStatus == LevelStatus.NotFinished) return;

        foreach (var linkedLevel in linkedLevels)
        {
            linkedLevel.Activate(false);
        }

        foreach (var link in links)
        {
            link.color = LevelTree.linkColors.enabled;
        }

        SetSprite(levelStatus);

        Activate(true);
    }

    public void OnClicked()
    {
        var sceneController = FindObjectOfType<SceneController>();
        sceneController.Load_Level(this);
    }


    public enum Type { Triangle, Hexagon, Diamond }
}

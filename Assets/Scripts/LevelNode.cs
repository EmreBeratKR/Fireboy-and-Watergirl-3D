using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNode : MonoBehaviour
{
    [SerializeField] private LevelNode[] linkedLevels;

    public int levelNumber => this.transform.GetSiblingIndex() + 1;


    public void SetStatus()
    {
        LevelStatus levelStatus = DataManager.LoadLevelStatus(this.levelNumber);

        if (levelStatus == LevelStatus.NotFinished)
        {
            foreach (var linkedLevel in linkedLevels)
            {
                linkedLevel.gameObject.SetActive(false);
            }
        }
        
        else if (levelStatus is LevelStatus.SemiFinished or LevelStatus.FullFinished)
        {
            this.gameObject.SetActive(true);
        }
    }
}

using UnityEngine;

public class LevelTree : Scenegleton<LevelTree>
{
    private void Start()
    {
        foreach (Transform child in this.transform)
        {
            child.GetComponent<LevelNode>().SetStatus();
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class LevelNumberToggle : MonoBehaviour
{
    private const string EnabledLabel = "Hide Level Numbers";
    private const string DisabledLabel = "Show Level Numbers";

    [SerializeField] private Toggle toggle;
    [SerializeField] private Text label;


    public void OnValueChanged()
    {
        label.text = toggle.isOn ? EnabledLabel : DisabledLabel;

        MyEventSystem.RaiseLevelNumberToggled(toggle.isOn);
    }
}

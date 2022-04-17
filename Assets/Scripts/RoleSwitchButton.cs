using UnityEngine;
using UnityEngine.UI;

public class RoleSwitchButton : MonoBehaviour
{
    [SerializeField] private Text buttonText;


    private string ButtonText => "Switch to " + RoleSwitcher.OppositeRole(RoleSwitcher.MasterRole);

    private void Start()
    {
        buttonText.text = ButtonText;
    }

    public void SwitchRoles()
    {
        RoleSwitcher.MasterRole = RoleSwitcher.OppositeRole(RoleSwitcher.MasterRole);

        buttonText.text = ButtonText;

        RoleSwitcher.RaiseRoleSwitchedEvent();
    }
}

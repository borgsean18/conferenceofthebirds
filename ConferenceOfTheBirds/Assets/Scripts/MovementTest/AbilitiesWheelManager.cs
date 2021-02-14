using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesWheelManager : MonoBehaviour
{
    public GameObject abilitiesPanel;
    public GameObject abilitiesSelectionText;

    // Start is called before the first frame update
    void Start()
    {
        abilitiesPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ToggleAbilitiesPanel();
    }

    private void ToggleAbilitiesPanel()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            abilitiesPanel.SetActive(!abilitiesPanel.activeSelf);
        }
    }

    public void None()
    {
        string abilityName = "None";
        abilitiesSelectionText.GetComponent<Text>().text = abilityName;
        CharacterAbilitiesScript.current.currentAbility = CharacterAbilitiesScript.abilities.none;
    }

    public void PowerDash()
    {
        string abilityName = "Power Dash";
        abilitiesSelectionText.GetComponent<Text>().text = abilityName;
        CharacterAbilitiesScript.current.currentAbility = CharacterAbilitiesScript.abilities.powerDash;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ModuleMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _moduleNameText; 

    private void Start ()
    {
        _moduleNameText.text = SaveSystem.instance.GetModuleById (SaveSystem.selectedModuleId).name;
    }

    public void GoToCardsMode ()
    {
        SceneManager.LoadScene ("CardsMode");
    }

    public void GoToTestMode ()
    {
        SceneManager.LoadScene ("TestMode");
    }
    
    public void GoToSettings ()
    {
        SceneManager.LoadScene ("ModuleSettingsMenu");
    }

    public void BackToModulesListMenu ()
    {
        SceneManager.LoadScene ("ModulesListMenu");
    }
}

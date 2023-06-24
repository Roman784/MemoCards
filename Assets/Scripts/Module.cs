using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Module : MonoBehaviour
{
    private int _id;
    private string _name;
    [SerializeField] private TMP_Text _nameText;

    private void Start ()
    {
        _nameText.text = _name;
    }

    public void SetId (int id)
    {
        _id = id;
    }

    public void SetName (string name)
    {
        _name = name;
        _nameText.text = _name;
    }

    // Переход в настройки модуля.
    public void OpenSettings ()
    {
        SaveSystem.selectedModuleId = _id;
        SceneManager.LoadScene ("ModuleMenu");
    }
}

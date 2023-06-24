using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardFromSettings : Card
{
    [SerializeField] private TMP_InputField _termInputField;
    [SerializeField] private TMP_InputField _definitionInputField;

    public override void SetTerm (string term)
    {
        _term = term;
        _termInputField.text = _term;
    }

    public override void SetDefinition (string definition)
    {
        _definition = definition;
        _definitionInputField.text = _definition;
    }

    public void InputFieldChanged ()
    {
        _term = _termInputField.text;
        _definition = _definitionInputField.text;

        SaveSystem.instance.ChangeCardTermAndDefinition (_moduleId, _id, _term, _definition);
    }

    public void Delete ()
    {
        ModuleSettingsMenu.instance.DeleteCard (this);
    }
}

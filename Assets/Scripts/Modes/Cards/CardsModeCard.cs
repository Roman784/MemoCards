using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof (Animator))]
public class CardsModeCard : Card
{
    [SerializeField] private TMP_Text _termText;
    [SerializeField] private TMP_Text _definitionText;

    private bool _isDefinitionOpen; // Состояние определения, открыто ли оно.

    private Animator _animator;

    private void Start ()
    {
        _animator = GetComponent<Animator> ();
    }

    public override void SetTerm (string newTerm)
    {
        _term = newTerm;
        _termText.text = _term;
    }

    public override void SetDefinition (string newDefinition)
    {
        _definition = newDefinition;
        _definitionText.text = _definition;
    }

    // Показывает/скрывает определение.
    public void OpenCloseCard ()
    {
        _isDefinitionOpen = !_isDefinitionOpen;

        if (_isDefinitionOpen == true)
        {
            _animator.SetTrigger ("DefinitionAppearance");
        }
        else
        {
            _animator.SetTrigger ("DefinitionDisappearance");
        }
    }

    public void HideDefinition ()
    {
        _isDefinitionOpen = false;
        _definitionText.gameObject.SetActive (false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    protected int _moduleId;
    protected int _id;
    protected string _term;
    protected string _definition;

    public int GetId ()
    {
        return _id;
    }

    public void SetId (int newId)
    {
        _id = newId;
    }

    public int GetModuleId ()
    {
        return _moduleId;
    }

    public void SetModuleId (int newModuleId)
    {
        _moduleId = newModuleId;
    }

    public virtual void SetTerm (string term)
    {
        _term = term;
    }

    public virtual void SetDefinition (string definition)
    {
        _definition = definition;
    }
}

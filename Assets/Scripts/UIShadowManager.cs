using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof (Shadow))]
public class UIShadowManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Shadow _shadow;

    private Vector3 _originalPosition;
    private Vector3 _shiftedPosition;

    private void Start ()
    {
        _shadow = GetComponent<Shadow> ();

        _originalPosition = transform.localPosition;
        _shiftedPosition = transform.localPosition + (Vector3)_shadow.effectDistance;
    }

    public void OnPointerEnter (PointerEventData pointerEventData)
    {
        _shadow.enabled = false;
        transform.localPosition = _shiftedPosition;
    }

    public void OnPointerExit (PointerEventData pointerEventData)
    {
        _shadow.enabled = true;
        transform.localPosition = _originalPosition;
    }
}

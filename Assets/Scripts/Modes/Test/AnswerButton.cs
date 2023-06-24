using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof (Animator))]
public class AnswerButton : MonoBehaviour
{
    private string _value;
    [SerializeField] private TMP_Text _valueText;

    private Animator _animator;
    [SerializeField] private AnimationClip _correctAnswerAnimationClip;

    private void Start ()
    {
        _animator = GetComponent<Animator> ();
    }

    public void SetValue (string value)
    {
        _value = value;
        _valueText.text = _value;
    }

    public string GetValue ()
    {
        return _value;
    }

    public void CheckAnswer ()
    {
        TestMode.instance.CheckAnswer (this);
    }

    // Проигрывает анимацию правильного ответа на кнопке и возвращает длину этой анимации.
    public void PlayCorrectAnswerAnimation (out float animationLength)
    {
        _animator.SetTrigger ("CorrectAnswer");
        animationLength = _correctAnswerAnimationClip.length;
    }

    public void PlayWrongAnswerAnimation ()
    {
        _animator.SetTrigger ("WrongAnswer");
    }
}

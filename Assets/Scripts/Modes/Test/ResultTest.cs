using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultTest : MonoBehaviour
{
    [SerializeField] private TMP_Text _questionCountText;
    [SerializeField] private TMP_Text _wrongAnswersCountText;

    public void SetData (int questionCount, int wrongAnswersCount)
    {
        _questionCountText.text = questionCount.ToString ();
        _wrongAnswersCountText.text = wrongAnswersCount.ToString ();
    }

    public void RestartTest ()
    {
        SceneManager.LoadScene ("TestMode");
    }
}

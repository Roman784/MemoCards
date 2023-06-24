using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TestMode : MonoBehaviour
{
    public static TestMode instance;

    private List<CardData> _questionCards = new List<CardData> ();
    private List<CardData> _answerCards = new List<CardData> ();

    private string _answer; // Ответ на текущий вопрос.
    [SerializeField] private TMP_Text _questionText; // Отображение текущего вопроса.
    [SerializeField] private List<AnswerButton> _answerButtons = new List<AnswerButton> ();

    [SerializeField] private ResultTest _resultTestPanel;
    private int _wrongAnswersCount;

    private void Awake ()
    {
        if (instance != null && instance != this)
        {
            Destroy (gameObject);
            return;
        }
        instance = this;
    }

    private void Start ()
    {
        // Заполняем два списка: возможные вопросы и ответы.
        List<CardData> cards = SaveSystem.instance.GetCards (SaveSystem.selectedModuleId);
        foreach (CardData card in cards)
        {
            _questionCards.Add (card);
            _answerCards.Add (card);
        }

        NextQuestion ();
    }

    private void NextQuestion ()
    {
        if (_questionCards.Count == 0 || _answerCards.Count == 0) 
        {
            CompleteTest ();
            return;
        }

        // Выбираем случайную карточку для текущего вопроса из списка вопросов.
        // Отображаем её термин(вопрос) и запоминаем определение(ответ).
        CardData card = _questionCards [Random.Range (0, _questionCards.Count)];
        _questionText.text  = card.term;
        _answer = card.definition;
        
        // Удаляем выбранную карточку из списка вопросов, чтобы она не повторялась.
        _questionCards.Remove (card);

        // Заполняем кнопки с ответами случайными определениями.
        foreach (AnswerButton answerButton in _answerButtons)
        {
            answerButton.SetValue (_answerCards[Random.Range (0, _answerCards.Count)].definition);
        }

        // Случайной кнопке устанавливаем правильный ответ.
        _answerButtons[Random.Range (0, _answerButtons.Count)].SetValue (_answer);
    }

    public void CheckAnswer (AnswerButton answerButton)
    {
        if (answerButton.GetValue () == _answer)
        {
            float animationLength = 0;
            answerButton.PlayCorrectAnswerAnimation (out animationLength);
            Invoke (nameof (NextQuestion), animationLength);
        }
        else
        {
            _wrongAnswersCount += 1;
            answerButton.PlayWrongAnswerAnimation ();
        }
    }

    private void CompleteTest ()
    {
        _resultTestPanel.gameObject.SetActive (true);

        int questionCount = SaveSystem.instance.GetCards (SaveSystem.selectedModuleId).Count;
        _resultTestPanel.SetData (questionCount, _wrongAnswersCount);
    }

    public void BackToModuleMenu ()
    {
        SceneManager.LoadScene ("ModuleMenu");
    }
}

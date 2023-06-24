using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class ModuleSettingsMenu : MonoBehaviour
{
    public static ModuleSettingsMenu instance;

    [SerializeField] private TMP_InputField _moduleNameInputField;
    private string _moduleName;

    [SerializeField] private CardFromSettings _cardPrefab;
    [SerializeField] private float _cardHeight;
    [SerializeField] private RectTransform _scrollViewContent; // Область скролбара для автоматической сортировки карточек. 

    private List<CardFromSettings> _spawnedCards = new List<CardFromSettings> ();

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
        // Установка названия модуля.
        _moduleName = SaveSystem.instance.GetModuleById (SaveSystem.selectedModuleId).name;
        _moduleNameInputField.text = _moduleName;

        StartCoroutine (InstantiateSavedCards ());
    }

    public void CardNameInputFieldChanged ()
    {
        _moduleName = _moduleNameInputField.text;
        SaveSystem.instance.ChangeModuleName (SaveSystem.selectedModuleId, _moduleName);
    }

    // Создаёт новую карточку в бд и её экземпляр на сцене.
    public void AddCard ()
    {
        CardData newCard = SaveSystem.instance.AddCard (SaveSystem.selectedModuleId);
        InstantiateCard (newCard.id, newCard.term, newCard.definition);
    }

    // Создаёт сохранённые карточки на сцене.
    // Задержка между итерациями для эффекта постепенного появления карточек. 
    private IEnumerator InstantiateSavedCards ()
    {
        List<CardData> cards = SaveSystem.instance.GetCards (SaveSystem.selectedModuleId);
        foreach (CardData card in cards)
        {
            yield return new WaitForSeconds (0.1f);

            InstantiateCard (card.id, card.term, card.definition);
        }
    }

    // Создаёт экземпляр карточки на сцене и настраивает её поля.
    private void InstantiateCard (int id, string term, string definition)
    {
        CardFromSettings newCard = Instantiate (_cardPrefab, Vector3.zero, Quaternion.identity);

        _spawnedCards.Add (newCard);
        ResizeScrollViewContent ();        
        
        newCard.transform.SetParent (_scrollViewContent);
        newCard.transform.localScale = new Vector2 (1, 1);

        newCard.SetModuleId (SaveSystem.selectedModuleId);
        newCard.SetId (id);
        newCard.SetTerm (term);
        newCard.SetDefinition (definition);
    }

    public void DeleteModule ()
    {
        SaveSystem.instance.DeleteModule (SaveSystem.selectedModuleId);
        SceneManager.LoadScene ("ModulesListMenu");
    }

    public void DeleteCard (CardFromSettings card)
    {
        SaveSystem.instance.DeleteCard (card.GetModuleId (), card.GetId ());

        _spawnedCards.Remove (card);
        ResizeScrollViewContent ();

        Destroy (card.gameObject);
    }

    // Изменяет размер пролистываемого окна в скролбаре, чтобы оно захватывало все карточки.
    private void ResizeScrollViewContent ()
    {
        float offset = 800f; 
        _scrollViewContent.sizeDelta = new Vector2 (_scrollViewContent.sizeDelta.x, _spawnedCards.Count * _cardHeight + offset);
    }

    // Перекидывает на страницу текущего модуля.
    public void CompleteSetting ()
    {
        SceneManager.LoadScene ("ModuleMenu");
    }

    public void BackToModulesListMenu ()
    {
        SceneManager.LoadScene ("ModulesListMenu");
    }
}

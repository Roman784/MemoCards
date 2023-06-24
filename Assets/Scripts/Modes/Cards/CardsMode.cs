using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardsMode : MonoBehaviour
{
    [SerializeField] private Transform _canvas;
    [SerializeField] private Transform _spawnPoint; // Точка появляения карточек.

    [SerializeField] private CardsModeCard _fullscreenCardPrefab;

    private List<CardsModeCard> _spawnedCards = new List<CardsModeCard> ();
    private int _currentCardIndex;

    private void Start ()
    {
        InstantiateSavedCards ();
    }

    // Создаёт все сохранённые карточки на сцене. 
    // Выключает объект сразу после создания, для включения первой, стартовой карточки.
    private void InstantiateSavedCards ()
    {
        List<CardData> cards = SaveSystem.instance.GetCards (SaveSystem.selectedModuleId);
        if (cards.Count > 0)
        {
            foreach (CardData card in cards)
            {
                CardsModeCard spawnedCard = InstantiateCard (card.id, card.term, card.definition);
                spawnedCard.gameObject.SetActive (false);
            }

            _spawnedCards[_currentCardIndex].gameObject.SetActive (true);
        }
    }

    // Переходит к следующей (или предыдущей, если шаг отрицательный) карточке на <step> позиций.
    public void ChangeCard (int step)
    {
        if (_spawnedCards.Count == 0) return;

        // Выключение текущей карочки.
        _spawnedCards[_currentCardIndex].HideDefinition ();
        _spawnedCards[_currentCardIndex].gameObject.SetActive (false);
        
        _currentCardIndex += step;

        // Проверка на границы списка.
        _currentCardIndex = _currentCardIndex > _spawnedCards.Count - 1 ? 0 : _currentCardIndex;
        _currentCardIndex = _currentCardIndex < 0 ? _spawnedCards.Count - 1 : _currentCardIndex;

        _spawnedCards[_currentCardIndex].gameObject.SetActive (true);
    }

    // Перемешивает позиции карточек, чтобы на сцене они показывались в случайном порядке.
    public void ShuffleCardQueue ()
    {
        if (_spawnedCards.Count == 0) return;

        // Выключаем текущую карточки.
        _spawnedCards[_currentCardIndex].HideDefinition ();
        _spawnedCards[_currentCardIndex].gameObject.SetActive (false);

        // Перемешиваем позиции карточек в общем списке.
        for (int i = _spawnedCards.Count - 1; i >= 1; i--)
        {
            int j = Random.Range (0, i + 1);
    
            CardsModeCard temporaryCard = _spawnedCards[j];
            _spawnedCards[j] = _spawnedCards[i];
            _spawnedCards[i] = temporaryCard;
        }

        // Включаем первую карточку.
        _currentCardIndex = 0;
        _spawnedCards[_currentCardIndex].gameObject.SetActive (true);
    }

    // Создаёт экземпляр карточки на сцене, настраивает её поля.
    // Также возвращает созданную карточку.
    private CardsModeCard InstantiateCard (int id, string term, string definition)
    {
        CardsModeCard newCard = Instantiate (_fullscreenCardPrefab, _spawnPoint.position, Quaternion.identity);
        _spawnedCards.Add (newCard);

        newCard.transform.SetParent (_canvas);
        newCard.transform.localScale = new Vector2 (1, 1);

        newCard.SetModuleId (SaveSystem.selectedModuleId);
        newCard.SetId (id);
        newCard.SetTerm (term);
        newCard.SetDefinition (definition);

        return newCard;
    }

    public void BackToModuleMenu ()
    {
        SceneManager.LoadScene ("ModuleMenu");
    }
}

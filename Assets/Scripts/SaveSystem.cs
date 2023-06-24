using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    [SerializeField] private Db _db; // Экземпляр БД.

    private string _savePath; // Путь к файлу сохранения.
    [SerializeField] private string _saveFileName;

    public static int selectedModuleId; // Id модуля в котором мы работаем.

    private void Awake ()
    {
        if (instance != null && instance != this)
        {
            Destroy (gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad (gameObject);

        // Установка пути к файлу сохранения в зависимости от платформы устройства.
        #if UNITY_ANDROID && !UNITY_EDITOR
            _savePath = Path.Combine (Application.persistentDataPath, _saveFileName);
        #else
            _savePath = Path.Combine (Application.dataPath, _saveFileName);
        #endif

        LoadData ();
    }

    private void OnApplicationQuit ()
    {
        SaveData ();
    }

    private void OnApplicationPause (bool pauseStatus)
    {
        if (Application.platform == RuntimePlatform.Android)
            SaveData ();
    }

    [ContextMenu("Save")]
    private void SaveData ()
    {   
        try
        {
            string json = JsonUtility.ToJson (_db, true);
            File.WriteAllText (_savePath, json);
        }
        catch { Debug.Log ("Save data error"); }
    }

    [ContextMenu("Load")]
    private void LoadData ()
    {
        if (!File.Exists (_savePath)) return;

        try 
        {
            string json = File.ReadAllText (_savePath);
            _db = JsonUtility.FromJson<Db> (json);
        }
        catch { Debug.Log ("Load data error"); }
    }

    public List<ModuleData> GetModules ()
    {
        return _db.modules;
    }

    public ModuleData GetModuleById (int id)
    {
        foreach (ModuleData module in _db.modules)
        {
            if (module.id == id)
            {
                return module;
            }
        }
        return new ModuleData ();
    }

    public List<CardData> GetCards (int moduleId)
    {
        return GetModuleById (moduleId).cards;
    }

    public CardData GetCardById (int moduleId, int cardId)
    {
        List<CardData> cards = GetModuleById (moduleId).cards;
        foreach (CardData card in cards)
        {
            if (card.id == cardId)
            {
                return card;
            }
        }
        return new CardData ();
    }

    // Создаёт новый модуль и возвращает его.
    public ModuleData AddModule ()
    {
        // Получение незанятого/нового id для этого модуля.
        List<IIdentifiered> modulesList = new List<IIdentifiered> (_db.modules);
        int id = GetFreeId (modulesList);

        // Создание модуля и сохранение его в бд.
        ModuleData newModule = new ModuleData ();
        newModule.id = id;
        newModule.name = "New module";
        newModule.cards = new List<CardData> ();

        _db.modules.Add (newModule);
        SaveData ();

        return newModule;
    }

    // Создаёт новую карточку и возвращает её.
    public CardData AddCard (int moduleId)
    {
        ModuleData module =  GetModuleById (moduleId);

        // Получение незанятого/нового id для этой карточки.
        List<IIdentifiered> cardsList = new List<IIdentifiered> (module.cards);;
        int id = GetFreeId (cardsList);

        // Создание карточки и сохранение её в бд.
        CardData newCard = new CardData ();

        newCard.id = id;
        newCard.term = "";
        newCard.definition = "";

        module.cards.Add (newCard);
        SaveData ();

        return newCard;
    }

    public void ChangeModuleName (int id, string newName)
    {
        foreach (ModuleData module in _db.modules)
        {
            if (module.id == id)
            {
                module.name = newName;
                break;
            }
        }

        SaveData ();
    }

    public void ChangeCardTermAndDefinition (int moduleId, int cardId, string newTerm, string newDefinition)
    {
        foreach (ModuleData module in _db.modules)
        {
            if (module.id == moduleId)
            {
                foreach (CardData card in module.cards)
                {
                    if (card.id == cardId)
                    {
                        card.term = newTerm;
                        card.definition = newDefinition;
                        break;
                    }
                }
            }
        }

        SaveData ();
    }

    // Удаляет модуль и все его карточки соответственно. 
    public void DeleteModule (int id)
    {
        foreach (ModuleData module in _db.modules)
        {
            if (module.id == id)
            {
                _db.modules.Remove (module);
                break;
            }
        }

        SaveData ();
    }

    // Удаляет одну конкретную карточку.
    public void DeleteCard (int moduleId, int cardId)
    {
        List<CardData> cards = GetModuleById (moduleId).cards;
        foreach (CardData card in cards)
        {
            if (card.id == cardId)
            {
                cards.Remove (card);
                break;
            }
        }

        SaveData ();
    }

    [ContextMenu("DeleteAll")]
    public void DeleteAll ()
    {
        _db = new Db ();
        SaveData ();
    }

    // Возвращает свободный/незанятый id для модулей или карточек, или создаёт новый если такого нет.
    private int GetFreeId (List<IIdentifiered> identifieredObjectsList)
    {
        if (identifieredObjectsList.Count > 0)
        {
            // Заполнение списка всех id для поиска доступных.
            List<int> ids = new List<int> ();
            foreach (IIdentifiered obj in identifieredObjectsList)
            {
                ids.Add (obj.GetId ());
            }

            // Поиск свободного id.
            for (int id = 0; id <= ids.Max () + 1; id++)
            {
                if (ids.Contains (id) == false)
                {
                    return id;
                }
            }
        }

        return 0;
    }
}

[System.Serializable]
public class Db
{
    public List<ModuleData> modules;
}

[System.Serializable]
public class ModuleData : IIdentifiered
{
    public int id;
    public string name;
    public List<CardData> cards;

    public int GetId () { return id;}
}

[System.Serializable]
public class CardData : IIdentifiered
{
    public int id;
    public string term;
    public string definition;

    public int GetId () { return id;}
}

public interface IIdentifiered
{
    public int GetId ();
}
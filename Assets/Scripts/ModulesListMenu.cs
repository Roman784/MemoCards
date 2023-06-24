using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class ModulesListMenu : MonoBehaviour
{
    [SerializeField] private Module _modulePrefab;
    [SerializeField] private float _moduleHeight;
    [SerializeField] private RectTransform _scrollViewContent; // Область скролбара для автоматической сортировки модулей. 

    private List<Module> _spawnedModules = new List<Module> ();

    private void Start ()
    {
        StartCoroutine (InstantiateSavedModules ());
    }

    // Создаёт новый модуль в базе данных и перекидывает в его настройки.
    public void AddModule ()
    {
        ModuleData newModule = SaveSystem.instance.AddModule ();

        SaveSystem.selectedModuleId = newModule.id;
        SceneManager.LoadScene ("ModuleSettingsMenu");
    }

    // Создаёт сохранённые модули на сцене.
    // Задержка между итерациями для эффекта постепенного появления модулей.  
    private IEnumerator InstantiateSavedModules ()
    {
        foreach (ModuleData module in SaveSystem.instance.GetModules ())
        {
            yield return new WaitForSeconds (0.1f);

            InstantiateModule (module.id, module.name);
        }
    }

    // Создаёт экземпляр модуля на сцене и настраивает его поля.
    private void InstantiateModule (int id, string name)
    {
        Module newModule = Instantiate (_modulePrefab, Vector3.zero, Quaternion.identity);
        _spawnedModules.Add (newModule);

        // Изменение пролистываемого окна в скролбаре, чтобы оно захватывало все модули.
        float offset = 500f;
        _scrollViewContent.sizeDelta = new Vector2 (_scrollViewContent.sizeDelta.x, _spawnedModules.Count * _moduleHeight + offset);

        newModule.transform.SetParent (_scrollViewContent);
        newModule.transform.localScale = new Vector2 (1, 1);

        newModule.SetId (id);
        newModule.SetName (name);
    }
}

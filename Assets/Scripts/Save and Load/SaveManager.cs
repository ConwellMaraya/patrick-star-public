using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

public class SaveManager : MonoBehaviour 
{
    public static SaveManager instance;


    private string fileName;
    private string filePath;
    [SerializeField] private bool encryptData;
    private GameData gameData;
    [SerializeField] private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;
    private bool saveDataExist = false;

    [ContextMenu("Delete save file")]
    public void DeleteSavedData()
    {
        dataHandler = new FileDataHandler(filePath, fileName,encryptData);
        dataHandler.Delete();
    }

    private void Awake()
    {
        

        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }


    private void Start()
    {
        GameObject Login = GameObject.Find("LoginStuff");
        fileName = "data." + Login.GetComponent<LoginStuffScript>().userSaveName;
        filePath = "idbfs/" + Login.GetComponent<LoginStuffScript>().userSaveName;
        Debug.Log(fileName + " SAVE MANAGER");
        Debug.Log(filePath + " SAVE MANAGER");
        dataHandler = new FileDataHandler(filePath, fileName,encryptData);
        saveManagers = FindAllSaveManagers();

        if (Directory.Exists(filePath))
            saveDataExist = true;

        //Invoke("LoadGame", .05f);


        
        LoadGame();

        
        
        
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No saved data found!");
            NewGame();
        }

        

        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {

        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
        Debug.Log("Quited and Saveded");
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }

    public bool HasSavedData()
    {
        return saveDataExist;
    }

    private static readonly Regex sWhitespace = new Regex(@"\s+");
    public static string ReplaceWhitespace(string input, string replacement)
    {
        return sWhitespace.Replace(input, replacement);
    }

    IEnumerator WaitWithDelayThenCondition(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds); // Delay first

        Debug.Log("Delay passed and condition met!");
    }

}

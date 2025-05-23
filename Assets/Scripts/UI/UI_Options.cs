using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UI_Options : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void ExitGameOptions()
    {
        Debug.Log("Exit game");
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }
#endif
        GameObject Login = GameObject.Find("LoginStuff");
        GameObject saveM = GameObject.Find("SaveManager");
        saveM.GetComponent<SaveManager>().SaveGame();
        string combFilePath = saveM.GetComponent<SaveManager>().filePath + "/" + saveM.GetComponent<SaveManager>().fileName;
        await SaveUpload.UploadJsonFileAsync(combFilePath, Login.GetComponent<LoginStuffScript>().userSaveName, Login.GetComponent<LoginStuffScript>().projectUrl, Login.GetComponent<LoginStuffScript>().apiKey, Login.GetComponent<LoginStuffScript>().tableName,Login.GetComponent<LoginStuffScript>().playerId);
        Application.Quit();
    }
}

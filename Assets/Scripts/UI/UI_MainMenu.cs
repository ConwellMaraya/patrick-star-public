using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using System.IO;
using System.Threading;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject continueButton;
    [SerializeField] UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject gameManager;
    private string filePath;

    private void Start()
    {
        GameObject Login = GameObject.Find("LoginStuff");
        GameObject saveM = GameObject.Find("SaveManager");
        filePath = Application.persistentDataPath + "/" + "data.json";
        Debug.Log(filePath + " MENU");
        if (!System.IO.File.Exists(filePath))
            continueButton.SetActive(false);
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f, gameManager.GetComponent<GameManager>().levelCounter));
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSavedData();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f,0));
    }

    public void ExitGame()
    {
        Debug.Log("Exit game");
        Application.Quit();
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay, int levelCounter)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);

        if (levelCounter == 4)
            sceneName = "Boss Level";
        else 
            sceneName = "Level" + gameManager.GetComponent<GameManager>().currLevel.ToString();
        SceneManager.LoadScene(sceneName);
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

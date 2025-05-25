using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject continueButton;
    [SerializeField] UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject gameManager;
    private string filePath;

    private void Start()
    {
        if (SaveManager.instance.HasSavedData() == false)
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
        //Application.Quit();
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    public GameObject Exit;
    public CapsuleCollider2D capsule;
    [SerializeField] UI_FadeScreen fadeScreen;
    [SerializeField] private string sceneName;
    public GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        if (Exit != null)
        {
            capsule = Exit.GetComponent<CapsuleCollider2D>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            gameManager.GetComponent<GameManager>().levelCounter++;
            StartCoroutine(LoadSceneWithFadeEffect(1.5f, gameManager.GetComponent<GameManager>().levelCounter));
        }
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay, int levelCounter)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);

        if (levelCounter == 4)
            sceneName = "Boss Level";
        else
            sceneName = "Level" + gameManager.GetComponent<GameManager>().levelArrangement[levelCounter];
        SceneManager.LoadScene(sceneName);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

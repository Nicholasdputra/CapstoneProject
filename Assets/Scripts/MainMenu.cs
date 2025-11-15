using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayBGMLoop(0);
    }

    public void StartGame()
    {
        AudioManager.Instance.PlaySFXOnce(0);
        UnityEngine.SceneManagement.SceneManager.LoadScene("ResourceIsland1");
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlaySFXOnce(0);
        Application.Quit();
    }
}

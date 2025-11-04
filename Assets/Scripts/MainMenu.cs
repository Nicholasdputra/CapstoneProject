using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DetermineGameState()
    {
        // Check if there exists game save
        // If yes
        // Show continue game option
    }
    
    public void NewGame()
    {
        // Start a new game
        SceneManager.LoadScene("ResourceIsland1");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}

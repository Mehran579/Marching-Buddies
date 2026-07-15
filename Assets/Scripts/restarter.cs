using UnityEngine;
using UnityEngine.SceneManagement;

public class restarter : MonoBehaviour
{
    public Canvas canvas;
    Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 0;
        canvas.worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void mainemenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("main menu");
    }
}

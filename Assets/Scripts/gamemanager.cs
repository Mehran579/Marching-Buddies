using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject startPanel;

    private void Start()
    {
        Time.timeScale = 0f;
        startPanel.SetActive(true);
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}

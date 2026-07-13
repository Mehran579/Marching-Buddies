using UnityEngine;
using UnityEngine.SceneManagement;

public class scenechanger : MonoBehaviour
{
    public void level1()
    {
        SceneManager.LoadScene(1);
    }
    public void level2()
    {
        SceneManager.LoadScene(2);
    }
    public void level3()
    {
        SceneManager.LoadScene(3);
    }
    public void level4()
    {
        SceneManager.LoadScene(4);
    }
    public void level5()
    {
        SceneManager.LoadScene(5);

    }
    public void bosslevel()
    {
        SceneManager.LoadScene("boss scene 3");
    }
    
}

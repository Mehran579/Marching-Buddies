using UnityEngine;
//using UnityEngine.SceneManagement;

public class tempdeath : MonoBehaviour
{
    public GameObject restartpanel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.CompareTag("final player") || (collision.transform.parent != null && collision.transform.parent.CompareTag("final player"))||collision.transform.parent.CompareTag("Player"))
        {
            //Debug.Log("WOuld hv been killed if i wrote the logic");
            restartpanel.SetActive(true);
            //restartpanel.GetComponent<restarter>.stop();
            //collision.transform.parent.position = Vector3.zero;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

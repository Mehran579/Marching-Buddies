using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class cinemachinemanager : MonoBehaviour
{
    public CinemachineCamera cam;
    public CinemachinePositionComposer composer;
    public Transform player;
    public Transform finalcrystal;
    public float maxdistance = 20f;
    public float smooth = 5f;

    private void Start()
    {
        //Debug.Log(SceneManager.GetActiveScene().name);
    }
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "boss scene 3" && finalcrystal != null)
        {

            float t = Mathf.InverseLerp(0f, maxdistance, Vector2.Distance(player.position, finalcrystal.position));
            float targetzoom = Mathf.Lerp(3f, 9f, t);
            cam.Lens.OrthographicSize = Mathf.Lerp(cam.Lens.OrthographicSize, targetzoom, Time.deltaTime * smooth);
            Vector3 offset = composer.TargetOffset;
            offset.y = Mathf.Lerp(1.84f, 5f, t);
            composer.TargetOffset = Vector3.Lerp(composer.TargetOffset, offset, smooth * Time.deltaTime);
        }
        if(cam.Target.TrackingTarget.gameObject.CompareTag("final player"))
        {
            cam.Lens.OrthographicSize = Mathf.Lerp(cam.Lens.OrthographicSize,45,smooth* Time.deltaTime);
            cam.GetComponent<CinemachinePositionComposer>().Composition.DeadZone.Size = new Vector2(40f, 40f);
            //cam.GetComponent<CinemachinePositionComposer>()
        }   
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loaded: " + scene.name);
        if (SceneManager.GetActiveScene().name == "boss scene 3")
        {
            //Debug.Log("scene 3 is active");
            finalcrystal = GameObject.FindWithTag("final point").transform;
        }    
    }
}


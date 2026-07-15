using UnityEngine;

public class pulse : MonoBehaviour
{
    public SpriteRenderer sr;
    public Color colorA = Color.white;
    public Color colorB = Color.red;
    public float speed = 3f;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        //colorB = sr.color;
    }
    void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
        sr.color = Color.Lerp(colorA, colorB, t);
    }
}
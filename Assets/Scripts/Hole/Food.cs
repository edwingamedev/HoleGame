using System.Collections;
using UnityEngine;

public class Food : MonoBehaviour
{
    public int points;
    private const int MatRenderQueue = 1999; // this is used to render the food inside the hole.
    public Hole ConsumerHole { get; set; }
    
    private bool pointsGiven = false;
    
    private void Awake()
    {
        Material mat = GetComponent<Renderer>().material;

        mat.renderQueue = MatRenderQueue;
    }

    public void Consume()
    {
        if (pointsGiven || ConsumerHole == null)
        {
            return;
        }

        ConsumerHole.AddPoints(this);
        pointsGiven = true;

        StartCoroutine(ShrinkAndDestroy());
    }

    private IEnumerator ShrinkAndDestroy()
    {
        float duration = 0.4f;
        float time = 0f;
        Vector3 originalScale = transform.localScale;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}

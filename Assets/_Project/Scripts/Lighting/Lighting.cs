using UnityEngine;

public class Lighting : MonoBehaviour
{
    public float rotationSpeed = 10f;

    private Light sunLight;

    void Start()
    {
        sunLight = GetComponent<Light>();
    }

    void Update()
    {
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        float angle = transform.eulerAngles.x;
        float brightness = Mathf.Sin(angle * Mathf.Deg2Rad);
        brightness = (brightness + 1f) / 2f;

        sunLight.intensity = brightness;

        if (brightness > 0.5f)
        {
            sunLight.color = Color.Lerp(Color.yellow, Color.white, (brightness - 0.5f) * 2f);
        }
        else
        {
            sunLight.color = Color.Lerp(Color.blue, Color.yellow, brightness * 2f);
        }
    }
}
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    const float tau = 2f * Mathf.PI;

    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 10f;
    Vector3 startVector;
    float movementFactor;

    // Start is called before the first frame update
    void Start()
    {
        startVector = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { // Zero check that is float safe
            return;
        }

        float cycles = Time.time / period;
        float rawSinWave = Mathf.Sin(cycles * tau);
        movementFactor = (rawSinWave / 2f) + 0.5f;
        Vector3 offsetVector = movementFactor * movementVector;
        transform.position = startVector + offsetVector;
    }
}

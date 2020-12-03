using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    Vector3 startVector;

    [Range(0,1)]
    [SerializeField]
    float movementFactor;

    // Start is called before the first frame update
    void Start()
    {
        startVector = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offsetVector = movementFactor * movementVector;
        transform.position = startVector + offsetVector;
    }
}

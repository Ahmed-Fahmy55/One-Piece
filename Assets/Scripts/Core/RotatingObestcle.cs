using UnityEngine;

public class RotatingObestcle : MonoBehaviour
{
    public float rotationSpeed = 45f;

    void Update()
    {
        // Rotate the obstacle around the Z-axis
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}

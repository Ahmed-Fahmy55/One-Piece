using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float xParallaxMultiplayer;
    [SerializeField, Range(0, 1)] float yParallaxMultiplayer;


    private Camera _camera;
    private Vector3 _lastCamPosition;

    private void Awake()
    {
        _camera = Camera.main;
    }

    void Start()
    {
        _lastCamPosition = _camera.transform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = _camera.transform.position - _lastCamPosition;
        transform.position += new Vector3(deltaMovement.x * xParallaxMultiplayer, deltaMovement.y * yParallaxMultiplayer, deltaMovement.z);
        _lastCamPosition = _camera.transform.position;

    }
}

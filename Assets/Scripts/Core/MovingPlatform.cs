using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] float _stoppingDistance = .2f;
    [SerializeField] Transform[] _points;



    private int _currentPointIndex;
    private Vector2 _currentVelocity;
    private Vector2 _previousPosition;


    private void Update()
    {
        HandleMovement();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag != "Player") return;
        collision.collider.GetComponent<PlayerController>().SetPlatForm(this);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag != "Player") return;
        collision.collider.GetComponent<PlayerController>().SetPlatForm(null);
    }

    private void HandleMovement()
    {
        if (Vector2.Distance(transform.position, _points[_currentPointIndex].position) < _stoppingDistance)
        {
            _currentPointIndex++;
            if (_currentPointIndex >= _points.Length) _currentPointIndex = 0; ;
        }
        transform.position = Vector2.MoveTowards(transform.position, _points[_currentPointIndex].position, _moveSpeed * Time.deltaTime);
        CalculateVelocity();
    }

    void CalculateVelocity()
    {
        _currentVelocity = ((Vector2)transform.position - _previousPosition) / Time.deltaTime;
        _previousPosition = transform.position;
    }
    public Vector2 GetCurrentSpeed()
    {
        return _currentVelocity;
    }
}

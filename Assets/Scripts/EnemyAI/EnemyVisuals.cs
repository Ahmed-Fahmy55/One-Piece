using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    [SerializeField] SpriteRenderer enemySprite;

    private EnemyAI _enemyAI;

    private void Awake()
    {
        _enemyAI = GetComponentInParent<EnemyAI>();
    }

    private void Update()
    {
        HandleFlip();
    }
    private void HandleFlip()
    {
        transform.localScale = new Vector3(_enemyAI.ShouldFaceLeft() ? -1 : 1, 1, 1);
    }
}

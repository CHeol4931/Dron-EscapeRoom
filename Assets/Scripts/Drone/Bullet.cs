using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;

    private Vector3 shooterPosition;
    private bool hasHit;
    public void Init(Vector3 direction, float speed, Vector3 shootPos)
    {
        shooterPosition = shootPos;
        var rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifeTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        hasHit = true;

        var enemy = collision.collider.GetComponentInParent<Enemy>();
        if (enemy != null) enemy.OnShot(shooterPosition);

        Destroy(gameObject);
    }
}

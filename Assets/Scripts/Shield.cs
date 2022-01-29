using System;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] Vector3 startPosition = Vector3.zero;
    [SerializeField] Vector2 yClamps = Vector2.zero;

    [SerializeField] float moveSpeed = 5f;

    public event Action<Projectile> onProjectileHit;

    private void Start()
    {
        transform.position = startPosition;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            MoveShield(Vector3.up);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            MoveShield(Vector3.down);
        }
    }
    
    private void MoveShield(Vector3 direction)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        Vector3 newPosition = transform.position;

        newPosition.y = Mathf.Clamp(newPosition.y, GetMinYClamp(), GetMaxYClamp());
        transform.position = newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile == null) return;

        onProjectileHit(projectile);
    }

    public float GetMinYClamp()
    {
        return yClamps.x;
    }
    public float GetMaxYClamp()
    {
        return yClamps.y;
    }
}

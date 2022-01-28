using System;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] Vector3 startPosition = Vector3.zero;
    [SerializeField] float moveSpeed = 5f;

    [SerializeField] float minYClamp = 0f;
    [SerializeField] float maxYClamp = 0f;

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

        newPosition.y = Mathf.Clamp(newPosition.y, minYClamp, maxYClamp);
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
        return minYClamp;
    }
    public float GetMaxYClamp()
    {
        return maxYClamp;
    }
}

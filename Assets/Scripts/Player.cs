using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int playerPoints = 0;
    [SerializeField] float castleHealth = 100f;

    Shield shield = null;

    private void Awake()
    {
        shield = FindObjectOfType<Shield>();
        shield.onProjectileHit += OnShieldHit;
    }

    public void AddPoints(int pointsToAdd)
    {
        playerPoints += pointsToAdd;
    }

    public void SubtractPoints(int pointsToSubtract)
    {
        playerPoints -= pointsToSubtract;
        playerPoints = Mathf.Clamp(playerPoints, 0, int.MaxValue);
    }

    private void OnShieldHit(Projectile projectile)
    {
        AddPoints(projectile.GetPointsReward());

        projectile.SetIsActive(false);
        projectile.gameObject.SetActive(false);
    }

    public void DamageHealth(float damageAmount)
    {
        castleHealth -= damageAmount;
        castleHealth = Mathf.Clamp(castleHealth, 0f, 100f);

        if (castleHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        print("Die");
        //Turn off shield movement
        //Stop Projectile launcher
        //Log high score
        //Show end screen(play again/quit)
    }

    private void OnTriggerEnter(Collider other)
    {
        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile == null) return;

        SubtractPoints(projectile.GetPointsReward());
        DamageHealth(projectile.GetDamageAmount());

        projectile.SetIsActive(false);
        projectile.gameObject.SetActive(false);
    }
}

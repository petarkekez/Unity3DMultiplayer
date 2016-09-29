using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    public RectTransform healthBar;

    public const int maxHealth = 100;
    [SyncVar(hook = "OnHealthChange")]
    public int currentHealth = maxHealth;

    public bool destroyOnDeath;

    private NetworkStartPosition[] spawnPoints;


    void Start()
    {
        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                currentHealth = maxHealth;
                RpcRespawn();
            }
        }
        
    }

    private void OnHealthChange(int health)
    {
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
                transform.position = spawnPoint;
            }
            else
            {
                transform.position = Vector3.zero;
            }
        }
    }
}

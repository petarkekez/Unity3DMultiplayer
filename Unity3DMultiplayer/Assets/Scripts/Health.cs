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

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
            RpcRespawn();
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
            transform.position = Vector3.zero;
            //currentHealth = maxHealth;
        }
    }
}

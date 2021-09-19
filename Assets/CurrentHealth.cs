using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;

public class CurrentHealth : NetworkBehaviour
{

    public const int maxHealth = 100;

    public bool destroyOnDeath;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    public RectTransform healthBar;

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

                // called on the Server, will be invoked on the Clients
                RpcRespawn();
            }
        }
    }

    void OnChangeHealth(int currentHealth)
    {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // Set the player’s position to origin
            transform.position = Vector3.zero;
        }
    }
}


/*{
public const int maxHealth = 100;

[SyncVar(hook = "OnChangeHealth")]
public int currentHealth = maxHealth;
public RectTransform healthBar;

public void TakeDamage(int amount)
{
    if (!isServer)
    {
        return;

    }

    currentHealth -= amount;
    if (currentHealth <= 0)
    {
        currentHealth = 0;
        Debug.Log("Dead!");
        RpcRespawn(); 
    }
}
void OnChangeHealth(int currentHealth){
    healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
}

[ClientRpc]
void RpcRespawn()
{
    if (isLocalPlayer)
    {
        // move back to zero location
        transform.position = Vector3.zero;
    }
}
} */






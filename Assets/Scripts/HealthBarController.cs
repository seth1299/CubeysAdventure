using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{

    public GameObject player, boss, healthBar1, healthBar2, healthBar3, healthBar4, bossHealthBar3, bossHealthBar2;
    private PlayerController PCS;
    private BossController BCS;

    private int health, bossHealth;

    void Start()
    {
        PCS = player.GetComponent<PlayerController>();
        BCS = boss.GetComponent<BossController>();
    }

    // Update is called once per frame
    void Update()
    {
        health = PCS.GetHealth();
        bossHealth = BCS.GetHealth();

        if (health == 4)
        {
            healthBar1.SetActive(true);
            healthBar2.SetActive(true);
            healthBar3.SetActive(true);
            healthBar4.SetActive(true);
        }
        else if (health == 3)
        {
            healthBar1.SetActive(true);
            healthBar2.SetActive(true);
            healthBar3.SetActive(true);
            healthBar4.SetActive(false);
        }
        else if (health == 2)
        {
            healthBar1.SetActive(true);
            healthBar2.SetActive(true);
            healthBar3.SetActive(false);
            healthBar4.SetActive(false);
        }
        else if (health == 1)
        {
            healthBar1.SetActive(true);
            healthBar2.SetActive(false);
            healthBar3.SetActive(false);
            healthBar4.SetActive(false);
        }

        if (bossHealth == 2)
            bossHealthBar3.SetActive(false);
        else if (bossHealth == 1)
            bossHealthBar2.SetActive(false);
    }
}

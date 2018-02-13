using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTrigger : MonoBehaviour
{

    public PowerUpType powerUpType;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            var player = other.GetComponent<PlayerController>();
            if (powerUpType == PowerUpType.DecoyExplode)
            {
                player.decoyExplode += 2;
            }

            if (powerUpType == PowerUpType.Dragon)
            {
                player.dragonPower += 3;
            }
            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyController : MonoBehaviour
{

    public GameObject explosionPrefab;
    // Use this for initialization
    GameObject explosionObject = null;
    float delay = 0;
    bool explode = false;
    EnemyController enemyTarget = null;
    PlayerController player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (delay > 3F && explode && explosionObject == null)
        {
            Debug.Log("Boom!");
            explosionObject = Instantiate(explosionPrefab, transform.position, transform.rotation);
            explode = false;
            enemyTarget.health = 0F;
            enemyTarget.isDead = true;
            player.decoyExplode--;
        }

        if (delay > 5F)
        {
            Debug.Log("Destroy self");
            if (explosionObject != null)
                Destroy(explosionObject);
            explosionObject = null;
            gameObject.SetActive(false);
        }
        if (explode || explosionObject != null)
            delay += Time.deltaTime;
    }

    public void InflictDamage(EnemyController enemy)
    {       
        if (player.decoyExplode > 0)
        {
            explode = true;
            enemyTarget = enemy;            
        }
    }
}

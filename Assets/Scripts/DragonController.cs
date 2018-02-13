using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour {

    GameObject player;
    public GameObject explosionPrefab;
    Queue<GameObject> explosions = new Queue<GameObject>();
    Animator animator;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = player.transform.rotation;
        var pos = player.transform.position;
        pos.y += 4F;
        transform.position = pos;

        if (Input.GetMouseButtonDown(0) && player.GetComponent<PlayerController>().dragonPower>0)
        {
            animator.Play("Breath_Fs");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                var explosion = Instantiate(explosionPrefab, hit.point, transform.rotation);
                explosion.GetComponent<ParticleSystem>();
                
                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.GetComponent<EnemyController>().health = 0F;
                    hit.transform.GetComponent<EnemyController>().isDead = true;
                }
                player.GetComponent<PlayerController>().dragonPower--;
            }
        }
    }
}

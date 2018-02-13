using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public bool isOver = false;
    public Text gameOver;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isOver)
        {
            Time.timeScale = 0F;
            gameOver.text = "Game Over";
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScene");
        }

        if(Input.GetKey(KeyCode.Return))
        {
            SceneManager.LoadScene("MainScene");
        }
	}
}

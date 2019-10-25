using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SummonOnStart : MonoBehaviour {

    int charactors;
    public GameObject[] sHero = new GameObject[4];
    [SerializeField] GameObject respawn = null;
    public int sceneNum = 3;

    void Awake() {

        charactors = TurnOnTheStage.charactorNum;
        SceneLoader.Instance.MoveToOtherScene(sHero, sceneNum);

        respawn = GameObject.FindWithTag("Respawn");
            for (int i = 0; i < 4; i++)
            {
                if (charactors == i)
                {
                  respawn = Instantiate(sHero[i], respawn.transform.position, Quaternion.identity);
                }
            }
        
	}
	
}

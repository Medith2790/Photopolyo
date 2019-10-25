using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TurnOnTheStage : MonoBehaviour {

    [Header("Option")]
    [SerializeField] bool bTurnLeft = false;
    [SerializeField] bool bTurnRight = false;
    [SerializeField] Quaternion turn = Quaternion.identity;
    [SerializeField] int value = 0;
    

    public static int charactorNum = 0;

    
    void Start () {
        //각 초기화
        turn.eulerAngles = new Vector3(0, value, 0);
       
	}
	
	void Update () {
		
        if(bTurnLeft)
        {
            Debug.Log("Left");
            charactorNum++;
            if (charactorNum == 4)
                charactorNum = 0;
                

            //각도를 -90도
            value -= 90;
            // 불 변수를 취소
            bTurnLeft = false;
        }
        
        if(bTurnRight)
        {
            Debug.Log("Right");
            charactorNum--;
            if (charactorNum == -1)
                charactorNum = 3;

            //각도를 +90도
            value += 90;

            //불 변수 취소
            bTurnRight = false;
        }
        //각도를 잼
        turn.eulerAngles = new Vector3(0, value, 0);

        //회전
        transform.rotation = Quaternion.Slerp(transform.rotation, turn, Time.deltaTime * 5.0f);
	}

    public void turnLeft()
    {
        //버튼 컨트롤러 제어
        bTurnLeft = true;
        bTurnRight = false;
    }


    public void turnRight()
    {
        //버튼 컨트롤러 제어
        bTurnLeft = false;
        bTurnRight = true;
    }

    public void turnStage()
    {
        //게임플레이 씬
        SceneManager.LoadScene("CharacterSelect");
    }
}

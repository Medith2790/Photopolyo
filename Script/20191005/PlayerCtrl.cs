using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//인스펙터창에 표시하려면 [System.Serializable]를 해주어야함
[System.Serializable]
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip runB;
    public AnimationClip runF;
    public AnimationClip runL;
    public AnimationClip runR;
}



public class PlayerCtrl : MonoBehaviour {


    /*public변수선언*/
    public PlayerAnim playerAnim;

    [HideInInspector] public Animation anim;                //인스펙터창에서 안보이게 할때

    /*private변수 선언*/
    [SerializeField] private float moveSpeed = 5.0f;         //이동속도
    [SerializeField] private float rotSpeed = 80.0f;          //회전속도
    private float h = 0.0f, v = 0.0f, r = 0.0f;
    private Transform tr;

    private void OnEnable()
    {
        GameManager.OnitemChange += UpdateSetup;
    }

    private void UpdateSetup()
    {
        moveSpeed = GameManager.Instance.gameData.Speed;

    }

    void Start () {

        tr = GetComponent<Transform>();
        //컴포넌트와 연결하는 방법은 여러가지가 있지만
        //여기에서는 앞부분에 this.gameObject가 빠진것
        anim = GetComponent<Animation>();
        anim.clip = playerAnim.idle;
        anim.Play();

        //불러온 데이터 값을 moveSpeed에 적용
        moveSpeed = GameManager.Instance.gameData.Speed;
        
	}
	

	void Update () {

        /*변위값*/
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        tr.Translate(moveDir * moveSpeed * Time.deltaTime, Space.Self);
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);
        /*Vector3.up기준*/
        //Y축 기준으로 rotSpeed만큼의 속도로 회전
        CharacterMove();
    }


    /*이동함수*/
    //마지막 0.3f은 변경될때까지의 대기시간
    void CharacterMove()
    {
        if (v >= 0.1f)
        {
            anim.CrossFade(playerAnim.runF.name, 0.3f); //전진 애니메이션

        }

        else if (v <= -0.1f)
        {
            anim.CrossFade(playerAnim.runB.name, 0.3f); // 뒤로가기

        }


        else if (h >= 0.1f)
        {
            anim.CrossFade(playerAnim.runR.name, 0.3f); //오른쪽

        }

        else if (h <= -0.1f)
        {
            anim.CrossFade(playerAnim.runL.name, 0.3f); // 왼쪽

        }
        else
        {
            anim.CrossFade(playerAnim.idle.name, 0.3f);  // 정지시
        }
    }
}


//deltaTime : 매 초마다 일정하게 실행
//Space.Self : 로컬좌표계 - 캐릭터 축에 맞게 이동
//Space.World : 월드좌표계 - 축상태 앞뒤 좌우로 이동
//normalized : 항상0, 1로 표시(정규화 표시방법)

/*벡터

 1) Vector3.forward == Vector3(0,0,1);
 2) Vector3.back == Vector3(0,0,-1);
 3) Vector3.left == Vector3(-1,0,0);
 4) Vector3.Right == Vector3(1,0,0);
 5) Vector3.UP == Vector3(0,1,0);
 6) Vector3.Down == Vector3(0,-1,0);
 7) Vector3.one == Vector3(1,1,1);
 8) Vector3.zero == Vector3(0,0,0);


 */

/*
 회전
 Rotate(회전할 기준좌표 * Time.deltaTime*회전속도*변위입력값);
 */

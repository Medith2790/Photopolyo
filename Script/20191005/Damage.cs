using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Damage : MonoBehaviour {

    private const string bulletTag = "Bullet";
    private const string enemyTag = "ENEMY";

    private float initHP;
    private float currentHP = 0f;
    private Color CurrentColor;
    private readonly Color initColor = new Vector4(0, 1.0f, 0.0f, 1.0f);        //RGBA색상값인 G,A부분을 1.0f = 초기색상은 녹색


    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;
    public Image bloodScreen;
    public Image hpBar;


    private void OnEnable()
    {
        GameManager.OnitemChange += UpdateSetup;
    }

    private void UpdateSetup()
    {
        initHP = GameManager.Instance.gameData.hp;
        currentHP += GameManager.Instance.gameData.hp - currentHP;
    }

    void Start () {

        initHP = GameManager.Instance.gameData.hp;
        currentHP = initHP;
        hpBar.color = initColor;
        CurrentColor = initColor;
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == bulletTag)
        {
            Destroy(other.gameObject);
            StartCoroutine(ShowBloodScreen());

            currentHP -= 5.0f;
            DisplayHpbar();
            if(currentHP < 0.0f)
            {
                PlayerDie();
            }
        }
    }

    private void PlayerDie()
    {

        OnPlayerDie();





        //Debug.Log("Player Die~!!!");

        //태그를 이용하여 모든 Enemy 오브젝트를 추출
        //GameObject[] enemics = GameObject.FindGameObjectsWithTag(enemyTag);

        //모든 EnemyAI스크립트에 있는 PlayerDie함수 호출
        //for(int i=0; i<enemics.Length; i++)
        //{
            //enemics[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
            /*SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
         는 해당 오브젝트안에 있는 모든 스크립트를 검색하여 해당 함수를 호출   
         SendMessageOptions = 해당 함수가 없을시 오류메시지를 반환할 것인가? 안할 것인가? 호출
         */
        //}
    }

    //체력이 50%까지는 점점R값을 증가
    //50%이하는 점점 G값을 하락
    void DisplayHpbar()
    {
        //생명게이지가 50%일때
        if((currentHP/initHP)> 0.5f)
        {
            CurrentColor.r = (1 - (currentHP / initHP)) * 2.0f;             //노랑색으로 하기 위해서 2를 곱함
        }

        //생명게이지가 50%미만일때
        else
        { //최대값~최소값 : 0.5~0
            //체력이 0이 될때 생명게이지를 빨간색으로 만들기 위해 곱하기2
            CurrentColor.g = (currentHP / initHP) * 2.0f;
        }

        hpBar.color = CurrentColor;
        hpBar.fillAmount = (currentHP / initHP);
    }

    //피격시 체력효과
    IEnumerator ShowBloodScreen()
    {
        //0.1초간 이미지가 보인 후...
        bloodScreen.color = new Color(1, 0, 0, UnityEngine.Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(0.1f);

        //다시 이미지를 투명하게 만듬
        bloodScreen.color = Color.clear;
    }
}

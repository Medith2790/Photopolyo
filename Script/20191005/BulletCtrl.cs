using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour {


    public float damage = 10.0f;
    public float speed = 1000.0f;

    private Transform tr;
    private Rigidbody rb;
    private TrailRenderer trail;

    void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
        damage = GameManager.Instance.gameData.damage;

    }
    private void OnEnable()
    {
        rb.AddForce(transform.forward * speed);

        GameManager.OnitemChange += UpdateSetup;
    }

    private void UpdateSetup()
    {
        damage = GameManager.Instance.gameData.damage;
    }

    private void OnDisable()
    {
        trail.Clear();
        tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
        rb.Sleep();                         //정지한다.
    }

    void Start () {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}


//AddForce : 월드좌표
//AddRelayForce : 상대좌표

/*Is Trigger 옵션체크 x*/
// void OnCollisionEnter 두 물체 간의 충돌이 일어나기 시작했을 때
// void OnCollisionStay 두 물체 간의 충돌이 지속될 때
// void OnCollisionExit 두 물체가 서로 떨어졌을 때

/*Is Trigger 옵션체크 o*/
// void OnTriggerEnter 두 물체 간의 충돌이 일어나기 시작했을 때
// void OnTriggerStay 두 물체 간의 충돌이 지속될때
// void OnTriggerExit 두 물체가 서로 떨어졌을 때
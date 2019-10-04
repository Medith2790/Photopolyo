using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//클래스나 구조체를 인스펙터에 표시할때 사용
[System.Serializable]
public struct PlayerSfx
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}


//무기타입
public enum WeaponType
{
    RIFLE = 0,
    ShotGun
}



public class FireCtrl : MonoBehaviour {


    public WeaponType currentWeapon = WeaponType.RIFLE;
    public Image magazineImg;
    public Text magazineText;
    public int maxBullet = 10;
    public int remainigBullet = 10;

    [SerializeField] float reloadTime = 2.0f;
    [SerializeField] bool isReloading = false;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform firePos;
    [SerializeField] PlayerSfx playerSfx;
    [SerializeField] ParticleSystem cartridge;          //총알파편
    [SerializeField] ParticleSystem muzzleFlash;        //총구화염 이펙트
    [SerializeField] AudioSource _audio;

    [SerializeField] Shake shake;


    [Header("Change Weapon")]
    //변경할 무기이미지
    public Sprite[] weaponIcons;
    //교체할 무기 이미지UI
    public Image weaponImg;
    

	void Start () {
        muzzleFlash = GameObject.FindWithTag("MuzzleFlash").GetComponent<ParticleSystem>();
        _audio = GetComponent<AudioSource>();
        shake = GameObject.Find("CameraRig").GetComponent<Shake>();
	}
	
	void Update () {
		
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }


        //마우스 왼쪽 버튼을 클릭했을 때 FIre함수 호출
        if(!isReloading && Input.GetMouseButtonDown(0))
        {
            --remainigBullet;  //감소
            Fire();

            //남은 총알이 없을 경우 재장전 코루틴 호출
            if(remainigBullet==0)
            {
                StartCoroutine(Reloading());
            }
        }
	}

    IEnumerator Reloading()
    {
        isReloading = true;
        _audio.PlayOneShot(playerSfx.reload[(int)currentWeapon], 1.0f);

        yield return new WaitForSeconds(playerSfx.reload[(int)currentWeapon].length + 0.3f);

        isReloading = false;
        magazineImg.fillAmount = 1.0f;
        remainigBullet = maxBullet;
        UpdateBulletText();
    }

    public void OnChangeWapon()
    {
        currentWeapon = (WeaponType)((int)++currentWeapon % 2);
        weaponImg.sprite = weaponIcons[(int)currentWeapon];
    }

    void Fire()
    {
        StartCoroutine(shake.ShakeCamera(0.05f, 0.03f, 0.1f));

        //Instantiate(bullet, firePos.position, firePos.rotation);

        var _bullet = GameManager.Instance.GetBullet();
        if(_bullet != null)
        {
            _bullet.transform.position = firePos.position;
            _bullet.transform.rotation = firePos.rotation;
            _bullet.SetActive(true);
        }
        cartridge.Play();
        muzzleFlash.Play();
        FireSfx();
        magazineImg.fillAmount = (float)remainigBullet / (float)maxBullet;
        UpdateBulletText();
    }

    private void UpdateBulletText()
    {
        magazineText.text = string.Format("<color=#ff0000>{0}</color>{1}", remainigBullet, maxBullet);
    }

    private void FireSfx()
    {
        var _sfx = playerSfx.fire[(int)currentWeapon];
        _audio.PlayOneShot(_sfx, 1.0f);
    }
}


/*
 
Instantiate<GameObject>(bullet, firePos.posion, firePos.rotation);                  //위치와 각도에서 불렛이 나간다.
Instantiate<GameObject>(bullet, firePos.posion, firePos.rotation, null);            //null은 그룹값 없기때문에 비워둔다.
Instantiate<GameObject>(bullet, firePos);                    //firePos위치에서 bullet이 발사
Instantiate<GameObject>(bullet, firePos, false);            //처음 실행할때 발사가 안되며, true를 받을때 firePos위치에서 bullet이 발사

 
*/

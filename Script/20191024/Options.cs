using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Options : MonoBehaviour {


    [Header("GameObject")]
    [SerializeField] GameObject mainP;
    [SerializeField] GameObject secondP;
    [SerializeField] GameObject thirdP;

    [Header("Music")]
    [SerializeField] Slider slider;
    [SerializeField] AudioSource audio;
    private float backVo1 = 1f;


    [Header("Light")]
    [SerializeField] Slider lightSlider;
    [SerializeField] Light light;
    private float backLight = 1f;

    private void Start()
    {
        /*음악 볼륨 값을 얻어옴*/
        backVo1 = PlayerPrefs.GetFloat("backvo1", 1f);
        slider.value = backVo1;
        audio.volume = slider.value;


        /*밝기 조절 값을 얻어옴*/
        backLight = PlayerPrefs.GetFloat("backLight", 1f);
        lightSlider.value = backLight;
        light.intensity = lightSlider.value;
       
    }


    public void OptionBtnOn()
    {
        mainP.SetActive(false);
        secondP.SetActive(true);
        thirdP.SetActive(false);
    }

    public void CommonBtnOff()
    {
        mainP.SetActive(true);
        secondP.SetActive(false);
        thirdP.SetActive(false);
    }

    public void CreditBtnOn()
    {
        mainP.SetActive(false);
        secondP.SetActive(false);
        thirdP.SetActive(true);
    }


    public void MusicBolume()
    {
        //값을 저장
        audio.volume = slider.value;
        backVo1 = slider.value;
        PlayerPrefs.SetFloat("backvo1", backVo1);

    }


    public void LightDirection()
    {
        //값을 저장
        light.intensity = lightSlider.value;
        backLight = lightSlider.value;
        PlayerPrefs.SetFloat("backLight", backLight);
    }
}

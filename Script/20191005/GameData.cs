using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DataInfo
{
    [System.Serializable]
    public class GameData
    {
        public int killCount = 0;
        public float hp = 100.0f;
        public float damage = 5.0f;
        public float Speed = 5.0f;
        public List<Item> equipItem = new List<Item>();
    }


    [System.Serializable]
    public class Item
    {
        //아이템 종류선언
        public enum ItemType { HP, SPEED, GRENADE, DAMAGE }
        
        //계산방식 선언
        public enum ItemCalc { INC_VALUE, PERCENT}

        public ItemType itemType;                       //아이템 종류
        public ItemCalc itemCalc;                       //아이템 적용시 계산방식
        public string name;                             //아이템 이름
        public string desc;                             //아이템 소개
        public float value;                             //계산 값

    }

}

/*
 * [System.Serializable]
 * 객체 직렬화란 객체를 통신이나 파일 또는 메모리에 저장하기 위해 일련의 바이트로 변환하는 것
 * 메모리 상에 흩어져 저장된 데이터를 일렬로 재 배열해서 전송하기에 용이한 데이터로 변경
 */
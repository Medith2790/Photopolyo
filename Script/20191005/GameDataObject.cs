using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataInfo;     //아이템 클래스에 접근하기 위해 명시한 네임스페이스


[CreateAssetMenu (fileName ="GameDataS0", menuName ="Create GameData", order = 1)]

public class GameDataObject : ScriptableObject {


    public int killCount = 0;
    public float hp = 120.0f;
    public float damage = 2.5f;
    public float Speed = 5.0f;
    public List<Item> equipItem = new List<Item>();


}

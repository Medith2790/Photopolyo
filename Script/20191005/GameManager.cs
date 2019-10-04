using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataInfo;

public class GameManager : MonoBehaviour {

    public static GameManager Instance = null;

    public CanvasGroup inventoryCG;

    //DataManager를 저장할 변수
    private DataManager dataManager;
    public GameDataObject gameData;             
    //public GameData gameData;

   //인벤토리의 아이템이 변경됐을 때 발생시킬 이벤트 정의
    public delegate void itemChangeDelegete();
    public static event itemChangeDelegete OnitemChange;

    [Header("Enemy Create Info")]
    //적캐릭터가 출현할 위치를 담을 배열
    public Transform[] points;

    //적 캐릭터 프리팹을 저장할 변수
    public GameObject enemy;

    //적 캐릭터를 생성할 주기
    public float createTime = 2.0f;

    //적캐릭터의 최대 생성개수
    public int maxEnemy = 10;

    //게임 종료 여부를 판단할 변수
    public bool isGameOver = false;

    [Header("Object Pool")]
    public GameObject bulletPrefab;
    public int maxPool = 10;                                    //10개 저장
    public List<GameObject> bulletPool = new List<GameObject>();


    [Header("Pause")]
    [SerializeField] bool isPaused;

    [Header("PlayerPrefs")]
    [HideInInspector] public int killCount; //PlayerPrefs를 활용한 데이터 저장
    public Text killCountTxt;

    [Header("SlotList")]
    private GameObject slotList;       //SlotList게임오브젝트를 저장할 변수
    public GameObject[] itemObjects;    //ItemList 하위에 있는 네개의 아이템을 저장할 배열



    private void Awake()
    {
        //싱글턴
        if(Instance== null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        //DataManager를 추출
        dataManager = GetComponent<DataManager>();

        //DataManager초기화
        dataManager.Initialize();

        //인벤토리에 추가된 아이템을 검색하기 위해 SlotList게임 오브젝트를 추출
        slotList = inventoryCG.transform.Find("SlotList").gameObject;

        //게임의 초기 데이터 로드
        LoadGameData();

        //총알 생성
        CreatePooling();
    }




    void Start () {
        OnInventoryOpen(false);

        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        if(points.Length > 0)
        {
            StartCoroutine(this.CreateEnemy());
        }
	}

    //앱이 종료되는 시점에 호출되는 이벤트 함수
    private void OnApplicationQuit()
    {
        //게임 종료전 게임 데이터를 저장한다.
        SaveGameData();
    }

    public void AddItem(Item item)
    {
        //보유 아이템에에 같은 아이템이 있으면 추가하지 않고 빠져나감
        if (gameData.equipItem.Contains(item)) return;

        //아이템을 GameData.equipItem배열에 추가
        gameData.equipItem.Add(item);

        //아이템의 종류에 따라 분기처리
        switch(item.itemType)
        {
            case Item.ItemType.HP:
                //아이템의 계산 방식에 따라 연산처리
                if (item.itemCalc == Item.ItemCalc.INC_VALUE)
                    gameData.hp += item.value;
                else
                    gameData.hp += gameData.hp * item.value;
                break;

            case Item.ItemType.DAMAGE:
                if (item.itemCalc == Item.ItemCalc.INC_VALUE)
                    gameData.damage += item.value;
                else
                    gameData.damage += gameData.damage * item.value;
                break;

            case Item.ItemType.SPEED:
                if (item.itemCalc == Item.ItemCalc.INC_VALUE)
                    gameData.Speed += item.value;
                else
                    gameData.Speed += gameData.Speed * item.value;
                break;

            case Item.ItemType.GRENADE:
                break;

        }

        //아이템이 변경된 것을 실시간으로 반영하기 위한 이벤트발생
        OnitemChange();

        UnityEditor.EditorUtility.SetDirty(gameData);

    }

    //이벤토리에서 아이템을 제거했을 때 데이터를 갱신
    public void RemoveItem(Item item)
    {
        gameData.equipItem.Remove(item);

        //아이템의 종류에 따라 분기처리
        switch (item.itemType)
        {
            case Item.ItemType.HP:
                //아이템의 계산 방식에 따라 연산처리
                if (item.itemCalc == Item.ItemCalc.INC_VALUE)
                    gameData.hp -= item.value;
                else
                    gameData.hp = gameData.hp /(1.0f + item.value);
                break;

            case Item.ItemType.DAMAGE:
                if (item.itemCalc == Item.ItemCalc.INC_VALUE)
                    gameData.damage -= item.value;
                else
                    gameData.damage = gameData.damage /(1.0f + item.value);
                break;

            case Item.ItemType.SPEED:
                if (item.itemCalc == Item.ItemCalc.INC_VALUE)
                    gameData.Speed -= item.value;
                else
                    gameData.Speed = gameData.Speed /(1.0f + item.value);
                break;

            case Item.ItemType.GRENADE:
                break;

        }
        //.asset파일에 데이터 저장
        UnityEditor.EditorUtility.SetDirty(gameData);

        //아이템이 변경된것을 실시간으로 반영하기 위한 이벤트 발생
        OnitemChange();
    }


    void LoadGameData()
    {
        //ScriptableObject는 전역적으로 접근이 가능해서 별도로 로드하는 과정이 필요 없다.


        //DataManager를 통해 파일에 저장된 데이터 불러오기
        //GameData data = dataManager.Load();

        //gameData.hp = data.hp;
        //gameData.damage = data.damage;
        //gameData.speed = data.Speed;
        //gameData.killCount = data.killCount;
        //gameData.equipItem = data.equipItem;

        //보유한 아이템이 있을때만 호출
        if (gameData.equipItem.Count > 0)
        {
            InventorySetup();
        }


        //KILL_COUNT키로 저장된 값을 로드
        //killCount = PlayerPrefs.GetInt("KILL_COUNT", 0);
        killCountTxt.text = "KILL" + gameData.killCount.ToString("0000");
    }

    private void InventorySetup()
    {
        //SlotList 하위에 있는 모든 Slot을 추출
        var slots = slotList.GetComponentsInChildren<Transform>();

        //보유하고있는 아이템의 개수만큼 반복
        for (int ItemRepeat= 0; ItemRepeat < gameData.equipItem.Count; ItemRepeat++)
        {

            //인벤토리 UI에 있는 Slot개수만큼 반복
            for(int SlotRepeat=1; SlotRepeat < slots.Length; SlotRepeat++)
            {
                //Slot하위에 다른 아이템이 있으면 다음 인덱스로 넘어감
                if (slots[SlotRepeat].childCount > 0) continue;

                //보유한 아이템의 종류에 따라서 인덱스를 추출(itemType에있는 enum의 0,1,2,3번호를 추출)
                int itemIndex = (int)gameData.equipItem[ItemRepeat].itemType;

                //아이템의 부모를 Slot 오브젝트로 변경
                itemObjects[itemIndex].GetComponent<Transform>().SetParent(slots[SlotRepeat]);

                //아이템의 ItemInfo 클래스의 itemData에 로드한 데이터 값을 저장
                itemObjects[itemIndex].GetComponent<ItemInfo>().itemData = gameData.equipItem[ItemRepeat];

                //아이템을 Slot에 추가하면 바깥 for구문으로 빠져나감.
                break;
            }
        }
    }

    void SaveGameData()
    {
        //dataManager.Save(gameData);
        
        //.asset 파일에 데이터 저장
        UnityEditor.EditorUtility.SetDirty(gameData);
    }

    public void IncKillCount()
    {
        ++gameData.killCount;
        killCountTxt.text= "KILL" + gameData.killCount.ToString("0000");
        //PlayerPrefs.SetInt("KILL_COUNT", killCount);
    }

    IEnumerator CreateEnemy()
    {
        while(!isGameOver)
        {
            int enemyCount = (int)GameObject.FindGameObjectsWithTag("ENEMY").Length;
            if (enemyCount < maxEnemy)
            {
                yield return new WaitForSeconds(createTime);

                int idx = Random.Range(1, points.Length);
                Instantiate(enemy, points[idx].position, points[idx].rotation);
            }
            else yield return null;
        }

    }

    public GameObject GetBullet()
    {
        for(int i=0; i<bulletPool.Count; i++)
        {
            if(bulletPool[i].activeSelf == false)
            {
                return bulletPool[i];
            }
        }
        return null;
    }

    public void CreatePooling()
    {
        GameObject objectPools = new GameObject("ObjectPools");
        for(int i = 0; i<maxPool; i++)
        {
            var obj = Instantiate<GameObject>(bulletPrefab, objectPools.transform);
            obj.name = "Bullet_" + i.ToString("00");                    //불렛을 2자리수로 표현
            obj.SetActive(false);
            bulletPool.Add(obj);
        }
    }

    /*인벤토리 열고 닫기버튼 호출*/
    public void OnInventoryOpen(bool isOpened)
    {
        inventoryCG.alpha = (isOpened) ? 1.0f : 0.0f;

        inventoryCG.interactable = isOpened;
        inventoryCG.blocksRaycasts = isOpened;
    }

    public void OnPauseClick()
    {
        //일시 정지 값을 토글시킴
        isPaused = !isPaused;
        Time.timeScale = (isPaused) ? 0.0f : 1.0f;

        //주인공객체를 추출
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        var scripts = playerObj.GetComponents<MonoBehaviour>();
        foreach(var script in scripts)
        {
            script.enabled = !isPaused;
        }

        //weapon버튼 정지
        var canvasGroup = GameObject.Find("Panel Weapon").GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isPaused;
    }
}


//timeScale은 속도를 조정
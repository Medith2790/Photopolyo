using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler

{

    private Transform itemTr;
    private Transform inventoryTr;
    private Transform itemListTr;
    private CanvasGroup canvasGroup;


    public static GameObject draggingitem = null;

    /*드래그 시작중*/
    public void OnBeginDrag(PointerEventData eventData)
    {
        this.transform.SetParent(inventoryTr);
        draggingitem = this.gameObject;

        canvasGroup.blocksRaycasts = false;
    }

    /*드래그 시작*/
    public void OnDrag(PointerEventData eventData)
    {
        itemTr.position = Input.mousePosition;
    }

    /*드래그 종료*/
    public void OnEndDrag(PointerEventData eventData)
    {
        draggingitem = null;


        canvasGroup.blocksRaycasts = true;
        if(itemTr.parent == inventoryTr)
        {
            itemTr.SetParent(itemListTr.transform);

            //슬롯에 추가된 아이템의 갱신을 알림
            GameManager.Instance.RemoveItem(GetComponent<ItemInfo>().itemData);
        }
    }

    // Use this for initialization
    void Start () {

        itemTr = GetComponent<Transform>();
        inventoryTr = GameObject.Find("Inventory").GetComponent<Transform>();
        itemListTr = GameObject.Find("ItemList").GetComponent<Transform>();
        canvasGroup = GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

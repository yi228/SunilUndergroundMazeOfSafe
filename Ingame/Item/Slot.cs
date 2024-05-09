using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
public class Slot : MonoBehaviour
{
    public PlayerController Owner;
    public Items item; //획득한 아이템
    public int itemCount; //획득한 아이템 개수
    public Image itemImage; //아이템의 이미지
    [SerializeField]
    private TextMeshProUGUI text_Count;
    [SerializeField]
    private GameObject go_CountImage;
    public GameObject ItemDetail;
    private Image DetailImage;
    private TextMeshProUGUI DetailName,DetailExplaination;
    public Button useButton;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ClickSlot);
        Owner = GameObject.Find("Player").GetComponent<PlayerController>();
        ItemDetail.SetActive(false);
    }
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }
    //아이템 획득
    public void AddItem(Items _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;
        if (item.itemType != Items.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
            //SetSlotCount(itemCount);
        }
        else
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }
        SetColor(1);
    }
    //아이템 개수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();
        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }
    //슬롯 초기화
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);
        text_Count.text = "0";
        go_CountImage.SetActive(false);

    }

    public void ClickSlot()
    {

        if (item != null)
        {
            ItemDetail.SetActive(true);
            DetailImage = GameObject.Find("DetailImage").GetComponent<Image>();
            DetailName = GameObject.Find("DetailName").GetComponent<TextMeshProUGUI>();
            DetailExplaination = GameObject.Find("DetailExplaination").GetComponent<TextMeshProUGUI>();
            useButton = GameObject.Find("UseButton").GetComponent<Button>();
            DetailImage.sprite = item.itemImage;
            DetailName.text = item.itemName;
            DetailExplaination.text = item.itemDetail;
            useButton.onClick.RemoveAllListeners();
            useButton.onClick.AddListener(ClickUse);
        }
    }
    public void ClickUse()
    {
        if (item.itemType == Items.ItemType.Equipment) //장착 타입
        {

        }
        else //소모타입
        {
            UseItem(item.itemName);
            SetSlotCount(-1);
        }
        Managers.Sound.Play("Effect/Item/UseItem");
        ItemDetail.SetActive(false);
    }
    public void UseItem(string itemName)
    {
        switch (itemName)
        {
            case "AttackUpgrade":
                Owner.Stat.attack++;
                break;
            case "Double Bullet":
                Owner.isDoubleBullet = true;
                break;
            case "HpMax":
                Owner.Stat.hp=Owner.Stat.MaxHp;
                break;
            case "HpPotion":
                Owner.Stat.hp++;
                break;
            case "SpeedUpgrade":
                Owner.Stat.MoveSpeed++;
                break;
            case "DefenseUpgrade":
                Owner.Stat.Defense++;
                break;

        }
    }
}

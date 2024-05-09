using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Item")]
public class Items : ScriptableObject
{
    public string itemName; //아이템 이름
    public ItemType itemType; //아이템 유형
    public Sprite itemImage; //아이템 이미지
    public GameObject itemPrefab; //아이템의 프리팹
    public string itemDetail;//아이템 설명
    public string weaponType; //무기 유형

    public enum ItemType
    {
        Used,
        Equipment,
        ETC
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Item")]
public class Items : ScriptableObject
{
    public string itemName; //������ �̸�
    public ItemType itemType; //������ ����
    public Sprite itemImage; //������ �̹���
    public GameObject itemPrefab; //�������� ������
    public string itemDetail;//������ ����
    public string weaponType; //���� ����

    public enum ItemType
    {
        Used,
        Equipment,
        ETC
    }
    
}

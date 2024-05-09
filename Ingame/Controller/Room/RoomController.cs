using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public Define.RoomType RoomType;
    public Vector3 StartPoint, MiddlePoint;
    public List<Transform> MonsterPoints = new List<Transform>();
    public List<RoomController> ConnectedRooms = new List<RoomController>();
    public List<Define.DoorDir> DoorDirs = new List<Define.DoorDir>();
    public List<DoorController> Doors = new List<DoorController>();
    public Dictionary<int, MonsterController> Monsters = new Dictionary<int, MonsterController>();
    int monsterSpawnPercent = 50;

    private bool _isClear = false;
    private bool _isStart = false;
    public bool IsClear { get { return _isClear; } set { _isClear = value; } }
    public bool IsStart { get { return _isStart; } set { _isStart = value; } }
    public virtual void Init()
    {
        InitStartPoint();
        InitMiddlePoint();
        InitMonsterPoints();
    }
    protected virtual void InitStartPoint()
    {
        GameObject startPoint = GameObject.Find("StartPoint");
        if (startPoint != null)
            StartPoint = startPoint.transform.position;
    }
    protected virtual void InitMiddlePoint()
    {
        Transform middlePoint = transform.Find("MiddlePoint");
        if (middlePoint != null)
            MiddlePoint = middlePoint.transform.position;
    }
    protected virtual void InitMonsterPoints()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "MonsterPoint")
            {
                MonsterPoints.Add(child);
            }
        }
        foreach (Transform child in transform)
        {
            if (child.tag == "MonsterPoint")
            {
                child.gameObject.SetActive(false);
            }
        }
        InitMonster();
    }
    protected virtual void InitMonster()
    {
        int count = 0;
        foreach (Transform monster in MonsterPoints)
        {
            int rand = UnityEngine.Random.Range(1, 101);
            if (rand > monsterSpawnPercent)
            {
                // TODO - Default Rush
                Managers.Spawn.SpawnMonster(this, MonsterPoints[count++].transform.position, Define.MonsterType.Rush);
            }
        }
        // TODO - ���� ������ �������� ��ü
        if (count == 0 && RoomType == Define.RoomType.Common && this != Managers.Room.StartRoom)
        {
            InitShop();
        }
    }
    // TODO
    public void InitShop()
    {
        // ������ ����
        List<GameObject> items = new List<GameObject> ();
        items.Add(Managers.Resource.Instantiate("Items/AttackUpgrade"));
        items.Add(Managers.Resource.Instantiate("Items/DefenseUpgrade"));
        items.Add(Managers.Resource.Instantiate("Items/Double Bullet"));
        items.Add(Managers.Resource.Instantiate("Items/HpPotion"));
        items.Add(Managers.Resource.Instantiate("Items/SpeedUpgrade"));
        foreach (GameObject item in items)
            item.transform.SetParent(this.transform);
        // ������ ��ġ
        Transform[] children = GetComponentsInChildren<Transform>();
        List<Transform> spawnPoints = new List<Transform>();
        foreach(Transform child in children)
        {
            if (child.tag == "ItemPoint")
                spawnPoints.Add(child);
        }
        for (int i = 0; i < items.Count; i++)
            items[i].transform.position = spawnPoints[i].transform.position;

        Debug.Log($"���� ��ȯ {name}");
    }
    public virtual void AddTestDoors(int y, int x)
    {
        // �� �Ʒ� ���� ������
        int[] dy = { 1, -1, 0, 0 };
        int[] dx = { 0, 0, -1, 1 };
        for (int dir = 0; dir < 4; dir++)
        {
            int ny = y + dy[dir];
            int nx = x + dx[dir];
            // ����
            if (!(0 <= ny && ny < Managers.Room.Size && 0 <= nx && nx < Managers.Room.Size)) continue;
            // ���� �ֳ�
            if (Managers.Room.Tiles[ny, nx].Tile == Define.TileType.Wall) continue;
            ConnectedRooms.Add(Managers.Room.Tiles[ny, nx].Room);
            DoorDirs.Add((Define.DoorDir)dir);
        }
    }
    public virtual void StageClear()
    {
        Debug.Log($"{name} �������� Ŭ����");
        IsClear = true;
    }
    public virtual void CopyFrom(RoomController source)
    {
        RoomType = source.RoomType;
        StartPoint = source.StartPoint;
        // ����� ���� �̸����� ã���ֱ�
        foreach (RoomController room in source.ConnectedRooms)
        {
            RoomController r = GameObject.Find("@" + room.name).GetComponent<RoomController>();
            ConnectedRooms.Add(r);
        }
        DoorDirs = source.DoorDirs;
        IsClear = source.IsClear;
        IsStart = source.IsStart;
    }
}

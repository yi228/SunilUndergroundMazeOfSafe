using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager
{
    public class TileInfo
    {
        public Define.TileType Tile;
        public RoomController Room;
    }
    public List<RoomController> TestRooms = new List<RoomController>();
    public List<RoomController> Rooms = new List<RoomController>();
    private MapGenerator Map = new MapGenerator();
    // SideWinder�� ����� ������ Room �׽�Ʈ ���� ����
    // �̴ϸʿ� �̿��ϱ�
    GameObject _rootTestRoom;
    // ���� ������ Room���� ������ Map
    GameObject _rootMap;
    #region Map Components
    public TileInfo[,] Tiles;
    public int Size { get; private set; }

    public int DestY { get; private set; }
    public int DestX { get; private set; }
    public int StartY;
    public int StartX;
    #endregion
    // ������
    public RoomController BossRoom;
    public RoomController StartRoom;
    public PlayerController Player;
    public int StageLevel = 1;
    // �Ϲ� �� ����(��, ��)
    public int CommonStageCount = 7;
    // �Ϲ� �� ������ ��
    public int MaxCommonRooms = 4;
    public void Init()
    {
        Debug.Log("Current stage level: " + StageLevel.ToString());
        Clear();
        // ���� ���� �����͸� ������� �� ����
        InitRoom();
        InitPlayer();
        SetNavMesh.instance.Build();
    }
    // 1.SideWinder�� ���� ������ Tilemap�� �ð�ȭ
    void InitRoom()
    {
        _rootTestRoom = new GameObject { name = "@RootTestRoom" };
        Map.Initialize(CommonStageCount);
        // CS0052 ������ ���� �Ű��ֱ�
        Tiles = new TileInfo[CommonStageCount, CommonStageCount];
        Size = Map.Size;
        DestY = Map.DestY;
        DestX = Map.DestX;
        StartY = Map.StartY;
        StartX = Map.StartX;
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("TestRoom");
        // ������ �ִ� Tilemap ����
        foreach (GameObject tile in tiles)
            Managers.Resource.Destroy(tile);
        // ������ Tilemap �ð�ȭ
        int cnt = 1;
        for (int y = 0; y < Map.Size; y++)
        {
            for (int x = 0; x < Map.Size; x++)
            {
                GameObject go = Managers.Resource.Instantiate($"Room/{Map.Tiles[y, x]}", new Vector3(x, y, 0), _rootTestRoom.transform);
                Tiles[y, x] = new TileInfo();
                Tiles[y, x].Tile = Map.Tiles[y, x];
                // ���� �濡 �߰� ���ϱ�
                if (Tiles[y, x].Tile != Define.TileType.Wall)
                {
                    go.name = $"Room_{cnt++}";
                    Tiles[y, x].Room = go.GetComponent<RoomController>();
                    if (Tiles[y, x].Tile == Define.TileType.Start)
                    {
                        StartRoom = Tiles[y, x].Room;
                        StartRoom.RoomType = Define.RoomType.Start;
                    }
                    else if (Tiles[y, x].Tile == Define.TileType.Boss)
                    {
                        BossRoom = Tiles[y, x].Room;
                        BossRoom.RoomType = Define.RoomType.Boss;
                    }
                    else
                        Tiles[y, x].Room.RoomType = Define.RoomType.Common;
                }
            }
        }
        // �ʿ� �����ؾ��ϴ� ���� ����� ����� Room ���� �Է�
        for (int y = 0; y < Map.Size; y++)
        {
            for (int x = 0; x < Map.Size; x++)
            {
                if (Tiles[y, x].Tile != Define.TileType.Wall)
                {
                    Tiles[y, x].Room.AddTestDoors(y, x);
                    TestRooms.Add(Tiles[y, x].Room);
                }
            }
        }
        InitMap();
    }
    // 2.�ð�ȭ�� Ÿ�ϵ��� Room���� ��ü
    void InitMap()
    {
        _rootMap = new GameObject { name = "@RootMap" };
        // �ʵ��� ���� �� �Ÿ�
        int intervalX = 30;
        int intervalY = 15;
        for (int i = 0; i < TestRooms.Count; i++)
        {
            RoomController room = TestRooms[i];
            GameObject roomTemplate = null;
            if (room.RoomType == Define.RoomType.Common)
            {
                int rand = Random.Range(1, MaxCommonRooms + 1);
                roomTemplate =
                    Managers.Resource.Instantiate
                    ($"Room/Level {StageLevel}/{room.RoomType}Room_Template_{rand}",
                    new Vector3(room.transform.position.x * intervalX, room.transform.position.y * intervalY, 0), _rootMap.transform);
            }
            else
            {
                roomTemplate =
                    Managers.Resource.Instantiate
                    ($"Room/Level {StageLevel}/{room.RoomType}Room_Template",
                    new Vector3(room.transform.position.x * intervalX, room.transform.position.y * intervalY, 0), _rootMap.transform);
            }
            // �̸����� ã�� ���� �̸� �����ֱ�
            roomTemplate.name = "@" + TestRooms[i].name;
            RoomController rc = roomTemplate.GetComponent<RoomController>();
            // �ϼ��� Room ���� Add
            Rooms.Add(rc);
        }
        for(int i = 0; i < Rooms.Count; i++)
        {
            // ������Ÿ�� Room ���� ����
            Rooms[i].CopyFrom(TestRooms[i]);
        }
        _rootTestRoom.SetActive(false);
        SpawnDoor();
    }
    // 3.Room�� �ʿ��� ��� ��ġ ex) �ٸ������� ���ϴ� �� ���
    void SpawnDoor()
    {
        // 1.�� ��ȯ
        foreach (RoomController room in Rooms)
        {
            for (int i = 0; i < room.DoorDirs.Count; i++)
            {
                string name = room.DoorDirs[i].ToString();
                Vector3 pos = room.transform.Find(name).position;
                // �׸��� ���� ���� ��ȯ - TODO
                DoorController door = Managers.Resource.Instantiate("Room/Door", pos, room.transform).GetComponent<DoorController>();
                room.Doors.Add(door);
            }
        }
        // 2. �� ����
        foreach (RoomController room in Rooms)
        {
            for (int i = 0; i < room.DoorDirs.Count; i++)
            {
                DoorController door = room.Doors[i];
                // door�� ���� �Է�
                door.ConnectedRoom = room.ConnectedRooms[i];
                Transform t = door.ConnectedRoom.transform;
                door.Dir = room.DoorDirs[i];
            }
            foreach (DoorController door in room.Doors)
                door.Init();
            foreach (DoorController door in room.Doors)
                door.ConnectDoor();
            if (room.RoomType == Define.RoomType.Start)
            {
                StartRoom = room;
                StartRoom.Init();
            }
            else if (room.RoomType == Define.RoomType.Boss)
            {
                BossRoom = room;
            }
        }
    }
    void InitPlayer()
    {
        // ���ȯ ����
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) 
            Managers.Resource.Destroy(player);
        GameObject playerUI = GameObject.Find("UI_Player");
        if (playerUI != null)
            Managers.Resource.Destroy(playerUI);
        // �÷��̾� ��ȯ
        Player = Managers.Resource.Instantiate("Creature/Player", StartRoom.StartPoint).GetComponent<PlayerController>();
        Managers.Room.Player = Player;
        // �÷��̾� ����� ��ŸƮ������ ����
        Player.Room = StartRoom;
        Player.InitPlayerUI();
    }
    public void Clear()
    {
        Rooms.Clear();
        TestRooms.Clear();
        Map = new MapGenerator();
        _rootTestRoom = null;
        _rootMap = null;
        Tiles = null;
        Player = null;
    }
}

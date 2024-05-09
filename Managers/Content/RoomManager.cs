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
    // SideWinder를 사용해 생성한 Room 테스트 도안 폴더
    // 미니맵에 이용하기
    GameObject _rootTestRoom;
    // 실제 생성된 Room으로 구성된 Map
    GameObject _rootMap;
    #region Map Components
    public TileInfo[,] Tiles;
    public int Size { get; private set; }

    public int DestY { get; private set; }
    public int DestX { get; private set; }
    public int StartY;
    public int StartX;
    #endregion
    // 보스방
    public RoomController BossRoom;
    public RoomController StartRoom;
    public PlayerController Player;
    public int StageLevel = 1;
    // 일반 방 개수(행, 렬)
    public int CommonStageCount = 7;
    // 일반 방 종류의 수
    public int MaxCommonRooms = 4;
    public void Init()
    {
        Debug.Log("Current stage level: " + StageLevel.ToString());
        Clear();
        // 랜덤 생성 데이터를 기반으로 방 생성
        InitRoom();
        InitPlayer();
        SetNavMesh.instance.Build();
    }
    // 1.SideWinder를 통해 생성한 Tilemap의 시각화
    void InitRoom()
    {
        _rootTestRoom = new GameObject { name = "@RootTestRoom" };
        Map.Initialize(CommonStageCount);
        // CS0052 문제로 직접 옮겨주기
        Tiles = new TileInfo[CommonStageCount, CommonStageCount];
        Size = Map.Size;
        DestY = Map.DestY;
        DestX = Map.DestX;
        StartY = Map.StartY;
        StartX = Map.StartX;
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("TestRoom");
        // 기존에 있던 Tilemap 삭제
        foreach (GameObject tile in tiles)
            Managers.Resource.Destroy(tile);
        // 생성된 Tilemap 시각화
        int cnt = 1;
        for (int y = 0; y < Map.Size; y++)
        {
            for (int x = 0; x < Map.Size; x++)
            {
                GameObject go = Managers.Resource.Instantiate($"Room/{Map.Tiles[y, x]}", new Vector3(x, y, 0), _rootTestRoom.transform);
                Tiles[y, x] = new TileInfo();
                Tiles[y, x].Tile = Map.Tiles[y, x];
                // 벽은 방에 추가 안하기
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
        // 맵에 존재해야하는 문의 방향과 연결된 Room 정보 입력
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
    // 2.시각화된 타일들을 Room으로 교체
    void InitMap()
    {
        _rootMap = new GameObject { name = "@RootMap" };
        // 맵들의 끝과 끝 거리
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
            // 이름으로 찾기 위해 이름 맞춰주기
            roomTemplate.name = "@" + TestRooms[i].name;
            RoomController rc = roomTemplate.GetComponent<RoomController>();
            // 완성된 Room 정보 Add
            Rooms.Add(rc);
        }
        for(int i = 0; i < Rooms.Count; i++)
        {
            // 프로포타입 Room 정보 복사
            Rooms[i].CopyFrom(TestRooms[i]);
        }
        _rootTestRoom.SetActive(false);
        SpawnDoor();
    }
    // 3.Room에 필요한 요소 배치 ex) 다른방으로 통하는 문 등등
    void SpawnDoor()
    {
        // 1.문 소환
        foreach (RoomController room in Rooms)
        {
            for (int i = 0; i < room.DoorDirs.Count; i++)
            {
                string name = room.DoorDirs[i].ToString();
                Vector3 pos = room.transform.Find(name).position;
                // 그림에 따라 각도 변환 - TODO
                DoorController door = Managers.Resource.Instantiate("Room/Door", pos, room.transform).GetComponent<DoorController>();
                room.Doors.Add(door);
            }
        }
        // 2. 문 정보
        foreach (RoomController room in Rooms)
        {
            for (int i = 0; i < room.DoorDirs.Count; i++)
            {
                DoorController door = room.Doors[i];
                // door에 정보 입력
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
        // 재소환 방지
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) 
            Managers.Resource.Destroy(player);
        GameObject playerUI = GameObject.Find("UI_Player");
        if (playerUI != null)
            Managers.Resource.Destroy(playerUI);
        // 플레이어 소환
        Player = Managers.Resource.Instantiate("Creature/Player", StartRoom.StartPoint).GetComponent<PlayerController>();
        Managers.Room.Player = Player;
        // 플레이어 현재방 스타트방으로 설정
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

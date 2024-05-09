using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DoorController : MonoBehaviour
{
    public RoomController ParentRoom;
    public RoomController ConnectedRoom;
    // 연결된 문을 통과하면 나오는 반대편 문 좌표
    public Vector3 ConnectedPos;
    Define.DoorDir ConnectedDoorDir;
    // Room 기준 문이 가진 방향
    public Define.DoorDir Dir;
    private bool _doorActivated = false;
    void Update()
    {
        if (Managers.Game.puzzleClear && _doorActivated)
        {
            Managers.Game.puzzleClear = false;
            MoveNextRoom();
        }
    }
    void MoveNextRoom()
    {
        Vector3 pos = Vector3.zero;
        switch (ConnectedDoorDir)
        {
            case Define.DoorDir.Top:
                pos = Vector3.down;
                break;
            case Define.DoorDir.Bottom:
                pos = Vector3.up;
                break;
            case Define.DoorDir.Left:
                pos = Vector3.right;
                break;
            case Define.DoorDir.Right:
                pos = Vector3.left;
                break;
        }
        Managers.Room.Player.transform.position = ConnectedPos + pos;
        Managers.Room.Player.Room = ConnectedRoom;
        Debug.Log(Managers.Room.Player.Room);
        StartCoroutine(CoMoveNextRoom());
    }
    private IEnumerator CoMoveNextRoom()
    {
        PostProcessProfile _pp = Managers.Game.postProcess;
        ChromaticAberration _chroma;
        Grain _grain;
        _pp.TryGetSettings<ChromaticAberration>(out _chroma);
        _pp.TryGetSettings<Grain>(out _grain);
        while (_chroma.intensity.value > 0)
        {
            _chroma.intensity.value -= 0.01f;
            _grain.intensity.value -= 0.01f;
            yield return new WaitForSeconds(0.007f);
        }
    }
    public void Init()
    {
        RoomController room = transform.parent.GetComponent<RoomController>();
        ParentRoom = room;
        // 문 각도 변환 및 연결된 문 위치 추정
        switch (Dir)
        {
            case Define.DoorDir.Top:
                ConnectedDoorDir = Define.DoorDir.Bottom;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Define.DoorDir.Bottom:
                ConnectedDoorDir = Define.DoorDir.Top;
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case Define.DoorDir.Left:
                ConnectedDoorDir = Define.DoorDir.Right;
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case Define.DoorDir.Right:
                ConnectedDoorDir = Define.DoorDir.Left;
                transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
        }

    }
    public void ConnectDoor()
    {
        string name = ConnectedDoorDir.ToString();
        ConnectedPos = ConnectedRoom.transform.Find(name).transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && (ParentRoom.IsClear == true || ParentRoom.Monsters.Count == 0))
        {
            _doorActivated = true;
            StartCoroutine(CoEnterDoor());  
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            _doorActivated = false;
    }
    private IEnumerator CoEnterDoor()
    {
        PostProcessProfile _pp = Managers.Game.postProcess;
        ChromaticAberration _chroma;
        Grain _grain;
        _pp.TryGetSettings<ChromaticAberration>(out _chroma);
        _pp.TryGetSettings<Grain>(out _grain);
        _chroma.intensity.value = 0f;
        while (_chroma.intensity.value < 1)
        {
            _chroma.intensity.value += 0.01f;
            _grain.intensity.value += 0.01f;
            yield return new WaitForSeconds(0.007f);
        }
        Managers.Game.SetPuzzle();
        // 퍼즐 실행할거면 밑에 MoveNextRoom 주석처리해야 함
        //MoveNextRoom();
    }
}
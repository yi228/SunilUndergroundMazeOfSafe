using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BossRoomController : RoomController
{
    public MonsterController Boss;
    void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        RoomType = Define.RoomType.Boss;
        InitBossMonster();
    }
    void InitBossMonster()
    {
        Define.MonsterType _monsterType;
        switch (Managers.Room.StageLevel)
        {
            case 1:
                _monsterType = Define.MonsterType.Rush;
                break;
            case 2:
                _monsterType = Define.MonsterType.Fire;
                break;
            case 3:
                _monsterType = Define.MonsterType.Boss;
                break;
            default:
                _monsterType = Define.MonsterType.Rush;
                break;
        }
        Boss = Managers.Spawn.SpawnBossMonster(this, MiddlePoint, _monsterType);
    }
    public override void StageClear()
    {
        base.StageClear();
    }
}

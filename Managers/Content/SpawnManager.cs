using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager
{
    Dictionary<int, MonsterController> _monsters = new Dictionary<int, MonsterController>();
    GameObject _rushMonster;
    GameObject _fireMonster;
    GameObject _bossMonster;
    // �Ϲ� ���� ������ ��
    public int MaxCommonMonsters = 3;

    int Id = 0;
    public void Init()
    {
        //_rushMonster = Managers.Resource.Load<GameObject>("Creature/Monster/Monster_1000");
        // TODO
        //fireMonster = Managers.Resource.Load<GameObject>("Creature/Monster_1000");
        //bossMonster = Managers.Resource.Load<GameObject>("Creature/Monster_1000");
    }
    // TODO - ���� ���� ���� �� ���� �ٲ��ߵ�
    public void SpawnMonster(RoomController room, Vector3 pos, Define.MonsterType type = Define.MonsterType.Rush)
    {
        Id++;
        int rand = Random.Range(1000, 1000 + MaxCommonMonsters);
        MonsterController monster = Managers.Resource.Instantiate($"Creature/Monster/Monster_{rand}").GetComponent<MonsterController>();
        monster.transform.position = pos;
        monster.Id = Id;
        monster.Room = room;
        monster.transform.SetParent(room.transform);
        room.Monsters.Add(Id, monster);
    }
    // TODO - ���� ���� ���� ������ ����
    public MonsterController SpawnBossMonster(RoomController room, Vector3 pos, Define.MonsterType type = Define.MonsterType.Rush)
    {
        Id++;
        MonsterController monster = Managers.Resource.Instantiate("Creature/Monster/Monster_9000").GetComponent<MonsterController>();
        monster.transform.position = pos;
        monster.Id = Id;
        monster.Room = room;
        monster.transform.SetParent(room.transform);
        room.Monsters.Add(Id, monster);
        return monster;
    }
}

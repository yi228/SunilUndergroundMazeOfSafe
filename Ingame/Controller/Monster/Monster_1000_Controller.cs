using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_1000_Controller : MonsterController
{
    protected override void Init()
    {
        base.Init();
        _monsterType = Define.MonsterType.Fire;
        Gold = 10;
    }
    void Start()
    {
        Init();
    }

    void Update()
    {
        base.UpdateController();
    }
    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D(collision);
    }
}

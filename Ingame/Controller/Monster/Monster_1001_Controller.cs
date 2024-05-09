using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Monster_1001_Controller : MonsterController
{
    protected override void Init()
    {
        base.Init();
        _monsterType = Define.MonsterType.Rush;
        Gold = 7;
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

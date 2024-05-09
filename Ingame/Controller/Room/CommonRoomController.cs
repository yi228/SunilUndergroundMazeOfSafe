using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonRoomController : RoomController
{
    void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        RoomType = Define.RoomType.Common;
    }
    public override void StageClear()
    {
        base.StageClear();
    }
}

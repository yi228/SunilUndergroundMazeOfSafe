using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum HeartType
    {
        Empty,
        Full
    }
    public enum TileType
    {
        Common,
        Wall,
        Boss,
        Start
    }
    public enum DoorDir
    {
        Top,
        Bottom,
        Left,
        Right,
        None,
    }
    public enum MonsterType
    {
        None,
        Rush,
        Fire,
        FireRush,
        Boss
    }
    public enum CommonType
    {
        None,
        Common,
        Puzzle,
        Item,
        Special,
    }
    public enum RoomType
    {
        None,
        Common,
        Start,
        Boss,
    }
    public enum MoveDir
    {
        None,
        Up,
        Down,
        Left,
        Right,
    }
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
    }

    public enum CreatureState
    {
        Idle,
        Moving,
        Attack,
        Dead,
    }

    public enum State
    {
        Die,
        Moving,
        Idle,
        Skill,
    }

    public enum Layer
    {
        Monster = 8,
        Ground = 9,
        Block = 10,
    }

    public enum Scene
    {
        Unknown,
        OutGame,
        InGame,
        StartMenu,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
    }

    public enum CameraMode
    {
        QuarterView,
    }
}

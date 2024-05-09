using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager
{
    private GameObject mainCamara;
    public PostProcessProfile postProcess;
    private bool IsFirstStage = true;
    public bool puzzleClear = false;
    public bool playerDead = false;
    private int _puzzleSelect;
    public int Gold;
    public Stat Stat = new Stat();

    public void SetPuzzle()
    {
        _puzzleSelect = Random.Range(1, 4);
        //_puzzleSelect = 2;
        switch (_puzzleSelect)
        {
            case 1:
                Managers.Resource.Instantiate("Puzzles/SlidingPrefab");
                break;
            case 2:
                Managers.Room.Player.PlayerUI.gameObject.SetActive(false);
                Managers.Room.Player.RB.velocity = Vector2.zero;
                Managers.Room.Player.State = Define.CreatureState.Idle;
                mainCamara = Camera.main.gameObject;
                Managers.Resource.Instantiate("Puzzles/MazePrefab");
                break;
            case 3:
                Managers.Resource.Instantiate("Puzzles/BaseballPrefab");
                break;
            default:
                break;
        }
    }
    public void PuzzleSolve()
    {
        puzzleClear = true;
    }
    public void PlayerUIOn()
    {
        mainCamara.SetActive(true);
        Managers.Room.Player.PlayerUI.gameObject.SetActive(true);
    }
    public void SetPlayerStat()
    {
        Stat.MaxAttack = Managers.Room.Player.Stat.MaxAttack;
        Stat.MaxHp = Managers.Room.Player.Stat.MaxHp;
        Stat.Hp = Managers.Room.Player.Stat.MaxHp;
        Stat.Attack = Managers.Room.Player.Stat.Attack;
        Stat.Defense = Managers.Room.Player.Stat.Defense;
        Stat.MoveSpeed = Managers.Room.Player.Stat.MoveSpeed;
        Stat.SkillSpeed = Managers.Room.Player.Stat.SkillSpeed;
    }
    public void GetPlayerStat()
    {
        // 첫 시작일 땐 구글 시트의 디폴트 값으로 가져옴
        if (IsFirstStage)
        {
            if (Managers.Data.PlayerStat.Count == 0) 
                return;
            if(Managers.Room.Player != null)
            {
                Managers.Room.Player.Stat.MaxAttack = Managers.Data.PlayerStat[0];
                Managers.Room.Player.Stat.MaxHp = Managers.Data.PlayerStat[1];
                Managers.Room.Player.Stat.Hp = Managers.Data.PlayerStat[2];
                Managers.Room.Player.Stat.Attack = Managers.Data.PlayerStat[3];
                Managers.Room.Player.Stat.Defense = Managers.Data.PlayerStat[4];
                Managers.Room.Player.Stat.MoveSpeed = Managers.Data.PlayerStat[5];
                Managers.Room.Player.Stat.SkillSpeed = Managers.Data.PlayerStat[6];
            }
            IsFirstStage = false;
        }
        // TODO
        else if (Stat.MaxAttack == 0)
        {
            Managers.Room.Player.Stat.MaxAttack = Managers.Data.PlayerStat[0];
            Managers.Room.Player.Stat.MaxHp = Managers.Data.PlayerStat[1];
            Managers.Room.Player.Stat.Hp = Managers.Data.PlayerStat[2];
            Managers.Room.Player.Stat.Attack = Managers.Data.PlayerStat[3];
            Managers.Room.Player.Stat.Defense = Managers.Data.PlayerStat[4];
            Managers.Room.Player.Stat.MoveSpeed = Managers.Data.PlayerStat[5];
            Managers.Room.Player.Stat.SkillSpeed = Managers.Data.PlayerStat[6];
        }
        else
        {
            Managers.Room.Player.Stat.MaxAttack = Stat.MaxAttack;
            Managers.Room.Player.Stat.MaxHp = Stat.MaxHp;
            Managers.Room.Player.Stat.Hp = Stat.Hp;
            Managers.Room.Player.Stat.Attack = Stat.Attack;
            Managers.Room.Player.Stat.Defense = Stat.Defense;
            Managers.Room.Player.Stat.MoveSpeed = Stat.MoveSpeed;
            Managers.Room.Player.Stat.SkillSpeed = Stat.SkillSpeed;
        }
    }
    // 인게임에서 나가면 null로 밀어줘야할 것들
    public void Clear()
    {
        IsFirstStage = true;
        Stat = null;
    }
}


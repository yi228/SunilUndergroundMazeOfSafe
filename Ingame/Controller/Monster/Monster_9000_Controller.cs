using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_9000_Controller : MonsterController
{
    GameObject portal;
    [SerializeField] private Transform[] bulletBurstPoints;
    [SerializeField] private Transform centerPoint;
    protected override void Init()
    {
        base.Init();
        switch (Managers.Room.StageLevel)
        {
            case 1:
                _monsterType = Define.MonsterType.Rush;
                RushSpeed = 6;
                break;
            case 2:
                _monsterType = Define.MonsterType.Fire;
                RushSpeed = 4;
                break;
            case 3:
                _monsterType = Define.MonsterType.Boss;
                RushSpeed = 5;
                break;
            default:
                _monsterType = Define.MonsterType.Rush;
                break;
        }
        portal = GameObject.FindWithTag("Portal");
        portal.SetActive(false);

        Gold = 100;
    }
    void Start()
    {
        Init();
    }
    void Update()
    {
        base.UpdateController();
    }
    // TODO
    protected override void UpdateMoving()
    {
        SetDir();
        switch (Dir)
        {
            case Define.MoveDir.Left:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                tr_HpSlider.rotation = Quaternion.Euler(0, 0, 0);
                anim.Play($"MONSTER_{Id}_IDLE_MOVE");
                break;
            case Define.MoveDir.Right:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                tr_HpSlider.rotation = Quaternion.Euler(0, 0, 0);
                anim.Play($"MONSTER_{Id}_IDLE_MOVE");
                break;
        }
    }
    protected override void FireBulletBurst()
    {
        base.FireBulletBurst();
        if (_canFire)
            StartCoroutine(CoFireBulletBurst());
    }
    private IEnumerator CoFireBulletBurst()
    {
        if (State == Define.CreatureState.Dead)
            yield return null;
        _canFire = false;
        State = Define.CreatureState.Attack;
        for(int i=0; i<7; i++)
        {
            GameObject go = ObjectPoolManager.instance.monsterBulletPool.Get();
            go.transform.position = bulletBurstPoints[i].position;
            go.GetComponent<MonsterBulletController>().Dir = (bulletBurstPoints[i].position - centerPoint.position).normalized;
            go.GetComponent<MonsterBulletController>().Owner = this;
        }
        yield return new WaitForSeconds(1f);
        State = Define.CreatureState.Idle;
        _canFire = true;
    }
    protected override void OnDead(float time = 0f)
    {
        // TODO - 다음 레벨 디자인 해야 함
        Managers.Room.StageLevel++;
        portal.SetActive(true);
        base.OnDead(time);
    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
            collision.gameObject.GetComponent<PlayerController>().OnDamaged(stat.Attack / 2);
    }
    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D(collision);
    }
}

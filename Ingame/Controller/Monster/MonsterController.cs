using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Spine.Unity;
using Unity.VisualScripting;

public class MonsterController : CreatureController
{
    public PlayerController Target;
    public RoomController Room;
    private NavMeshAgent navMesh;
    public Slider hpSlider;
    public Transform tr_HpSlider;
    public Define.MonsterType _monsterType;
    protected SkeletonAnimation _sa;
    public int Id = 0;
    public int Gold;
    public float RushSpeed = 10f;
    // �� �� �뵵 Ÿ�̸�
    public float _rushAttackTimer = 0f;
    // ��ũ��Ʈ Id = ���� �ĺ� Id�� ����
    public int ScriptId;
    private GameObject _dotEffect;
    private bool _dotHit = false;
    protected override void Init()
    {
        base.Init();
        InitTag();
        InitTarget();
        InitUI();
        InitScriptID();
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.enabled = true;
        navMesh.speed = Stat.MoveSpeed;
        navMesh.updateRotation = false;
        navMesh.updateUpAxis = false;
        _sa = GetComponent<SkeletonAnimation>();
    }
    void InitScriptID()
    {
        Component[] components = GetComponents<Component>();
        foreach (Component component in components)
        {
            if (component.name.Contains("Monster_"))
            {
                string scriptId = component.name.Substring(8);
                ScriptId = int.Parse(scriptId);
                return;
            }
        }
        // ���� ��Ȳ
        ScriptId = -1;
        Debug.Log("cant find monster script ID");
        return;
    }
    void InitTag()
    {
        gameObject.tag = "Monster";
    }
    void InitUI()
    {
        hpSlider = transform.GetComponentInChildren<Slider>();
    }
    void InitTarget()
    {
        Target = Managers.Room.Player;
    }
    protected void SetDir()
    {
        if (State == Define.CreatureState.Dead)
            return;
        // ��������Ʈ ��ȯ
        if (Target.transform.position.x - transform.position.x > 0)
        {
            Dir = Define.MoveDir.Right;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Target.transform.position.x - transform.position.x <= 0)
        {
            Dir = Define.MoveDir.Left;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    protected void UpdateHpSlider()
    {
        hpSlider.value = (float)base.Stat.Hp / (float)base.Stat.MaxHp;
    }
    void MeasureTime()
    {
        _rushAttackTimer += Time.deltaTime;
    }
    protected override void UpdateController()
    {
        if (!Managers.Game.playerDead)
        {
            UpdateHpSlider();
            if (State == Define.CreatureState.Dead)
            {
                navMesh.isStopped = true;
                return;
            }
            MeasureTime();
            if (State != Define.CreatureState.Dead)
                FollowPlayer();
            UpdateDotEffectPos();
        }
    }
    // �� ���⿡ ���� �͸� ó�� TODO 4����
    protected override void UpdateIdle()
    {
        //if (Target == null || this.Room == null || State == Define.CreatureState.Dead)
        //    return;
        //if (Target.Room == this.Room)
        //{
        //    State = Define.CreatureState.Moving;
        //    return;
        //}
        //else
        //{
        //    State = Define.CreatureState.Idle;
        //}
        //switch (Dir)
        //{
        //    case Define.MoveDir.Left:
                
        //        transform.rotation = Quaternion.Euler(0, 0, 0);
        //        //HP�ٰ� �ڽ� ��ü�� ���� �¿� ���� �ż� HP�ٴ� �״�� �ΰ��ϴ� �ڵ�
        //        tr_HpSlider.rotation = Quaternion.Euler(0, 0, 0); 
        //        anim.Play($"MONSTER_{ScriptId}_IDLE_LEFT");
        //        break;
        //    case Define.MoveDir.Right:
        //        transform.rotation = Quaternion.Euler(0, 180, 0);
        //        tr_HpSlider.rotation= Quaternion.Euler(0, 0, 0);
        //        anim.Play($"MONSTER_{ScriptId}_IDLE_LEFT");
        //        break;
        //}
    }
    protected override void UpdateMoving()
    {
        //if (State == Define.CreatureState.Dead)
        //    return;
        //SetDir();
        //switch (Dir)
        //{
        //    case Define.MoveDir.Left:
        //        transform.rotation = Quaternion.Euler(0, 0, 0);
        //        tr_HpSlider.rotation = Quaternion.Euler(0, 0, 0);
        //        //anim.Play($"MONSTER_{ScriptId}_IDLE_MOVE");
        //        break;
        //    case Define.MoveDir.Right:
        //        transform.rotation = Quaternion.Euler(0, 180, 0);
        //        tr_HpSlider.rotation = Quaternion.Euler(0, 0, 0);
        //        //anim.Play($"MONSTER_{ScriptId}_IDLE_MOVE");
        //        break;
        //}
        //FollowPlayer();
    }
    // Rush Ÿ���� Attack ��� ���� ���� ����
    protected override void UpdateAttack()
    {
        SetDir();
        switch (Dir)
        {
            case Define.MoveDir.Left:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                tr_HpSlider.rotation = Quaternion.Euler(0, 0, 0);
                anim.Play($"MONSTER_{ScriptId}_IDLE_ATTACK");
                break;
            case Define.MoveDir.Right:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                tr_HpSlider.rotation = Quaternion.Euler(0, 0, 0);
                anim.Play($"MONSTER_{ScriptId}_IDLE_ATTACK");
                break;
        }
    }
    bool moveflag = false;
    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();
    }
    protected virtual void FollowPlayer()
    {
        if (Target == null || this.Room == null || State == Define.CreatureState.Dead)
            return;
        if (Target.Room == this.Room && State != Define.CreatureState.Dead)
        {
            SetDir();
            if (moveflag == false)
            {
                AnimWalk();
                moveflag = true;
            }
            //Vector2 direction = (Target.transform.position - transform.position).normalized;
            //rb.velocity = direction * Stat.MoveSpeed;
            
            navMesh.SetDestination(Target.transform.position);
            if (State != Define.CreatureState.Dead)
            {
                switch (_monsterType)
                {
                    case Define.MonsterType.Rush:
                        if (Vector2.Distance(Target.transform.position, transform.position) <= 1f && State != Define.CreatureState.Dead)
                            navMesh.isStopped = true;
                        else
                            navMesh.isStopped = false;
                        break;
                    case Define.MonsterType.Fire:
                        if (Vector2.Distance(Target.transform.position, transform.position) <= 3f && State != Define.CreatureState.Dead)
                        {
                            navMesh.isStopped = true;
                            FireBullet();
                        }
                        else
                            navMesh.isStopped = false;
                        break;
                    case Define.MonsterType.Boss:
                        if (Vector2.Distance(Target.transform.position, transform.position) <= 3f && State != Define.CreatureState.Dead)
                        {
                            navMesh.isStopped = true;
                            FireBulletBurst();
                        }
                        else
                            navMesh.isStopped = false;
                        break;
                }
                State = Define.CreatureState.Moving;
            }
            return;
        }
        else
        {
            State = Define.CreatureState.Idle;
        }
    }
    public void OnDotDamage(int damage)
    {
        StartCoroutine(CoDotDamage(damage));
    }
    protected void UpdateDotEffectPos()
    {
        if(_dotEffect != null)
            _dotEffect.transform.position = transform.position;
    }
    private IEnumerator CoDotDamage(int damage)
    {
        if(_dotEffect == null)
            _dotEffect = Managers.Resource.Instantiate("Effects/DotEffect");
        for (int i=0; i<5; i++)
        {
            OnDamaged(damage);
            if (State == Define.CreatureState.Dead && _dotEffect != null)
                Destroy(_dotEffect);
            yield return new WaitForSeconds(1f);
        }
        if(_dotEffect != null)
            Destroy(_dotEffect);
    }
    protected override void OnDead(float time = 0f)
    {
        State = Define.CreatureState.Dead;
        navMesh.isStopped = true;
        _canFire = false;
        Room.Monsters.Remove(this.Id);
        Target.Gold += Gold;
        if (Room.Monsters.Count == 0)
            Room.StageClear();
        // �÷��̾ ��� �ڷ� �и��� �� ����
        Target.RB.velocity = Vector2.zero;
        // sa ����
        _sa.state.ClearTracks();
        AnimDeath();
        base.OnDead(1f);
    }
    // ���߿� ���� ���� ���� �Լ� ���� TODO
    // �׽�Ʈ�� ������ ���� �Լ�
    public virtual IEnumerator CoRushAttack()
    {
        if (State == Define.CreatureState.Dead)
            yield return null;
        State = Define.CreatureState.Attack;
        // ���� �� FollowPlayer �ϱ� ������ ������ 0.5�ʷ� ����
        // �ִϸ��̼ǿ� ���� ���߿� ���� TODO
        Target.OnDamaged(stat.Attack);
        AnimAttack();
        yield return new WaitForSeconds(0.5f);
        _rushAttackTimer = 0f;
        State = Define.CreatureState.Moving;
        moveflag = false;
    }
    public IEnumerator CoRush()
    {
        if (Target == null) yield break;
        Debug.Log("Rush!");
        Vector2 direction = (Target.transform.position - transform.position).normalized;
        rb.velocity = direction * RushSpeed;
        if (Vector2.Distance(Target.transform.position, transform.position) <= 1.2f)
            rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.5f);

        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1f);

        StartCoroutine(CoRush());
    }
    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        //if (_monsterType == Define.MonsterType.Rush && collision.gameObject.tag == "Player" && _rushAttackTimer >= stat.AttackSpeed)
        //{
        //    _rushAttackTimer = 0f;
        //    StartCoroutine(CoRushAttack());
        //}
    }
    protected void FireBullet()
    {
        if (State == Define.CreatureState.Dead) 
            return;
        if(_monsterType == Define.MonsterType.Fire && _canFire)
            StartCoroutine(CoFireBullet());
    }
    protected bool _canFire = true;
    private IEnumerator CoFireBullet()
    {
        if (State == Define.CreatureState.Dead) 
            yield return null;
        _canFire = false;
        State = Define.CreatureState.Attack;
        //GameObject go = Managers.Resource.Instantiate("Creature/MonsterBullet");
        GameObject go = ObjectPoolManager.instance.monsterBulletPool.Get();
        go.transform.position = transform.position;
        go.GetComponent<MonsterBulletController>().Dir = (Target.transform.position - transform.position).normalized;
        go.GetComponent<MonsterBulletController>().Owner = this;
        yield return new WaitForSeconds(1f);
        State = Define.CreatureState.Idle;
        _canFire = true;
    }
    protected virtual void FireBulletBurst()
    {
        if (State == Define.CreatureState.Dead)
            return;
    }
    // Names are: Idle, Walk, Death, Hurt and Attack
    public void AnimIdle(bool isLoop = true)
    {
        if (State == Define.CreatureState.Dead)
            return;
        _sa.skeleton.SetSkin("Side");
        _sa.skeleton.SetSlotsToSetupPose();
        string AnimationName = "Idle";
        _sa.AnimationState.SetAnimation(0, "Side_" + AnimationName, isLoop);
    }
    public void AnimWalk(bool isLoop = true)
    {
        if (State == Define.CreatureState.Dead)
            return;
        _sa.skeleton.SetSkin("Side");
        _sa.skeleton.SetSlotsToSetupPose();
        string AnimationName = "Walk";
        _sa.AnimationState.SetAnimation(0, "Side_" + AnimationName, isLoop);
    }
    public void AnimDeath(bool isLoop = false)
    {
        _sa.skeleton.SetSkin("Side");
        _sa.skeleton.SetSlotsToSetupPose();
        string AnimationName = "Death";
        _sa.AnimationState.SetAnimation(0, "Side_" + AnimationName, isLoop);
    }
    public void AnimHurt(bool isLoop = false)
    {
        if (State == Define.CreatureState.Dead)
            return;
        _sa.skeleton.SetSkin("Side");
        _sa.skeleton.SetSlotsToSetupPose();
        string AnimationName = "Hurt";
        _sa.AnimationState.SetAnimation(0, "Side_" + AnimationName, isLoop);
    }
    public void AnimAttack(bool isLoop = false)
    {
        if (State == Define.CreatureState.Dead)
            return;
        _sa.skeleton.SetSkin("Side");
        _sa.skeleton.SetSlotsToSetupPose();
        string AnimationName = "Attack";
        _sa.AnimationState.SetAnimation(0, "Side_" + AnimationName, isLoop);
    }
}
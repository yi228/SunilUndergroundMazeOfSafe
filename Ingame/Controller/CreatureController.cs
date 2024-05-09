using UnityEngine;


public class CreatureController : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer sr;
    //protected Define.CreatureState state = Define.CreatureState.Idle;
    protected Stat stat;
    public Define.MoveDir Dir;
    public Rigidbody2D RB { get { return rb; } set { rb = value; } }
    public Define.CreatureState State = Define.CreatureState.Idle;
    public Stat Stat { get { return stat; } set { stat = value; } }
    public int Hp { get { return stat.Hp; } set { stat.Hp = value; } }
    protected virtual void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        stat = GetComponent<Stat>();
    }
    // 자식에게 물려줄 Update문들은 UpdateAnimaiton에 넣기 - TODO

    protected virtual void UpdateController()
    {

    }
    protected virtual void UpdateAnimation()
    {
        switch (State)
        {
            case Define.CreatureState.Idle:
                UpdateIdle();
                break;
            case Define.CreatureState.Moving:
                UpdateMoving();
                break;
            case Define.CreatureState.Attack:
                UpdateAttack();
                break;
        }
    }
    protected virtual void UpdateIdle()
    {

    }
    protected virtual void UpdateMoving()
    {

    }
    protected virtual void UpdateAttack()
    {

    }
    public virtual int OnDamaged(int damage)
    {
        damage = Mathf.Clamp(damage - stat.Defense, 0, damage);
        Hp -= damage;
        Debug.Log($"{gameObject.name}이 {damage} 데미지를 입음");
        Debug.Log($"{gameObject.name}의 남은 체력: {Hp}");

        if (Hp <= 0)
        {
            Hp = 0;
            State = Define.CreatureState.Dead;
            OnDead();
        }
        else
        {
            //sr.color = new Color(255, 0, 0, 0.6f);
            //Invoke("OffDamaged", 0.5f);
            DamageEffect();
        }

        return damage;
    }
    protected virtual void DamageEffect()
    {

    }
    // 죽는 거 처리
    void OffDamaged()
    {
        sr.color = new Color(255, 255, 255, 1);
    }
    protected virtual void OnDead(float time = 0f)
    {
        if (GetComponent<BoxCollider2D>() != null)
            GetComponent<BoxCollider2D>().isTrigger = true;
        if(GetComponent<CapsuleCollider2D>() != null)
            GetComponent<CapsuleCollider2D>().isTrigger = true;
        Debug.Log($"{gameObject.name}가 사망함");
        Managers.Resource.Destroy(gameObject, time);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;

struct TypeFlag
{
    public bool Burst;
    public bool Dot;
    public bool Chain;
};

public class BulletController : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }
    public PlayerController Owner;
    private Vector2 fireDir=Vector2.down;
    private float bulletSpeed = 15f;
    public bool touched = false;
    private TypeFlag typeFlag;
    
    private Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if(fireDir != Vector2.zero)
            Fire();
    }
    public void SetTypeEffect(int _bulletType)
    {
        switch (_bulletType)
        {
            case 0:
                typeFlag.Burst = false;
                typeFlag.Dot = false;
                typeFlag.Chain = false;
                break;
            case 1:
                typeFlag.Burst = true;
                typeFlag.Dot = false;
                typeFlag.Chain = false;
                break;
            case 2:
                typeFlag.Burst = false;
                typeFlag.Dot = true;
                typeFlag.Chain = false;
                break;
            case 3:
                typeFlag.Burst = false;
                typeFlag.Dot = false;
                typeFlag.Chain = true;
                break;
        }
    }
    public void SetDir(Define.MoveDir _fireDir)
    {
        switch (_fireDir)
        {
            case Define.MoveDir.Up:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                fireDir = Vector2.up;
                break;
            case Define.MoveDir.Down:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                fireDir = Vector2.down;
                break;
            case Define.MoveDir.Left:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                fireDir = Vector2.left;
                break;
            case Define.MoveDir.Right:
                transform.rotation = Quaternion.Euler(0, 0, 270);
                fireDir = Vector2.right;
                break;
        }
    }
    private void Fire()
    {
        rigid.velocity = fireDir * bulletSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") && !touched)
        {
            touched = true;
            if (typeFlag.Burst)
            {
                GameObject _effect = Managers.Resource.Instantiate("Effects/BurstEffect");
                Destroy(_effect, 1f);
                _effect.transform.position = transform.position;
                RaycastHit2D[] inRange = Physics2D.CircleCastAll(transform.position, 2f, Vector2.up);
                if (inRange.Length > 0)
                    foreach (RaycastHit2D _target in inRange)
                        if (_target.transform.GetComponent<MonsterController>() != null)
                            _target.transform.GetComponent<MonsterController>().OnDamaged(Owner.Stat.Attack / 6);
            }
            else if (typeFlag.Dot)
                collision.GetComponent<MonsterController>().OnDotDamage(Owner.Stat.Attack / 4);
            else if (typeFlag.Chain)
            {
                collision.GetComponent<MonsterController>().OnDamaged(Owner.Stat.Attack / 2);
                GameObject _effect1 = Managers.Resource.Instantiate("Effects/ChainEffect");
                Destroy(_effect1, 1f);
                _effect1.transform.position = collision.transform.position;
                RaycastHit2D[] chained = Physics2D.CircleCastAll(transform.position, 2f, Vector2.up);
                if (chained.Length > 0)
                    foreach (RaycastHit2D _target in chained)
                        if (_target.transform.GetComponent<MonsterController>() != null && collision.GetComponent<MonsterController>() != _target.transform.GetComponent<MonsterController>())
                        {
                            _target.transform.GetComponent<MonsterController>().OnDamaged(Owner.Stat.Attack / 2);
                            GameObject _effect2 = Managers.Resource.Instantiate("Effects/ChainEffect");
                            Destroy(_effect2, 1f);
                            _effect2.transform.position = _target.transform.position;
                            break;
                        }
            }
            else
            {
                GameObject _effect = Managers.Resource.Instantiate("Effects/BulletEffect");
                Destroy(_effect, 1f);
                _effect.transform.position = transform.position;
                collision.GetComponent<MonsterController>().OnDamaged(Owner.Stat.Attack);
            }
            Managers.Sound.Play("Effect/Hit/Hit_1");
            Pool.Release(gameObject); 
        }
        if (collision.gameObject.CompareTag("Collider") || collision.gameObject.CompareTag("Wall"))
            Pool.Release(gameObject);
    }
}

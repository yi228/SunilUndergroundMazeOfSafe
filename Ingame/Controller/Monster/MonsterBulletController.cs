using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterBulletController : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }
    private Vector2 _dir;
    public Vector2 Dir { get { return _dir; } set { _dir = value; } }
    private MonsterController _owner;
    public MonsterController Owner { get { return _owner; } set { _owner = value; } }
    private float _bulletSpeed;
    private Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        _bulletSpeed = 15f;
    }
    void Update()
    {
        rigid.velocity = _dir * _bulletSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(_owner != null)
                _owner.Target.OnDamaged(_owner.Stat.Attack);
            Pool.Release(gameObject);
        }
        if (collision.gameObject.CompareTag("Collider") || collision.gameObject.CompareTag("Wall"))
            Pool.Release(gameObject);
    }
}

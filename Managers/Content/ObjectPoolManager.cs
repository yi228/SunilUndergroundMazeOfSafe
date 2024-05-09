using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    private int defaultCapacity = 5;
    private int maxPoolSize = 10;
    private GameObject monBulletPrefab;
    private GameObject playerBulletPrefab;

    public IObjectPool<GameObject> monsterBulletPool { get; private set; }
    public IObjectPool<GameObject> playerBulletPool { get; private set; }

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        Init();
    }
    private void Init()
    {
        monBulletPrefab = Managers.Resource.Load<GameObject>("Creature/MonsterBullet");
        playerBulletPrefab = Managers.Resource.Load<GameObject>("Creature/Bullet");
        monsterBulletPool = new ObjectPool<GameObject>(CreateMonBullet, GetItem, ReleaseItem, DeleteItem, true, defaultCapacity, maxPoolSize);
        playerBulletPool = new ObjectPool<GameObject>(CreatePlayerBullet, GetItem, ReleaseItem, DeleteItem, true, defaultCapacity, maxPoolSize);

        for (int i = 0; i < defaultCapacity; i++)
        {
            MonsterBulletController monBullet = CreateMonBullet().GetComponent<MonsterBulletController>();
            monBullet.Pool.Release(monBullet.gameObject);
            BulletController playerBullet = CreatePlayerBullet().GetComponent<BulletController>();
            playerBullet.Pool.Release(playerBullet.gameObject);
        }
    }
    private GameObject CreateMonBullet()
    {
        GameObject go = Instantiate(monBulletPrefab, gameObject.transform);
        go.GetComponent<MonsterBulletController>().Pool = monsterBulletPool;
        return go;
    }
    private GameObject CreatePlayerBullet()
    {
        GameObject go = Instantiate(playerBulletPrefab, gameObject.transform);
        go.GetComponent<BulletController>().Pool = playerBulletPool;
        return go;
    }
    private void GetItem(GameObject _poolItem)
    {
        _poolItem.SetActive(true);
    }
    private void ReleaseItem(GameObject _poolItem)
    {
        _poolItem.SetActive(false);
    }
    private void DeleteItem(GameObject _poolItem)
    {
        Destroy(_poolItem);
    }
}
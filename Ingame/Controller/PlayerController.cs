using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using System.Collections;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerController : CreatureController
{
    private int _gold;
    private float attackTimer = 1f;
    public int bulletNum = 0;
    public bool isDoubleBullet = false;
    private RoomController _room;
    public UI_Player PlayerUI;
    [SerializeField] private ItemInventory_Popup theInventory;
    [SerializeField] private GameObject[] player4Dir;
    [SerializeField] private Transform[] firePoints;
    private Transform curFirePoint;
    private AnimationManager animManager;
    public int Gold
    {
        get { return _gold; }
        set 
        {
            _gold = value;
            Managers.Game.Gold = _gold;
            PlayerUI.UpdateAll();
        }
    }
    public RoomController Room { 
        get { return _room; }
        set
        {
            if (_room != null)
            {
                CameraController camera = Camera.main.GetComponent<CameraController>();
                camera.prev = _room.MiddlePoint;
                camera.next = value.MiddlePoint;
                camera.UpdateCamera();
                _room = value;
                PlayerUI.UpdateDataUI();
                BossRoomController bossRoom = _room.GetComponent<BossRoomController>();
                if (bossRoom != null && bossRoom.Boss != null)
                {
                    StartCoroutine(bossRoom.Boss.CoRush());
                }
            }
            else
            {
                _room = value;
                _room.Init();
                Camera.main.transform.position = new Vector3(_room.MiddlePoint.x, _room.MiddlePoint.y, -10);
            }
        }
    }
    void Start()
    {
        Init();
    }
    void Update()
    {
        base.UpdateAnimation();
        attackTimer += Time.deltaTime;
        if (stat.hp > 0)
            Managers.Game.playerDead = false;
        //stat.hp = 100;
    }
    protected override void UpdateIdle()
    {
        animManager.SetState(CharacterState.Idle);
        SwitchDir();
    }
    protected override void UpdateMoving()
    {
        animManager.SetState(CharacterState.Walk);
        SwitchDir();
    }
    private void SwitchDir()
    {
        switch (Dir)
        {
            case Define.MoveDir.Up:
                ChangeDir(1);
                break;
            case Define.MoveDir.Down:
                ChangeDir(0);
                break;
            case Define.MoveDir.Left:
                ChangeDir(2);
                break;
            case Define.MoveDir.Right:
                ChangeDir(3);
                break;
        }
    }
    private void ChangeDir(int _dir)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == _dir)
            {
                player4Dir[i].SetActive(true);
                curFirePoint = firePoints[i];
            }
            else
                player4Dir[i].SetActive(false);
        }
    }
    protected override void Init()
    {
        base.Init();
        animManager = GetComponent<AnimationManager>();
        // 이전 스테이지 스탯 가져오기
        Managers.Game.GetPlayerStat();
        PlayerUI.UpdateAll();
        curFirePoint = firePoints[0];

        Managers.Game.postProcess = GameObject.Find("PP Volume").GetComponent<PostProcessVolume>().profile;
    }
    public void InitPlayerUI()
    {
        PlayerUI = Managers.UI.ShowSceneUI<UI_Player>("UI_Player");
        PlayerUI.Player = this;
        PlayerUI.SettingUI();
        theInventory = PlayerUI.gameObject.GetComponentInChildren<ItemInventory_Popup>();
        print(theInventory);
        theInventory.gameObject.SetActive(false);
    }
    public void changeWeapon()
    {
        bulletNum++;
        bulletNum = bulletNum % 4;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            int cost = collision.GetComponent<ItemPickUp>().Cost;
            if (cost <= Gold)
            {
                Gold -= collision.GetComponent<ItemPickUp>().Cost;
                theInventory.AcquireItem(collision.gameObject.GetComponent<ItemPickUp>().item, 1);
                Destroy(collision.gameObject);
            }
        }
    }
    protected override void UpdateAttack()
    {
        if (attackTimer > 1f)
        {
            animManager.Jab();
            attackTimer = 0f;
        }
    }
    public void DoAttack()
    {
        Shoot();
        if (isDoubleBullet)
        {
            Invoke("Shoot", 0.1f);
        }
    }
    private void Shoot()
    {
        GameObject _bulletGo = ObjectPoolManager.instance.playerBulletPool.Get();
        switch (bulletNum)
        {
            case 0:
                _bulletGo.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arts/Sprite/Bullet0_Image");
                break;
            case 1:
                _bulletGo.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arts/Sprite/Bullet1_Image");
                break;
            case 2:
                _bulletGo.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arts/Sprite/Bullet2_Image");
                break;
            case 3:
                _bulletGo.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Arts/Sprite/Bullet3_Image");
                break;
        }
        BulletController bullet = _bulletGo.GetComponent<BulletController>();
        bullet.SetTypeEffect(bulletNum);
        bullet.touched = false;
        _bulletGo.transform.position = curFirePoint.position;
        bullet.SetDir(Dir);
        bullet.Owner = this;
        Managers.Sound.Play("Effect/Bullet/FireBullet");
    }
    // UpdateAttack 보호수준 때문에 public Fire로 한 번 거쳐서 UI_Player에 호출
    public override int OnDamaged(int damage)
    {
        int dmg = base.OnDamaged(damage);
        if (stat.hp <= 0)
        {
            Managers.Game.playerDead = true;
            RestartIngame();
        }
        PlayerUI.UpdateHeartUI();
        return dmg;
    }
    private void RestartIngame()
    {
        Managers.Game.SetPlayerStat();
        Managers.Scene.ReloadCurrentScene();
    }
    public void Fire()
    {
        UpdateAttack();
    }
    protected override void DamageEffect()
    {
        animManager.Hit();
    }
}

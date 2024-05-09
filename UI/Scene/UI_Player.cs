using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Player : UI_Scene
{
    Button upButton;
    Button downButton;
    Button leftButton;
    Button rightButton;
    Button attackButton;
    Button weaponchangeButton;

    public List<Image> Hearts = new List<Image>();
    public TextMeshProUGUI ClearRoomText, CurrentRoomText, GoldText;
    public Slider hpSlider;
    public Image weaponImage;
    
    enum Buttons
    {
        UpButton,
        DownButton,
        LeftButton,
        RightButton,
        AttackButton,
        WeaponChangeButton,
        ItemInventoryButton
    }
    enum Texts
    {
        ClearRoomText,
        CurrentRoomText,
        GoldText
    }

    public PlayerController Player;
    public Stat playerStat;
    public void SettingUI()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        InitButtons();
        InitDataUI();
        InitHearts();
    }
    private void Update()
    {
        if (Player != null && hpSlider != null)
            hpSlider.value = (float)Player.Stat.Hp / (float)Player.Stat.MaxHp;
        else
            FindPlayer();
    }
    void InitHearts()
    {
        GameObject panel = GameObject.Find("HeartPanel");
        for (int i = 0; i < panel.transform.childCount; i++)
        {
            Hearts.Add(panel.transform.GetChild(i).GetComponent<Image>());
        }
    }
    // 체력이 증감함에 따른 UI 최신화
    public void UpdateHeartUI()
    {
        for (int i = Hearts.Count - 1; i >= Managers.Room.Player.Hp; i--)
        {
            Hearts[i].GetComponent<HeartController>().Type = Define.HeartType.Empty;
        }
    }
    // TODO
    void InitDataUI()
    {
        //ClearRoomText = GetTextMeshProUGUI((int)Texts.ClearRoomText);
        //CurrentRoomText = GetTextMeshProUGUI((int)Texts.CurrentRoomText);
        //ClearRoomText.text = $"";
        //CurrentRoomText.text = $"Current Room: {Player.Room.name}";
        GoldText = GetTextMeshProUGUI((int)Texts.GoldText);
    }
    void InitButtons()
    {
        upButton = GetButton((int)Buttons.UpButton);
        downButton = GetButton((int)Buttons.DownButton);
        leftButton = GetButton((int)Buttons.LeftButton);
        rightButton = GetButton((int)Buttons.RightButton);
        attackButton = GetButton((int)Buttons.AttackButton);
        weaponchangeButton = GetButton((int)Buttons.WeaponChangeButton);
        #region Moving Panel Init
        // 버튼 눌렀을 때 함수 추가
        // Up
        {
            EventTrigger trigger = upButton.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) => { MoveUp(); });
            trigger.triggers.Add(entry);
        }
        // Down
        {
            EventTrigger trigger = downButton.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) => { MoveDown(); });
            trigger.triggers.Add(entry);
        }
        // Left
        {
            EventTrigger trigger = leftButton.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) => { MoveLeft(); });
            trigger.triggers.Add(entry);
        }
        // Right
        {
            EventTrigger trigger = rightButton.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) => { MoveRight(); });
            trigger.triggers.Add(entry);
        }
        // 버튼 땠을 때 추가
        // Up
        {
            EventTrigger trigger = upButton.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((eventData) => { ReleaseMove(); });
            trigger.triggers.Add(entry);
        }
        // Down
        {
            EventTrigger trigger = downButton.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((eventData) => { ReleaseMove(); });
            trigger.triggers.Add(entry);
        }
        // Left
        {
            EventTrigger trigger = leftButton.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((eventData) => { ReleaseMove(); });
            trigger.triggers.Add(entry);
        }
        // Right
        {
            EventTrigger trigger = rightButton.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((eventData) => { ReleaseMove(); });
            trigger.triggers.Add(entry);
        }
        #endregion
        attackButton.onClick.AddListener(Player.Fire);
        weaponchangeButton.onClick.AddListener(ChangeWeapon);

    }

    void ChangeWeapon()
    {
        Player.changeWeapon();
        switch (Player.bulletNum)
        {
            case 0:
                weaponImage.sprite = Resources.Load<Sprite>("Arts/Sprite/Bullet0_Image");
                break;
            case 1:
                weaponImage.sprite = Resources.Load<Sprite>("Arts/Sprite/Bullet1_Image");
                break;
            case 2:
                weaponImage.sprite = Resources.Load<Sprite>("Arts/Sprite/Bullet2_Image");
                break;
            case 3:
                weaponImage.sprite = Resources.Load<Sprite>("Arts/Sprite/Bullet3_Image");
                break;
        }
    }

    void MoveUp()
    {
        FindPlayer();
        Player.RB.velocity = Vector2.up.normalized * Player.Stat.MoveSpeed;
        Player.Dir = Define.MoveDir.Up;
        Player.State = Define.CreatureState.Moving;
    }
    void MoveDown()
    {
        FindPlayer();
        Player.RB.velocity = Vector2.down.normalized * Player.Stat.MoveSpeed;
        Player.Dir = Define.MoveDir.Down;
        Player.State = Define.CreatureState.Moving;
    }
    void MoveLeft()
    {
        FindPlayer();
        Player.RB.velocity = Vector2.left.normalized * Player.Stat.MoveSpeed;
        Player.Dir = Define.MoveDir.Left;
        Player.State = Define.CreatureState.Moving;
    }
    void MoveRight()
    {
        FindPlayer();
        Player.RB.velocity = Vector2.right.normalized * Player.Stat.MoveSpeed;
        Player.Dir = Define.MoveDir.Right;
        Player.State = Define.CreatureState.Moving;
    }
    void ReleaseMove()
    {
        FindPlayer();
        Player.RB.velocity = Vector2.zero;
        Player.State = Define.CreatureState.Idle;
    }
    void FindPlayer()
    {
        if (Player == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go == null) return;
            PlayerController player = go.GetComponent<PlayerController>();
            Managers.Room.Player = player;
            Player = player;
        }
    }
    public void UpdateDataUI()
    {
        //CurrentRoomText.text = $"Current Room: {Player.Room.name}";
        GoldText.text = $"{Managers.Game.Gold}";
    }
    public void UpdateAll()
    {
        UpdateHeartUI();
        UpdateDataUI();
    }
}

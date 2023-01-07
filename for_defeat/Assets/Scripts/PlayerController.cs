using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //플레이어 속도
    [SerializeField] private float speed;
    public float PlayerSpeed => speed;
    //플레이어 가속도(이동에 가속이 존재한다면)
    [SerializeField] private float accel;
    public float PlayerAccel => accel;
    //플레이어 이동의 오차범위
    [SerializeField] private float moveError;
    public float PlayerMoveError => moveError;
    //최대 체력
    [SerializeField] private float maxHP;
    public float PlayerMaxHP => maxHP;
    //현재 체력
    [SerializeField] private float curHP;
    public float PlayerCurHP => curHP;
    //최대 분노게이지량
    [SerializeField] private float maxAngerGauge = 999f;
    //현재 분노게이지량
    [SerializeField] private float curAngerGauge;
    public float CurAngerGauge => curAngerGauge;
    private Vector3 mousePosition;
    private Vector3 targetPosition;
    
    public List<Skill> skillList = new();
    
    public enum PlayerState
    {
        Move,
        Wait,
        Cast
    }

    public enum PlayerSkill
    {
        Erosion,
        Pizza,
        ShockWave,
        Trap,
        Flash,
        Length
    }

    private bool isQActive = true;
    public bool IsQActive => isQActive;
    [SerializeField] private float QCoolDown;
    public float MaxQCoolDown => QCoolDown;
    private float curQCoolDown;
    public float CurQCoolDown => curQCoolDown;

    private bool isWActive = true;
    public bool IsWActive => isWActive;
    [SerializeField] private float WCoolDown;
    public float MaxWCoolDown => WCoolDown;
    private float curWCoolDown;
    public float CurWCoolDown => curWCoolDown;

    private bool isEActive = true;
    public bool IsEActive => isEActive;
    [SerializeField] private float ECoolDown;
    public float MaxECoolDown => ECoolDown;
    private float curECoolDown;
    public float CurECoolDown => curECoolDown;

    private bool isRActive = true;
    public bool IsRActive => isRActive;
    [SerializeField] private float RCoolDown;
    public float MaxRCoolDown => RCoolDown;
    private float curRCoolDown;
    public float CurRCoolDown => curRCoolDown;

    private bool isFlashActive = true;
    public bool IsFlashActive => isFlashActive;
    [SerializeField] private float FlashCoolDown;
    public float MaxFlashCoolDown => FlashCoolDown;
    private float curFlashCoolDown;
    public float CurFlashCoolDown => curFlashCoolDown;
    public bool isFlashUsed = false;

    public StateMachine stateMachine;
    private Dictionary<PlayerState, IState> dicState = new Dictionary<PlayerState, IState>();

    private void Awake()
    {
        GameManager.Instance.player = this;
    }

    private void Start()
    {
        curHP = maxHP;
        curAngerGauge = 0f;
        
        StartCoroutine(EAngerGaugeAscend());
        
        IState move = new PlayerMove(this);
        IState wait = new PlayerWait(this);
        IState cast = new PlayerCast(this);

        dicState.Add(PlayerState.Move, move);
        dicState.Add(PlayerState.Wait, wait);
        dicState.Add(PlayerState.Cast, cast);

        stateMachine = new StateMachine(dicState[PlayerState.Wait]);

        foreach(var skill in skillList) skill.origin = gameObject;
    }

    private void Update()
    {
        KeyBoardInput();
        stateMachine.DoOperateUpdate();
    }

    public void UpdateState(PlayerState type)
    {
        stateMachine.SetState(dicState[type]);
    }

    public void UpdateState(PlayerState type, PlayerSkill skillType)
    {
        ((PlayerCast)dicState[PlayerState.Cast]).skillIdx = (int)skillType;
        stateMachine.SetState(dicState[type]);
    }
    
    //Input에 따른 각 공격
    private void KeyBoardInput()
    {
        if(Input.GetKey(KeyCode.Q) && isQActive)
        {
            UpdateState(PlayerState.Cast, PlayerSkill.Erosion);
            StartCoroutine(EErosionCD());
        }
        else if(Input.GetKey(KeyCode.W) && isWActive)
        {
            UpdateState(PlayerState.Cast, PlayerSkill.Pizza);
            StartCoroutine(EPizzaCD());
        }
        else if(Input.GetKey(KeyCode.E) && isEActive)
        {
            UpdateState(PlayerState.Cast, PlayerSkill.ShockWave);
            StartCoroutine(EShockWaveCD());
        }
        else if(Input.GetKey(KeyCode.R) && isRActive)
        {
            UpdateState(PlayerState.Cast, PlayerSkill.Trap);
            StartCoroutine(ETrapCD());
        }
        else if(Input.GetKey(KeyCode.Space) && isFlashActive)
        {
            UpdateState(PlayerState.Cast, PlayerSkill.Flash);
        }
    }

    public void GetDamage(float damage)
    {
        curHP -= damage;
        Debug.Log(damage);
        if(curHP <= 0) PlayerDie();
    }

    public void PlayerDie()
    {
        //do something
    }

    private IEnumerator EAngerGaugeAscend()
    {
        while(true)
        {
            curAngerGauge += Time.deltaTime;
            curAngerGauge = Mathf.Min(curAngerGauge, maxAngerGauge);
            yield return null;
        }   
    }

    private IEnumerator EErosionCD()
    {
        isQActive = false;
        curQCoolDown = QCoolDown;
        while(curQCoolDown >= 0)
        {
            curQCoolDown -= Time.deltaTime;
            yield return null;
        }
        isQActive = true;
    }

    private IEnumerator EPizzaCD()
    {
        isWActive = false;
        curWCoolDown = WCoolDown;
        while(curWCoolDown >= 0)
        {
            curWCoolDown -= Time.deltaTime;
            yield return null;
        }
        isWActive = true;
    }

    private IEnumerator EShockWaveCD()
    {
        isEActive = false;
        curECoolDown = ECoolDown;
        while(curECoolDown >= 0)
        {
            curECoolDown -= Time.deltaTime;
            yield return null;
        }
        isEActive = true;
    }

    private IEnumerator ETrapCD()
    {
        isRActive = false;
        curRCoolDown = RCoolDown;
        while(curRCoolDown >= 0)
        {
            curRCoolDown -= Time.deltaTime;
            yield return null;
        }
        isRActive = true;
    }

    public IEnumerator EFlashCD()
    {
        isFlashActive = false;
        curFlashCoolDown = FlashCoolDown;
        while(curFlashCoolDown >= 0)
        {
            curFlashCoolDown -= Time.deltaTime;
            yield return null;
        }
        isFlashActive = true;
    }
    
}
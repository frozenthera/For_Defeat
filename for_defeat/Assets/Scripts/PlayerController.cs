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
    [SerializeField] private float maxAngerGauge;
    //현재 분노게이지량
    [SerializeField] private float curAngerGauge;
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
    [SerializeField] private float QCoolDown;
    private float curQCoolDown;

    private bool isWActive = true;
    [SerializeField] private float WCoolDown;
    private float curWCoolDown;

    private bool isEActive = true;
    [SerializeField] private float ECoolDown;
    private float curECoolDown;

    private bool isRActive = true;
    [SerializeField] private float RCoolDown;
    private float curRCoolDown;

    public StateMachine stateMachine;
    private Dictionary<PlayerState, IState> dicState = new Dictionary<PlayerState, IState>();

    private void Start()
    {
        curHP = maxHP;
        //targetPosition = transform.position;
        IState move = new PlayerMove(this);
        IState wait = new PlayerWait(this);
        IState cast = new PlayerCast(this);

        dicState.Add(PlayerState.Move, move);
        dicState.Add(PlayerState.Wait, wait);
        dicState.Add(PlayerState.Cast, cast);

        stateMachine = new StateMachine(dicState[PlayerState.Wait]);
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
    }

    //분노 게이지 시스템

    private void Awake()
    {
        GameManager.Instance.player = this;
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
    
}
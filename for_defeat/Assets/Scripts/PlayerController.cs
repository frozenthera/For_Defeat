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
    
    [SerializeField] private List<PlayerSkill> skillList = new();
    
    public enum PlayerState
    {
        Move,
        Wait
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

    public StateMachine stateMachine;
    private Dictionary<PlayerState, IState> dicState = new Dictionary<PlayerState, IState>();

    private void Start()
    {
        curHP = maxHP;
        //targetPosition = transform.position;
        IState move = new PlayerMove();
        IState wait = new PlayerWait();

        dicState.Add(PlayerState.Move, move);
        dicState.Add(PlayerState.Wait, wait);

        stateMachine = new StateMachine(dicState[PlayerState.Wait]);
    }

    private void Update()
    {
        stateMachine.DoOperateUpdate();
    }

    public void UpdateState(PlayerState type)
    {
        stateMachine.SetState(dicState[type]);
    }

    //대쉬

    //Input에 따른 각 공격

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
}
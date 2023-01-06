using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBehaviour : MonoBehaviour
{
    public Dictionary<string, Skill> heroSKillDic = new(); 
    private PlayerController player;

    [SerializeField] private float heroSpeed;
    public float HeroSpeed => heroSpeed;
    
    [SerializeField] private float heroNormalBDelay;
    public float HeroNormalBDelay => heroNormalBDelay;
    [SerializeField] private float heroNormalADelay;
    public float HeroNormalADelay => heroNormalADelay;

    public enum HeroState
    {
        Idle,
        Move,
        Attack,
        Cast
    }

    public StateMachine stateMachine;

    private Dictionary<HeroState, IState> dicState = new Dictionary<HeroState, IState>();

    private void Start()
    {
        IState idle = new HeroIdle(this);
        IState move = new HeroMove(this);
        IState attack = new HeroAttack(this);
        IState cast = new HeroCast(this);

        dicState.Add(HeroState.Idle, idle);
        dicState.Add(HeroState.Move, move);
        dicState.Add(HeroState.Attack, attack);
        dicState.Add(HeroState.Cast, cast);

        stateMachine = new StateMachine(dicState[HeroState.Move]);

        player = GameManager.Instance.player;
    }

    private void Update()
    {
        stateMachine.DoOperateUpdate();
    }

    public void UpdateState(HeroState type)
    {
        stateMachine.SetState(dicState[type]);
    }

    //플레이어 인식과 인식 시에 어떻게 행동해야하는지

    //대쉬

    //용사 스킬



}
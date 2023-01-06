using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBehaviour : MonoBehaviour
{

    private PlayerController player;

    [SerializeField] private float heroSpeed;
    public float HeroSpeed => heroSpeed;
    
    [SerializeField] private float heroNormalBDelay;
    public float HeroNormalBDelay => heroNormalBDelay;
    [SerializeField] private float heroNormalADelay;
    public float HeroNormalADelay => heroNormalADelay;

    [SerializeField] private float heroNormalAttackDamage;
    public float HeroNormalAttackDamage => heroNormalAttackDamage;

    [SerializeField] private float heroRecogRad;
    public float HeroRecogRad => heroRecogRad;

    private bool isSlashable;
    public bool IsSlashable => isSlashable;
    [SerializeField] private float heroSlashCD;

    [SerializeField] private bool isDashable;
    [SerializeField] private float heroDashCD;
    
    public List<Skill> skillList = new();

    public enum HeroState
    {
        Idle,
        Move,
        Attack,
        Cast,
        Length
    }

    public enum heroSKill
    {
        Slash,
        Dash,
        Length
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

        StartCoroutine(ESlashCD());
        StartCoroutine(EDashCD());

        foreach(var skill in skillList) skill.origin = gameObject;

        player = GameManager.Instance.player;
    }

    private void Update()
    {
        if(isSlashable)
        {
            UpdateState(HeroState.Cast, heroSKill.Slash);   
            StartCoroutine(ESlashCD());
        }
        else if(isDashable)
        {
            UpdateState(HeroState.Cast, heroSKill.Dash);
            StartCoroutine(EDashCD());
        }
        stateMachine.DoOperateUpdate();
    }

    public void UpdateState(HeroState type)
    {
        stateMachine.SetState(dicState[type]);
    }

    public void UpdateState(HeroState type, heroSKill skillType)
    {
        ((HeroCast)dicState[HeroState.Cast]).skillIdx = (int)skillType;
        stateMachine.SetState(dicState[type]);

    }

    //용사 스킬 쿨다운 관리
    private IEnumerator ESlashCD()
    {
        isSlashable = false;
        float curSlashCD = heroSlashCD;
        while(curSlashCD >= 0)
        {
            curSlashCD -= Time.deltaTime;
            yield return null;
        }
        isSlashable = true;
    }

    private IEnumerator EDashCD()
    {
        isDashable = false;
        float curDashCD = heroDashCD;
        while(curDashCD >= 0)
        {
            curDashCD -= Time.deltaTime;
            yield return null;
        }
        isDashable = true;
    }

}
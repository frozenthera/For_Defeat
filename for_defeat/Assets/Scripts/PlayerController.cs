using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : UnitBehaviour
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
    public float MaxAngerGauge => maxAngerGauge;
    //현재 분노게이지량
    [SerializeField] private float curAngerGauge;
    public float CurAngerGauge => curAngerGauge;
    private Vector3 mousePosition;
    private Vector3 targetPosition;
    
    public List<PlayerSkill> skillList = new();
    
    public enum EPlayerState
    {
        Move,
        Wait,
        Cast
    }

    public enum EPlayerSkill
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
    private Dictionary<EPlayerState, IState> dicState = new Dictionary<EPlayerState, IState>();
    public Dictionary<EPlayerSkill, IEnumerator> ECoolDownDic = new();

    public Animator playerAnim;

    [SerializeField] private GameObject RangeIndicatorPrefab;
    public GameObject RangeIndicator;
    [SerializeField] private float flashRadius;
    public float FlashRadius => flashRadius;
    public GameObject ShockWaveObjectPrefab;
    public GameObject ShockWaveIndicatorPrefab;
    public Mesh viewMesh;
    [SerializeField] private PlayerShockWave playerShockWave;
    public GameObject indicator;
    public MeshFilter viewMeshFilter;
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public float meshResolution;
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public int AngerStep;

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

        dicState.Add(EPlayerState.Move, move);
        dicState.Add(EPlayerState.Wait, wait);
        dicState.Add(EPlayerState.Cast, cast);

        ECoolDownDic.Add(EPlayerSkill.Erosion, EErosionCD());
        ECoolDownDic.Add(EPlayerSkill.Pizza, EPizzaCD());
        ECoolDownDic.Add(EPlayerSkill.ShockWave, EShockWaveCD());
        ECoolDownDic.Add(EPlayerSkill.Trap, ETrapCD());
        ECoolDownDic.Add(EPlayerSkill.Flash, EFlashCD());

        stateMachine = new StateMachine(dicState[EPlayerState.Wait]);

        foreach(var skill in skillList) skill.origin = gameObject;

        playerAnim = GetComponent<Animator>();

        RangeIndicator = Instantiate(RangeIndicatorPrefab, Vector3.zero, Quaternion.identity);
        RangeIndicator.transform.localScale = Vector3.forward + new Vector3(1,1,0) * flashRadius * (((int)curAngerGauge/333)+1) * 10f;
        RangeIndicator.gameObject.SetActive(false);
    }

    private void Update()
    {
        KeyBoardInput();
        stateMachine.DoOperateUpdate();
    }

    public void UpdateState(EPlayerState type)
    {
        stateMachine.SetState(dicState[type]);
    }

    public void UpdateState(EPlayerState type, EPlayerSkill skillType)
    {
        ((PlayerCast)dicState[EPlayerState.Cast]).skillIdx = (int)skillType;
        stateMachine.SetState(dicState[type]);
    }
    
    //Input에 따른 각 공격
    private void KeyBoardInput()
    {
        if(isInDelay) return;
        if(Input.GetKey(KeyCode.Q) && isQActive)
        {
            UpdateState(EPlayerState.Cast, EPlayerSkill.Erosion);
        }
        else if(Input.GetKey(KeyCode.W) && isWActive)
        {
            UpdateState(EPlayerState.Cast, EPlayerSkill.Pizza);
        }
        else if(Input.GetKeyDown(KeyCode.E) && isEActive)
        {
            indicator = Instantiate(ShockWaveIndicatorPrefab, this.transform.position, Quaternion.identity);
            indicator.transform.SetParent(this.transform);
            viewMesh = new Mesh();
            viewMesh.name = "View Mesh";
            
            Vector3 _targetPosition = Vector3.right;        
            DrawFieldOfView(_targetPosition);

            indicator.transform.GetChild(0).GetComponent<MeshFilter>().mesh = viewMesh;
            StartCoroutine(ShockWaveWaiting());
        }
        else if(Input.GetKey(KeyCode.R) && isRActive)
        {
            UpdateState(EPlayerState.Cast, EPlayerSkill.Trap);
        }
        else if(Input.GetKey(KeyCode.Space) && isFlashActive)
        {
            RangeIndicator.transform.position = transform.position;
            RangeIndicator.transform.parent = transform;
            RangeIndicator.gameObject.SetActive(true);
            StartCoroutine(FlashWaiting());
        }
    }

    public void GetDamage(float damage, float angerGaugeGain)
    {
        GainAngergauge(angerGaugeGain);
        curHP -= damage;
        Debug.Log(damage);
        if(curHP <= 0) PlayerDie();
    }

    public void PlayerDie()
    {
        //do something
    }

    public void UseAngergauge(float value)
    {
        curAngerGauge -= value;
        if(curAngerGauge < 0) curAngerGauge = 0;
    }

    public void GainAngergauge(float value)
    {
        curAngerGauge += value;
        if(curAngerGauge > maxAngerGauge) curAngerGauge = maxAngerGauge;
    }

    private IEnumerator EAngerGaugeAscend()
    {
        while(true)
        {
            curAngerGauge += Time.deltaTime * 10;
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

    public IEnumerator EPizzaCD()
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

    public IEnumerator EShockWaveCD()
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
    
    public IEnumerator FlashWaiting()
    {
        yield return new WaitUntil(()=> Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1));
        if(Input.GetMouseButtonDown(0))
        {
            RangeIndicator.transform.localScale = Vector3.forward + new Vector3(1,1,0) * flashRadius * (((int)curAngerGauge/333)+1) * 10f;
            UpdateState(EPlayerState.Cast, EPlayerSkill.Flash);
        }
        else if(Input.GetMouseButtonDown(1))
        {
            RangeIndicator.gameObject.SetActive(false);
        }
    }

    public IEnumerator ShockWaveWaiting()
    {
        yield return new WaitUntil(()=> Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1));
        if(Input.GetMouseButtonDown(0))
        {
            UpdateState(EPlayerState.Cast, EPlayerSkill.ShockWave);
        }
        else if(Input.GetMouseButtonDown(1))
        {
            Destroy(indicator);
        }
        yield return null;
    }

    public void DrawFieldOfView(Vector3 targetVec)
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();

        
        for (int i = 0; i <= stepCount; i++)
        {
            Vector3 direction = Quaternion.AngleAxis(-viewAngle / 2 + stepAngleSize * i, Vector3.back) * targetVec;
            ViewCastInfo newViewCast = ViewCast(direction);   
            viewPoints.Add(newViewCast.point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];
        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = playerShockWave.transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    ViewCastInfo ViewCast(Vector3 vec)
    {
        Vector3 dir = playerShockWave.transform.TransformVector(vec).normalized;
        return new ViewCastInfo(false, dir * viewRadius, viewRadius, 0f);
    }
}
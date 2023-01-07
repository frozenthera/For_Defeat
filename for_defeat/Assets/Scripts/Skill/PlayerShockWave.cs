using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShockWave : PlayerSkill
{
    public GameObject ShockWaveObjectPrefab;
    public GameObject ShockWaveIndicatorPrefab;
    private Mesh viewMesh;
    public MeshFilter viewMeshFilter;
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    [SerializeField] private float damage;
    [SerializeField] private float knuckBackSpeed;
    [SerializeField] private float knuckBackMultiplier;
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

    public float meshResolution;
    public override IEnumerator _OnSkillActive()
    {
        GameObject indicator = Instantiate(ShockWaveIndicatorPrefab, origin.transform.position, Quaternion.identity);
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        
        Vector3 _targetPosition = Vector3.right;        
        DrawFieldOfView(_targetPosition);

        indicator.transform.GetChild(0).GetComponent<MeshFilter>().mesh = viewMesh;
        
        yield return new WaitUntil(()=> Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1));
        if(Input.GetMouseButtonDown(0))
        {
            GameObject go = Instantiate(ShockWaveObjectPrefab, origin.transform.position, Quaternion.identity);    
            
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition = (targetPosition-origin.transform.position);
            targetPosition = new Vector3(targetPosition.x, targetPosition.y, 0f);
            targetPosition.Normalize();
            DrawFieldOfView(targetPosition);
            viewMeshFilter = go.GetComponent<MeshFilter>();
            viewMeshFilter.mesh = viewMesh;
            
            ShockWaveObject SWO = go.GetComponent<ShockWaveObject>();
            int AngerStep = (int)(GameManager.Instance.player.CurAngerGauge / 333) + 1;
            SWO.damage = damage;
            SWO.knuckBackVec = Mathf.Lerp(AngerStep + 1, AngerStep, (GameManager.Instance.hero.transform.position - origin.transform.position).magnitude / ((AngerStep + 1) * viewRadius + 0.34f)) * knuckBackMultiplier * (GameManager.Instance.hero.transform.position - origin.transform.position).normalized;
            SWO.knuckBackSec = SWO.knuckBackVec.magnitude / knuckBackSpeed;
            
            //Mesh to polygon collider
            Vector3[] vertices;
            Vector2[] vertices2d;
            vertices = viewMesh.vertices;
            vertices2d = new Vector2[vertices.Length];
            for(var i=0; i<vertices.Length; i++)
            {
                vertices2d[i] = new Vector2(vertices[i].x, vertices[i].y);
            }
            PolygonCollider2D poly2d = go.GetComponent<PolygonCollider2D>();
            poly2d.points = vertices2d;

            Destroy(indicator);

            StartCoroutine(GameManager.Instance.player.EShockWaveCD());
        }
        //행동 취소
        else if(Input.GetMouseButtonDown(1))
        {
            Destroy(indicator);
        }
        yield return null;
    }

    void DrawFieldOfView(Vector3 targetVec)
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
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
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
        Vector3 dir = transform.TransformVector(vec).normalized;
        return new ViewCastInfo(false, dir * viewRadius, viewRadius, 0f);
    }
}
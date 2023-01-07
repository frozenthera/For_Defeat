using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPizza : PlayerSkill
{
    public GameObject PizzaObjectPrefab;
    public GameObject PizzaIndicatorPrefab;
    private Mesh viewMesh;
    public MeshFilter viewMeshFilter;
    public float viewRadius;
    [Range(0, 360)]
    private float viewAngle;
    //damage per 0.5sec
    [SerializeField] private float DOTDamage;
    [SerializeField] private float DOTLastTime;
    private GameObject indicator;

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
    public void MakeIndicator()
    {
        indicator = Instantiate(PizzaIndicatorPrefab, origin.transform.position, Quaternion.identity, GameManager.Instance.player.transform);
        viewMesh = new Mesh();
        int AngerStep = (int)(GameManager.Instance.player.CurAngerGauge / 333);
        viewAngle = 30f;
        viewRadius = (AngerStep+2) * viewRadius;
        Vector3 _targetPosition = Vector3.right;
        DrawFieldOfView(_targetPosition, 6);

        indicator.transform.GetChild(0).GetComponent<MeshFilter>().mesh = viewMesh;
    }

    public override IEnumerator _OnSkillActive()
    {       
        GameObject go = Instantiate(PizzaObjectPrefab, origin.transform.position, Quaternion.identity);    
        
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition = (targetPosition-origin.transform.position);
        targetPosition = new Vector3(targetPosition.x, targetPosition.y, 0f);
        targetPosition.Normalize();
        DrawFieldOfView(targetPosition, 6);
        viewMeshFilter = go.GetComponent<MeshFilter>();
        viewMeshFilter.mesh = viewMesh;

        PizzaObject PO = go.GetComponent<PizzaObject>();
        PO.DOTDamge = DOTDamage;
        PO.DOTLastTime = DOTLastTime;

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
        StartCoroutine(GameManager.Instance.player.EPizzaCD());
        yield return null;
    }

    void DrawFieldOfView(Vector3 targetVec, int n)
    {
        int stepCount = 2;
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        Vector3 vec = targetVec;

        for(int j=0; j<n; j++)
        {
            for (int i = 0; i < stepCount; i++)
            {
                Vector3 direction = Quaternion.AngleAxis(-viewAngle / 2 + stepAngleSize * i * 2, Vector3.back) * vec;
                ViewCastInfo newViewCast = ViewCast(direction);   
                viewPoints.Add(newViewCast.point);
            }
            vec = Quaternion.AngleAxis(360f/n, Vector3.back) * vec;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];
        
        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount-1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
        }

        for (int i=0; i<n; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = 2*i + 1;
            triangles[i * 3 + 2] = 2*i + 2;
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

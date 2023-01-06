using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //플레이어 속도
    [SerializeField] private float speed;
    //플레이어 가속도(이동에 가속이 존재한다면)
    [SerializeField] private float accel;
    //최대 체력
    [SerializeField] private float maxHP;
    //현재 체력
    [SerializeField] private float curHP;
    //최대 분노게이지량
    [SerializeField] private float maxAngerGauge;
    //현재 분노게이지량
    [SerializeField] private float curAngerGauge;
    private Camera mainCamera;
    private Vector3 mousePosition;
    private Vector3 targetPosition;
    private void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        targetPosition = transform.position;
    }

    private void Update()
    {
        //Input에 따른 이동
        if(Input.GetMouseButtonDown(1))
        {
            mousePosition = Input.mousePosition;
            mousePosition.z = mainCamera.farClipPlane;
            targetPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            targetPosition.y = transform.position.y;
            Debug.Log(targetPosition);
        }
        transform.position += speed * (targetPosition - transform.position).normalized * Time.deltaTime;
    }

    //대쉬

    //Input에 따른 각 공격

    //분노 게이지 시스템

    
}
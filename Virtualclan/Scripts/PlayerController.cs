using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    int goParam = Animator.StringToHash("goParam");
    [Header("Movement")]
    public float maxForwardSpeed = 5f;
    [Range(4, 8)]
    public float maxBackwardSpeed = 4f;
    // run限定速度=5
    private float runSpeed = 5;
    bool canMove = true;
    float speed;
    float rotateSpeed;
    [Range(1, 60)]
    public float acceleration = 20.0f;
    [Range(0, 500)]
    public float maxRotateSpeed = 150f;
    [Range(0, 2500)]
    public float rotateAcceleration = 600f;
    Vector3 moveDelta = Vector3.zero;
    public List<EnemyController> enemies;
    public GameObject enemyPoint;
    public GameObject playerPoint;
    public Transform minMap;
    float lastTime = 0;
    public Camera m_Camera;

    PlayerInputHandler m_PlayerInputeHandler;
    CharacterController controller;

    void Start()
    {
        m_PlayerInputeHandler = GetComponent<PlayerInputHandler>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 移动
        if (canMove)
        {
            getSpeed();
            if (speed != 0)
            {
                MoveHandle();
            }
            else
            {
                animator.SetFloat(goParam, speed);
            }
        }

        // 始终着地
        controller.Move(Vector3.down * 0.1f);

        // 每3秒更新小地图
        if ((Time.time - lastTime) > 3f)
        {
            UpdateMinMap();
        }
    }

    private void getSpeed()
    {
        // 刷新前进速度，旋转速度
        var targetSpeed = Input.GetAxisRaw("Vertical");
        targetSpeed *= targetSpeed > 0 ? maxForwardSpeed : maxBackwardSpeed;
        if (targetSpeed > speed)
        {
            speed = Mathf.Min(targetSpeed, speed + acceleration * Time.deltaTime);
        }
        else if (targetSpeed < speed)
        {
            speed = Mathf.Max(targetSpeed, speed - acceleration * Time.deltaTime);
        }

        var targetRotateSpeed = Input.GetAxisRaw("Horizontal");
        targetRotateSpeed *= maxRotateSpeed;
        if (targetRotateSpeed > rotateSpeed)
        {
            rotateSpeed = Mathf.Min(targetRotateSpeed, rotateSpeed + rotateAcceleration * Time.deltaTime);
        }
        else if (targetRotateSpeed < rotateSpeed)
        {
            rotateSpeed = Mathf.Max(targetRotateSpeed, rotateSpeed - rotateAcceleration * Time.deltaTime);
        }

        moveDelta = new Vector3(0, moveDelta.y, speed + 5);
        moveDelta = transform.TransformDirection(moveDelta);
    }

    private void MoveHandle()
    {
        // (1)旋转
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        // (2)移动
        controller.Move(moveDelta * Time.deltaTime);
        // (3)动画
        animator.SetFloat(goParam, speed + runSpeed);
    }

    private void UpdateMinMap()
    {
        // (1)清空小地图上一次的所有点
        foreach (Transform oldPoint in minMap)
        {
            if (null != oldPoint)
            {
                Destroy(oldPoint.gameObject);
            }
        }
        // (2)将玩家置于中心点
        GameObject playerPointImage = Instantiate(playerPoint, minMap);
        playerPointImage.name = "playerPoint";
        playerPointImage.transform.SetParent(minMap, false);//如果没有false默认实例化在世界坐标
        lastTime = Time.time;
        // (3)动态增加敌人
        // 获取所有敌人碰撞体
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 20, (enemies[0].transform.position - transform.position).normalized, 20);
        // 距离distance:小地图半径为43，所以:实际距离*4=minmap上敌人距离玩家的距离=distance
        // 角度x:自由角度摄像机与玩家于xz平面的投影方向与玩家敌人方向的夹角=x
        // 综上：敌人坐标=(distance*sinx,distance*cosx,0)
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(hit.transform.position, transform.position);
                Vector3 player2Enemydir = (hit.transform.position - transform.position).normalized;
                Vector3 camera2PlayerDir = Vector3.ProjectOnPlane(transform.position - m_Camera.transform.position, Vector3.up);
                float angle = Vector3.SignedAngle(camera2PlayerDir, player2Enemydir, Vector3.up);
                GameObject enemyPointImage = Instantiate(enemyPoint, minMap);
                enemyPointImage.name = "enemyPoint";
                enemyPointImage.transform.SetParent(minMap, false);
                distance = Mathf.Clamp(distance * 4, 0, 90);
                enemyPointImage.transform.localPosition = new Vector3(distance * (Mathf.Sin(angle * Mathf.Deg2Rad)), distance * (Mathf.Cos(angle * Mathf.Deg2Rad)), 0);
            }
        }
    }

}

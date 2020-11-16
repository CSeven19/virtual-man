using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public PatrolPath patrolPath { get; set; } // 路径点,于PatrolPath.cs中被初始化
    int m_PathDestinationNodeIndex;
    public UnityEngine.AI.NavMeshAgent m_NavMeshAgent { get; private set; }
    public float pathReachingRadius = 2f;
    public PlayerController m_PlayerController;

    void Start()
    {
        m_NavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }


    void Update()
    {
        if (!isPlayerInDetectiveRadius())
        {
            UpdatePathDestination();
            SetNavDestination(GetDestinationOnPath());
        }
        else
        {
            SetNavDestination(m_PlayerController.transform.position);
        }
        if(canAttack()){
            SceneManager.LoadSceneAsync("Battle");
        }
    }

    // 更新目标路径点
    public void UpdatePathDestination(bool inverseOrder = false)
    {
        if (IsPathValid())
        {
            // Check if reached the path destination
            if ((transform.position - GetDestinationOnPath()).magnitude <= pathReachingRadius)
            {
                // 更新路径点的变化
                // increment path destination index
                m_PathDestinationNodeIndex = inverseOrder ? (m_PathDestinationNodeIndex - 1) : (m_PathDestinationNodeIndex + 1);
                if (m_PathDestinationNodeIndex < 0)
                {
                    m_PathDestinationNodeIndex += patrolPath.pathNodes.Count;
                }
                if (m_PathDestinationNodeIndex >= patrolPath.pathNodes.Count)
                {
                    m_PathDestinationNodeIndex -= patrolPath.pathNodes.Count;
                }
            }
        }
    }


    // 获取目标路径点Vector3
    public Vector3 GetDestinationOnPath()
    {
        if (IsPathValid())
        {
            return patrolPath.GetPositionOfPathNode(m_PathDestinationNodeIndex);
        }
        else
        {
            return transform.position;
        }
    }

    // 设置目标路径点给导航代理
    public void SetNavDestination(Vector3 destination)
    {
        if (m_NavMeshAgent)
        {
            m_NavMeshAgent.SetDestination(destination);
        }
    }

    // 寻路与导航相关
    // 路径点存在且数量大于0
    private bool IsPathValid()
    {
        return patrolPath && patrolPath.pathNodes.Count > 0;
    }

    private bool isPlayerInDetectiveRadius()
    {
        Ray mRay = new Ray(transform.position, (m_PlayerController.transform.position - transform.position).normalized);
        RaycastHit hitInfo;
        if (Physics.Raycast(mRay, out hitInfo, 5))
        {
            return hitInfo.collider.CompareTag("Player") ? true : false;
        }
        return false;
    }

    private bool canAttack()
    {
        Ray mRay = new Ray(transform.position, (m_PlayerController.transform.position - transform.position).normalized);
        RaycastHit hitInfo;
        if (Physics.Raycast(mRay, out hitInfo, 1))
        {
            return hitInfo.collider.CompareTag("Player") ? true : false;
        }
        return false;
    }
}

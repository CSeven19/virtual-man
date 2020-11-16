using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleController : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent m_NavMeshAgent { get; private set; }
    public Transform playerTransform;
    public Transform originalTransform;
    public ActionSort m_ActionSort;

    // Start is called before the first frame update
    void Start()
    {
        m_NavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_ActionSort.isEnemyAttack)
        {
            this.SetNavDestination(playerTransform.position);
        }
        else
        {
            this.SetNavDestination(originalTransform.position);
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
}

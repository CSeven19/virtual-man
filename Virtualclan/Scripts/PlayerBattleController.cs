using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleController : MonoBehaviour
{
    Animator animator;
    int goParam = Animator.StringToHash("goParam");
    public UnityEngine.AI.NavMeshAgent m_NavMeshAgent { get; private set; }
    public Transform enemyTransform;
    public Transform originalTransform;
    public ActionSort m_ActionSort;
    public GameObject particle1;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        m_NavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_ActionSort.isEnemyAttack)
        {
            animator.SetFloat(goParam, 6f);
            this.SetNavDestination(enemyTransform.position);
        }
        else
        {
            this.SetNavDestination(originalTransform.position);
        }
        if (Input.GetKeyDown(KeyCode.Q)){
            Instantiate(particle1,enemyTransform);
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

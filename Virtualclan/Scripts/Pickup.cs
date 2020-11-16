using UnityEngine;
using UnityEngine.Events;
// 拾取物基业务类
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Pickup : MonoBehaviour
{
    public UnityAction onPick;
    public Rigidbody pickupRigidbody { get; private set; }

    Collider m_Collider;
    Vector3 m_StartPosition;
    bool m_HasPlayedFeedback;

    private void Start()
    {
        // 获取物理相关组件rb,collider
        pickupRigidbody = GetComponent<Rigidbody>();
        DebugUtility.HandleErrorIfNullGetComponent<Rigidbody, Pickup>(pickupRigidbody, this, gameObject);
        m_Collider = GetComponent<Collider>();
        DebugUtility.HandleErrorIfNullGetComponent<Collider, Pickup>(m_Collider, this, gameObject);

        // 确保拾取物为不受物理系统的可触发的对象
        // ensure the physics setup is a kinematic rigidbody trigger
        pickupRigidbody.isKinematic = true;
        m_Collider.isTrigger = true;

        // Remember start position for animation
        m_StartPosition = transform.position;
    }

    private void Update()
    {

    }

    // 触发器触发pick逻辑
    private void OnTriggerEnter(Collider other)
    {
        if (onPick != null)
        {
            onPick.Invoke(); // 反射pickup各自对应的处理函数(比如JetpackPickup.cs的OnPicked())
        }
        
    }
}

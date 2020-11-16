using UnityEngine;
// 拾取派生业务类-物品
public class ObjectPickup : MonoBehaviour
{

    Pickup m_Pickup;
    public BagController m_BagController;
    private Obj obj;
    void Start()
    {   
        obj = GetComponent<Obj>();
        m_Pickup = GetComponent<Pickup>();
        DebugUtility.HandleErrorIfNullGetComponent<Pickup, ObjectPickup>(m_Pickup, this, gameObject);
        m_Pickup.onPick += OnPicked;
    }

    void OnPicked()
    {
        BagController.objs.Add(Instantiate(obj)); // 必须复制，不然Destroy(gameObject)后obj变为空引用
        Destroy(gameObject);
    }
}

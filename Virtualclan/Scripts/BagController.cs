using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BagController : MonoBehaviour
{
    public GameObject inventory;
    public static List<Obj> objs;
    // Start is called before the first frame update
    void Start()
    {
        objs = new List<Obj>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActiveBagUI()
    {
        if (null != inventory)
        {
            if (inventory.activeSelf)
            {
                inventory.SetActive(false);
            }
            else
            {
                inventory.SetActive(true);
                foreach (Obj obj in objs)
                {
                    if (null != obj)
                    {
                        Transform img1 = inventory.transform.Find("Pickup1");
                        if (null != img1)
                        {
                            // gameobject.transform:是获取当前游戏对象的transform组件.所以在start函数中 gameobject.transform 和this.transform,指向的都是同一个对象。即：gameobject.transform == this.transform == transform
                            // transform.gameobject:可以这么理解为：获取当前transform组件所在的gameobect
                            img1.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}

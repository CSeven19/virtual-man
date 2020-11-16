using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

public class AutoHouse : MonoBehaviour
{
    List<string> m_All_Furniture;
    [Tooltip("house Prefab")]
    public GameObject house;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Post());
        m_All_Furniture = new List<string> { "sofa", "cabinet", "table" };
    }

    IEnumerator Post()
    {
        // (1) 请求
        WWWForm form = new WWWForm();
        // form.AddField("key", "value");
        UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost:21567/getautohouse", form);
        yield return webRequest.SendWebRequest();

        // (2) 响应
        if (webRequest.isHttpError || webRequest.isNetworkError)
            Debug.Log(webRequest.error);
        else
        {
            JsonData data = JsonMapper.ToObject(webRequest.downloadHandler.text);
            IDictionary dict = data as IDictionary;
            DecorateHouse(data, dict);
        }
    }

    // 自动装饰房子
    private void DecorateHouse(JsonData data, IDictionary dict)
    {
        // 计算比例
        Vector3 length = house.transform.Find("Box066").GetComponent<MeshFilter>().mesh.bounds.size;
        float xlength = length.x * transform.lossyScale.x;
        float ylength = length.y * transform.lossyScale.y;
        float width_scale = (float)(data["width"]) / xlength;
        float height_scale = (float)(data["height"]) / ylength;

        // 布局家具
        float distanceXNow = 0;
        float distanceYNow = 0;
        foreach (string key in dict.Keys)
        {
            if (m_All_Furniture.Contains(key))
            {
                GameObject m_Furniture = Instantiate(Resources.Load<GameObject>(key));
                m_Furniture.transform.SetParent(house.transform);
                float xlengthNow = ((float)data[key][1][0] - (float)data[key][0][0]) / (width_scale*20);
                float ylengthNow = ((float)data[key][1][1] - (float)data[key][0][1]) / (height_scale*20);
                m_Furniture.transform.localPosition = new Vector3(xlengthNow-distanceXNow, ylengthNow-distanceYNow, 0.5f);
                distanceXNow += xlengthNow;
                distanceYNow += ylengthNow;
            }
        }
    }
}

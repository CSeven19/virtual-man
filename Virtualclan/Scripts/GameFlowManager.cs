using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    // 鼠标皮肤
    public Texture CursorTexture;

    // Start is called before the first frame update
    void Start()
    {
        // 禁止显示光标
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        // 光标位置修改鼠标皮肤
        Vector3 mousePos = Input.mousePosition;
        //因为GUI坐标系原点是左上角，而屏幕坐标系原点是在左下角，所以要转换  
        GUI.DrawTexture(new Rect(mousePos.x - CursorTexture.width / 2, Screen.height - mousePos.y - CursorTexture.height / 2, CursorTexture.width, CursorTexture.height), CursorTexture);
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadFirstScene : MonoBehaviour
{
    [Tooltip("What is the name of the scene we want to load when clicking the button?")]
    public string SceneName = "FirstScene";
    public Button btn;
    
    void Start(){
        LoadTargetScene();
    }
    public void LoadTargetScene()
    {
        btn.onClick.AddListener(()=>{
            SceneManager.LoadSceneAsync(SceneName);
        });
    }
}

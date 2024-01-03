using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Remember to add Managers scene to Scene in Build Settings
/// </summary>
public class SceneInstantiate : MonoBehaviour
{
    [SerializeField]
    private Object persistentScene;
    private void Awake()
    {
        SceneManager.LoadSceneAsync(persistentScene.name, LoadSceneMode.Additive);
    }
    [ContextMenu("ChangeScene : Second Scene")]
    public void NextScene() 
    {
        SceneManager.UnloadSceneAsync("FirstScene");
        SceneManager.LoadSceneAsync("SecondScene", LoadSceneMode.Additive);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameScene[] GameScenes;

    public static GameManager Instance;
    public int Progress = 0; // 0 = intro, 1 = main menu, 2 = level one, 3 = level two, 4 = level three, 5 = outro

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        LoadSceneInSeconds(SceneType.Intro, 0);        
    }

    public void LoadSceneInSeconds(SceneType sceneType, float seconds = 0)
    {
        StartCoroutine(loadSceneInSeconds(sceneType, seconds));
    }

    IEnumerator loadSceneInSeconds(SceneType sceneType, float seconds)
    {
        Debug.Log($"load scene in {seconds} seconds: {sceneType} = {(int)sceneType}");
        yield return new WaitForSeconds(seconds);
        if ((int)sceneType > Progress)
            Progress = (int)sceneType;
        Debug.Log($"current progress: {Progress}");
        GameObject newScene = Instantiate(getScenePrefab(sceneType));
        newScene.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    GameObject getScenePrefab(SceneType sceneType)
    {
        foreach (GameScene scene in GameScenes)
        {
            if (scene.type == sceneType)
                return scene.prefab;
        }
        Debug.Log("Scene not found");
        return null;
    }
}

public enum SceneType
{
    Intro,
    MainMenu,
    LevelOne,
    LevelTwo,
    LevelThree,
    Outro,
}

[System.Serializable]
public class GameScene
{
    public SceneType type;
    public GameObject prefab;
}
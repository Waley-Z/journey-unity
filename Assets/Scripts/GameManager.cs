using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameScene[] GameScenes;

    public static GameManager Instance;
    public int Progress = 0; // 0 = main menu, 1 = level 1, 2 = level 2, 3 = level 3, 4 = outro

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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
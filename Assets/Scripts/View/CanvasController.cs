using System;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [Serializable]
    public enum SceneType
    {
        Main,
        Treading,
        Inform,
        Next
    }

    [SerializeField] private SceneType sceneType;
    [SerializeField] private GameObject[] sceneObjects;

    private void FixedUpdate()
    {
        UpdateVisibleFalseChild();
        switch (sceneType)
        {
            case SceneType.Main:
                sceneObjects[0].SetActive(true);
                break;
            case SceneType.Treading:
                sceneObjects[1].SetActive(true);
                break;
            case SceneType.Inform:
                sceneObjects[2].SetActive(true);
                break;
            case SceneType.Next:
                sceneObjects[3].SetActive(true);
                break;
        }
    }

    private void UpdateVisibleFalseChild()
    {
        foreach (var sceneObject in sceneObjects) sceneObject.SetActive(false);
    }

    public void ChangeTreadingScene()
    {
        ChangeScene(SceneType.Treading);
    }

    public void ChangeInformScene()
    {
        ChangeScene(SceneType.Inform);
    }

    public void ChangeNextScene()
    {
        ChangeScene(SceneType.Next);
    }

    public void ChangeMainScene()
    {
        ChangeScene(SceneType.Main);
    }

    public void ChangeScene(SceneType scene)
    {
        sceneType = scene;
    }
}
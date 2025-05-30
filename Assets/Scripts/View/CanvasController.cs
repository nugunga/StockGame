using System;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [Serializable]
    public enum SceneType
    {
        Entry = 0,
        Main,
        Treading,
        Inform,
        Next,
        End
    }


    [SerializeField] private SceneType sceneType;
    [SerializeField] private GameObject[] sceneObjects;
    [SerializeField] private GameObject globalPanel;

    private void OnValidate()
    {
        UIUpdate();
    }

    private void UpdateVisibleFalseChild()
    {
        foreach (var sceneObject in sceneObjects) sceneObject.SetActive(false);
    }

    public void ChangeEntryScene()
    {
        ChangeScene(SceneType.Entry);
    }

    public void ChangeMainScene()
    {
        ChangeScene(SceneType.Main);
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

    public void ChangeEndScene()
    {
        ChangeScene(SceneType.End);
    }

    public void ChangeScene(SceneType scene)
    {
        sceneType = scene;
        UIUpdate();
    }

    public void UIUpdate()
    {
        UpdateVisibleFalseChild();
        sceneObjects[(int)sceneType].SetActive(true);
        globalPanel.SetActive(sceneType != SceneType.Entry);
    }
}
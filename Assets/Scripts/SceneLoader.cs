using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneLoader", menuName = "ScriptableObjects/SceneLoader", order = 1)]
public class SceneLoader : ScriptableObject
{
    public void NextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void PrevScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

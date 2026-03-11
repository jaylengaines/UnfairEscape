using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour
{
    private static readonly HashSet<string> CursorUnlockScenes = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase)
    {
        "GameOverScene",
        "TitleScreen",
        "WinScreen"
    };

    public void LoadSceneByName(string sceneName)
    {
        ApplyCursorForScene(sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        ApplyCursorForScene(currentSceneName);
        SceneManager.LoadScene(currentSceneName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public static void ApplyCursorForScene(string sceneName)
    {
        if (!CursorUnlockScenes.Contains(sceneName)) return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
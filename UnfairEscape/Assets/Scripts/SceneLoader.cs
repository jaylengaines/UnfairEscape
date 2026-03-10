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
        UnlockCursorIfNeeded(sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        UnlockCursorIfNeeded(currentSceneName);
        SceneManager.LoadScene(currentSceneName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    private void UnlockCursorIfNeeded(string sceneName)
    {
        if (!CursorUnlockScenes.Contains(sceneName)) return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
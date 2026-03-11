using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManagement : MonoBehaviour
{
    public void LoadLevel(int levelIndex)
    {
        StartCoroutine(LoadScene(levelIndex));
    }

    IEnumerator LoadScene(int levelIndex)
    {
        AudioManager.Instance.PlayClick();
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(levelIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
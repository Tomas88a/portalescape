using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public Button backToGame;
    public Button restartGame;
    public Button exitGame;
    // Start is called before the first frame update
    void Start()
    {
        backToGame.onClick.AddListener(BackToGame);
        restartGame.onClick.AddListener(RestartGame);
        exitGame.onClick.AddListener(ExitGame);
    }



    public void BackToGame()
    {
        Debug.Log("BackToGame");

        // 模拟ESC按键功能 - 查找NewBehaviourScript并切换暂停状态
        NewBehaviourScript playerScript = FindObjectOfType<NewBehaviourScript>();
        if (playerScript != null)
        {
            // 模拟ESC按键的逻辑：切换鼠标锁定状态
            playerScript.TogglePauseState();
        }
        else
        {
            Debug.LogWarning("未找到NewBehaviourScript组件");
        }
    }

    public void RestartGame()
    {
        Debug.Log("RestartGame");

        // 重新加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void ExitGame()
    {
        Debug.Log("ExitGame");
        SceneManager.LoadScene("StartScene");
    }
}

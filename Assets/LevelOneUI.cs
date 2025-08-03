using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelOneUI : MonoBehaviour
{
    public Button uiButton;
    // Start is called before the first frame update
    void Start()
    {
        // 启动协程
        
    }

    // Update is called once per frame
    void Update()
    {
        // 检测鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            if (uiButton != null)
            {
                uiButton.gameObject.SetActive(true);
                Debug.Log("鼠标左键点击，UI按钮已激活");
                StartCoroutine(LogAfterThreeSeconds());
            }
        }
    }

    // 协程：三秒后输出日志
    IEnumerator LogAfterThreeSeconds()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("art_Level2_v3_with_trigger");
    }
}

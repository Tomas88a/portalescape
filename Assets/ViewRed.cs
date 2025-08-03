using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ViewRed : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;
    public GameObject redObject;
    // Start is called before the first frame update
    void Start()
    {
        // 鼠标悬停时触发LogHaHa方法
        // 不需要在这里添加监听器，因为我们使用IPointerEnterHandler接口
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 

    // 实现IPointerEnterHandler接口，当鼠标悬停时触发
    public void OnPointerEnter(PointerEventData eventData)
    {
        redObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        redObject.SetActive(false);
    }
}

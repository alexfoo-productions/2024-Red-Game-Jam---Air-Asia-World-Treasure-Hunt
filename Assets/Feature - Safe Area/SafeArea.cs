using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafeArea : MonoBehaviour
{
    [SerializeField] RectTransform MainUIPanel;

    private void Awake()
    {
        Debug.Log(Screen.safeArea);
        Debug.Log(Screen.width);
        Debug.Log(Screen.height);
        Debug.Log(MainUIPanel.sizeDelta.x);
        Debug.Log(Screen.safeArea.yMax);

        Vector2 mainPosMin = MainUIPanel.anchorMin;
        mainPosMin.y = Screen.safeArea.y / Screen.height;
        MainUIPanel.anchorMin = mainPosMin;

        Vector2 mainPosMax = MainUIPanel.anchorMax;
        mainPosMax.y = 1- ((Screen.safeArea.y / Screen.height) * 1);
        MainUIPanel.anchorMax = mainPosMax;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

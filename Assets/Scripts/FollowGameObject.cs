using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    public Vector3 UIoffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
            ((RectTransform)transform).anchoredPosition = Camera.main.WorldToScreenPoint(target.transform.position + offset) + UIoffset;
    }
}

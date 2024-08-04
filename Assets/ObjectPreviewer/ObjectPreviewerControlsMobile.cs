using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Library;

public class ObjectPreviewerControlsMobile : ObjectPreviewerControls
{
    protected override IEnumerator checkIsUIonClick(doAfterUICheck doThis)
    {
        yield return new WaitForEndOfFrame();

#if !UNITY_EDITOR && UNITY_WEBGL
        if (WebglUtils.CheckIfMobile())
        {
            if (IsPointerOverUIObject(Touchscreen.current.primaryTouch.position.ReadValue()))
            {
                //LogUtil.Log("UI Detected Touch");
                yield break;
            }
        }
        else
#endif
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //LogUtil.Log("UI Detected Mouse");
                yield break;
            }
        }

        yield return doThis();
    }
}
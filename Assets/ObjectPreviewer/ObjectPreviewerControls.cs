using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Library;

public class ObjectPreviewerControls : MonoBehaviour
{
    public enum ORBIT_SCHEME
    {
        NULL,
        KEYBOARD,
        POINTER
    }

    public enum DOLLY_SCHEME
    {
        NULL,
        KEYBOARD,
        MOUSE,
        TOUCH
    }

    public ObjectPreviewer objectPreviewer;

    public ObjectPreviewerInputAction objectPreviewerInputAction;
    Vector2 deltaOrbit;
    Vector2 deltaDolly;
    Vector2 touch0Position;
    Vector2 touch1Position;
    float pinchAmount;

    public ORBIT_SCHEME orbit_scheme;
    public DOLLY_SCHEME dolly_scheme;

    protected delegate IEnumerator doAfterUICheck();

    void Start()
    {
        deltaOrbit = Vector2.zero;
        
        objectPreviewerInputAction = new ObjectPreviewerInputAction();        

        // ORBIT
        objectPreviewerInputAction.ObjectPreviewerActionMap.OrbitKeyboard.started += OrbitKeyboard_started;
        objectPreviewerInputAction.ObjectPreviewerActionMap.OrbitKeyboard.performed += OrbitKeyboard_performed;
        objectPreviewerInputAction.ObjectPreviewerActionMap.OrbitKeyboard.canceled += OrbitKeyboard_canceled;

        objectPreviewerInputAction.ObjectPreviewerActionMap.OrbitMouseActivation.started += OrbitMouseActivation_started;
        objectPreviewerInputAction.ObjectPreviewerActionMap.OrbitMouseActivation.canceled += OrbitMouseActivation_canceled;

        objectPreviewerInputAction.ObjectPreviewerActionMap.OrbitMouse.started += OrbitMouse_started;
        objectPreviewerInputAction.ObjectPreviewerActionMap.OrbitMouse.performed += OrbitMouse_performed;
        objectPreviewerInputAction.ObjectPreviewerActionMap.OrbitMouse.canceled += OrbitMouse_canceled;

        // DOLLY
        objectPreviewerInputAction.ObjectPreviewerActionMap.DollyMouse.started += DollyMouse_started;
        objectPreviewerInputAction.ObjectPreviewerActionMap.DollyMouse.performed += DollyMouse_performed;
        objectPreviewerInputAction.ObjectPreviewerActionMap.DollyMouse.canceled += DollyMouse_canceled;

        objectPreviewerInputAction.ObjectPreviewerActionMap.DollyTouch1.started += DollyTouch1_started;
        objectPreviewerInputAction.ObjectPreviewerActionMap.DollyTouch1.performed += DollyTouch1_performed;
        objectPreviewerInputAction.ObjectPreviewerActionMap.DollyTouch1.canceled += DollyTouch1_canceled;

        objectPreviewerInputAction.ObjectPreviewerActionMap.DollyTouch2Activation.started += DollyTouch2Activation_started;
        objectPreviewerInputAction.ObjectPreviewerActionMap.DollyTouch2Activation.canceled += DollyTouch2Activation_canceled;

        objectPreviewerInputAction.ObjectPreviewerActionMap.DollyTouch2.started += DollyTouch2_started;
        objectPreviewerInputAction.ObjectPreviewerActionMap.DollyTouch2.performed += DollyTouch2_performed;
        objectPreviewerInputAction.ObjectPreviewerActionMap.DollyTouch2.canceled += DollyTouch2_canceled;

        objectPreviewer.init();

        //LogUtil.Log(Screen.width + ", " + Screen.height);
    }

    private void OnDisable()
    {        
        objectPreviewerInputAction.ObjectPreviewerActionMap.Disable();
    }

    // Pointer
    private void OrbitMouseActivation_started(InputAction.CallbackContext obj)
    {        
        doAfterUICheck func = null;
        switch (obj.interaction)
        {
            case UnityEngine.InputSystem.Interactions.PressInteraction:
                func = startOrbit;
                break;

            case UnityEngine.InputSystem.Interactions.MultiTapInteraction:                
                func = doReset;
                break;
        }

        if (EventSystem.current != null)
            StartCoroutine(checkIsUIonClick(func));
        else
            StartCoroutine(func());
    }

    IEnumerator startOrbit()
    {        
        if (orbit_scheme != ORBIT_SCHEME.NULL)
            yield break;

        orbit_scheme = ORBIT_SCHEME.POINTER;
    }

    IEnumerator doReset()
    {        
        objectPreviewer.resetCamera();
        yield break;
    }

    protected virtual IEnumerator checkIsUIonClick(doAfterUICheck doThis)
    {        
        yield return new WaitForEndOfFrame();

        if (EventSystem.current.IsPointerOverGameObject())
        {
            //LogUtil.Log("UI Detected Mouse");
            yield break;
        }

        yield return doThis();        
    }

    /// <summary>
    /// Cast a ray to test if Input.mousePosition is over any UI object in EventSystem.current. This is a replacement
    /// for IsPointerOverGameObject() which does not work on Android in 4.6.0f3
    /// </summary>
    protected bool IsPointerOverUIObject(Vector2 pos)
    {
        // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
        // the ray cast appears to require only eventData.position.
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(pos.x, pos.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void OrbitMouseActivation_canceled(InputAction.CallbackContext obj)
    {
        // turns off dolly if 1st finger is lifted
        if (dolly_scheme != DOLLY_SCHEME.NULL)
        {
            resetDolly();
        }

        if (orbit_scheme == ORBIT_SCHEME.POINTER)
            orbit_scheme = ORBIT_SCHEME.NULL;
        else
            return;

        deltaOrbit = Vector2.zero;        
    }

    private void OrbitMouse_started(InputAction.CallbackContext obj)
    {        
        if (orbit_scheme != ORBIT_SCHEME.NULL)
            return;

        orbit_scheme = ORBIT_SCHEME.POINTER;

        deltaOrbit = obj.ReadValue<Vector2>();        
    }
        
    private void OrbitMouse_performed(InputAction.CallbackContext obj)
    {
        if (orbit_scheme == ORBIT_SCHEME.POINTER)
        {            
            deltaOrbit = obj.ReadValue<Vector2>();

            orbit();
        }
    }

    private void OrbitMouse_canceled(InputAction.CallbackContext obj)
    {
        if (orbit_scheme == ORBIT_SCHEME.POINTER)
            orbit_scheme = ORBIT_SCHEME.NULL;
        else
            return;

        deltaOrbit = Vector2.zero;        
    }

    private void DollyMouse_started(InputAction.CallbackContext obj)
    {   
        deltaDolly = obj.ReadValue<Vector2>();
        dolly();
    }

    private void DollyMouse_performed(InputAction.CallbackContext obj)
    {   
        deltaDolly = obj.ReadValue<Vector2>();
        dolly();
    }

    private void DollyMouse_canceled(InputAction.CallbackContext obj)
    {     
        deltaDolly = Vector2.zero;
        dolly();
    }

    // Keyboard
    private void OrbitKeyboard_started(InputAction.CallbackContext obj)
    {
        if (orbit_scheme != ORBIT_SCHEME.NULL)
            return;

        orbit_scheme = ORBIT_SCHEME.KEYBOARD;
        
        deltaOrbit = obj.ReadValue<Vector2>();        
        orbit();
    }

    private void OrbitKeyboard_performed(InputAction.CallbackContext obj)
    {        
        if (orbit_scheme == ORBIT_SCHEME.KEYBOARD)
        {
            orbit_scheme = ORBIT_SCHEME.KEYBOARD;

            deltaOrbit = obj.ReadValue<Vector2>();
            orbit();
        }
    }

    private void OrbitKeyboard_canceled(InputAction.CallbackContext obj)
    {
        if (orbit_scheme == ORBIT_SCHEME.KEYBOARD)
            orbit_scheme = ORBIT_SCHEME.NULL;
        else
            return;

        deltaOrbit = Vector2.zero;        
    }

    // Touch
    private void DollyTouch1_started(InputAction.CallbackContext obj)
    {
        
    }

    private void DollyTouch1_performed(InputAction.CallbackContext obj)
    {
        touch0Position = obj.ReadValue<Vector2>();
        
        if (dolly_scheme == DOLLY_SCHEME.TOUCH)
        {            
            pinch();
        }
        else
        {

        }
    }

    private void DollyTouch1_canceled(InputAction.CallbackContext obj)
    {
        
    }
    
    private void DollyTouch2Activation_started(InputAction.CallbackContext obj)
    {   
        dolly_scheme = DOLLY_SCHEME.TOUCH;

        // disables orbit when dollying
        if (orbit_scheme != ORBIT_SCHEME.NULL)
            orbit_scheme = ORBIT_SCHEME.NULL;

        // set initial pinch value for comparison when dolly pinch is performed later
        deltaDolly.y = Vector2.Distance(touch0Position, touch1Position) / Screen.width * 50f;
    }

    private void DollyTouch2Activation_canceled(InputAction.CallbackContext obj)
    {        
        deltaDolly = Vector2.zero;
        dolly();
        dolly_scheme = DOLLY_SCHEME.NULL;
    }

    private void DollyTouch2_started(InputAction.CallbackContext obj)
    {
        
    }

    private void DollyTouch2_performed(InputAction.CallbackContext obj)
    {
        if (dolly_scheme == DOLLY_SCHEME.TOUCH)
        {
            touch1Position = obj.ReadValue<Vector2>();
            pinch();
            dolly_scheme = DOLLY_SCHEME.TOUCH;
        }
    }

    private void DollyTouch2_canceled(InputAction.CallbackContext obj)
    {
        resetDolly();
        dolly();
    }

    void pinch()
    {        
        pinchAmount = Vector2.Distance(touch0Position, touch1Position) / Screen.width * 10f;

        deltaDolly.y = pinchAmount * ((pinchAmount < Mathf.Abs(deltaDolly.y))?-1:1);
        
        dolly();
    }

    void orbit()
    {
        if (dolly_scheme != DOLLY_SCHEME.NULL)
            return;

        switch (orbit_scheme)
        {
            case ORBIT_SCHEME.KEYBOARD:
            case ORBIT_SCHEME.POINTER:
                
                objectPreviewer.orbit(deltaOrbit);
                break;
        }
    }

    void resetDolly()
    {
        deltaDolly = Vector2.zero;
        dolly_scheme = DOLLY_SCHEME.NULL;
    }

    void dolly()
    {        
        objectPreviewer.dolly(deltaDolly.y);
    }

    // Update is called once per frame
    void Update()
    {
        //keyboard performed is triggered once, but ObjectPreviewer Update requires continuos value.
        if(orbit_scheme == ORBIT_SCHEME.KEYBOARD)
        {
            orbit();
        }
    }
}

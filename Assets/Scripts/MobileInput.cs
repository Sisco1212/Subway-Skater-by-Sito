using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static MobileInput Instance { set; get; }
    private const float DEADZONE = 100.0f;

    private bool tap, swipeRight, swipeLeft, swipeUp, swipeDown;
    private Vector2 swipeDelta, startTouch;

    public bool Tap { get { return tap;} }
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // reset the booleans to false;
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;
    
    #region Standalone Inputs
    if(Input.GetMouseButtonDown(0)) {
        tap = true;
        startTouch = Input.mousePosition;
    }
    else if (Input.GetMouseButtonUp(0)) {
        startTouch = swipeDelta = Vector2.zero;
    }

    #endregion
    

    #region Mobile Inputs
    if(Input.touches.Length != 0) {
        if(Input.touches[0].phase == TouchPhase.Began) {
        tap = true;
        startTouch = Input.mousePosition;
        }
    else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled) {
        startTouch = swipeDelta = Vector2.zero;
    }
    }

    #endregion

    //Calculate distance
    swipeDelta = Vector2.zero;
    if(startTouch != Vector2.zero) {
        if(Input.touches.Length != 0) {
        swipeDelta = Input.touches[0].position - startTouch;
        }
        else if(Input.GetMouseButton(0)) {
            swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }
    }

    //Check if we're beyond the deadzone
    if(swipeDelta.magnitude > DEADZONE) {
        //this is a confirmed swipe
        float x = swipeDelta.x;
        float y = swipeDelta.y;

        //check which direction has higher swipe
        if(Mathf.Abs(x) > Mathf.Abs(y)){
            //We're on right or left
            if(x < 0)
            swipeLeft = true;
            else 
            swipeRight = true;
        }
        else {
            //We're on up or down
            if(y < 0)
            swipeDown = true;
            else 
            swipeUp = true;
        }
    startTouch = swipeDelta = Vector2.zero;
    }
}


}

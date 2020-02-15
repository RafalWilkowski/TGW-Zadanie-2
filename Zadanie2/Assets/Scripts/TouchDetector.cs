using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TouchDetector : MonoBehaviour
{
 
    public static Action<Vector2> onFingerMoved;
    public static Action<Vector2> onFingerReleased;

    public LayerMask _layerToDetect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);       

            if (touch.phase == TouchPhase.Began)
            {
                // cr8 a ray from camera to world
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0));
                // get collider info
                RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, _layerToDetect);

                if (hitInfo.collider != null)
                {
                    IInteractable interactable = hitInfo.collider.GetComponent<IInteractable>();
                    if(interactable != null)
                    {
                        interactable.Interact();
                    }
                    else
                    {
                        Debug.Log("Didnt hit anything");
                    }
                }

                onFingerMoved?.Invoke(touch.position);
            }
            if(touch.phase == TouchPhase.Moved)
            {
                onFingerMoved?.Invoke(touch.position);
            }
            if(touch.phase == TouchPhase.Ended)
            {
                onFingerMoved?.Invoke(touch.position);
                onFingerReleased?.Invoke(touch.position);
            }

        }
        
        //detect touch on layer
        //detect object
        // send delegates

    }
}

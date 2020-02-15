using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TouchDetector : MonoBehaviour
{
    public static Action<Vector2> onFingerMoved;
    public static Action<Vector2> onFingerReleased;

    public LayerMask _layerToDetect;

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // touch position to world
            Vector2 touchScreenToWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.transform.position.z));
            if (touch.phase == TouchPhase.Began)
            {
                // cr8 a ray from camera to world
                Vector3 rayOrigin = new Vector3(touchScreenToWorldPosition.x, touchScreenToWorldPosition.y, Camera.main.transform.position.z);
                Ray ray = new Ray(rayOrigin, Vector3.forward);
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

                onFingerMoved?.Invoke(touchScreenToWorldPosition);
            }
            if(touch.phase == TouchPhase.Stationary)
            {
                onFingerMoved?.Invoke(touchScreenToWorldPosition);
            }
            if(touch.phase == TouchPhase.Moved)
            {
                onFingerMoved?.Invoke(touchScreenToWorldPosition);
            }
            if(touch.phase == TouchPhase.Ended)
            {
                onFingerMoved?.Invoke(touchScreenToWorldPosition);
                onFingerReleased?.Invoke(touchScreenToWorldPosition);
            }

        }        

    }
}

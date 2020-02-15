using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TouchDetector : MonoBehaviour
{
    public static Dictionary<int, Action<Vector2>> onFingerMovedDic = new Dictionary<int, Action<Vector2>>();
    public static Dictionary<int, Action<Vector2>> onFingerReleasedDic = new Dictionary<int, Action<Vector2>>();

    public LayerMask _layerToDetect;
    [SerializeField]
    private float _tapSize = 0.25f;

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            foreach(Touch touch in Input.touches)
            {
                Vector2 touchScreenToWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.transform.position.z));
                int touchID = touch.fingerId;
                if (touch.phase == TouchPhase.Began)
                {
                    // cr8 a ray from camera to world
                    Vector3 rayOrigin = new Vector3(touchScreenToWorldPosition.x, touchScreenToWorldPosition.y, Camera.main.transform.position.z);
                    Ray ray = new Ray(rayOrigin, Vector3.forward);
                    // get collider info
                    RaycastHit2D[] hitInfos = Physics2D.CircleCastAll(rayOrigin, _tapSize, Vector3.forward, Mathf.Infinity, _layerToDetect);//Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, _layerToDetect);
                    RaycastHit2D closestHitObject;
                    
                    if(hitInfos.Length > 0)
                    {
                        //findingg closest gem to touch point
                        closestHitObject = hitInfos[0];
                        float closestDistance = Vector2.Distance(touchScreenToWorldPosition, closestHitObject.collider.transform.position);
                        foreach (RaycastHit2D hit in hitInfos)
                        {
                            float distance = Vector2.Distance(touchScreenToWorldPosition, hit.collider.transform.position);
                            if(distance < closestDistance)
                            {
                                closestDistance = distance;
                                closestHitObject = hit;
                            }
                        }
                        // checking if object is interactable
                        if (closestHitObject.collider != null)
                        {
                            IInteractable interactable = closestHitObject.collider.GetComponent<IInteractable>();
                            if (interactable != null)
                            {
                                interactable.Interact(touchID);
                                if (onFingerMovedDic.ContainsKey(touchID))
                                {
                                    onFingerMovedDic[touchID]?.Invoke(touchScreenToWorldPosition);
                                }

                            }
                            else
                            {
                                Debug.Log("Didnt hit anything");
                            }
                        }
                    }

                    

                   

                    
                }
                if (onFingerMovedDic.ContainsKey(touchID))
                {
                    if (touch.phase == TouchPhase.Stationary)
                    {
                        onFingerMovedDic[touchID]?.Invoke(touchScreenToWorldPosition);
                    }
                    if (touch.phase == TouchPhase.Moved)
                    {
                        onFingerMovedDic[touchID]?.Invoke(touchScreenToWorldPosition);
                    }
                    if (touch.phase == TouchPhase.Ended)
                    {
                        onFingerMovedDic[touchID]?.Invoke(touchScreenToWorldPosition);
                        onFingerReleasedDic[touchID]?.Invoke(touchScreenToWorldPosition);
                    }
                }
                
            }
            /*Touch touch = Input.GetTouch(0);            
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
                        interactable.Interact(touch.fingerId);
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
            */
        }        

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TouchDetector : MonoBehaviour
{
    public static TouchDetector Instance;

    public Dictionary<int, Action<Vector2>> onFingerMovedDic = new Dictionary<int, Action<Vector2>>();
    public Dictionary<int, Action<Vector2>> onFingerReleasedDic = new Dictionary<int, Action<Vector2>>();

    public LayerMask _layerToDetect;
    [SerializeField]
    private float _tapBaseSize = 0.25f;
    [SerializeField]
    private float _maxTapSize = 0.75f;
    private float _currentTapSize = 0;
    private float _tapSizeAccel;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _currentTapSize = _tapBaseSize;
    }
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
                    RaycastHit2D[] hitInfos = Physics2D.CircleCastAll(rayOrigin, _currentTapSize, Vector3.forward, Mathf.Infinity, _layerToDetect);
                    RaycastHit2D closestHitObject;
                    
                    if(hitInfos.Length > 0)
                    {
                        //findingg closest gem to touch point
                        closestHitObject = hitInfos[0];
                        float closestDistance = Vector2.Distance(touchScreenToWorldPosition, closestHitObject.collider.transform.position);
                        // if there are more than 2 objects
                        if(hitInfos.Length > 1)
                        {
                            for(int i = 1; i < hitInfos.Length; i++)
                            {
                                float distance = Vector2.Distance(touchScreenToWorldPosition, hitInfos[i].collider.transform.position);
                                if (distance < closestDistance)
                                {
                                    closestDistance = distance;
                                    closestHitObject = hitInfos[i];
                                }
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
        }              
    }
    public void SetTapSize(float tapSizeThresholds, float currentThresholds)
    {
        // tap size accel before game started
        _tapSizeAccel = (_maxTapSize - _tapBaseSize) / tapSizeThresholds;
        // set current tap before game started
        _currentTapSize += _tapSizeAccel * (tapSizeThresholds - currentThresholds);
        // tap size acceleratin left to maximum speed now
        if(currentThresholds != 0)
        {
            _tapSizeAccel = (_maxTapSize - _currentTapSize) / currentThresholds;
        }       
    }

    public void IncreaseTapSize(float tapSizeThresholds)
    {
        if(!(_currentTapSize >= _maxTapSize)) _currentTapSize += _tapSizeAccel;
    }

}

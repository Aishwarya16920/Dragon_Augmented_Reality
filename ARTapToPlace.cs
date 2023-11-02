using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlace : MonoBehaviour
{
    
    
    
    
    private ARRaycastManager raycastManager;
    private GameObject SpawnedObject;
    
    [SerializeField]
    private GameObject PlacePrefab;

    private static List<ARRaycastHit> s_hits = new List<ARRaycastHit>();

    private void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }
    
    public static bool IsDoubleTap(){
        bool result = false;
        float MaxTimeWait = 1;
        float VariancePosition = 1;
 
        if( Input.touchCount == 1  && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            float DeltaTime = Input.GetTouch (0).deltaTime;
            float DeltaPositionLenght=Input.GetTouch (0).deltaPosition.magnitude;
 
            if ( DeltaTime> 0 && DeltaTime < MaxTimeWait && DeltaPositionLenght < VariancePosition)
                result = true;                
        }
        return result;
    }

    private void Update()
    {
        
        if (IsDoubleTap())
        {
            if (!TryGetTouchPosition(out Vector2 touchPosition))
            {
                return;
            }

            if (raycastManager.Raycast(touchPosition, s_hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = s_hits[0].pose;
                if (SpawnedObject == null)
                {
                    SpawnedObject = Instantiate(PlacePrefab, hitPose.position, hitPose.rotation);
                }
                else
                {
                    SpawnedObject.transform.position = hitPose.position;
                    SpawnedObject.transform.rotation = hitPose.rotation;
                }
            }
        }
    }
}

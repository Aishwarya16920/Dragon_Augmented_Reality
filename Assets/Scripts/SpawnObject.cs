using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class SpawnObject : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;

    [SerializeField]  
    private GameObject PlaceablePrefab;

    static List<ARRaycastHit> s_Hits= new List<ARRaycastHit>();

    private void Awake(){
        raycastManager= GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition){
        if(Input.touchCount>0){
            touchPosition= Input.GetTouch(0).position;
            return true;
        }

        touchPosition= default;
        return false;
    }

    private void Update(){
        if(!TryGetTouchPosition(out Vector2 touchPosition)){
            return;
        }
        if(raycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon)){
            var hitPose=s_Hits[0].pose;
            if(spawnedObject==null){
                spawnedObject=Instantiate(PlaceablePrefab,hitPose.position,hitPose.rotation);
            }
            else{
                spawnedObject.transform.position=hitPose.position;
                spawnedObject.transform.rotation=hitPose.rotation;
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;


public class ColocadorDeMuebles : MonoBehaviour
{
    public GameObject[] listaMuebles;

    private GameObject muebleAColocar;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();


        if (listaMuebles.Length > 0) { 
            muebleAColocar = listaMuebles[0];
        }
    }


    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch toque = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject(toque.fingerId))
                return;

            if (toque.phase == TouchPhase.Began)
            {
                if (raycastManager.Raycast(toque.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    Instantiate(muebleAColocar, hitPose.position, hitPose.rotation);
                }
                    
            }
        }

    }

    public void SeleccionarMueble(int indice)
    {
        if (indice >= 0 && indice < listaMuebles.Length)
        {
            muebleAColocar = listaMuebles[indice];
        }
    }
}

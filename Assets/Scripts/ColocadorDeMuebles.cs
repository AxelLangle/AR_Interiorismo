using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class ColocadorDeMuebles : MonoBehaviour
{
    public GameObject[] listaMuebles;
    private GameObject muebleAColocar;
    private GameObject muebleSeleccionado;

    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        if (listaMuebles.Length > 0)
            muebleAColocar = listaMuebles[0];
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
                Ray ray = Camera.main.ScreenPointToRay(toque.position);
                RaycastHit hit3D;
                bool tocoMueble = false;

                // 1. Lanzamos el láser 3D
                if (Physics.Raycast(ray, out hit3D))
                {
                    // 2. Filtro mágico: Si lo que chocamos NO tiene el componente ARPlane (es decir, no es el piso), entonces es un mueble
                    if (hit3D.transform.GetComponentInParent<ARPlane>() == null)
                    {
                        muebleSeleccionado = hit3D.transform.root.gameObject;
                        tocoMueble = true; // ¡Agarramos un mueble!
                    }
                }

                // 3. Si no tocamos ningún mueble, entonces PONEMOS uno nuevo en el piso
                if (!tocoMueble)
                {
                    if (raycastManager.Raycast(toque.position, hits, TrackableType.PlaneWithinPolygon))
                    {
                        Pose hitPose = hits[0].pose;
                        Instantiate(muebleAColocar, hitPose.position, hitPose.rotation);
                    }
                }
            }
            else if (toque.phase == TouchPhase.Moved && muebleSeleccionado != null)
            {
                // Movemos el mueble por la cuadrícula
                if (raycastManager.Raycast(toque.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    muebleSeleccionado.transform.position = hitPose.position;
                }
            }
            else if (toque.phase == TouchPhase.Ended)
            {
                // Soltamos el mueble
                muebleSeleccionado = null;
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
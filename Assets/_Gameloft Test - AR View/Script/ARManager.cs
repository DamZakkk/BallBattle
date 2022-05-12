using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ARManager : MonoBehaviour
{
    ARPlaneManager m_ARPlaneManager;

    Spawned spawned;
    public GameObject cameraAR;
    public Toggle planeVisualToggle;

    bool visualizer;
    Vector3 touchStart;

    [Space]
    public float _sensitivity;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    public Transform ArPos;

    public GameObject ar;
    public GameObject mainGame;
    public GameObject arCanvas;
    public Transform arOrigin;

    bool initAR;

    void Awake()
    {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
    }
    private void Start()
    {
        visualizer = true;
    }


    public void SetAR(bool value)
    {
        if (!initAR)
        {
            InitAR();
            return;
        }

        mainGame.SetActive(true);
        arCanvas.SetActive(true);
        TurnOffRealWorld(value);

    }

    public void InitAR()
    {
        ar.SetActive(true);
        arCanvas.SetActive(false);
        mainGame.SetActive(false);
        initAR = true;
        mainGame.transform.position = spawned.transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _mouseReference = Input.mousePosition;
        }
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * 0.004f);
        }
        else if (Input.GetMouseButton(0))
        {
            _mouseOffset = (Input.mousePosition - _mouseReference);
            _rotation.y = _mouseOffset.x;
            arOrigin.Rotate(_rotation * -_sensitivity, Space.World);
            _mouseReference = Input.mousePosition;

           

        }
    }

    public void TurnOffRealWorld(bool value)
    {
        cameraAR.SetActive(value);
        //spawned.envi.SetActive(!value);
        //spawned.plane.SetActive(value);
        //planeVisualToggle.gameObject.SetActive(value);

        foreach (var plane in m_ARPlaneManager.trackables)
            plane.gameObject.SetActive(false);
        m_ARPlaneManager.enabled = false;
    }

    void Zoom(float increment)
    {
        Vector3 scale = arOrigin.localScale;
        var scaleChange = Mathf.Clamp(scale.x - increment, 0.1f, 99999f);
        Vector3 nScale = Vector3.one * scaleChange;

        arOrigin.localScale = nScale;
    }

}

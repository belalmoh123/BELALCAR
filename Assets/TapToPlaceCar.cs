using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TapToPlaceCar : MonoBehaviour
{
    public GameObject car1Prefab;
    public GameObject car2Prefab;
    public ARRaycastManager raycastManager;
    public Camera arCamera;

    public Material[] bodyColors;
    public Material[] wheelColors;

    private GameObject selectedPrefab;
    private GameObject spawnedCar;

    private int bodyIndex = 0;
    private int wheelIndex = 0;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public void SelectCar1() { selectedPrefab = car1Prefab; Debug.Log("Car 1 Selected"); }
    public void SelectCar2() { selectedPrefab = car2Prefab; Debug.Log("Car 2 Selected"); }

    void Update()
    {
#if UNITY_EDITOR
        if (selectedPrefab == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 fakePos = arCamera.transform.position + arCamera.transform.forward * 2;
            Pose fakePose = new Pose(fakePos, Quaternion.identity);

            if (spawnedCar != null)
                Destroy(spawnedCar);

            spawnedCar = Instantiate(selectedPrefab, fakePose.position, fakePose.rotation);
            ApplyCustomization();
        }
#else
        if (selectedPrefab == null) return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPos = Input.GetTouch(0).position;

            if (raycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                if (spawnedCar != null)
                    Destroy(spawnedCar);

                spawnedCar = Instantiate(selectedPrefab, hitPose.position, hitPose.rotation);
                ApplyCustomization();
            }
        }
#endif
    }

    public void ChangeBodyColor()
    {
        if (spawnedCar == null) return;
        bodyIndex = (bodyIndex + 1) % bodyColors.Length;
        ApplyCustomization();
    }

    public void ChangeWheelColor()
    {
        if (spawnedCar == null) return;
        wheelIndex = (wheelIndex + 1) % wheelColors.Length;
        ApplyCustomization();
    }

    void ApplyCustomization()
    {
        Renderer[] renderers = spawnedCar.GetComponentsInChildren<Renderer>();
        foreach (var rend in renderers)
        {
            if (rend.name.ToLower().Contains("wheel"))
                rend.material = wheelColors[wheelIndex];
            else
                rend.material = bodyColors[bodyIndex];
        }
    }
}

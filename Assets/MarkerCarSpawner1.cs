using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class MarkerCarSpawner : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public GameObject[] carPrefabs; // One per image
    private Dictionary<string, GameObject> spawnedCars = new Dictionary<string, GameObject>();

    void OnEnable() => trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    void OnDisable() => trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage img in args.added)
        {
            SpawnCar(img);
        }

        foreach (ARTrackedImage img in args.updated)
        {
            if (img.trackingState == TrackingState.Tracking)
                UpdateCar(img);
            else if (img.trackingState == TrackingState.None)
                DisableCar(img.referenceImage.name);
        }
    }

    void SpawnCar(ARTrackedImage img)
    {
        string name = img.referenceImage.name;
        if (!spawnedCars.ContainsKey(name))
        {
            GameObject prefab = GetCarPrefabByName(name);
            if (prefab != null)
            {
                GameObject car = Instantiate(prefab, img.transform.position, img.transform.rotation);
                car.transform.parent = img.transform; // Parent to image
                spawnedCars[name] = car;
            }
        }
    }

    void UpdateCar(ARTrackedImage img)
    {
        string name = img.referenceImage.name;
        if (spawnedCars.ContainsKey(name))
        {
            GameObject car = spawnedCars[name];
            car.transform.position = img.transform.position;
            car.transform.rotation = img.transform.rotation;
            car.SetActive(true);
        }
    }

    void DisableCar(string name)
    {
        if (spawnedCars.ContainsKey(name))
            spawnedCars[name].SetActive(false);
    }

    GameObject GetCarPrefabByName(string imageName)
    {
        foreach (GameObject car in carPrefabs)
        {
            if (car.name == imageName)
                return car;
        }
        return null;
    }
}

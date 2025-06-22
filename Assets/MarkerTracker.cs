using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class MarkerTracker : MonoBehaviour
{
    public GameObject car1Prefab;
    public GameObject car2Prefab;

    private ARTrackedImageManager trackedImageManager;
    private Dictionary<string, GameObject> spawnedCars = new();

    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var image in args.added)
            UpdateSpawn(image);

        foreach (var image in args.updated)
            UpdateSpawn(image);
    }

    private void UpdateSpawn(ARTrackedImage image)
    {
        string name = image.referenceImage.name;

        if (!spawnedCars.ContainsKey(name))
        {
            GameObject prefab = null;

            if (name == "CarMarker1") prefab = car1Prefab;
            if (name == "CarMarker2") prefab = car2Prefab;

            if (prefab != null)
            {
                GameObject spawned = Instantiate(prefab, image.transform.position, image.transform.rotation);
                spawned.transform.parent = image.transform;
                spawnedCars[name] = spawned;
            }
        }
        else
        {
            spawnedCars[name].transform.position = image.transform.position;
            spawnedCars[name].transform.rotation = image.transform.rotation;
        }
    }
}

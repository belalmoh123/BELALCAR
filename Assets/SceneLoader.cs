// SceneLoader.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMarkerless()
    {
        SceneManager.LoadScene("MarkerLessAR");
    }

    public void LoadMarkerBased()
    {
        SceneManager.LoadScene("MarkerBasedAR");
    }
}

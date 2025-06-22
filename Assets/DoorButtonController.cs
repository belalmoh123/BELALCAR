using UnityEngine;

public class DoorButtonController : MonoBehaviour
{
    public string doorName = "DoorL";

    private GameObject currentDoor;

    public void FindDoorAndToggle()
    {
        GameObject car = GameObject.FindWithTag("Car"); // We'll tag the spawned car
        if (car == null) return;

        Transform door = car.transform.Find(doorName);
        if (door == null)
        {
            Debug.LogWarning("Could not find door: " + doorName);
            return;
        }

        Animator anim = door.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("ToggleDoor");
        }
        else
        {
            Debug.LogWarning("No Animator on door!");
        }
    }
}

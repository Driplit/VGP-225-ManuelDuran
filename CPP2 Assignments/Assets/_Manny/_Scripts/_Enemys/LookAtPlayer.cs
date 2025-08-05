using UnityEngine;

public class LookAtPlayer: MonoBehaviour
{
    
    public GameObject otherObject; // The other object to check
    public float maxAngle = 45f; // Maximum angle for them to be considered "looking at each other"
    public bool Spotted;

    private void Start()
    {
        otherObject = GameObject.Find("Player");
    }
    void Update()
    {
        if (otherObject == null) return;

        if (IsLookingAtEachOther(gameObject, otherObject))
        {
            Debug.Log($"{gameObject.name} and {otherObject.name} are looking in each other's direction!");
        }
    }

    public bool IsLookingAtEachOther(GameObject obj1, GameObject obj2)
    {
        // Get forward directions of both objects
        Vector3 dir1 = obj1.transform.forward; // Adjust to up if top-down game
        Vector3 dir2 = obj2.transform.forward;

        // Calculate the angle between the two forward directions
        float angle1 = Vector3.Angle(dir1, (obj2.transform.position - obj1.transform.position)); // Angle from obj1 to obj2
        float angle2 = Vector3.Angle(dir2, (obj1.transform.position - obj2.transform.position)); // Angle from obj2 to obj1
        Spotted = angle1 <= maxAngle && angle2 <= maxAngle;
        // If both angles are smaller than the maxAngle, they are considered "looking at each other"
        return Spotted;
    }
}
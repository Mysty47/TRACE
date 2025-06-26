using UnityEngine;

public class Sensor : MonoBehaviour
{
    public GameObject[] objectsToDelete;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            foreach (GameObject obj in objectsToDelete)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
    } 
}

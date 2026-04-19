using UnityEngine;

public class PickUp : MonoBehaviour
{
    public void PickUpItem()
    {
        Debug.Log("Item picked up: " + gameObject.name);
        Destroy(gameObject);
    }
}

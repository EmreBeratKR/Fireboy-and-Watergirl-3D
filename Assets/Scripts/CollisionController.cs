using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    [SerializeField] private GemType targetGem;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Gem")
        {
            if (other.gameObject.GetComponent<Gem>().type == targetGem)
            {
                Destroy(other.gameObject);
            }
        }
    }
}

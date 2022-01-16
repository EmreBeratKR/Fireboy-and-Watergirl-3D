using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactChecker : MonoBehaviour
{
    public List<ContactChecker> contacts;

    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out ContactChecker contact))
        {
            if (!contacts.Contains(contact))
            {
                contacts.Add(contact);
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.TryGetComponent(out ContactChecker contact))
        {
            if (contacts.Contains(contact))
            {
                contacts.Remove(contact);
            }
        }
    }

    public float GetWeight()
    {
        return GetComponent<Rigidbody>().mass * GetComponent<CustomGravity>().gravityScale;
    }
}

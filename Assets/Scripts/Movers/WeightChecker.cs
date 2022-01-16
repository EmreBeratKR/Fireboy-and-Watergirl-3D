using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightChecker : MonoBehaviour
{
    private List<ContactChecker> firstContacts = new List<ContactChecker>(); 
    private List<ContactChecker> visitedContacts = new List<ContactChecker>();


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ContactChecker contact))
        {
            if (!firstContacts.Contains(contact))
            {
                firstContacts.Add(contact);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ContactChecker contact))
        {
            if (firstContacts.Contains(contact))
            {
                firstContacts.Remove(contact);
            }
        }
    }

    public float TotalWeight()
    {
        visitedContacts.Clear();
        float total = 0f;
        foreach (var contact in firstContacts)
        {
            if (contact == null)
            {
                firstContacts.Remove(contact);
                break;
            }
            total += ContactWeight(contact); 
        }
        return total;
    }

    private float ContactWeight(ContactChecker contact)
    {
        float total = 0f;
        visitedContacts.Add(contact);
        total += contact.GetWeight();
        foreach (var con in contact.contacts)
        {
            if (con == null)
            {
                contact.contacts.Remove(con);
                break;
            }
            if (!visitedContacts.Contains(con))
            {
                total += ContactWeight(con);
            }
        }
        return total;
    }
}

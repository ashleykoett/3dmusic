using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateFollowers : MonoBehaviour
{
    public delegate void ActivateFollowersEvent(string groupName);
    public static event ActivateFollowersEvent OnActivateFollowers;
    public string groupName = "group1";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (OnActivateFollowers != null)
            {
                OnActivateFollowers(groupName);
            }
        }
    }
}

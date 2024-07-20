using UnityEngine;

public class DismissFollowers : MonoBehaviour
{
    public delegate void DismissFollowersEvent(string groupName);
    public static event DismissFollowersEvent OnDismiss;
    public string groupName = "group1";

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(OnDismiss != null)
            {
                OnDismiss(groupName);
            }
        }
    }
}

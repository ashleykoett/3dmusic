using UnityEngine;

public class DismissFollwers : MonoBehaviour
{
    public delegate void DismissFollowers(string groupName);
    public static event DismissFollowers OnDismiss;
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

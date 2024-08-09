using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private GameObject anchor;

    private void Update()
    {
        transform.position = anchor.transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!anchor.activeInHierarchy) return;
        if (!other.CompareTag("Bread")) return;
        Debug.Log(other.tag);
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
        {
            var moveDirection = (transform.position - rigidbody.transform.position).normalized;
            rigidbody.velocity = moveDirection * 10;
        }
    }
}
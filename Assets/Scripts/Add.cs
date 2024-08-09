using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Add : Spawnable
{
    [SerializeField] private float score = 1;

    public delegate void onTriggerTouching(float val);

    public static event onTriggerTouching onTriggerTouchingEvents;

    private Quaternion initialRotation;

    private void Awake()
    {
        initialRotation = transform.rotation;
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        transform.localRotation = initialRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (gameObject.activeInHierarchy) StartCoroutine(DelayDisable());
            return;
        }
        if (!collision.gameObject.CompareTag("Player")) return;
        onTriggerTouchingEvents?.Invoke(score);
        gameObject.SetActive(false);
        StopAllCoroutines();
    }

    private IEnumerator DelayDisable()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : Spawnable
{
    public delegate void onTriggerTouching();

    public static event onTriggerTouching onTriggerTouchingEvents;

    private void OnEnable()
    {
        StopAllCoroutines();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(DelayDisable());
            return;
        }
        if (!other.gameObject.CompareTag("Player")) return;
        onTriggerTouchingEvents?.Invoke();

        gameObject.SetActive(false);
    }

    private IEnumerator DelayDisable()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
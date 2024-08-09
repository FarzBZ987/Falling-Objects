using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distractions : Spawnable
{
    private Collider[] Colliders;

    public delegate void onTouchPlayer();

    public static event onTouchPlayer onTouchPlayerEvents;

    private void Awake()
    {
        Colliders = gameObject.GetComponents<Collider>();
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        foreach (var collider in Colliders)
        {
            if (collider.enabled) collider.enabled = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            onTouchPlayerEvents?.Invoke();
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (Collider collider in Colliders)
            {
                collider.enabled = false;
            }
        }
    }

    private IEnumerator delayDisable()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
    }
}
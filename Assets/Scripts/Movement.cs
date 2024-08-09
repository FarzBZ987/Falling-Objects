using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Movement : MonoBehaviour
{
    private bool canMove;
    private float moveDirection;
    [SerializeField] private float speed;
    private Rigidbody rb;
    [SerializeField] private GameObject MagnetField;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        InputManager.onTouchChangeEvents += SetMove;
        PowerUp.onTriggerTouchingEvents += SetMagneticField;
        PowerUp.onTriggerTouchingEvents += SetMagneticField;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void SetMove(bool val, float pos)
    {
        canMove = val;

        moveDirection = pos;

        Debug.Log("MoveDir = " + moveDirection.ToString());
        rb.velocity = canMove ? Vector3.right * speed * moveDirection : Vector3.zero;
    }

    private void SetMagneticField()
    {
        StopAllCoroutines();
        MagnetField.SetActive(true);
        StartCoroutine(disableMagneticField());
    }

    private IEnumerator disableMagneticField()
    {
        yield return new WaitForSeconds(5);
        MagnetField.SetActive(false);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        InputManager.onTouchChangeEvents -= SetMove;
        PowerUp.onTriggerTouchingEvents -= SetMagneticField;
        PowerUp.onTriggerTouchingEvents -= SetMagneticField;
    }
}
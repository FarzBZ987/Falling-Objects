using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Movement basket;
    [SerializeField] private GameObject panel;
    [SerializeField] private Spawn spawner;
    [SerializeField] private InputManager inputManager;
    public static bool gameStarted;

    private void OnEnable()
    {
        PlayButton.onClickEvents += StartGame;
        Distractions.onTouchPlayerEvents += Reset;
    }

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        gameStarted =
            false;

        panel.SetActive(true);
        basket.transform.position = new Vector3(0, basket.transform.position.y, basket.transform.position.z);
        basket.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        panel.SetActive(false);
        gameStarted = true;
        basket.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        PlayButton.onClickEvents -= StartGame;
        Distractions.onTouchPlayerEvents -= Reset;
    }
}
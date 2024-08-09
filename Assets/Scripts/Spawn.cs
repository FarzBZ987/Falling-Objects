using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private List<Spawns> spawnables;

    [System.Serializable]
    public struct Spawns
    {
        public GameObject spawnables;
        public float spawnCount;
    }

    [SerializeField] private List<GameObject> pooledNormal = new List<GameObject>();
    [SerializeField] private List<GameObject> pooledPowerUp = new List<GameObject>();
    [SerializeField] private List<GameObject> pooledPowerDown = new List<GameObject>();
    private bool _isSpawning;
    [SerializeField] private Transform boundLeft;
    [SerializeField] private Transform boundRight;
    private Rigidbody rb;

    private bool isSpawning
    {
        get => _isSpawning;
        set
        {
            if (!value)
            {
                timeElapsed = 0f;
                spawnTime = 0f;
            }
            _isSpawning = value;
        }
    }

    private float minSpawnDelay = 0.5f;
    private float maxSpawnDelay = 3f;
    private float spawnDelay;
    private float spawnTime;
    private float timeElapsed;

    private bool isMovingLeft = false;

    // Start is called before the first frame update
    private void OnEnable()
    {
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
        PlayButton.onClickEvents += StartSpawning;
        Distractions.onTouchPlayerEvents += StopSpawning;
    }

    private void OnDisable()
    {
        PlayButton.onClickEvents -= StartSpawning;
        Distractions.onTouchPlayerEvents -= StopSpawning;
    }

    private void StartSpawning() => isSpawning = true;

    private void StopSpawning()
    {
        isSpawning = false;
        foreach (var item in pooledPowerUp)
            item.gameObject.SetActive(false);
        foreach (var item in pooledPowerDown)
            item.gameObject.SetActive(false);
        foreach (var item in pooledNormal)
            item.gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        foreach (Spawns spawns in spawnables)
        {
            for (int i = 0; i < spawns.spawnCount; i++)
            {
                var obj = Instantiate(spawns.spawnables);
                obj.transform.parent = this.transform;
                if (obj.TryGetComponent<Add>(out Add normalObj)) pooledNormal.Add(obj);
                else if (obj.TryGetComponent<Distractions>(out Distractions dis)) pooledPowerDown.Add(obj);
                else if (obj.TryGetComponent<PowerUp>(out PowerUp powerUp)) pooledPowerUp.Add(obj);
                else Destroy(obj);
            }
        }
        pooledNormal.Shuffle();
        isSpawning = false;
        foreach (var item in pooledPowerUp)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in pooledPowerDown)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in pooledNormal)
        {
            item.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isSpawning) return;

        if (GameManager.gameStarted && (gameObject.transform.position.x >= 6.7f || gameObject.transform.position.x <= -6.7f)) SwitchMovement();
        rb.velocity = Vector3.left * (isMovingLeft ? 1 : -1) * 5;

        timeElapsed += Time.deltaTime;

        spawnTime += Time.deltaTime;

        spawnDelay = Mathf.Lerp(maxSpawnDelay, minSpawnDelay, (timeElapsed / (3 * 60)));

        if (spawnTime > spawnDelay)
        {
            SpawnObject();

            spawnTime = 0;
        }
    }

    private void SpawnObject()
    {
        var randomValue = Random.Range(0, 100);

        if (randomValue < 10)
        {
            spawnFrom(pooledPowerDown);
        }
        else if (randomValue < 15)
        {
            spawnFrom(pooledPowerUp);
        }
        else
        {
            pooledNormal.Shuffle();
            spawnFrom(pooledNormal);
        }
    }

    private void spawnFrom(List<GameObject> spawnList)
    {
        foreach (GameObject spawn in spawnList)
        {
            if (spawn.activeSelf) continue;
            spawn.SetActive(true);
            var randomizedSpawnLocation = Random.Range(boundLeft.position.x, boundRight.position.x);
            spawn.transform.position = new Vector3(randomizedSpawnLocation, transform.position.y, transform.position.z);
            if (spawn.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.velocity = Vector3.zero;
            }
            break;
        }
    }

    public void SetSpawn(bool val)
    {
        isSpawning = val;
    }

    private void SwitchMovement() => isMovingLeft = !isMovingLeft;
}

public static class ListShuffler
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
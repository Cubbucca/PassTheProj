using UnityEngine;

public class BeanSpawner : MonoBehaviour
{
    // Serialized Fields
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private float _rotationSpeed = 5f;

    // Private Fields
    private ShoppingCart _cart;
    

    private void Awake()
    {
        _cart = FindObjectOfType<ShoppingCart>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime, Space.World);
    }

    public void SpawnEnemy(EnemyManager managerRef)
    {
        var spawnedEnemy = Instantiate(_enemyPrefab, transform.position, transform.rotation);
        spawnedEnemy.manager = managerRef;
        spawnedEnemy.Init(_cart);
    }
}

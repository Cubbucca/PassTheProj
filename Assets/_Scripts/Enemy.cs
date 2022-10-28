using UnityEngine;
using Random = UnityEngine.Random; // Sam: This is cool didn't know you could do this.

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 20f;
    [SerializeField] private float _rotationSpeed = 20f;
    [SerializeField] private float _spawnForce = 10f;
    [SerializeField] private float _knockbackForce = 1f;
    [SerializeField] private float _waitDistance = 1f;
    [SerializeField] private float _health = 200f;
    [SerializeField] private int _value = 5;
    [SerializeField] private float _invunerableTime = 2f;
    [SerializeField] private GameObject _beanExplosionPrefab;

    public EnemyManager manager;

    private bool _shouldChase = false;
    private float _currentInvunerableTime;
    private Rigidbody _rb;
    private ShoppingCart _cart;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_currentInvunerableTime < _invunerableTime) _currentInvunerableTime += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (_cart == null || !_shouldChase || _cart.InSafeZone) return;

        // Move towards the player cart.
        var dirToCart = (_cart.transform.position - transform.position).normalized;
        if (Vector3.Distance(_cart.transform.position, transform.position) > _waitDistance)
            _rb.AddForce(dirToCart * _moveSpeed * Time.fixedDeltaTime);
    }

    public void Init(ShoppingCart cart)
    {
        var newSpawnForce = _spawnForce - Random.Range(-2f, 2f);
        _rb.AddForce(transform.up * newSpawnForce, ForceMode.Impulse);
        _cart = cart;
        
        Invoke(nameof(StartChasing), 2f);
    }

    public void StartChasing()
    {
        _shouldChase = true;
    }

    public void Hit(Vector3 transformPosition, float velocityMagnitude)
    {
        if (_currentInvunerableTime >= _invunerableTime)
        {
            _currentInvunerableTime = 0;
            _health -= velocityMagnitude;
            if (_health <= 0) {
                Die();
            }
            else
            {
                var dirAwayFromCart = -(transformPosition - transform.position).normalized;
                _rb.AddForce(dirAwayFromCart * _knockbackForce, ForceMode.Impulse);
            }    
        }  
    }

    public void TakeDamage(float damageAmount)
    {
        if (_currentInvunerableTime >= _invunerableTime)
        {
            _currentInvunerableTime = 0;
            _health -= damageAmount;
            if (_health <= 0)
            {
                Die();
            }
        }
        }

    void Die() {
        Instantiate(_beanExplosionPrefab, transform.position, Quaternion.identity);
        manager.currentNumberOfEnemies--;
        Destroy(this.gameObject);
        ScoreManager.Instance.AddScore(_value);
    }
    
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Safe Zone")) Die();
    }
}

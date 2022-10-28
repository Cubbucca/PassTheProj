using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class ShoppingCart : MonoBehaviour {
    [SerializeField] private float _maxSpeed = 25f;
    [SerializeField] private float _moveSpeed = 20f;
    [SerializeField] private float _rotationSpeed = 20f;
    [SerializeField] private Transform _frontOfCart;
    [SerializeField] private Vector3 _extents;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private TrailRenderer[] _skids;
    [SerializeField] private AudioSource _skidSource;

    [SerializeField] private float _skidThreshold = 1;

    private InputControls _inputControls;
    private Rigidbody _rb;
    private CinemachineImpulseSource _impulseSource;
    private Vector2 _input;
    private Vector3 _movement;

    public bool InSafeZone { get; private set; }
    public float VelocityMagnitude => _rb.velocity.magnitude;


    private void Awake() {
        _inputControls = new InputControls();

        _rb = GetComponent<Rigidbody>();

        _impulseSource = GetComponent<CinemachineImpulseSource>();

        SetupVisuals();
    }

    private void OnEnable() {
        _inputControls.Enable();
    }

    private void OnDisable() {
        _inputControls.Disable();
    }

    private void Update() {
        _input = _inputControls.ShoppingCart.Movement.ReadValue<Vector2>();

        _movement = new Vector3(0, 0, _input.y);

        HandleRam();
        HandleBlast();
        Move();

        HandleSkids();

        HandleVisuals();

        if(_inputControls.ShoppingCart.Pause.WasPressedThisFrame())
        {
            
            if(GameManager.instance._gameState == GameManager.GameStates.Paused)
            {
               
                GameManager.instance._gameState = GameManager.GameStates.Playing;
            }
            else if (GameManager.instance._gameState == GameManager.GameStates.Playing)
            {
               
                GameManager.instance._gameState = GameManager.GameStates.Paused;
            }
            GameManager.instance.CheckStates();
        }

    }

    public void ToggleSkids(bool PauseSkids)
    {
        if(!_skidSource.isActiveAndEnabled)
        { return; }

        if(PauseSkids)
        {
            _skidSource.Pause();
        }
        else
        {
            _skidSource.Play();
        }
    }

    void Move() {
        var overlappedEnemies = Physics.OverlapBox(_frontOfCart.position, _extents, Quaternion.identity, _enemyMask);

        if (overlappedEnemies.Length <= 0) return;

        foreach (var overlappedEnemy in overlappedEnemies) {
            var enemy = overlappedEnemy.GetComponent<Enemy>();
            if (enemy) enemy.Hit(transform.position, _rb.velocity.magnitude);
        }

        if (_rb.velocity.magnitude > _maxSpeed) {
            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxSpeed);
        }
    }

    #region Visuals

    [Header("VISUALS")]

    private ChromaticAberration _chrome;

    void SetupVisuals() {
        var volume = FindObjectOfType<Volume>();
        volume.profile.TryGet(out _chrome);
    }

    void HandleVisuals() {
        _chrome.intensity.value = Mathf.InverseLerp(10, _maxRamMagnitude, _rb.velocity.magnitude);
    }
    
    void HandleSkids() {
        var angular = Mathf.Abs(_rb.angularVelocity.y);
        var point = Mathf.InverseLerp(0, 6, angular);
        _skidSource.volume = point * 0.3f;
        // If angular velocity is greater than the skid treshold then emit the skid.
        var emit = angular > _skidThreshold;
        for (var i = 0; i < 2; i++) _skids[i].emitting = emit;
    }

    #endregion


    private void FixedUpdate() {
        AddRelativeForce(_movement);
        if (!_ramming) _rb.AddTorque(Vector3.up * _input.x * _rotationSpeed * Time.fixedDeltaTime);
    }

    private void AddRelativeForce(Vector3 dir) {
        _rb.AddRelativeForce(dir * _moveSpeed * Time.fixedDeltaTime);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_frontOfCart.position, _extents);
        //Drawing Sphere for Blast
        //Gizmos.DrawSphere(transform.position, _blastRadius);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Safe Zone")) InSafeZone = true;
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Safe Zone")) InSafeZone = false;
    }

    #region Abilities

    public static event Action<AbilityType,bool> OnAbilityUsable; // Type - Usable

    private readonly HashSet<AbilityType> _unlockedAbilities = new HashSet<AbilityType>();
    public void UnlockAbility(AbilityType type) => _unlockedAbilities.Add(type);
    private bool _hasAbility(AbilityType type) => _unlockedAbilities.Contains(type);

    [Header("RAM")] [SerializeField] private float _ramCooldown = 5;
    [SerializeField] private float _ramDuration = 1;
    [SerializeField] private float _ramSpeedMultiplier = 2;
    [SerializeField] private ParticleSystem _dashParticles;
    [SerializeField] private float _maxRamMagnitude = 20;
    private float _lastRam = Single.MinValue;
    private bool _ramming;
    private Vector3 _ramDirection;
    private bool _ramUsable;

    [Header("BLAST")]
    [SerializeField] float _blastCooldown = 5;
    [SerializeField] float _blastRadius = 3;
    [SerializeField] float _blastPower = 100;
    [SerializeField] float _blastDamage = 100;
    [SerializeField] ParticleSystem _blastParticles;
    [SerializeField] float maxBlastDistance;
    bool _canBlast;
    float _lastBlast = Single.MinValue;
    
    private void HandleRam() {

        if (!_ramUsable && _hasAbility(AbilityType.Ram) && _lastRam + _ramCooldown < Time.time) {
            OnAbilityUsable?.Invoke(AbilityType.Ram,true);
            _ramUsable = true;
        }
    
        if ( _inputControls.ShoppingCart.Ram.IsPressed() && _ramUsable) {
            _ramUsable = false;
            _lastRam = Time.time;
            _ramming = true;
            _ramDirection = _movement;

            _impulseSource.GenerateImpulse();
            _dashParticles.Play();
            
            OnAbilityUsable?.Invoke(AbilityType.Ram,false);
        }

        if (_ramming) {
          
            if(_rb.velocity.magnitude < _maxRamMagnitude) _rb.AddRelativeForce(_ramDirection * _moveSpeed * _ramSpeedMultiplier * Time.fixedDeltaTime);
           
            if (_lastRam + _ramDuration < Time.time) {
                _ramming = false;
                _dashParticles.Stop();
             
            }
        }
    }

    private void HandleBlast()
    {
        if(_hasAbility(AbilityType.Blast) && !_canBlast && _lastBlast + _blastCooldown < Time.time)
        {
            OnAbilityUsable?.Invoke(AbilityType.Blast, true);
            _canBlast = true;
            _blastParticles.Stop();
        }
        if(_inputControls.ShoppingCart.Blast.IsPressed() && _canBlast)
        {
            _canBlast = false;
            _lastBlast = Time.time;

            _impulseSource.GenerateImpulse();
            _blastParticles.Play();
            DoBlastRadius();
            OnAbilityUsable?.Invoke(AbilityType.Blast, false);
        }

    }
    
    void DoBlastRadius()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _blastRadius, transform.up, maxBlastDistance, _enemyMask, QueryTriggerInteraction.Ignore);
       
        
        if(hits.Length > 0)
        {
            foreach(RaycastHit hit in hits)
            {
               
                Vector3 forceDir = hit.transform.position - transform.position;
                hit.rigidbody.AddForce(forceDir.normalized * _blastPower);
                hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(_blastDamage);
            }
        }
        
    }


    #endregion
}
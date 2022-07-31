using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    #region Fields
    
    [Header("Movements")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _xThreshHold;
    [SerializeField] private Collider _collider;
    [SerializeField] private Rigidbody _rigidbody;
    
    [Header("Stack")]
    [SerializeField] private Transform _collectTransfrom;
    [SerializeField] private Vector3 _itemSize;

    private Animator _animator;
    private Vector2 _lastDirection = Vector2.zero;
    private float _moveX;

    private bool _canMove;
    private bool _isAlive;
    
    private readonly List<CollectableCube> _cubes = new();

    private readonly List<Rigidbody> _ragdollRigidbodies = new();
    private readonly List<Collider> _ragdollcolliders = new();

    #endregion

    #region Properties

    public Vector2 MoveDirection
    {
        set
        {
            if (value == _lastDirection) return;
            if (value == Vector2.zero || _lastDirection == Vector2.zero)
            {
                _lastDirection = value;
                return;
            }
            
            _moveX = transform.localPosition.x;
            var moveAmount = (value.x - _lastDirection.x) / (Screen.width * 0.9f) * (_xThreshHold * 2);
            _moveX += moveAmount;
            _moveX = Mathf.Clamp(_moveX, -_xThreshHold, _xThreshHold);

            _lastDirection = value;
        }
    }

    private bool IsAlive
    {
        get => _isAlive;
        set
        {
            if (_isAlive == value) return;

            _isAlive = value;
            EnableRagdoll(!value);

            if (!value)
            {
                Level.Current.EndLevel(false);
            }
        }
    }

    #endregion

    #region Public Methods

    public void AddItem(CollectableCube cube)
    {
        for (var i = 0; i < _cubes.Count; i++)
        {
            var item = _cubes[i];
            item.transform.localPosition += Vector3.up * _itemSize.y;
            item.MinY = (i + 1) * _itemSize.y;

            item.SimulateGravity = true;
        }

        _cubes.Add(cube);
        cube.transform.parent = _collectTransfrom;
        cube.transform.position = _collectTransfrom.position;
        cube.IsCollectable = false;
    }

    public void RemoveItem(CollectableCube cube)
    {
        if (!_cubes.Contains(cube)) return;
        
        _cubes.Remove(cube);
        cube.transform.parent = Level.Current.transform;
        cube.MinY = _itemSize.y / 2f;
        
        for (var i = 0; i < _cubes.Count; i++)
        {
            var item = _cubes[i];
            item.MinY = i * _itemSize.y;
        }
    }

    public void ClearItems()
    {
        for (var i = _cubes.Count - 1; i >= 0; i--)
        {
            var cube = _cubes[i];
            _cubes.Remove(cube);
            Destroy(cube.gameObject);
        }
    }

    public bool ThrowCube(Transform target)
    {
        var res = false;
        
        StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            if (_cubes.Count == 0)
            {
                yield break;
            }

            var cube = _cubes[0];
            res = true;
            
            RemoveItem(cube);
            
            yield return cube.Throw(target, Vector3.zero, 4f, 0.3f);
            cube.SimulateGravity = false;
        }

        return res;
    }

    #endregion

    #region Unity Event Functions

    protected void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        GetComponentsInChildren(true, _ragdollRigidbodies);
        GetComponentsInChildren(true, _ragdollcolliders);

        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.mass = 1;
            rigidbody.angularDrag = 0;
        }

        EnableRagdoll(false);
        
        Game.Instance.StateChanged += GameOnStateChanged;
    }

    protected void Update()
    {
        if (!_canMove || !IsAlive) return;

        if (transform.position.y < -0.1f)
        {
            IsAlive = false;
            return;
        }

        transform.localPosition += transform.forward * _moveSpeed * Time.deltaTime;

        if (Math.Abs(_moveX - transform.localPosition.x) < 0.01f) return;

        var position = transform.localPosition;
        position.x = _moveX;
        transform.localPosition = position;
    }

    protected void LateUpdate()
    {
        if (!_canMove) return;
        
        _animator.SetBool(AnimatorHash.IsMoving, _canMove);
        
        // Fix animation
        _animator.transform.localPosition = Vector3.zero;
    }

    protected void OnDestroy()
    {
        Game.Instance.StateChanged -= GameOnStateChanged;
    }

    #endregion

    #region Private Methods

    private void GameOnStateChanged(Game sender)
    {
        switch (sender.State)
        {
            case GameState.Playing:
                IsAlive = true;
                _canMove = true;
                break;
            case GameState.Victory:
                _canMove = false;
                _animator.SetBool(AnimatorHash.Victory, true);
                break;
            case GameState.Defeat:
                _canMove = false;
                IsAlive = false;
                ClearItems();
                break;
            case GameState.Menu:
                _canMove = false;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void EnableRagdoll(bool enable)
    {
        _animator.enabled = !enable;

        foreach (var collider in _ragdollcolliders)
        {
            if (collider == _collider) continue;

            collider.enabled = enable;
        }

        foreach (var rigidbody in _ragdollRigidbodies)
        {
            if (rigidbody == _rigidbody) continue;
            
            rigidbody.useGravity = enable;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }

    #endregion
}

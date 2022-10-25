using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MinionController : MonoBehaviour, IAttack, IHurt
{
    [SerializeField]
    GameObject _floatingTextPrefab;
    [SerializeField]
    float _attackDistance = 5f;
    [SerializeField]
    GameObject _enemyBase;
    [SerializeField]
    string EnemyTag = "Enemy";
    [SerializeField]
    int _damage = 5;
    [SerializeField]
    float _damageSpeedInSec = 1;

    IState _currentState;
    NavMeshAgent _agent;
    bool _dead = false;
    int _health = 100;
    float _sinceLastAttachInSec;
    GameObject _spottedEnemy = null;
    bool _enemyOnRange = false;

    public GameObject EnemyBase { get => _enemyBase; }
    public NavMeshAgent NavMeshAgent { get => _agent; }
    public GameObject SpottedEnemy { get => _spottedEnemy; }
    public bool EnemyOnRange { get => _enemyOnRange; }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        SetState(new ExploreState(_agent, _enemyBase.transform.position));
    }
    private void SetState(IState newState)
    {
        _currentState?.OnExit();
        if (_currentState == null || _currentState.GetType() != newState.GetType())
        {
            Debug.Log($"change state to {newState.GetType()}");
            _currentState = newState;
            _currentState.OnEnter();
        }
        _currentState = newState;
    }

    void Update()
    {
        var newState = _currentState.Tick(this);
        if (newState != null)
        {
            SetState(newState);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other is SphereCollider && other.tag == EnemyTag && _spottedEnemy == null)
        {
            _spottedEnemy = other.gameObject;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (_spottedEnemy != null && Vector3.Distance(transform.position, _spottedEnemy.transform.position) < _attackDistance)
        {
            _enemyOnRange = true;
        }
    }

    public void Attack(IHurt target)
    {
        _sinceLastAttachInSec += Time.deltaTime;
        if (_damageSpeedInSec < _sinceLastAttachInSec)
        {
            if (target.BeHurt(_damage))
            {
                _spottedEnemy = null;
            }
            _sinceLastAttachInSec = 0;
        }
    }

    public bool BeHurt(int damageReceive)
    {
        if (!_dead)
        {
            _health -= damageReceive;
            if (_floatingTextPrefab && _health > 0)
            {
                ShowFloatingText();
            }

            if (_health <= 0)
            {
                _dead = true;
                Destroy(gameObject);
            }
        }
        return _dead;
    }

    private void ShowFloatingText()
    {
        var go = Instantiate(_floatingTextPrefab, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = _health.ToString();
    }

}

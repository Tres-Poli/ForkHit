using System.Collections;
using UnityEngine;

public class Fork : MonoBehaviour
{
    [SerializeField]
    private float Speed;
    [SerializeField]
    protected GameObject _tail;
    protected Camera _cam;
    protected Vector2 _fallDirection;
    protected Vector3 _fallRotation;
    protected Rigidbody2D _body;
    protected PolygonCollider2D _collider;

    public ForkState State { get; protected set; }
    public float FallSpeed;
    public float MinFallRotation;
    public float MaxFallRotation;

    public delegate void ForkEvent(Fork fork);
    public static event ForkEvent ForkCollision;

    protected void Start()
    {
        State = ForkState.Idle;
        _body = GetComponent<Rigidbody2D>();
        _collider = GetComponent<PolygonCollider2D>();

        _collider.enabled = true;

        _cam = Camera.main;
        _fallDirection = new Vector2(Random.Range(-6, 6), Random.Range(-5, -2));
        _fallRotation = new Vector3(0, 0, Random.Range(MinFallRotation, MaxFallRotation));

        GameController.LevelComplete += OnLevelComplete;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (State == ForkState.Idle)
            {
                _body.velocity = new Vector2(0, 1) * Speed;
                // StartCoroutine(DrawTail());
            }
        }
    }

    private IEnumerator DrawTail()
    {
        while (State == ForkState.Idle)
        {
            var tail = Instantiate(_tail, transform.position, Quaternion.identity);
            StartCoroutine(Disappear(tail.GetComponent<Renderer>()));

            yield return new WaitForSeconds(0.001f);
        }
    }

    private IEnumerator Disappear(Renderer renderer)
    {
        while (renderer.material.color.a > 0)
        {
            var currColor = renderer.material.color;
            renderer.material.color = new Color(currColor.r, currColor.g, currColor.b, currColor.a - .1f);

            yield return new WaitForEndOfFrame();
        }

        Destroy(renderer.gameObject);
    }

    protected IEnumerator Fall()
    {
        Vector3 camView;
        do
        {
            camView = _cam.WorldToViewportPoint(transform.position);
            transform.Rotate(_fallRotation);
            yield return new WaitForSeconds(0);
        } while (camView.x <= 1 && camView.x >= 0 && camView.y <= 1 && camView.y >= 0);

        yield return null;
    }

    protected void OnLevelComplete()
    {
        transform.parent = null;
        _body.bodyType = RigidbodyType2D.Dynamic;
        _body.velocity = _fallDirection * FallSpeed;
        _collider.enabled = false;
        StartCoroutine(Fall());
    }

    protected void OnDestroy()
    {
        GameController.LevelComplete -= OnLevelComplete;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (State == ForkState.Idle)
        {
            var hitTarget = collision.gameObject.GetComponent<Target>();
            if (hitTarget != null)
            {
                _body.bodyType = RigidbodyType2D.Static;
                transform.parent = hitTarget.transform;
                State = ForkState.HitTarget;
            }
            else
            {
                State = ForkState.HitKnife;
                _body.velocity = _fallDirection * FallSpeed;
                _collider.enabled = false;
                StartCoroutine(Fall());
            }

            ForkCollision.Invoke(this);

            Debug.Log($"State: {State}");
        }
    }
}

public enum ForkState
{
    Idle = 0,
    HitKnife = 1,
    HitTarget = 2,
    LevelComplete = 3,
}


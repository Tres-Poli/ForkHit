using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Blinker))]
public class Target : MonoBehaviour
{
    [SerializeField]
    private float _deltaYShake;
    [SerializeField]
    private Vector3 _rotation;
    [SerializeField]
    private float RotationSpeed;
    private Blinker _blinker;
    private Rigidbody2D _body;

    [SerializeField]
    private GameObject[] _parts;
    [SerializeField]
    private float _impulseMultiplier;

    void Start()
    {
        foreach (var part in _parts)
        {
            part.GetComponent<Rigidbody2D>().simulated = false;
        }

        _blinker = GetComponent<Blinker>();
        _body = GetComponent<Rigidbody2D>();
        Fork.ForkCollision += OnForkCollision;
        GameController.LevelComplete += OnLevelComplete;
    }

    void Update()
    {
        transform.Rotate(_rotation * RotationSpeed * Time.deltaTime, Space.Self);
    }

    public void OnForkCollision(Fork fork)
    {
        StartCoroutine(_blinker.Blink());
        StartCoroutine(Shake());
    }

    public void StopRotation()
    {
        _rotation = Vector3.zero;
    }

    private IEnumerator Shake()
    {
        var defaultPosition = transform.position;
        transform.position = new Vector3(defaultPosition.x, defaultPosition.y + _deltaYShake, defaultPosition.z);
        yield return new WaitForSeconds(.05f);

        transform.position = defaultPosition;
    }

    public void Break()
    {
        _body.simulated = false;
        foreach (var part in _parts)
        {
            var body = part.GetComponent<Rigidbody2D>();
            var trans = part.GetComponent<Transform>();
            trans.parent = null;
            body.simulated = true;

            body.AddForce((trans.position - transform.position).normalized * _impulseMultiplier, ForceMode2D.Impulse);
        }
    }

    private void OnLevelComplete()
    {
        Break();
    }

    private void OnDestroy()
    {
        Fork.ForkCollision -= OnForkCollision;
        GameController.LevelComplete -= OnLevelComplete;

        foreach (var part in _parts)
        {
            Destroy(part.gameObject);
        }
    }
}

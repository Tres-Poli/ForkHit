using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Blinker : MonoBehaviour
{
    [SerializeField]
    private float _deltaAlphaBlink;
    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator Blink()
    {
        _renderer.color = new Color(1, 1, 1, _deltaAlphaBlink);
        yield return new WaitForSeconds(.1f);

        _renderer.color = new Color(1, 1, 1, 0);
        yield return null;
    }
}

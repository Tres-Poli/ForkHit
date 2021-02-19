using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extensions : MonoBehaviour
{
    public bool InRange(Vector3 position)
    {
        var camView = GetComponent<Camera>().WorldToViewportPoint(position);
        return camView.x <= 1 && camView.x >= 0 && camView.y <= 1 && camView.y >= 0;
    }
}

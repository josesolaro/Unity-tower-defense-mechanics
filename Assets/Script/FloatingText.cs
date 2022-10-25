using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{

    [SerializeField]
    float _destroyTime = 3f;
    [SerializeField]
    Vector3 _offset = new Vector3(0, 2, 0);
    void Start()
    {
        Destroy(gameObject, _destroyTime);
        transform.position += _offset;
    }

}

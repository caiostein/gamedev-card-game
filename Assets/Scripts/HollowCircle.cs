using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HollowCircle : MonoBehaviour
{
    private void Start()
    {
        Object.Destroy(gameObject, 0.5f);
    }
}

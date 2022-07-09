using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    [SerializeField] string tag;

    private void Awake()
    {
        GameObject[] musicObject = GameObject.FindGameObjectsWithTag(tag);
        if (musicObject.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlatformsPool : MonoBehaviour
{
    [SerializeField]
    PlatformController _platformPrefab;
    [SerializeField]
    float _secsBetweenPlatform = 0f;


    private ObjectPool<PlatformController> _pool;
    public ObjectPool<PlatformController> Pool { get { return _pool; } }

    private void Awake()
    {
        _pool = new ObjectPool<PlatformController>(OnCreatePrefab, OnGetPrefab, OnReleasePrefab, OnDestroyPrefab);
    }


    private void Start()
    {
        StartCoroutine(PlatformSpawner());
    }


    private IEnumerator PlatformSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(_secsBetweenPlatform);
            _pool.Get();
        }
    }

    private PlatformController OnCreatePrefab()
    {
        PlatformController newPlatform = Instantiate(_platformPrefab);
        newPlatform.gameObject.SetActive(false);
        return newPlatform;
    }


    public void OnGetPrefab(PlatformController platform)
    {
        
        platform.gameObject.SetActive(true);
        platform.Pool = _pool;
    }

    public void OnReleasePrefab(PlatformController platform)
    {
        platform.gameObject.SetActive(false);
    }

    public void OnDestroyPrefab(PlatformController platform)
    {
        Destroy(platform.gameObject);
    }
}

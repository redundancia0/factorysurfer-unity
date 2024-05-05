using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectilesPool : MonoBehaviour
{
    [SerializeField]
    ProjectileController _projectilePrefab;
    [SerializeField]
    float _secsBetweenProjectiles = 0f;


    private ObjectPool<ProjectileController> _pool;
    public ObjectPool<ProjectileController> Pool { get { return _pool; } }

    private void Awake()
    {
        _pool = new ObjectPool<ProjectileController>(OnCreatePrefab, OnGetPrefab, OnReleasePrefab, OnDestroyPrefab);
    }


    private void Start()
    {
        StartCoroutine(ProjectileSpawner());
    }


    private IEnumerator ProjectileSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(_secsBetweenProjectiles);
            _pool.Get();
        }
    }

    private ProjectileController OnCreatePrefab()
    {
        ProjectileController newProjectile = Instantiate(_projectilePrefab);
        newProjectile.gameObject.SetActive(false);
        return newProjectile;
    }


    public void OnGetPrefab(ProjectileController projectile)
    {
        
        projectile.gameObject.SetActive(true);
        projectile.Pool = _pool;
    }

    public void OnReleasePrefab(ProjectileController projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    public void OnDestroyPrefab(ProjectileController projectile)
    {
        Destroy(projectile.gameObject);
    }
}

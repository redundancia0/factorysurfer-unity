using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CoinsPool : MonoBehaviour
{
    [SerializeField]
    CoinController _coinPrefab;
    [SerializeField]
    float _secsBetweenCoins = 0f;


    private ObjectPool<CoinController> _pool;
    public ObjectPool<CoinController> Pool { get { return _pool; } }

    private void Awake()
    {
        _pool = new ObjectPool<CoinController>(OnCreatePrefab, OnGetPrefab, OnReleasePrefab, OnDestroyPrefab);
    }


    private void Start()
    {
        StartCoroutine(CoinSpawner());
    }


    private IEnumerator CoinSpawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(_secsBetweenCoins);
            _pool.Get();
        }
    }

    private CoinController OnCreatePrefab()
    {
        CoinController newCoin = Instantiate(_coinPrefab);
        newCoin.gameObject.SetActive(false);
        return newCoin;
    }


    public void OnGetPrefab(CoinController coin)
    {
        
        coin.gameObject.SetActive(true);
        coin.Pool = _pool;
    }

    public void OnReleasePrefab(CoinController coin)
    {
        coin.gameObject.SetActive(false);
    }

    public void OnDestroyPrefab(CoinController coin)
    {
        Destroy(coin.gameObject);
    }
}

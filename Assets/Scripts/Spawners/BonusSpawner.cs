using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _bonuses = new();
    [SerializeField] private float _timeForBonusSpawn = 20f;

    [Header("RangeForBonusSpawn")] 
    [SerializeField] private Transform _rt;
    [SerializeField] private Transform _lb;

    private int _count;

    void Start()
    {
        _count = _bonuses.Count;
        StartCoroutine(BonusInstantiate());
    }

    IEnumerator BonusInstantiate()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timeForBonusSpawn);
            var rand = Random.Range(0, _count);
            var newBonus = Instantiate(_bonuses[rand]);
            newBonus.transform.position = new Vector2(
                Random.Range(_lb.transform.position.x, _rt.transform.position.x),
                Random.Range(_lb.transform.position.y, _rt.transform.position.y));
            newBonus.SetActive(true);
        }
    }
}
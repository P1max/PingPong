using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawn : MonoBehaviour
{
    [SerializeField] private List<GameObject> Bonuses = new();
    [SerializeField] private float TimeForBonusSpawn = 20f;

    [Header("RangeForBonusSpawn")] 
    [SerializeField] private Transform _rt;
    [SerializeField] private Transform _lb;

    int count;

    void Start()
    {
        count = Bonuses.Count;
        StartCoroutine(BonusInstantiate());
    }

    IEnumerator BonusInstantiate()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeForBonusSpawn);
            var rand = Random.Range(0, count);
            var newBonus = Instantiate(Bonuses[rand]);
            newBonus.transform.position = new Vector2(
                Random.Range(_lb.transform.position.x, _rt.transform.position.x),
                Random.Range(_lb.transform.position.y, _rt.transform.position.y));
            newBonus.SetActive(true);
        }
    }
}
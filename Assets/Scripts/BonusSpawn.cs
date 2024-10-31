using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawn : MonoBehaviour
{

    [SerializeField] private List<GameObject> Bonuses = new();  
    [SerializeField] private float TimeForBonusSpawn = 20f;

    [Header("RangeForBonusSpawn")]
    [SerializeField] private Transform RT;
    [SerializeField] private Transform LB;


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
            GameObject NewBonus = Instantiate(Bonuses[rand]);
            NewBonus.transform.position = new Vector2(
                Random.Range(LB.transform.position.x, RT.transform.position.x),
                Random.Range(LB.transform.position.y, RT.transform.position.y));
            NewBonus.SetActive(true);
        }
    }
}


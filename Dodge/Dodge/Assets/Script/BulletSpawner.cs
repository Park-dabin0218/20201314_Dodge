using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab; // 생성할 탄알의 원본 프리팹
    public float spawnRateMin = 0.5f; // 최소 생성 주기
    public float spawnRateMax = 3f; // 최대 생성 주기

    private Transform target; // 발사할 대상
    private float spawnRate; // 생성 주기
    private float timeAfterSpawn; // 최근 생성 시점에서 지난 시간

    public int hp = 100;
    public HPBar hpbar;
    public GameObject level;

    public bool isMoving = false; // 서서 공격인지 아니면 쫒아가면서 공격상태인지를 판단하는 변수
    private NavMeshAgent nvAgent; // 네비게이션을 위한 변수

    public void GetDamage(int damage)
    {
        hp -= damage;
        hpbar.SetHP(hp);
        Debug.Log("BulletSpawner:" + hp);
        if(hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timeAfterSpawn = 0f;

        spawnRate = Random.Range(spawnRateMin, spawnRateMax);

        target = FindObjectOfType<PlayerController>().transform;

        StartCoroutine(MonsterAI());
        nvAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        timeAfterSpawn += Time.deltaTime;

        if(timeAfterSpawn >= spawnRate)
        {
            timeAfterSpawn = 0f;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

            bullet.transform.LookAt(target);
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
    }

    IEnumerator MonsterAI()
    {
        while (hp > 0)
        {
            yield return new WaitForSeconds(0.2f);

            if(isMoving)
            {
                nvAgent.destination = target.position;
                nvAgent.isStopped = false;
            }
            else
            {
                nvAgent.isStopped = true;
            }
        }
    }
}

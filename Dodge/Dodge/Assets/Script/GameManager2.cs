using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour
{
    public Text timeText;
    public GameObject bulletSpawnerPrefab;
    public GameObject itemPrefab;
    public GameObject level; // 불렛 등 레벨 수정할 변수
    int prevTime;
    int spawnerCounter = 0;
    private float surviveTime;
    private bool isGameover;

    bool isEvent = false;
    float eventCountTime;

    int prevEventTime;

    // 리스트 & 제너릭을 사용해야함!
    List<GameObject> itemList = new List<GameObject>();
    List<GameObject> spawnerList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        surviveTime = 0;
        isGameover = false;
        prevTime = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameover)
        {
            surviveTime += Time.deltaTime;
            timeText.text = "Time" + (int)surviveTime;

            int currTime = (int)(surviveTime % 5f);
            Debug.Log(prevTime + ", " + currTime);
            // 5초마다 불렛스파너 추가!
            if (currTime == 0 && prevTime != currTime)
            {
                Vector3 randposBullet = new Vector3(Random.Range(-20f, 20f), 0f, Random.Range(-8f, 8f));
                GameObject bulletSpawner = Instantiate(bulletSpawnerPrefab, randposBullet, Quaternion.identity);
                bulletSpawner.transform.parent = level.transform;
                bulletSpawner.transform.localPosition = randposBullet;

                spawnerList.Add(bulletSpawner);

                Vector3 randposItem = new Vector3(Random.Range(-20f, 20f), 0f, Random.Range(-8f, 8f));
                GameObject item = Instantiate(itemPrefab, randposItem, Quaternion.identity);
                item.transform.parent = level.transform;
                item.transform.localPosition = randposItem;

                itemList.Add(item);
            }
            prevTime = currTime;

            int eventTime = (int)(surviveTime % 10f);
            if(eventTime == 0 && prevEventTime != eventTime)
            {
                // 아이템들을 모두 삭제!
                foreach(GameObject item in itemList)
                {
                    Destroy(item);
                }
                itemList.Clear();

                // 불렛 스파너들을 쫒아오게 한다.
                foreach (GameObject spawner in spawnerList)
                {
                    spawner.GetComponent<BulletSpawner>().isMoving = true;
                }
                isEvent = true;
                eventCountTime = 0f;
            }
            prevEventTime = eventTime;

            eventCountTime = 0f;

            if(isEvent && eventCountTime > 2f)
            {
                eventCountTime = 0f;
                isEvent = false;

                foreach(GameObject spawner in spawnerList)
                {
                    spawner.GetComponent<BulletSpawner>().isMoving = false;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody; // 이동에 사용할 리지드바디 컴포넌트
    public float speed = 8f; // 이동 속력
    public float rotSpeed = 120.0f;

    private Transform tr;

    public int hp = 100;
    public HPBar hpbar;

    private float spawnRate = 0.2f;
    private float timerAfterSpawn;
    public GameObject playerbulletPrefab;

    // Start is called before the first frame updates
    void Start()
    {
        // 게임 오브젝트에서 Rigidbody 컴포넌트를 찾아 playerRigidbody에 할당
        playerRigidbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // 수평축과 수직축의 입력값을 감지하여 저장
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        // 실제 이동 속도를 입력값과 이동 속력을 사용해 결정
        float xSpeed = xInput * speed;
        float zSpeed = zInput * speed;

        // Vector3 속도를 (xSpeed, 0, zSpeed)로 생성
        Vector3 newVelocity = new Vector3(xSpeed, -8f, zSpeed);

        //리지드바디의 속도에 newVelocity 할당
        playerRigidbody.velocity = newVelocity;

        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            Vector3 projectedPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            Vector3 currentPos = transform.position;
            Vector3 rotation = projectedPos - currentPos;
            tr.forward = rotation;
        }

        timerAfterSpawn += Time.deltaTime;

        if (Input.GetButton("Fire1") && timerAfterSpawn >= spawnRate)
        {
            timerAfterSpawn = 0;
            GameObject bullet = Instantiate(playerbulletPrefab, transform.position, transform.rotation);
        }
    }

    void Die() // public 빼기 - 자신이 처리하는 것이 좋을 것 같다. [정보 은닉 개념을 잘 사용!!]
    {
        // 자신의 게임 오브젝트를 비활성화
        gameObject.SetActive(false);

        // 씬에 존재하는 GameManager 타입의 오브젝트를 찾아서 가져오기
        GameManager gameManager = FindObjectOfType<GameManager>();
        // 가져온 GameManger 오브젝트의 EndGame () 메서드 실행
        gameManager.EndGame();
    }

    public void GetDamage(int damage)
    {
        hp -= damage;
        hpbar.SetHP(hp);
        if(hp <= 0)
        {
            Die();
        }
    }

    public void GetHeal(int heal)
    {
        hp += heal;
        if (hp > 100)
        {
            hp = 100;
        }
        hpbar.SetHP(hp);
    }
}

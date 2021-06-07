using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 8f;
    private Rigidbody bulletRigidbody;
    public int damage = 30;

    // Start is called before the first frame update
    void Start()
    {
        //게임 오브젝트에서 Rigidbody 컴포넌트를 찾아 bulletRigidbody에 할당
        bulletRigidbody = GetComponent<Rigidbody>();
        //리지드바디의 속도 = 앞쪽 방향 * 이동 속력
        bulletRigidbody.velocity = transform.forward * speed;

        // 3초 뒤에 자신의 게임 오브젝트 파괴
        Destroy(gameObject, 3f);
    }

    // 트리거 충돌 시 자동으로 실행되는 메서드
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 상대방 게임 오브젝트가 Player 태그를 가진 경우
        if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();

            if(bullet != null)
            {
                Destroy(bullet.gameObject);
            }

            Destroy(gameObject);
        }
        else if (other.tag == "BulletSpawner")
        {
            BulletSpawner spawner = other.GetComponent<BulletSpawner>();

            if (spawner != null)
            {
                spawner.GetDamage(damage);
            }

            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

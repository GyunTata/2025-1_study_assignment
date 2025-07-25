using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    const float CharacterJumpPower = 7f;
    const int MaxJump = 2;
    int RemainJump = 0;
    GameManager GM;

    void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // 좌클릭시 RemainJump를 하나 소모하여 CharacterJumpPower의 힘으로 점프한다.
        // ---------- TODO ---------- 
        if(Input.GetMouseButtonDown(0)) {
            if (RemainJump > 0)
            {
                RemainJump = RemainJump - 1;
                Jump(CharacterJumpPower);
            }
        }
        // -------------------- 
    }   

    // Jump with power
    void Jump(float power)
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector3(0, power, 0), ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // tag가 Platform인 것과 충돌하면 RemainJump를 초기화한다.
        // tag가 Obstacle인 것과 충돌하면 게임 오버한다.
        // ---------- TODO ---------- 
        switch(col.gameObject.tag)
        {
            case "Platform":
                RemainJump = 2;
                break;

            case "Obstacle":
                GM.GameOver();
                break;
        }

        // -------------------- 
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // tag가 Point인 것과 충돌하면 Point를 하나 얻고, 충돌한 오브젝트를 삭제한다.
        // ---------- TODO ---------- 
        if (col.gameObject.tag == "Point")
        {
            GM.GetPoint(1);
            Destroy(col.gameObject);
        }
        // -------------------- 
    }
}

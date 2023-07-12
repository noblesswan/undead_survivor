using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    
    public Vector2 inputVec;
    public float speed;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
        // Input : 유니티에서 받는 모든 입력을 관리하는 클래스
    }

 
    void FixedUpdate()
    {
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        // MovePosition은 위치 이동이기 때문에 현재 위치도 더해주어야 함
        rigid.MovePosition(rigid.position + nextVec);
    }

    void OnMove(InputValue value)
    {
        // 이 함수에서 입력하는 값을 인자로 받음
        // InputValue 타입의 매개변수 작성 

        inputVec = value.Get<Vector2>();

        // 여기에선 
    }

    private void LateUpdate()
    {
        // 프레임이 종료되기 직전에 실행되는 함수
        if (inputVec.x != 0) {
            spriter.flipX = inputVec.x < 0;
        }
    }
}

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
        // Input : ����Ƽ���� �޴� ��� �Է��� �����ϴ� Ŭ����
    }

 
    void FixedUpdate()
    {
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        // MovePosition�� ��ġ �̵��̱� ������ ���� ��ġ�� �����־�� ��
        rigid.MovePosition(rigid.position + nextVec);
    }

    void OnMove(InputValue value)
    {
        // �� �Լ����� �Է��ϴ� ���� ���ڷ� ����
        // InputValue Ÿ���� �Ű����� �ۼ� 

        inputVec = value.Get<Vector2>();

        // ���⿡�� 
    }

    private void LateUpdate()
    {
        // �������� ����Ǳ� ������ ����Ǵ� �Լ�
        if (inputVec.x != 0) {
            spriter.flipX = inputVec.x < 0;
        }
    }
}

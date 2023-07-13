# 1강 - 2D 오브젝트 생성

## 픽셀아트 스프라이트 사전설정
1. Pixel Per Unit : 주 캐릭터의 실제 크기로 설정
2. Filter Mode : Point로 설정
3. Compression : None 으로 설정 (압축하면 화질이 깨짐)
4. Sprit Mode : Multiple 
5. Slice : Grid By Cell Size 

## 트랜스폼
위치, 회전, 크기 속성을 관리하는 컴포넌트

## 컴포넌트 기반 엔진
유니티는 컴포넌트 기반 엔진이다. 즉, 게임 객체를 컴포넌트로 구성한다 

## 스프라이트 렌더러
게임 상에 스프라이트를 그리는 컴포넌트

## Rigidbody
물리 영향을 받는 컴포넌트

## collider
물리적인 충돌 면(피격범위)을 결정하는 컴포넌트

## Order in Layer
Player에게 그림자 스프라이트를 넣으려 할 때, 그림자가 캐릭터 밑에 있어야 한다. 그렇기 때문에 캐릭터는 5 그림자는 0으로 설정해서 그림자를 뒤에 배치한다

# 2강 - 이동

## 키보드 입력 받기
1. 입력 값을 저장할 변수부터 제작
```csharp
Vector2 inputVec;
//변수의타입 변수의이름

```
## 물리 이동
```csharp
// 선언
Rigidbody2D rigid;

// 초기화는 Awake에서 진행
void Awake()
{
    rigid = GetComponent<Rigidbody2D>();
    // GetComponent<T> : 오브젝트에서 컴포넌트를 가져오는 함수
    // 이 코드를 통해 Player 안에 있는 RigidBody2D가 rigid 변수에 들어감
}
```

```csharp
    void FixedUpdate()
    {   // 물리 연산 프레임마다 호출되는 생명주기 함수 
        // 1. 힘을 준다
        rigid.AddForce(inputVec);

        // 2. 속도 제어
        rigid.velocity = inputVec;

        // 3. 위치 이동
        // MovePosition은 위치 이동이기 때문에 현재 위치도 더해주어야 함
        rigid.MovePosition(rigid.position + inputVec);
    }
```
3 위치 이동의 경우, rigid.position은 플레이어가 현재 (0,0) 위치에 존재함. inputVec은 입력받은 값이기 때문에 9가지의 방향으로 나갈 수 있음. 이 값을 더해주어야 함

## 물리 이동 구현

3번처럼 구현하면 프레임 별로 이동 속도가 달라질 수 있다는 문제점이 발생한다. 그렇기 때문에 어떤 컴퓨터 환경에서든 똑같은 속도를 받아내기 위해 따로 변수를 추가해줘야 한다

```csharp
Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
```
- noramlized의 경우, 대각선으로 이동할 시 피타고라스의 법칙을 적용해서 루트2로 움직이게 함
- Time class에서 fixedDeltaTime은 물리 프레임 하나가 소비한 시간. 즉, 물리 프레임만큼 소모된 시간. 
- 최종적으로 다음으로 나아가야할 방향의 크기까지 정해줌  

```csharp
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        // MovePosition은 위치 이동이기 때문에 현재 위치도 더해주어야 함
        rigid.MovePosition(rigid.position + nextVec);
```
최종적으로는 다음과 같은 코드의 형태를 만들어낼 수 있다. 대각선 이동 보정과 스피드, 물리 프레임까지 고정한 변수 `nextVec`을 현재 위치인 `rigid.position`에 더해줘서 이동 값을 보정한다. 

## GetAxis VS GetAxisRaw
GetAxis - 자연스럽게 움직임을 보정해줌<br>
GetAxisRaw - 움직임을 딱딱 끊어줌

# 2강+ - Input System
2강에서 Project Setting - Input Manager에서 값을 받아 사용했지만, 최근에는 Unity가 Input System을 사용해서 움직이는 방법을 구현해냈음 <br>
Window - Package Manager - Unity Registry - Input System 다운로드 <br>

## 인풋 액션 설정
Actions - 기존의 input Button과 비슷함 
Action Type - 버튼 클릭인지, 값을 내보내는 것인지 구분 
Control Type - value가 어떤 타입의 값인지,값의 형식을 결정
Interaction - 인풋의 호출 타이밍 지정. 사용자가 어떻게 행동할 때 작동되는지를 결정하는 속성
Processor - 인풋의 값을 후보정 

## 스크립트 적용
```csharp
    void Update()
    {
        inputVec.x = Input.GetAxis("Horizontal");
        inputVec.y = Input.GetAxis("Vertical");
        // Input : 유니티에서 받는 모든 입력을 관리하는 클래스
    }
```
해당 코드는 과감하게 삭제 한다.

```csharp
    void OnMove(InputValue value)
    {
        // 이 함수에서 입력하는 값을 인자로 받음
        // InputValue 타입의 매개변수 작성 

        inputVec = value.Get<Vector2>();
    }
```
해당 코드에서 `inputVec = value`를 사용할 수 없는 이유는 `inputVec`은 Vector2이고 `value`는 `InputValue`이기 때문이다. 그렇기 때문에 다음과 같이 코드를 사용해서 만드는 것이다. 참고로, `Get<Vector2>()`에서 `Get`은 함수이기 때문에 뒤에 `()`를 붙여줘야 한다. 

```csharp
    void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        // MovePosition은 위치 이동이기 때문에 현재 위치도 더해주어야 함
        rigid.MovePosition(rigid.position + nextVec);
    }

```
`OnMove`에서 `normalize`를 이미 하고 있기 때문에 해당 코드에서는 생략해도 된다.  

# 3강 - 셀 애니메이션 제작
## 방향 바라보기
Sprite Renderer - Flip X. <br>
즉, x가 -1일 때 반대 방향으로 봐야한다.이를 위해 스크립트를 통해 해결해야 한다.

```csharp
    private void LateUpdate()
    {
        // 프레임이 종료되기 직전에 실행되는 함수
        if (inputVec.x != 0) {
            spriter.flipX = inputVec.x < 0;
        }
    }
```
해당 코드를 작성하는 이유는 왼쪽으로 돌았을 때, 즉 x가 0보다 작을 때 flip을 통해 좌우반전을 시키기 위해서이다. 그러기 위해서 다음과 같이 코드를 작성했다. <br>
`만약 x가 0보다 작다면, flipx를 뒤집어라`를 코드로 실현하기 위해서 x가 0이 아니라면, 즉 `inputVec.x`가 0이 아닐 경우, flipX를 체크해야한다. <br>
해당 코드에서 `spriter.flipX = inputVec.x < 0;`는 true or false 값만 받아야 한다. x축이 -로 됐을 때인데, 여기에 또 if문을 써야 하는가? 그렇지 않다. <br>
만약 좌측 키를 누르고 있다면 -1이 되기 때문에 `inputVec.x < 0` 의 값은  true가 된다. true가 그대로 flipX에 들어간다. 우측은 +1인데, 0보다 크다. 그렇다면 `inputVec.x < 0` 식이 false, 즉 틀린 것이기 때문에 false가 flipX에 들어간다. 



using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    // 애니메이션 출력을 위한 해시값 선언
    [SerializeField] Animator animator;
    private static int idleHash = Animator.StringToHash("Idle");
    private static int runHash = Animator.StringToHash("Run");
    private static int jumpHash = Animator.StringToHash("Jump");
    private static int fallHash = Animator.StringToHash("Fall");
    private int curAniHash; // 현재 플레이어의 상태를 뜻하는 해시값

    // 이동에 필요한 참조와 값들 선언
    [SerializeField] Rigidbody2D rigid;         // 리기드바디2D 참조
    [SerializeField] Transform playerTransform; // 플레이어 스프라이트 좌우반전을 위한 트랜스폼 참조
    [SerializeField] bool isGrounded;           // 점프 활성화 여부

    // 이동 시 플레이어에게 가해지는 속도값(점프력) / 해당 속도값으로 인해 생기는 최대 가속도값
    [SerializeField] float movePower; [SerializeField] float maxMoveSpeed;
    [SerializeField] float jumpPower; [SerializeField] float maxMFallSpeed;

    private float x;        // 플레이어가 이동하는 방향의 축
    private float ScaleX;   // 플레이어의 x축의 스케일 값을 가져오는 float 자료형

    // =========================================================================================
    // =========================================================================================
    private void Update()
    {
        // A, D, 화살표 좌우로 플레이어 이동이 가능하다.
        x = Input.GetAxisRaw("Horizontal");

        // 스페이스 입력 시, 플레이어는 점프한다.
        if (Input.GetKeyDown(KeyCode.Space)) { Jump(); }

        // 플레이어 캐릭터의 발이 땅에 닿았는지에 대한 여부를 판단하여 점프를 제한하는 함수
        GroundCheck();

        // 애니메이션을 재생하는 함수
        AnimatorPlay();
    }
    // =========================================================================================
    private void FixedUpdate()
    {
        // 플레이어의 움직임 동작
        Move();
    }
    // =========================================================================================
    // =========================================================================================
    void Move()
    {
        // 이동 시 가속도를 조절해주는 if문
        if (rigid.velocity.x > maxMoveSpeed) { rigid.velocity = new Vector2(maxMoveSpeed, rigid.velocity.y); }
        else if (rigid.velocity.x < -maxMoveSpeed) { rigid.velocity = new Vector2(-maxMoveSpeed, rigid.velocity.y); }

        // 점프 후 떨어질 시 가해지는 가속도를 조절해줄 수 있는 if문
        if (rigid.velocity.y < -maxMFallSpeed) { rigid.velocity = new Vector2(rigid.velocity.x, -maxMFallSpeed); }

        // 플레이어의 스프라이트를 좌우반전 해줄 수 있는 코드 작성
        Vector3 dir = Vector3.zero;
        // 왼쪽을 바라보는 if문
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            ScaleX = -1;
            dir = Vector3.left;
            transform.localScale = new Vector3(ScaleX, 1, 1);
        }
        // 오른쪽을 바라보는 if문
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            ScaleX = 1;
            dir = Vector3.right;
            transform.localScale = new Vector3(ScaleX, 1, 1);
        }
        // 이동 코드 구현
        rigid.AddForce(dir * movePower, ForceMode2D.Force);
    }
    // =========================================================================================
    // =========================================================================================
    // 점프하는 기능의 함수
    void Jump()
    {
        if (isGrounded == false) return;
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
    // 점프수를 제한하는 함수
    void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.2f);
        if (hit.collider != null) { isGrounded = true; }
        else { isGrounded = false; }
    }
    // =========================================================================================
    // =========================================================================================
    void AnimatorPlay()
    {
        int checkAniHash;

        // 점프&떨어짐 애니메이션 해시값 설정 if문
        if (rigid.velocity.y > 1f) { checkAniHash = jumpHash; }
        else if (rigid.velocity.y < -1f) { checkAniHash = fallHash; }

        // 정자세&달리기 애니메이션 해시값 설정 if문
        else if (rigid.velocity.sqrMagnitude < 0.001f) { checkAniHash = idleHash; }
        else { checkAniHash = runHash; }

        // 애니메이션 재생 if 문
        if (curAniHash != checkAniHash)
        {
            curAniHash = checkAniHash;
            animator.Play(curAniHash);
        }
    }
}

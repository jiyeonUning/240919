using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    // �ִϸ��̼� ����� ���� �ؽð� ����
    [SerializeField] Animator animator;
    private static int idleHash = Animator.StringToHash("Idle");
    private static int runHash = Animator.StringToHash("Run");
    private static int jumpHash = Animator.StringToHash("Jump");
    private static int fallHash = Animator.StringToHash("Fall");
    private int curAniHash; // ���� �÷��̾��� ���¸� ���ϴ� �ؽð�

    // �̵��� �ʿ��� ������ ���� ����
    [SerializeField] Rigidbody2D rigid;         // �����ٵ�2D ����
    [SerializeField] Transform playerTransform; // �÷��̾� ��������Ʈ �¿������ ���� Ʈ������ ����
    [SerializeField] bool isGrounded;           // ���� Ȱ��ȭ ����

    // �̵� �� �÷��̾�� �������� �ӵ���(������) / �ش� �ӵ������� ���� ����� �ִ� ���ӵ���
    [SerializeField] float movePower; [SerializeField] float maxMoveSpeed;
    [SerializeField] float jumpPower; [SerializeField] float maxMFallSpeed;

    private float x;        // �÷��̾ �̵��ϴ� ������ ��
    private float ScaleX;   // �÷��̾��� x���� ������ ���� �������� float �ڷ���

    // =========================================================================================
    // =========================================================================================
    private void Update()
    {
        // A, D, ȭ��ǥ �¿�� �÷��̾� �̵��� �����ϴ�.
        x = Input.GetAxisRaw("Horizontal");

        // �����̽� �Է� ��, �÷��̾�� �����Ѵ�.
        if (Input.GetKeyDown(KeyCode.Space)) { Jump(); }

        // �÷��̾� ĳ������ ���� ���� ��Ҵ����� ���� ���θ� �Ǵ��Ͽ� ������ �����ϴ� �Լ�
        GroundCheck();

        // �ִϸ��̼��� ����ϴ� �Լ�
        AnimatorPlay();
    }
    // =========================================================================================
    private void FixedUpdate()
    {
        // �÷��̾��� ������ ����
        Move();
    }
    // =========================================================================================
    // =========================================================================================
    void Move()
    {
        // �̵� �� ���ӵ��� �������ִ� if��
        if (rigid.velocity.x > maxMoveSpeed) { rigid.velocity = new Vector2(maxMoveSpeed, rigid.velocity.y); }
        else if (rigid.velocity.x < -maxMoveSpeed) { rigid.velocity = new Vector2(-maxMoveSpeed, rigid.velocity.y); }

        // ���� �� ������ �� �������� ���ӵ��� �������� �� �ִ� if��
        if (rigid.velocity.y < -maxMFallSpeed) { rigid.velocity = new Vector2(rigid.velocity.x, -maxMFallSpeed); }

        // �÷��̾��� ��������Ʈ�� �¿���� ���� �� �ִ� �ڵ� �ۼ�
        Vector3 dir = Vector3.zero;
        // ������ �ٶ󺸴� if��
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            ScaleX = -1;
            dir = Vector3.left;
            transform.localScale = new Vector3(ScaleX, 1, 1);
        }
        // �������� �ٶ󺸴� if��
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            ScaleX = 1;
            dir = Vector3.right;
            transform.localScale = new Vector3(ScaleX, 1, 1);
        }
        // �̵� �ڵ� ����
        rigid.AddForce(dir * movePower, ForceMode2D.Force);
    }
    // =========================================================================================
    // =========================================================================================
    // �����ϴ� ����� �Լ�
    void Jump()
    {
        if (isGrounded == false) return;
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
    // �������� �����ϴ� �Լ�
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

        // ����&������ �ִϸ��̼� �ؽð� ���� if��
        if (rigid.velocity.y > 1f) { checkAniHash = jumpHash; }
        else if (rigid.velocity.y < -1f) { checkAniHash = fallHash; }

        // ���ڼ�&�޸��� �ִϸ��̼� �ؽð� ���� if��
        else if (rigid.velocity.sqrMagnitude < 0.001f) { checkAniHash = idleHash; }
        else { checkAniHash = runHash; }

        // �ִϸ��̼� ��� if ��
        if (curAniHash != checkAniHash)
        {
            curAniHash = checkAniHash;
            animator.Play(curAniHash);
        }
    }
}

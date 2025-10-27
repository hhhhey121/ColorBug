using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ȷ�� LineRenderer ����
[RequireComponent(typeof(PlayerLife), typeof(PlayerMovement), typeof(LineRenderer))]
public class PlayerLaserAbility : MonoBehaviour
{
    [Header("����״̬ (ֻ��)")]
    [SerializeField] private bool hasAbility = false;
    [SerializeField] private int laserAmmo = 0;
    private const int MAX_AMMO = 3;

    [Header("����")]
    public Transform firePoint; // �����
    public float absorbCooldown = 0.5f;
    private bool canAbsorb = true;

    [Header("�������")]
    // ���¡�"ǰ��2����λ"
    public float absorbCheckDistance = 2.0f;
    // ���¡������ӵĸ߶� (һ���ܴ��ֵ�����Ը߶Ȳ�)
    public float absorbCheckHeight = 10.0f;
    public LayerMask laserLayer;

    // ���¡��������Ҳ��Ϊ2����λ (����԰����޸�)
    public float fireCheckDistance = 2.0f;
    public float fireCheckHeight = 10.0f;
    public LayerMask blueSquareLayer;

    [Header("�Ӿ�����")]
    public float lineVisibleTime = 0.2f;
    public Color absorbLineColor = Color.cyan;
    public Color fireLineColor = Color.red;

    // �ڲ�����
    private PlayerLife playerLife;
    private PlayerMovement playerMovement;
    private LineRenderer lineRenderer;

    void Start()
    {
        playerLife = GetComponent<PlayerLife>();
        playerMovement = GetComponent<PlayerMovement>();
        lineRenderer = GetComponent<LineRenderer>();

        // ���� LineRenderer
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2; // ֻ��Ҫ������ (���, �յ�)
    }

    void Update()
    {
        if (playerLife.isDead || !hasAbility) return;

        // 1. ���� (Fire2 - ����Ҽ�)
        if (Input.GetButtonDown("Fire2"))
        {
            TryAbsorb();
        }

        // 2. ���� (Fire1 - ������)
        if (Input.GetButtonDown("Fire1"))
        {
            TryFire();
        }
    }

    private void TryAbsorb()
    {
        if (laserAmmo >= MAX_AMMO) return;
        if (!canAbsorb) return;

        // �����߼���ʹ�� OverlapBox
        float facingDir = playerMovement.GetFacingDirection();

        // ���ӵ����ĵ� (�ڷ����ǰ�� 1 ����λ��)
        Vector2 boxCenter = (Vector2)firePoint.position + new Vector2(facingDir * (absorbCheckDistance / 2), 0);
        // ���ӵĴ�С (��2����λ, ��10����λ)
        Vector2 boxSize = new Vector2(absorbCheckDistance, absorbCheckHeight);

        // ���
        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, laserLayer);

        if (hit != null) // �������м���
        {
            LethalLaser laser = hit.GetComponent<LethalLaser>();
            if (laser != null)
            {
                laser.BeAbsorbed(); // �������ʧ
                laserAmmo++;
                Debug.Log("���ճɹ�! ��ҩ: " + laserAmmo);
                StartCoroutine(AbsorbCooldownRoutine());
                // ���Ӿ����������ߵ�Ŀ������ĵ�
                StartCoroutine(ShowLaserLine(hit.transform.position, absorbLineColor));
            }
        }
    }

    private void TryFire()
    {
        if (laserAmmo <= 0) return;

        // �����߼���ʹ�� OverlapBox
        float facingDir = playerMovement.GetFacingDirection();
        Vector2 boxCenter = (Vector2)firePoint.position + new Vector2(facingDir * (fireCheckDistance / 2), 0);
        Vector2 boxSize = new Vector2(fireCheckDistance, fireCheckHeight);

        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, blueSquareLayer);

        if (hit != null) // ����������ɫ����
        {
            BlueSquare square = hit.GetComponent<BlueSquare>();
            if (square != null)
            {
                // �����߼���ֻ�гɹ����в����ĵ�ҩ
                square.BeDestroyed();
                laserAmmo--;
                Debug.Log("����ɹ�! ���з���! ʣ�൯ҩ: " + laserAmmo);
                // ���Ӿ����������ߵ�Ŀ������ĵ�
                StartCoroutine(ShowLaserLine(hit.transform.position, fireLineColor));
            }
        }
    }

    // (�� AbilityManager �ڽ���ʱ����)
    public void UnlockAndGiveFirstCharge()
    {
        hasAbility = true;
        laserAmmo = 1;
        Debug.Log("�������ѽ������ѻ�ü����������������� 1 �η��䣡");
    }

    public bool HasAbility()
    {
        return hasAbility;
    }

    private IEnumerator AbsorbCooldownRoutine()
    {
        canAbsorb = false;
        yield return new WaitForSeconds(absorbCooldown);
        canAbsorb = true;
    }

    // (��ʾ���ߵ�Э��)
    private IEnumerator ShowLaserLine(Vector3 targetPoint, Color color)
    {
        lineRenderer.enabled = true;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        // ����Ҫ�������֮ǰ��ѡ�� LineRenderer �� "Use World Space"
        // lineRenderer.SetPosition(0, firePoint.position);
        // lineRenderer.SetPosition(1, targetPoint);

        // ���Ƽ��������û�й�ѡ "Use World Space"���������ã�
        lineRenderer.SetPosition(0, firePoint.localPosition); // ��Է����
        lineRenderer.SetPosition(1, transform.InverseTransformPoint(targetPoint)); // ����������תΪ��������

        yield return new WaitForSeconds(lineVisibleTime);
        lineRenderer.enabled = false;
    }

    // ���޸ġ����Թ��ߣ���������
    private void OnDrawGizmosSelected()
    {
        if (playerMovement == null || firePoint == null) return;

        float facingDir = playerMovement.GetFacingDirection();

        // 1. ���������ա����� (��ɫ)
        Vector2 absorbCenter = (Vector2)firePoint.position + new Vector2(facingDir * (absorbCheckDistance / 2), 0);
        Vector2 absorbSize = new Vector2(absorbCheckDistance, absorbCheckHeight);
        Gizmos.color = new Color(0, 1, 1, 0.3f); // ��͸����ɫ
        Gizmos.DrawCube(absorbCenter, absorbSize);

        // 2. ���������䡱���� (��ɫ)
        Vector2 fireCenter = (Vector2)firePoint.position + new Vector2(facingDir * (fireCheckDistance / 2), 0);
        Vector2 fireSize = new Vector2(fireCheckDistance, fireCheckHeight);
        Gizmos.color = new Color(1, 0, 0, 0.3f); // ��͸����ɫ
        Gizmos.DrawCube(fireCenter, fireSize);
    }
}
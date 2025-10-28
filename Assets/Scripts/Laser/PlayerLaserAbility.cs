using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���޸ġ��Ƴ� LineRenderer, ��������
[RequireComponent(typeof(PlayerLife), typeof(PlayerMovement))]
public class PlayerLaserAbility : MonoBehaviour
{
    [Header("����״̬ (ֻ��)")]
    [SerializeField] private bool hasAbility = false;
    [SerializeField] private int laserAmmo = 0;
    private const int MAX_AMMO = 3;

    [Header("����")]
    public Transform firePoint;
    public float absorbCooldown = 0.5f;
    private bool canAbsorb = true;

    [Header("�������")]
    public float absorbCheckDistance = 4.0f; // �Ѹ�Ϊ4
    public float absorbCheckHeight = 10.0f;
    public LayerMask laserLayer;

    public float fireCheckDistance = 4.0f; // �Ѹ�Ϊ4
    public float fireCheckHeight = 10.0f;
    public LayerMask blueSquareLayer;

    // ���޸ġ��Ӿ�����
    // public float lineVisibleTime = 0.2f; // ������Ҫ
    // public Color absorbLineColor = Color.cyan; // ������Ҫ
    // public Color fireLineColor = Color.red; // ������Ҫ
    [Header("�Ӿ����� Prefab")]
    public GameObject laserEffectPrefab; // ��������������� LaserEffect Ԥ����

    // �ڲ�����
    private PlayerLife playerLife;
    private PlayerMovement playerMovement;
    // private LineRenderer lineRenderer; // ��ɾ����

    void Start()
    {
        playerLife = GetComponent<PlayerLife>();
        playerMovement = GetComponent<PlayerMovement>();
        // lineRenderer = GetComponent<LineRenderer>(); // ��ɾ����
        // lineRenderer.enabled = false; // ��ɾ����
        // lineRenderer.positionCount = 2; // ��ɾ����
    }

    void Update()
    {
        if (playerLife.isDead || !hasAbility) return;

        if (Input.GetButtonDown("Fire2")) // ����
        {
            Debug.Log("Fire2 pressed!"); // <-- �����־1
            TryAbsorb();
        }
        if (Input.GetButtonDown("Fire1")) // ����
        {
            Debug.Log("Fire1 pressed!"); // <-- �����־2
            TryFire();
        }
    }

    private void TryAbsorb()
    {
        if (laserAmmo >= MAX_AMMO) return;
        if (!canAbsorb) return;

        float facingDir = playerMovement.GetFacingDirection();
        Vector2 boxCenter = (Vector2)firePoint.position + new Vector2(facingDir * (absorbCheckDistance / 2), 0);
        Vector2 boxSize = new Vector2(absorbCheckDistance, absorbCheckHeight);
        Debug.Log("TryAbsorb: Checking OverlapBox..."); // <-- �����־3
        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, laserLayer);

        if (hit != null)
        {
            Debug.Log("TryAbsorb: Hit successful! Target: " + hit.name); // <-- �����־4
            LethalLaser laser = hit.GetComponent<LethalLaser>();
            if (laser != null)
            {
                laser.BeAbsorbed();
                laserAmmo++;
                Debug.Log("���ճɹ�! ��ҩ: " + laserAmmo);
                StartCoroutine(AbsorbCooldownRoutine());
                // ���޸ġ��Ӿ����� - ʵ����Ч��
                SpawnLaserEffect(hit.transform.position, true);
            }
            else
            {
                Debug.Log("TryAbsorb: OverlapBox missed!"); // <-- �����־5
            }
        }
    }

    private void TryFire()
    {
        Debug.Log("TryFire: Checking OverlapBox..."); // <-- �����־6
        if (laserAmmo <= 0) return;

        float facingDir = playerMovement.GetFacingDirection();
        Vector2 boxCenter = (Vector2)firePoint.position + new Vector2(facingDir * (fireCheckDistance / 2), 0);
        Vector2 boxSize = new Vector2(fireCheckDistance, fireCheckHeight);
        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, blueSquareLayer);

        if (hit != null)
        {
            Debug.Log("TryFire: Hit successful! Target: " + hit.name); // <-- �����־7
            BlueSquare square = hit.GetComponent<BlueSquare>();
            if (square != null)
            {
                square.BeDestroyed();
                laserAmmo--;
                Debug.Log("����ɹ�! ���з���! ʣ�൯ҩ: " + laserAmmo);
                // ���޸ġ��Ӿ����� - ʵ����Ч��
                SpawnLaserEffect(hit.transform.position, false);
            }
            else
            {
                Debug.Log("TryFire: OverlapBox missed!"); // <-- �����־8
            }
        }
    }

    // (UnlockAbility, HasAbility, AbsorbCooldownRoutine ���ֲ���)
    public void UnlockAbility()
    {
        hasAbility = true;
        laserAmmo = 0;
        Debug.Log("�������ѽ������ѻ�ü������������ڿ������ռ����ˡ�");
    }
    public bool HasAbility() { return hasAbility; }
    private IEnumerator AbsorbCooldownRoutine()
    {
        canAbsorb = false;
        yield return new WaitForSeconds(absorbCooldown);
        canAbsorb = true;
    }

    // ��ɾ����ShowLaserLine Э��
    // private IEnumerator ShowLaserLine(...) { ... }

    // ��������ʵ�����ͳ�ʼ��Ч���ķ���
    private void SpawnLaserEffect(Vector3 targetPoint, bool isAbsorb)
    {
        if (laserEffectPrefab != null && firePoint != null)
        {
            // �� FirePoint ��λ��ʵ����
            GameObject effectInstance = Instantiate(laserEffectPrefab, firePoint.position, Quaternion.identity);

            // ��ȡ�������ű�
            LaserEffectController controller = effectInstance.GetComponent<LaserEffectController>();
            if (controller != null)
            {
                // ��ʼ����������㡢�յ��Ч�����ͣ�
                controller.Initialize(firePoint.position, targetPoint, isAbsorb);
            }
            else
            {
                Debug.LogError("LaserEffect Prefab û�й��� LaserEffectController �ű�!");
                Destroy(effectInstance); // ������Чʵ��
            }
        }
    }

    // (OnDrawGizmosSelected ���ֲ��䣬���ڵ��Լ���)
    private void OnDrawGizmosSelected()
    {
        // ... (���ֲ���) ...
        if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement == null || firePoint == null) return;
        float facingDir = playerMovement.GetFacingDirection();
        Vector2 absorbCenter = (Vector2)firePoint.position + new Vector2(facingDir * (absorbCheckDistance / 2), 0);
        Vector2 absorbSize = new Vector2(absorbCheckDistance, absorbCheckHeight);
        Gizmos.color = new Color(0, 1, 1, 0.3f);
        Gizmos.DrawCube(absorbCenter, absorbSize);
        Vector2 fireCenter = (Vector2)firePoint.position + new Vector2(facingDir * (fireCheckDistance / 2), 0);
        Vector2 fireSize = new Vector2(fireCheckDistance, fireCheckHeight);
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(fireCenter, fireSize);
    }
}
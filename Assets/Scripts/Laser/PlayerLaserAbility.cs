using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 确保 LineRenderer 存在
[RequireComponent(typeof(PlayerLife), typeof(PlayerMovement), typeof(LineRenderer))]
public class PlayerLaserAbility : MonoBehaviour
{
    [Header("能力状态 (只读)")]
    [SerializeField] private bool hasAbility = false;
    [SerializeField] private int laserAmmo = 0;
    private const int MAX_AMMO = 3;

    [Header("配置")]
    public Transform firePoint; // 发射点
    public float absorbCooldown = 0.5f;
    private bool canAbsorb = true;

    [Header("检测设置")]
    public float absorbCheckDistance = 4.0f;
    public float absorbCheckHeight = 10.0f;
    public LayerMask laserLayer;

    public float fireCheckDistance = 4.0f;
    public float fireCheckHeight = 10.0f;
    public LayerMask blueSquareLayer;

    [Header("视觉反馈")]
    public float lineVisibleTime = 0.2f;
    public Color absorbLineColor = Color.cyan;
    public Color fireLineColor = Color.red;

    // 内部引用
    private PlayerLife playerLife;
    private PlayerMovement playerMovement;
    private LineRenderer lineRenderer;

    void Start()
    {
        playerLife = GetComponent<PlayerLife>();
        playerMovement = GetComponent<PlayerMovement>();
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        if (playerLife.isDead || !hasAbility) return;

        // 1. 吸收 (Fire2 - 鼠标右键)
        if (Input.GetButtonDown("Fire2"))
        {
            TryAbsorb();
        }

        // 2. 发射 (Fire1 - 鼠标左键)
        if (Input.GetButtonDown("Fire1"))
        {
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
        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, laserLayer);

        if (hit != null)
        {
            LethalLaser laser = hit.GetComponent<LethalLaser>();
            if (laser != null)
            {
                laser.BeAbsorbed();
                laserAmmo++;
                Debug.Log("吸收成功! 弹药: " + laserAmmo);
                StartCoroutine(AbsorbCooldownRoutine());
                StartCoroutine(ShowLaserLine(hit.transform.position, absorbLineColor));
            }
        }
    }

    private void TryFire()
    {
        if (laserAmmo <= 0) return;

        float facingDir = playerMovement.GetFacingDirection();
        Vector2 boxCenter = (Vector2)firePoint.position + new Vector2(facingDir * (fireCheckDistance / 2), 0);
        Vector2 boxSize = new Vector2(fireCheckDistance, fireCheckHeight);
        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, blueSquareLayer);

        if (hit != null)
        {
            BlueSquare square = hit.GetComponent<BlueSquare>();
            if (square != null)
            {
                square.BeDestroyed();
                laserAmmo--;
                Debug.Log("发射成功! 击中方块! 剩余弹药: " + laserAmmo);
                StartCoroutine(ShowLaserLine(hit.transform.position, fireLineColor));
            }
        }
    }

    // -----------------------------------------------------
    // !!! 【关键修复】 !!!
    // (由 AbilityManager 在解锁时调用)
    // 重命名了方法，并且把 laserAmmo 设为 0
    public void UnlockAbility()
    {
        hasAbility = true;
        laserAmmo = 0; // 【修复】解锁时弹药为 0
        Debug.Log("【能力已解锁】已获得激光能力！现在可以吸收激光了。");
    }
    // -----------------------------------------------------

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

    private IEnumerator ShowLaserLine(Vector3 targetPoint, Color color)
    {
        lineRenderer.enabled = true;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.SetPosition(0, firePoint.localPosition);
        lineRenderer.SetPosition(1, transform.InverseTransformPoint(targetPoint));
        yield return new WaitForSeconds(lineVisibleTime);
        lineRenderer.enabled = false;
    }

    // (调试工具：画出检测盒)
    private void OnDrawGizmosSelected()
    {
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
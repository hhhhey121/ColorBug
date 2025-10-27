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
    // 【新】"前方2个单位"
    public float absorbCheckDistance = 2.0f;
    // 【新】检测盒子的高度 (一个很大的值来忽略高度差)
    public float absorbCheckHeight = 10.0f;
    public LayerMask laserLayer;

    // 【新】发射距离也设为2个单位 (你可以按需修改)
    public float fireCheckDistance = 2.0f;
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

        // 设置 LineRenderer
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2; // 只需要两个点 (起点, 终点)
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

        // 【新逻辑】使用 OverlapBox
        float facingDir = playerMovement.GetFacingDirection();

        // 盒子的中心点 (在发射点前方 1 个单位处)
        Vector2 boxCenter = (Vector2)firePoint.position + new Vector2(facingDir * (absorbCheckDistance / 2), 0);
        // 盒子的大小 (宽2个单位, 高10个单位)
        Vector2 boxSize = new Vector2(absorbCheckDistance, absorbCheckHeight);

        // 检测
        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, laserLayer);

        if (hit != null) // 盒子里有激光
        {
            LethalLaser laser = hit.GetComponent<LethalLaser>();
            if (laser != null)
            {
                laser.BeAbsorbed(); // 命令激光消失
                laserAmmo++;
                Debug.Log("吸收成功! 弹药: " + laserAmmo);
                StartCoroutine(AbsorbCooldownRoutine());
                // 【视觉反馈】连线到目标的中心点
                StartCoroutine(ShowLaserLine(hit.transform.position, absorbLineColor));
            }
        }
    }

    private void TryFire()
    {
        if (laserAmmo <= 0) return;

        // 【新逻辑】使用 OverlapBox
        float facingDir = playerMovement.GetFacingDirection();
        Vector2 boxCenter = (Vector2)firePoint.position + new Vector2(facingDir * (fireCheckDistance / 2), 0);
        Vector2 boxSize = new Vector2(fireCheckDistance, fireCheckHeight);

        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, blueSquareLayer);

        if (hit != null) // 盒子里有蓝色方块
        {
            BlueSquare square = hit.GetComponent<BlueSquare>();
            if (square != null)
            {
                // 【新逻辑】只有成功击中才消耗弹药
                square.BeDestroyed();
                laserAmmo--;
                Debug.Log("发射成功! 击中方块! 剩余弹药: " + laserAmmo);
                // 【视觉反馈】连线到目标的中心点
                StartCoroutine(ShowLaserLine(hit.transform.position, fireLineColor));
            }
        }
    }

    // (由 AbilityManager 在解锁时调用)
    public void UnlockAndGiveFirstCharge()
    {
        hasAbility = true;
        laserAmmo = 1;
        Debug.Log("【能力已解锁】已获得激光能力，并储存了 1 次发射！");
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

    // (显示连线的协程)
    private IEnumerator ShowLaserLine(Vector3 targetPoint, Color color)
    {
        lineRenderer.enabled = true;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        // 【重要】如果你之前勾选了 LineRenderer 的 "Use World Space"
        // lineRenderer.SetPosition(0, firePoint.position);
        // lineRenderer.SetPosition(1, targetPoint);

        // 【推荐】如果你没有勾选 "Use World Space"，这样设置：
        lineRenderer.SetPosition(0, firePoint.localPosition); // 相对发射点
        lineRenderer.SetPosition(1, transform.InverseTransformPoint(targetPoint)); // 将世界坐标转为本地坐标

        yield return new WaitForSeconds(lineVisibleTime);
        lineRenderer.enabled = false;
    }

    // 【修改】调试工具：画出检测盒
    private void OnDrawGizmosSelected()
    {
        if (playerMovement == null || firePoint == null) return;

        float facingDir = playerMovement.GetFacingDirection();

        // 1. 画出“吸收”盒子 (青色)
        Vector2 absorbCenter = (Vector2)firePoint.position + new Vector2(facingDir * (absorbCheckDistance / 2), 0);
        Vector2 absorbSize = new Vector2(absorbCheckDistance, absorbCheckHeight);
        Gizmos.color = new Color(0, 1, 1, 0.3f); // 半透明青色
        Gizmos.DrawCube(absorbCenter, absorbSize);

        // 2. 画出“发射”盒子 (红色)
        Vector2 fireCenter = (Vector2)firePoint.position + new Vector2(facingDir * (fireCheckDistance / 2), 0);
        Vector2 fireSize = new Vector2(fireCheckDistance, fireCheckHeight);
        Gizmos.color = new Color(1, 0, 0, 0.3f); // 半透明红色
        Gizmos.DrawCube(fireCenter, fireSize);
    }
}
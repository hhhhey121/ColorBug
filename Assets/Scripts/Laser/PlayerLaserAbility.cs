using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 引入 TextMeshPro 命名空间

// 【修改】添加 AudioSource
[RequireComponent(typeof(PlayerLife), typeof(PlayerMovement), typeof(AudioSource))]
public class PlayerLaserAbility : MonoBehaviour
{
    [Header("能力状态 (只读)")]
    [SerializeField] private bool hasAbility = false;
    [SerializeField] private int laserAmmo = 0;
    private const int MAX_AMMO = 3;

    [Header("UI (炮弹显示)")]
    [SerializeField] private TextMeshProUGUI ammoText; // 拖入你的 TextMeshPro UI

    [Header("配置")]
    public Transform firePoint;
    public float absorbCooldown = 0.5f;
    private bool canAbsorb = true;

    [Header("检测设置")]
    public float absorbCheckDistance = 2.0f; // 
    public float absorbCheckHeight = 10.0f;
    public LayerMask laserLayer;

    public float fireCheckDistance = 2.0f; // 
    public float fireCheckHeight = 10.0f;
    public LayerMask blueSquareLayer;

    [Header("视觉反馈 Prefab")]
    public GameObject laserEffectPrefab; // 拖入你的 LaserEffect 预制体

    // 【新增】音效
    [Header("音效")]
    public AudioClip absorbSound; // 拖入吸收音效
    public AudioClip fireSound;   // 拖入发射音效

    // 内部引用
    private PlayerLife playerLife;
    private PlayerMovement playerMovement;
    private AudioSource audioSource; // 【新增】音频播放器

    void Start()
    {
        playerLife = GetComponent<PlayerLife>();
        playerMovement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>(); // 【新增】获取 AudioSource

        // 在游戏开始时就更新一次UI
        UpdateAmmoText();
    }

    void Update()
    {
        if (playerLife.isDead || !hasAbility) return;

        if (Input.GetButtonDown("Fire2")) // 吸收
        {
            Debug.Log("Fire2 pressed!"); // <-- 添加日志1
            TryAbsorb();
        }
        if (Input.GetButtonDown("Fire1")) // 发射
        {
            Debug.Log("Fire1 pressed!"); // <-- 添加日志2
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
        Debug.Log("TryAbsorb: Checking OverlapBox..."); // <-- 添加日志3
        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, laserLayer);

        if (hit != null)
        {
            Debug.Log("TryAbsorb: Hit successful! Target: " + hit.name); // <-- 添加日志4
            LethalLaser laser = hit.GetComponent<LethalLaser>();
            if (laser != null)
            {
                // 【新增】播放吸收音效
                // ...
                if (audioSource != null && absorbSound != null)
                {
                    // 在 PlayOneShot 的第二个参数中传入音量
                    audioSource.PlayOneShot(absorbSound, 2f);
                }
                // ...

                laser.BeAbsorbed();
                laserAmmo++;
                UpdateAmmoText(); // 更新UI
                Debug.Log("吸收成功! 弹药: " + laserAmmo);
                StartCoroutine(AbsorbCooldownRoutine());
                // 【修改】视觉反馈 - 实例化效果
                SpawnLaserEffect(hit.transform.position, true);
            }
            else
            {
                Debug.Log("TryAbsorb: OverlapBox missed!"); // <-- 添加日志5
            }
        }
    }

    private void TryFire()
    {
        Debug.Log("TryFire: Checking OverlapBox..."); // <-- 添加日志6
        if (laserAmmo <= 0) return;

        float facingDir = playerMovement.GetFacingDirection();
        Vector2 boxCenter = (Vector2)firePoint.position + new Vector2(facingDir * (fireCheckDistance / 2), 0);
        Vector2 boxSize = new Vector2(fireCheckDistance, fireCheckHeight);
        Collider2D hit = Physics2D.OverlapBox(boxCenter, boxSize, 0f, blueSquareLayer);

        if (hit != null)
        {
            Debug.Log("TryFire: Hit successful! Target: " + hit.name); // <-- 添加日志7
            BlueSquare square = hit.GetComponent<BlueSquare>();
            if (square != null)
            {
                // 【新增】播放发射音效
                //if (audioSource != null && fireSound != null)
                //{
                //    audioSource.PlayOneShot(fireSound);
                //}
                // ...
                if (audioSource != null && fireSound != null)
                {
                    // 在 PlayOneShot 的第二个参数中传入音量
                    audioSource.PlayOneShot(fireSound, 2f);
                }
                // ...

                square.BeDestroyed();
                laserAmmo--;
                UpdateAmmoText(); // 更新UI
                Debug.Log("发射成功! 击中方块! 剩余弹药: " + laserAmmo);
                // 【修改】视觉反馈 - 实例化效果
                SpawnLaserEffect(hit.transform.position, false);
            }
            else
            {
                Debug.Log("TryFire: OverlapBox missed!"); // <-- 添加日志8
            }
        }
    }

    // (UnlockAbility, HasAbility, AbsorbCooldownRoutine 保持不变)
    public void UnlockAbility()
    {
        hasAbility = true;
        laserAmmo = 0;
        UpdateAmmoText(); // 更新UI
        Debug.Log("【能力已解锁】已获得激光能力！现在可以吸收激光了。");
    }
    public bool HasAbility() { return hasAbility; }
    private IEnumerator AbsorbCooldownRoutine()
    {
        canAbsorb = false;
        yield return new WaitForSeconds(absorbCooldown);
        canAbsorb = true;
    }

    // 专门用于更新弹药UI的方法
    private void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = laserAmmo + " / " + MAX_AMMO;
        }
    }

    // 实例化和初始化效果的方法
    private void SpawnLaserEffect(Vector3 targetPoint, bool isAbsorb)
    {
        if (laserEffectPrefab != null && firePoint != null)
        {
            // 在 FirePoint 的位置实例化
            GameObject effectInstance = Instantiate(laserEffectPrefab, firePoint.position, Quaternion.identity);

            // 获取控制器脚本
            LaserEffectController controller = effectInstance.GetComponent<LaserEffectController>();
            if (controller != null)
            {
                // 初始化（传递起点、终点和效果类型）
                controller.Initialize(firePoint.position, targetPoint, isAbsorb);
            }
            else
            {
                Debug.LogError("LaserEffect Prefab 没有挂载 LaserEffectController 脚本!");
                Destroy(effectInstance); // 清理无效实例
            }
        }
    }

    // (OnDrawGizmosSelected 保持不变，用于调试检测盒)
    private void OnDrawGizmosSelected()
    {
        // ... (保持不变) ...
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // ���� TextMeshPro �����ռ�

// ���޸ġ���� AudioSource
[RequireComponent(typeof(PlayerLife), typeof(PlayerMovement), typeof(AudioSource))]
public class PlayerLaserAbility : MonoBehaviour
{
    [Header("����״̬ (ֻ��)")]
    [SerializeField] private bool hasAbility = false;
    [SerializeField] private int laserAmmo = 0;
    private const int MAX_AMMO = 3;

    [Header("UI (�ڵ���ʾ)")]
    [SerializeField] private TextMeshProUGUI ammoText; // ������� TextMeshPro UI

    [Header("����")]
    public Transform firePoint;
    public float absorbCooldown = 0.5f;
    private bool canAbsorb = true;

    [Header("�������")]
    public float absorbCheckDistance = 2.0f; // 
    public float absorbCheckHeight = 10.0f;
    public LayerMask laserLayer;

    public float fireCheckDistance = 2.0f; // 
    public float fireCheckHeight = 10.0f;
    public LayerMask blueSquareLayer;

    [Header("�Ӿ����� Prefab")]
    public GameObject laserEffectPrefab; // ������� LaserEffect Ԥ����

    // ����������Ч
    [Header("��Ч")]
    public AudioClip absorbSound; // ����������Ч
    public AudioClip fireSound;   // ���뷢����Ч

    // �ڲ�����
    private PlayerLife playerLife;
    private PlayerMovement playerMovement;
    private AudioSource audioSource; // ����������Ƶ������

    void Start()
    {
        playerLife = GetComponent<PlayerLife>();
        playerMovement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>(); // ����������ȡ AudioSource

        // ����Ϸ��ʼʱ�͸���һ��UI
        UpdateAmmoText();
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
                // ������������������Ч
                // ...
                if (audioSource != null && absorbSound != null)
                {
                    // �� PlayOneShot �ĵڶ��������д�������
                    audioSource.PlayOneShot(absorbSound, 2f);
                }
                // ...

                laser.BeAbsorbed();
                laserAmmo++;
                UpdateAmmoText(); // ����UI
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
                // �����������ŷ�����Ч
                //if (audioSource != null && fireSound != null)
                //{
                //    audioSource.PlayOneShot(fireSound);
                //}
                // ...
                if (audioSource != null && fireSound != null)
                {
                    // �� PlayOneShot �ĵڶ��������д�������
                    audioSource.PlayOneShot(fireSound, 2f);
                }
                // ...

                square.BeDestroyed();
                laserAmmo--;
                UpdateAmmoText(); // ����UI
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
        UpdateAmmoText(); // ����UI
        Debug.Log("�������ѽ������ѻ�ü������������ڿ������ռ����ˡ�");
    }
    public bool HasAbility() { return hasAbility; }
    private IEnumerator AbsorbCooldownRoutine()
    {
        canAbsorb = false;
        yield return new WaitForSeconds(absorbCooldown);
        canAbsorb = true;
    }

    // ר�����ڸ��µ�ҩUI�ķ���
    private void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = laserAmmo + " / " + MAX_AMMO;
        }
    }

    // ʵ�����ͳ�ʼ��Ч���ķ���
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

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BrawlerCameraFollow : MonoBehaviour
{
    [Tooltip("�����������������������嶼�ϵ�����")]
    public List<Transform> players;

    [Tooltip("����������")]
    public Camera mainCamera;

    [Tooltip("����Ļ��Ե���������絥λ�ڱ߾�")]
    public float worldPadding = 1.0f;

    private float halfScreenWidth;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        UpdateHalfWidth();
    }

    void LateUpdate()
    {
        players.RemoveAll(item => item == null);
        if (players == null || players.Count == 0 || mainCamera == null)
        {
            return;
        }

        // ʵʱ������Ļ��ȣ��Է����ڴ�С�ı�
        UpdateHalfWidth();

        // 1. ��̬�ҵ���ǰ���������
        float minX = players.Min(player => player.position.x);
        float maxX = players.Max(player => player.position.x);

        // 2. ����������ĵ�ġ����롱λ�ã���ҵ����ģ�
        float centerPointX = (minX + maxX) / 2.0f;

        // 3. ������� *����* ���صı߽�

        // �������ص���СXֵ (Ϊ������ǰ������ 'maxX' ��������Ļ��)
        // cameraX + halfWidth - padding >= maxX  =>  cameraX >= maxX - halfWidth + padding
        float minCameraX = maxX - halfScreenWidth + worldPadding;

        // �������ص����Xֵ (Ϊ������������� 'minX' ��������Ļ��)
        // cameraX - halfWidth + padding <= minX  =>  cameraX <= minX + halfWidth - padding
        float maxCameraX = minX + halfScreenWidth - worldPadding;

        // 4. ǯ�����λ��
        // ���ǰѡ��������ĵ㡱���ڡ�����ı߽硱֮��
        // ע�⣺���� PlayerMovement �ű��Ĵ��ڣ�minCameraX ��Զ������� maxCameraX
        float targetX = Mathf.Clamp(centerPointX, minCameraX, maxCameraX);

        // 5. ����������������λ�� (ֻ����X)
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    private void UpdateHalfWidth()
    {
        if (mainCamera.orthographic)
        {
            halfScreenWidth = mainCamera.orthographicSize * mainCamera.aspect;
        }
    }
}
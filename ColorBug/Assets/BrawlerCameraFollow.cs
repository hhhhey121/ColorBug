using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BrawlerCameraFollow : MonoBehaviour
{
    [Tooltip("把你的两个（或多个）玩家物体都拖到这里")]
    public List<Transform> players;

    [Tooltip("你的主摄像机")]
    public Camera mainCamera;

    [Tooltip("在屏幕边缘留出的世界单位内边距")]
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

        // 实时更新屏幕宽度，以防窗口大小改变
        UpdateHalfWidth();

        // 1. 动态找到最前和最后的玩家
        float minX = players.Min(player => player.position.x);
        float maxX = players.Max(player => player.position.x);

        // 2. 计算相机中心点的“理想”位置（玩家的中心）
        float centerPointX = (minX + maxX) / 2.0f;

        // 3. 计算相机 *必须* 遵守的边界

        // 必须遵守的最小X值 (为了让最前面的玩家 'maxX' 保持在屏幕内)
        // cameraX + halfWidth - padding >= maxX  =>  cameraX >= maxX - halfWidth + padding
        float minCameraX = maxX - halfScreenWidth + worldPadding;

        // 必须遵守的最大X值 (为了让最后面的玩家 'minX' 保持在屏幕内)
        // cameraX - halfWidth + padding <= minX  =>  cameraX <= minX + halfWidth - padding
        float maxCameraX = minX + halfScreenWidth - worldPadding;

        // 4. 钳制相机位置
        // 我们把“理想中心点”夹在“必须的边界”之间
        // 注意：由于 PlayerMovement 脚本的存在，minCameraX 永远不会大于 maxCameraX
        float targetX = Mathf.Clamp(centerPointX, minCameraX, maxCameraX);

        // 5. 更新这个焦点物体的位置 (只更新X)
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
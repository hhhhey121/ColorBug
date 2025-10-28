using UnityEngine;

// 这是一个标记脚本，里面不需要任何代码。
// 它的唯一作用就是让 PlayerLife 脚本知道“我撞到的是一个特殊的地刺”。
public class FinalWinSpike : MonoBehaviour
{
    // (确保这个物体也拥有 "Trap" 标签，
    // 这样 PlayerLife 的 OnCollisionEnter2D 才能首先被触发)
}
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public enum CharacterState { Combined, Split }
    public CharacterState currentState = CharacterState.Combined;//初始状态静止

    public GameObject partA; // 部分A（大）player2
    public GameObject partB; // 部分B（小）player
    public KeyCode splitCombineKey = KeyCode.Space;

    public float combineDistance = 1.2f; // 自动组合的触发距离
    public float combineCooldown = 0.5f;
    private float lastCombineTime = 0f;

    public float partAMass = 2f; // 部分A的质量
    public float partBMass = 1f; // 部分B的质量
    public float partASpeed = 4f; // 部分A的移动速度
    public float partBSpeed = 4f; // 部分B的移动速度
    public float partAJumpSpeed = 10f; // 部分A的跳跃速度
    public float partBJumpSpeed = 5f; // 部分B的跳跃速度

    //组合时的偏移位置
    public Vector3 combinedOffset = new Vector3(0, 1, 0);

    // 组件引用
    private Rigidbody2D rbA;
    private Rigidbody2D rbB;
    private PlayerMovement playerMovementA;
    private PlayerMovement playerMovementB;

    


    void Start()
    {
        // 获取两个部分所需的组件
        if (partA != null)
        {
            rbA = partA.GetComponent<Rigidbody2D>();
            playerMovementA = partA.GetComponent<PlayerMovement>();
            // 初始化时设置部分A的物理属性
            if (rbA != null) rbA.mass = partAMass;
            if (playerMovementA != null)
            {
                playerMovementA.playerSpeed = partASpeed;
                playerMovementA.jumpSpeed = partAJumpSpeed;
                playerMovementA.enabled = (currentState == CharacterState.Combined); // 初始组合时只启用A的控制
            }
        }

        if (partB != null)
        {
            rbB = partB.GetComponent<Rigidbody2D>();
            playerMovementB = partB.GetComponent<PlayerMovement>();
            // 初始化时设置部分B的物理属性
            if (rbB != null) rbB.mass = partBMass;
            if (playerMovementB != null)
            {
                playerMovementB.playerSpeed = partBSpeed;
                playerMovementB.jumpSpeed = partBJumpSpeed;
                playerMovementB.enabled = false; // 初始组合时禁用B的控制
            }
        }

        // 初始为组合状态，将部分B的位置设置到部分A
        if (currentState == CharacterState.Combined && partA != null && partB != null)
        {
            //添加父子关系绑定
            partB.transform.SetParent(partA.transform);
            partB.transform.localPosition = combinedOffset;
            partB.transform.localRotation = Quaternion.identity;
            if (rbB != null) rbB.isKinematic = true; // 组合时部分B不受物理影响
        }
    }

    void Update()
    {
        // 检测分离/组合按键输入
        if (Input.GetKeyDown(splitCombineKey) && Time.time > lastCombineTime + combineCooldown)
        {
            ToggleSplitCombine();
        }





        // 如果在分离状态，检查是否满足自动组合条件
        if (currentState == CharacterState.Split)
        {
            CheckCombineCondition();
        }
    }



    

  




    /// <summary>
    /// 切换分离/组合状态
    /// </summary>
    void ToggleSplitCombine()
    {
        if (currentState == CharacterState.Combined)
        {
            SplitCharacter();
        }
        else
        {
            TryCombine();
        }
    }

    /// <summary>
    /// 分离角色
    /// </summary>
    void SplitCharacter()
    {
        if (partA == null || partB == null) return;

        currentState = CharacterState.Split;
        lastCombineTime = Time.time;

        partB.transform.SetParent(null);

        // 启用两个部分的独立控制
        if (playerMovementA != null) playerMovementA.enabled = true;
        if (playerMovementB != null)
        {
            playerMovementB.enabled = true;
            // 确保部分B恢复物理模拟
            if (rbB != null) rbB.isKinematic = false;




        }


        // 可以给两个部分一个轻微的反向力，使其自然分开
        if (rbA != null) rbA.AddForce(new Vector2(-1, 0.5f) * 2f, ForceMode2D.Impulse);
        if (rbB != null) rbB.AddForce(new Vector2(1, 0.5f) * 2f, ForceMode2D.Impulse);

        Debug.Log("角色已分离！");
    }




    



    /// <summary>
    /// 尝试组合角色
    /// </summary>
    void TryCombine()
    {
        if (partA == null || partB == null) return;

        // 简单的距离检测，可以在这里添加更多组合条件（如相对速度、特定区域等）
        float distance = Vector2.Distance(partA.transform.position, partB.transform.position);
        // 添加调试语句，输出combineDistance的值
        Debug.Log("当前组合距离设定: " + combineDistance + ", 实际距离: " + distance);

        if (distance <= combineDistance)
        {
            CombineCharacter();
        }
        else
        {
            Debug.Log("两部分距离过远，无法组合。当前距离: " + distance.ToString("F2"));
        }
        
    }

    /// <summary>
    /// 组合角色
    /// </summary>
    void CombineCharacter()
    {
        currentState = CharacterState.Combined;
        lastCombineTime = Time.time;


       


        //// 禁用部分B的独立控制，组合后以部分A为主体
        if (playerMovementB != null) playerMovementB.enabled = false;
        // 组合时部分B不受物理影响，跟随部分A
        if (rbB != null) rbB.isKinematic = true;

        // 将部分B的位置设置到部分A头部
        if (partA != null && partB != null)
        {
            partB.transform.SetParent(partA.transform);
            partB.transform.localPosition = new Vector3(0, 1, 0);// 部分A的头顶在y轴正方向1个单位
            partB.transform.localRotation = Quaternion.identity;
        }




        Debug.Log("角色已组合！");
    }










    /// <summary>
    /// 检查自动组合条件
    /// </summary>
    void CheckCombineCondition()
    {
        if (partA == null || partB == null) return;

        float distance = Vector2.Distance(partA.transform.position, partB.transform.position);
        if (distance <= combineDistance && Time.time > lastCombineTime + combineCooldown)
        {
            // 自动组合提示
            Debug.Log("满足自动组合条件，按空格键组合");
        }
    }

    // 可选的：在Scene视图中绘制Gizmos以便于调试
    void OnDrawGizmosSelected()
    {
        if (partA != null && partB != null)
        {
            // 绘制组合距离范围
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(partA.transform.position, combineDistance);
            // 绘制两部分之间的连线
            Gizmos.color = currentState == CharacterState.Combined ? Color.green : Color.white;
            Gizmos.DrawLine(partA.transform.position, partB.transform.position);
        }
    }
}


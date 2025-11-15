using UnityEngine;
using UnityEngine.InputSystem;

namespace DebugPlayer
{
    public class Runner : MonoBehaviour, DebugPlayerControl.IPlayerActions
    {
        [SerializeField] private float moveSpeed = 5f; // 移動速度
        [SerializeField] private float jumpForce = 7f; // ジャンプ力
        [SerializeField] private Transform groundCheck; // 地面チェック用のトランスフォーム
        [SerializeField] private LayerMask groundLayer; // 地面レイヤー
        [SerializeField] private float groundCheckRadius = 0.2f; // 地面チェックの半径

        private Rigidbody rb; // リジッドボディコンポーネント
        private bool isGrounded; // 地面に接地しているかどうか
        private DebugPlayerControl controls; // プレイヤーコントロール
        private float moveInput; // 移動入力値

        /// <summary>
        /// 初期化処理
        /// </summary>
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            controls = new DebugPlayerControl();
            controls.Player.SetCallbacks(this);
        }

        /// <summary>
        /// 毎フレームの更新処理
        /// </summary>
        private void Update()
        {
            CheckGroundStatus();
            HandleMovement();
        }

        /// <summary>
        /// 有効化されたときの処理
        /// </summary>
        private void OnEnable()
        {
            controls.Player.Enable();
        }

        /// <summary>
        /// 無効化されたときの処理
        /// </summary>
        private void OnDisable()
        {
            controls.Player.Disable();
        }

        /// <summary>
        /// 地面接地状態をチェックする
        /// </summary>
        private void CheckGroundStatus()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        }

        /// <summary>
        /// 移動処理
        /// </summary>
        private void HandleMovement()
        {   
            rb.linearVelocity = new Vector3(moveInput * moveSpeed, rb.linearVelocity.y, rb.linearVelocity.z);
        }


        
        #region InputActionsの入力処理

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput=context.ReadValue<float>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(isGrounded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            }
        }

        #endregion
    }
}

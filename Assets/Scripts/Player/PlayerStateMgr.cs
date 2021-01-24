using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerStateMgr : MonoBehaviour
    {
        public static PlayerStateMgr Instance;
        
        public FaceDir FaceToDir;
        public FaceDir BackToDir;
        public Vector2 FollowPoint;
        private FaceDir _lastDir;
        
        private BoxCollider2D _box;
        private Rigidbody2D _rb;
        
        private Vector2 _faceDir;
        private Vector2 _moveDir;

        
        public float walkSpeed = 10f;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
        
        
        void Start()
        {
            FaceToDir = FaceDir.Down;
            _lastDir = FaceDir.Up;
            _box = transform.GetComponent<BoxCollider2D>();
            _rb = transform.GetComponent<Rigidbody2D>();

        }


        public void OnMove(InputAction.CallbackContext context)
        {
            _moveDir = context.ReadValue<Vector2>();
            
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    _faceDir = _moveDir;
                    PlayerAnimStateMgr.SetFaceDir(_faceDir.y);
                    SetPlayerFaceTo(_faceDir);
                    if (_faceDir.x > 0)
                    {
                        transform.localScale = Vector3.one;
                    }
                    else if (_faceDir.x < 0) //==0的时候保持上一次的方向就好
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    
                    break;
                case InputActionPhase.Started:
//                    if (context.interaction is SlowTapInteraction)
                    break;
                case InputActionPhase.Canceled:
                    break;
            }
            
            if(_moveDir != Vector2.zero)
                PlayerAnimStateMgr.TryChangeState(PlayerState.Walk);
            else
                PlayerAnimStateMgr.TryChangeState(PlayerState.Idle);
            
        }

        private void Update()
        {
            _rb.velocity = _moveDir * walkSpeed;
        }

        public static PlayerStateMgr GetInstance()
        {
            return Instance;
        }
        
        public void SetPlayerFaceTo(Vector2 faceDir)
        {
            if(faceDir== Vector2.zero)
                return;
            _lastDir = FaceToDir;
            if (faceDir.y > 0)
            {
                FaceToDir = FaceDir.Up;
                BackToDir = FaceDir.Down;
                FollowPoint = new Vector2(0,0.8f) + new Vector2(Random.Range(-0.5f,0.5f),Random.Range(0.5f,-0.2f));
            }
            else if (faceDir.y < 0)
            {
                FaceToDir = FaceDir.Down;
                BackToDir = FaceDir.Up;
                FollowPoint = new Vector2(0,-0.8f)+ new Vector2(Random.Range(-0.5f,0.5f),Random.Range(-0.5f,0.2f));
            }
            else if (faceDir.x > 0)
            {
                FaceToDir = FaceDir.Right;
                BackToDir = FaceDir.Left;
                FollowPoint = new Vector2(0.8f,0)+ new Vector2(Random.Range(0.5f,-0.2f),Random.Range(-0.5f,0.5f));
            }
            else if (faceDir.x < 0)
            {
                FaceToDir = FaceDir.Left;
                BackToDir = FaceDir.Right;
                FollowPoint = new Vector2(-0.8f,0) + new Vector2(Random.Range(-0.5f,0.2f),Random.Range(-0.5f,0.5f));
            }

            FollowPoint += (Vector2)transform.position;

        }

    }
    

}

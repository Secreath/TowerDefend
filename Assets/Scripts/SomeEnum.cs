
namespace Player
{
    public enum PlayerState
        {
            Idle,
            Walk
        }
    
        public enum FaceDir
        {
            Up,
            Down,
            Right,
            Left
        }

}

namespace Enemy
{
    public enum EnemyState
    {
        Idle,
        Attack,
        Walk,
        Dead
    }

}

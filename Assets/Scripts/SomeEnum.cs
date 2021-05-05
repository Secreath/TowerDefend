
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
        WalkToEnemy,
        Walk,
        Dead
    }
}

namespace Follower
{
    public enum FollowState
    {
        InRange,
        OutRange,
        CanAttack,
        FollowEnemy
    }
}

namespace ui
{
    public enum UiType
    {
        ChooseBtn,
        UpGradeBtn,
        End
    }
    
    public enum UiState
    {
        Close,
        OpenChoosePanel,
        OpenUpGradePanel,
        OpenSettingPanel
    }
}

namespace tower
{
     public enum TowerType
        {
            Fire = 0,
            Shoot = 1,
            Spawn = 2,
            End
        }
    
    public enum UpgradeType
    {
        Type1 = 0,
        Type2 = 1,
        Type3 = 2,
        End
    }
}
public enum GameState
{
    PlayGame,
    OpenUi,
    PickTower,
    StopGame
}
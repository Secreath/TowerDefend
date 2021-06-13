
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

    public enum EnemyType
    {
        Type1,
        Type2,
        Type3
    }
}

namespace Follower
{
    public enum MoveState
    {
        InRange,
        OutRange,
        CanAttack,
        FollowEnemy,
        FollowPlayer,
        FarEnemy,
        FarPlayer,
        FarOther,
        Stop
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
        Soldier = 2,
        UpGrade = 3,
        End
    }
    
    public enum UpgradeType
    {
        Type1 = 0,
        Type2 = 1,
        Type3 = 2,
        End
    }
    
    public enum TowerState
    {
        Attack = 0,
        Upgrading = 1,
        Idle = 2,
        UpgradComplete = 3,
        End
    }
}

namespace SaveRes
{
    public enum FileExsitStatus
    {
        NoPath=0,
        FileExsit=1,
        NoFile=2
    }
}
public enum GameState
{
    PlayGame,
    OpenUi,
    PickTower,
    StopGame
}

public enum JState
{
    PickUp,
    PutDown,
    UpGrade,
    ControllUi
}


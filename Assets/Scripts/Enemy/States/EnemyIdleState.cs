namespace AnimalsEscape.States
{
    public class EnemyIdleState : IState
    {
        EnemyAnimations _enemyAnimations;
        public EnemyIdleState(EnemyAnimations enemyAnimations)
        {
            _enemyAnimations = enemyAnimations;
        }
        public void Enter()
        {
            _enemyAnimations.PlayIdle();
        }

        public void Exit()
        {
        
        }

        public void Run()
        {
        
        }
    }
}

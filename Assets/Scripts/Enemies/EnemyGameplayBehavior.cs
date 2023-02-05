using System;
using System.Collections;
using Pathfinding;
using UnityEngine;

public class EnemyGameplayBehavior : MonoBehaviour
{
    public float delayStartMovingOnSpawn = 1.0f;
    public int damagePerHit = 1;
    public int waterStolenByHit = 10;
    public float delayBetweenAnimAndHit = 0.5f;

    public class EnemyStateMachine : StateMachine
    {
        protected EnemyGameplayBehavior behavior;
        public EnemyGameplayBehavior Behavior => behavior;

        public EnemyStateMachine(EnemyGameplayBehavior behavior, params State[] states) : base(states)
        {
            this.behavior = behavior;
        }
    }

    [SerializeField] private float AttackInterval;
    [SerializeField] private States currentDebugState;
    
    private AIDestinationSetter destinationSetter;
    private AIPath aiPath;
    private Seeker seeker;
    private Health health;
    private Animator animator;

    private EnemyStateMachine stateMachine;
    IEnumerator Start()
    {
        destinationSetter = GetComponentInChildren<AIDestinationSetter>();
        aiPath = GetComponentInChildren<AIPath>();
        seeker = GetComponentInChildren<Seeker>();
        health = GetComponentInChildren<Health>();
        animator = GetComponentInChildren<Animator>();

        stateMachine = new EnemyStateMachine(this, new IdleState(), new MovingState(), new AttackingState(),
            new DeadState());
        stateMachine.Initialize();
        currentDebugState = stateMachine.CurrentState.GetStateType();
        stateMachine.OnStateChanged -= OnStateChanged;
        stateMachine.OnStateChanged += OnStateChanged;
        // Debug.Log(destinationSetter);
        // Debug.Log(ReferencesSingleton.Instance);

        yield return new WaitForSeconds(delayStartMovingOnSpawn);

        if (ReferencesSingleton.Instance != null)
            destinationSetter.target = ReferencesSingleton.Instance.treeRef.transform;
    }

    private void Update()
    {
        stateMachine.Update(Time.deltaTime);
    }

    public void InvokeDealDamage()
    {
        Invoke("DealDamage", delayBetweenAnimAndHit);
    }

    void DealDamage()
    {
        Transform target = destinationSetter.target;
        if (target != null)
        {
            if (target.GetComponentInChildren<Gem>())
                target.GetComponentInChildren<Health>().TakeDamage(damagePerHit);
            else if (target.CompareTag("Player"))
            {
                ReferencesSingleton.Instance.player.WaterReservoir.UseWater(waterStolenByHit);
            }
        }
    }

    private void OnStateChanged(States previousstate, States nextstate)
    {
        currentDebugState = nextstate;
    }
    
    public abstract class EnemyState : State
    {
        protected EnemyStateMachine StateMachine => (EnemyStateMachine)stateMachine;
        protected EnemyGameplayBehavior Behavior => StateMachine.Behavior;
    }
    
    public class IdleState : EnemyState
    {
        public override States GetStateType()
        {
            return States.Idle;
        }

        public override void OnEnterState(States previousState)
        {
            Behavior.animator.SetTrigger("Idle");
        }

        public override void OnStateUpdate(float deltaTime)
        {
            if (StateMachine.Behavior.health.IsDead())
            {
                stateMachine.GoToState(States.Dead);
                return;
            }
            
            if (StateMachine.Behavior.aiPath.hasPath)
            {
                stateMachine.GoToState(States.Moving);
            }
        }

        public override void OnExitState(States nextState)
        {
            
        }
    }

    public class MovingState : EnemyState
    {
        public override States GetStateType()
        {
            return States.Moving;
        }

        public override void OnEnterState(States previousState)
        {
            
        }

        public override void OnStateUpdate(float deltaTime)
        {
            if (StateMachine.Behavior.health.IsDead())
            {
                stateMachine.GoToState(States.Dead);
                return;
            }
            
            Behavior.animator.SetFloat("MoveSpeed", Behavior.aiPath.velocity.magnitude);
            
            if (StateMachine.Behavior.aiPath.reachedDestination)
            {
                stateMachine.GoToState(States.Attacking);
            }
        }

        public override void OnExitState(States nextState)
        {
            Behavior.animator.SetFloat("MoveSpeed", 0);
        }
    }
    
    public class AttackingState : EnemyState
    {
        public override States GetStateType()
        {
            return States.Attacking;
        }

        private float attackTimer = 0f;
        public override void OnEnterState(States previousState)
        {
            attackTimer = Behavior.AttackInterval;
        }

        public override void OnStateUpdate(float deltaTime)
        {
            if (StateMachine.Behavior.health.IsDead())
            {
                stateMachine.GoToState(States.Dead);
            }

            attackTimer += deltaTime;
            while (attackTimer > Behavior.AttackInterval)
            {
                attackTimer -= Behavior.AttackInterval;
                Attack();
            }

        }

        // private Coroutine attackRoutine;

        void Attack()
        {
            Behavior.animator.SetTrigger("Attack");
            StateMachine.Behavior.InvokeDealDamage();
            // attackRoutine = Behavior.StartCoroutine(AttackCoroutine);
        }

        // System.Collections.IEnumerator AttackCoroutine()
        // {
        //     
        //     yield break;
        // }

        public override void OnExitState(States nextState)
        {
            // if (attackRoutine != null)
            // {
            //     Behavior.StopCoroutine(attackRoutine);
            //     attackRoutine = null;
            // }
        }
    }
    
    public class DeadState : EnemyState
    {
        public override States GetStateType()
        {
            return States.Dead;
        }

        public override void OnEnterState(States previousState)
        {
            
        }

        public override void OnStateUpdate(float deltaTime)
        {
            if (StateMachine.Behavior.health.IsDead())
            {
                stateMachine.GoToState(States.Dead);
            }
        }

        public override void OnExitState(States nextState)
        {
            
        }
    }
}

public enum States
{
    Idle,
    Moving,
    Attacking,
    Dead
}

public class StateMachine
{
    protected State[] states;
    protected State currentState;

    public delegate void StateChangedEvent(States previousState, States nextState);

    public event StateChangedEvent OnStateChanged;
    public State CurrentState => currentState;
    public StateMachine(params State[] states)
    {
        this.states = states;
        currentState = states[0];
    }

    public virtual void Initialize()
    {
        for (var index = 0; index < states.Length; index++)
        {
            states[index].Initialize(this);
        }
        currentState.OnEnterState(default);
    }

    public void GoToState(States nextState)
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].GetStateType() == nextState)
            {
                State previousState = currentState;
                currentState.OnExitState(nextState);
                currentState = states[i];
                currentState.OnEnterState(previousState.GetStateType());
                OnStateChanged?.Invoke(previousState.GetStateType(), currentState.GetStateType());
                break;
            }
        }
    }

    public void Update(float deltaTime)
    {
        currentState.OnStateUpdate(deltaTime);
    }
    
}

public abstract class State
{
    protected StateMachine stateMachine;

    public void Initialize(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    public abstract States GetStateType();
    public abstract void OnEnterState(States previousState);

    public abstract void OnStateUpdate(float deltaTime);
    public abstract void OnExitState(States nextState);
}

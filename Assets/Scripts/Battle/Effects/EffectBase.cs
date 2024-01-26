using FluffyDuck.Util;
using System.Collections.Generic;
using UnityEngine;

public class EffectBase : MonoBehaviour, IPoolableComponent
{
    [SerializeField, Tooltip("Particle Main")]
    protected ParticleSystem Particle_Effect;

    protected EffectFactory Factory;

    protected Transform Target_Transform;

    protected Vector3 Target_Position;

    protected Vector3 Direction;

    protected List<ParticleSystem> All_Particle_List = new List<ParticleSystem>();

    public bool Is_Action { get; protected set; } = false;

    public bool Is_Loop { get; protected set; } = false;

    protected System.Action<EffectBase> Finish_Callback;

    protected float Delta;
    protected float Duration;
    /// <summary>
    /// 전투 배속에 따른 속도 배율
    /// </summary>
    protected float Effect_Speed_Multiple = 1f;

    public virtual void MoveTarget(Transform target, float duration)
    {
        Target_Transform = target;
        this.Target_Position = Target_Transform.position;
        this.Duration = duration;
        this.Direction = (this.transform.position - this.Target_Position).normalized;
        this.Delta = 0f;
        Is_Action = true;
        if (Particle_Effect != null)
        {
            SetParticleAllSpeedMultiple(Particle_Effect, Effect_Speed_Multiple);
            Particle_Effect.Play();
        }
    }
    public virtual void MoveTarget(Vector3 target, float duration)
    {
        this.Target_Position = target;
        this.Duration = duration;
        this.Direction = (this.transform.position - target).normalized;
        this.Delta = 0f;
        Is_Action = true;
        if (Particle_Effect != null)
        {
            SetParticleAllSpeedMultiple(Particle_Effect, Effect_Speed_Multiple);
            Particle_Effect.Play();
        }
    }

    public virtual void MoveTarget(List<Vector3> path, float duration)
    {
        this.Duration = duration;
        this.Delta = 0f;
        Is_Action = true;
        if (Particle_Effect != null)
        {
            SetParticleAllSpeedMultiple(Particle_Effect, Effect_Speed_Multiple);
            Particle_Effect.Play();
        }
    }

    public virtual void StartParticle(float duration, bool loop = false)
    {
        this.Duration = duration / Effect_Speed_Multiple;
        this.Delta = 0f;
        Is_Action = true;
        Is_Loop = loop;
        if (Particle_Effect != null)
        {
            SetParticleAllSpeedMultiple(Particle_Effect, Effect_Speed_Multiple);
            Particle_Effect.Play();
        }
        
    }

    public virtual void StartParticle(Transform target, float duration, bool loop = false)
    {
        this.Target_Transform = target;
        this.Duration = duration / Effect_Speed_Multiple;
        this.Delta = 0f;
        Is_Action = true;
        Is_Loop = loop;
        if (Particle_Effect != null)
        {
            SetParticleAllSpeedMultiple(Particle_Effect, Effect_Speed_Multiple);
            Particle_Effect.Play();
        }
    }
    /// <summary>
    /// 해당 오브젝트 내의 모든 파티클을 찾아서 배속을 조절한다.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="speed_multiple"></param>
    protected void SetParticleAllSpeedMultiple(ParticleSystem obj, float speed_multiple)
    {
        if (obj != null)
        {
            //var children = obj.GetComponentsInChildren<ParticleSystem>();
            //if (children != null && children.Length > 0)
            //{
            //    int cnt = children.Length;
            //    for (int i = 0; i < cnt; i++)
            //    {
            //        ParticleSystem child = children[i];
            //        var main = child.main;
            //        main.simulationSpeed = speed_multiple;
            //    }
            //}
            int cnt = All_Particle_List.Count;
            for (int i = 0; i < cnt; i++)
            {
                var child = All_Particle_List[i];
                var main = child.main;
                main.simulationSpeed = speed_multiple;
            }
            //  경우에 따라 애니메이터가 있을 수 있음. 
            var animator = obj.GetComponent<Animator>();
            if (animator != null)
            {
                animator.speed = speed_multiple;
            }
            
        }
    }

    public virtual void SetEffectSpeedMultiple(float multiple)
    {
        Effect_Speed_Multiple = multiple;
        if (Is_Action)
        {
            SetParticleAllSpeedMultiple(Particle_Effect, Effect_Speed_Multiple);
        }
    }
    /// <summary>
    /// 모든 파티클 재생이 완료되었는지 체크
    /// </summary>
    /// <returns></returns>
    protected bool CheckParticleComplete()
    {
        if (All_Particle_List.Count == 0)
        {
            return false;
        }
        return !All_Particle_List.Exists(x => x.isPlaying && x.IsAlive());
    }

    public void SetFinishCallback(System.Action<EffectBase> cb)
    {
        Finish_Callback = cb;
    }

    public void SetEffectFactory(EffectFactory fac)
    {
        this.Factory = fac;
    }

    public void SetLoop(bool loop) { this.Is_Loop = loop; }

    public virtual void UnusedEffect()
    {
        //  pool effect 
        Factory.UnusedEffectBase(this);
        Is_Action = false;
        Is_Loop = false;
    }

    public virtual void SetData(params object[] data) { }


    public virtual void OnPuase()
    {
        Is_Action = false;
        if (Particle_Effect != null)
        {
            Particle_Effect.Pause(true);
        }
    }

    public virtual void OnResume()
    {
        Is_Action = true;
        if (Particle_Effect != null)
        {
            Particle_Effect.Play(true);
        }
        
    }

    public virtual void Show(bool show) { }

    

    public virtual void Spawned()
    {
        Is_Action = false;
        Is_Loop = false;
        //  모든 파티클 컴포넌트를 리스트에 담아둔다
        if (Particle_Effect != null)
        {
            var list = Particle_Effect.GetComponentsInChildren<ParticleSystem>();
            if (list != null)
            {
                All_Particle_List.Clear();
                All_Particle_List.AddRange(list);
            }
        }
    }
    public virtual void Despawned()
    {
        if (Particle_Effect != null)
        {
            Particle_Effect.Stop();
        }
    }


}

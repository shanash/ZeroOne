using FluffyDuck.Util;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

public class EffectBase : MonoBehaviour, IPoolableComponent
{
    [SerializeField, Tooltip("Particle Effect")]
    protected ParticleSystem Particle_Effect;

    [SerializeField, Tooltip("Spine Effect")]
    protected SkeletonAnimation Skel_Effect;

    protected EffectFactory Factory;

    protected Transform Target_Transform;

    protected Vector3 Target_Position;

    protected Vector3 Direction;

    public bool Is_Action { get; protected set; } = false;

    public bool Is_Loop { get; protected set; } = false;

    protected System.Action<EffectBase> Finish_Callback;

    protected float Delta;
    protected float Duration;
    
    public virtual void MoveTarget(Transform target, float duration)
    {
        Target_Transform = target;
        this.Target_Position = Target_Transform.position;
        this.Duration = duration;
        this.Direction = (this.transform.position - this.Target_Position).normalized;
        this.Delta = 0f;
        Is_Action = true;
    }
    public virtual void MoveTarget(Vector3 target, float duration)
    {
        this.Target_Position = target;
        this.Duration = duration;
        this.Direction = (this.transform.position - target).normalized;
        this.Delta = 0f;
        Is_Action = true;
    }

    public virtual void MoveTarget(List<Vector3> path, float duration)
    {
        this.Duration = duration;
        this.Delta = 0f;
        Is_Action = true;
    }

    public virtual void StartParticle(float duration, bool loop = false)
    {
        this.Duration = duration;
        this.Delta = 0f;
        Is_Action = true;
        Is_Loop = loop;
    }

    public virtual void StartParticle(Transform target, float duration, bool loop = false)
    {
        this.Target_Transform = target;
        this.Duration = duration;
        this.Delta = 0f;
        Is_Action = true;
        Is_Loop = loop;
    }

    

    public void SetFinishCallback(System.Action<EffectBase> cb)
    {
        Finish_Callback = cb;
    }

    public void SetEffectFactory(EffectFactory fac)
    {
        this.Factory = fac;
    }

    public void SetLoop(bool loop) {  this.Is_Loop = loop; }

    public virtual void UnusedEffect()
    {
        //  pool effect 
        Factory.UnusedEffectBase(this);
        Is_Action = false;
        Is_Loop = false;
    }

    public virtual void SetData(params object[] data) { }

    /// <summary>
    /// 스파인 이펙트의 애니메이션 진행중인 모든 트랙 반환
    /// </summary>
    /// <returns></returns>
    protected TrackEntry[] FindAllTracks()
    {
        if (Skel_Effect != null)
        {
            return Skel_Effect.AnimationState.Tracks.Items;
        }
        return null;
    }

    public virtual void OnPuase() 
    {
        Is_Action = false;
        if (Particle_Effect != null)
        {
            Particle_Effect.Pause(true);
        }
        var tracks = FindAllTracks();
        if (tracks != null)
        {
            int len = tracks.Length;
            for (int i = 0; i < len; i++)
            {
                tracks[i].TimeScale = 0f;
            }
        }
    }

    public virtual void OnResume() 
    {
        Is_Action = true;
        if (Particle_Effect != null)
        {
            Particle_Effect.Play(true);
        }
        var tracks = FindAllTracks();
        if (tracks != null)
        {
            int len = tracks.Length;
            for (int i = 0; i < len; i++)
            {
                tracks[i].TimeScale = 1f;
            }
        }
    }
    
    public virtual void Spawned()
    {
        
        Is_Action = false;
        Is_Loop = false;
    }
    public virtual void Despawned()
    {
        
    }

    
}

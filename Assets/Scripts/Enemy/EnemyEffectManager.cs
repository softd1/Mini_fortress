using UnityEngine;
using System.Collections.Generic;

public abstract class StatusEffect
{
    public float duration { get; set; }
    public float interval;
    public float timer = 0f;

    public int stack { get; protected set; }
    public bool isFinished => duration <= 0f;
    public int maxStack = 1;

    public virtual float speedMultiplier => 1f;
    public virtual float damageTakenMultiplier => 1f;
    public virtual bool equalType(StatusEffect other) => GetType() == other.GetType();

    public abstract void apply();
    public virtual void addStack() { stack++; }
    public abstract void update(float deltaTime, EnemyEffectManager manager);
    public abstract void end();
    public abstract StatusEffect Clone();
}

public class FrozenEffect : StatusEffect
{
    private readonly float slowAmount;

    public FrozenEffect(float duration, float slowAmount, int stack = 1)
    {
        this.duration = duration;
        this.slowAmount = slowAmount;
        this.stack = stack;
        this.maxStack = 2;
    }

    public override void apply() { /* 이펙트 시작 시 필요한 로직 */ }

    public override void update(float deltaTime, EnemyEffectManager manager)
    {
        duration -= deltaTime;
    }

    public override void end() { /* 이펙트 종료 시 필요한 로직 */ }

    public override StatusEffect Clone()
        => new FrozenEffect(duration, slowAmount, stack);

    public override float speedMultiplier
        => 1f - slowAmount * stack;
}

public class FireEffect : StatusEffect
{
    private readonly int damagePerTick;

    public FireEffect(float duration, int damagePerSecond, int stack = 1)
    {
        this.duration = duration;
        this.damagePerTick = damagePerSecond;
        this.stack = stack;
        this.maxStack = 4;
        this.interval = 1f;  // 1초마다 데미지
    }

    public override void apply() { /* 이펙트 시작 시 필요한 로직 */ }

    public override void update(float deltaTime, EnemyEffectManager manager)
    {
        duration -= deltaTime;
        timer += deltaTime;
        if (timer >= interval)
        {
            timer -= interval;
            manager.damageEffect(damagePerTick);
        }
    }

    public override void end() { /* 이펙트 종료 시 필요한 로직 */ }

    public override StatusEffect Clone()
        => new FireEffect(duration, damagePerTick, stack);
}

public class ShockEffect : StatusEffect
{
    public ShockEffect(float duration, int stack = 1)
    {
        this.duration = duration;
        this.stack = stack;
        this.maxStack = 2;
    }

    public override void apply() { }

    public override void update(float deltaTime, EnemyEffectManager manager)
    {
        duration -= deltaTime;
    }

    public override void end() { }

    public override StatusEffect Clone()
        => new ShockEffect(duration, stack);

    public override float damageTakenMultiplier
        => 1f + 0.5f * stack;
}

public class EnemyEffectManager : MonoBehaviour
{
    private List<StatusEffect> effects = new List<StatusEffect>();
    public float speedMultiplier { get; private set; } = 1f;
    public float damageMultiplier { get; private set; } = 1f;

    public void applyEffect(StatusEffect effect)
    {
        foreach (var curr in effects)
        {
            if (curr.equalType(effect))
            {
                if (curr.stack < effect.maxStack)
                    curr.addStack();
                curr.duration = Mathf.Max(curr.duration, effect.duration);
                return;
            }
        }
        var clone = effect.Clone();
        clone.apply();
        effects.Add(clone);
    }

    public void damageEffect(int damage)
    {
        var enemy = GetComponent<Enemy>();
        if (enemy != null)
            enemy.takeDamage(damage, fixedDamage: true);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            var e = effects[i];
            e.update(dt, this);
            if (e.isFinished)
            {
                e.end();
                effects.RemoveAt(i);
            }
        }

        float spdMul = 1f;
        float dmgMul = 1f;
        foreach (var e in effects)
        {
            spdMul = Mathf.Min(spdMul, e.speedMultiplier);
            dmgMul *= e.damageTakenMultiplier;
        }
        speedMultiplier = spdMul;
        damageMultiplier = dmgMul;
    }
}
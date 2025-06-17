using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;

public enum enemyEffect
{
    Frozen,
    Fire,
    Shock
}

public abstract class StatusEffect
{
    public float duration { get; set; }

    public float interval;
    public float timer = 0.0f;

    public int stack { get; protected set; }
    public bool isFinished => duration <= 0f;

    public int maxStack = 1;
    public virtual float speedMultiplier => 1f;
    public virtual float damageTakenMultiplier => 1f;


    public virtual bool equalType(StatusEffect other) => GetType() == other.GetType();

    public abstract void apply();

    public virtual void addStack()
    {
        stack++; 
    }

    public abstract void update(float deltaTime, EnemyEffectManager manager);
    public abstract void end();

    public abstract StatusEffect Clone();
}
public class FrozenEffect : StatusEffect
{
    public FrozenEffect(float duration, int stack = 1)
    {
        this.duration = duration;
        this.stack = stack;

        maxStack = 2;
    }

    public override void apply()
    {

    }

    public override void update(float deltaTime, EnemyEffectManager manager)
    {
        duration -= deltaTime;
        timer += deltaTime;
    }

    public override void end()
    {

    }

    public override StatusEffect Clone()
    {
        return new FrozenEffect(duration, stack);
    }

    public override float speedMultiplier => 1f - 0.15f * stack;
}

public class FireEffect : StatusEffect
{

    public FireEffect(float duration, int stack = 1)
    {
        this.duration = duration;
        this.stack = stack;

        maxStack = 4;
        interval = 0.25f;
    }

    public override void apply()
    {

    }
    public override void update(float deltaTime, EnemyEffectManager manager)
    {
        duration -= deltaTime;
        timer += deltaTime;

        if (timer >= interval)
        {
            timer -= interval;

           manager.damageEffect(stack);
        }
    }

    public override void end()
    {

    }

    public override StatusEffect Clone()
    {
        return new FireEffect(duration,stack);
    }
}

public class ShockEffect : StatusEffect
{
    public ShockEffect(float duration, int stack = 1)
    {
        this.duration = duration;
        this.stack = stack;

        maxStack = 2;
    }
    public override void apply()
    {

    }
    public override void update(float deltaTime, EnemyEffectManager manager)
    {
        duration -= deltaTime;
    }

    public override void end()
    {

    }

    public override float damageTakenMultiplier => 0.5f * stack + 1f;

    public override StatusEffect Clone()
    {
        return new ShockEffect(duration, stack);
    }
}

public class EnemyEffectManager : MonoBehaviour
{
    private List<StatusEffect> effects = new List<StatusEffect>();
    public float speedMultiplier { get; private set; } = 1f;
    public float damageMultiplier { get; private set; } = 1f;

    public void applyEffect (StatusEffect effect)
    {
        foreach (StatusEffect curr in effects)
        {
            if(curr.equalType(effect))
            {
                if(curr.stack < effect.maxStack)
                {
                    curr.addStack();
                }
                curr.duration = Mathf.Max(curr.duration, effect.duration);

                return;
            }
        }

        StatusEffect clone = effect.Clone();
        clone.apply();
        effects.Add(clone);
    }

    public void damageEffect(int damage)
    {
        DefaultEnemy enemy = GetComponent<DefaultEnemy>();

        enemy.takeDamage(damage, true);
    }



    public void Update()
    {
        // run Timer

        float dt = Time.deltaTime;

        float speedMultiple = 1.0f;
        float damageMultiple = 1.0f;

        for(int i = 0; i < effects.Count; i++)
        {
            StatusEffect effect = effects[i];

            effect.duration -= dt;
            
            if(effect.duration <= 0f)
            {
                effect.end();
                effects.RemoveAt(i);
                continue;
            }

            effect.timer -= dt;

            if (effect.interval > 0f)
            {

                while (effect.timer <= 0f)
                {
                    effect.update(dt, GetComponent<EnemyEffectManager>());
                    effect.timer += effect.interval;
                }
            }

            speedMultiple = Mathf.Min(speedMultiple, effect.speedMultiplier);
            damageMultiple *= effect.damageTakenMultiplier;
        }

        speedMultiplier = speedMultiple;
        damageMultiplier = damageMultiple;
    }
}
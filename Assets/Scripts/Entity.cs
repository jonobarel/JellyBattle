using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : ColorFightersBase

{
    protected Entity entity;
    protected Renderer bodyRenderer;
    protected Light bodyLight;
    protected ParticleSystem particleSys;
    protected ParticleSystem.MainModule particleColor;

    public virtual void Awake()
    {
        bodyLight = GetComponent<Light>();
        entity = GetComponent<Entity>();
        bodyRenderer = GetComponentInChildren<Renderer>();
        particleSys = GetComponentInChildren<ParticleSystem>();
        particleColor = GetComponentInChildren<ParticleSystem>().main;
    }

    public Entity Owner {
        get {return entity;}
        set {entity = value;}
    }
    
    public Color entityColor
    {
        get { return bodyRenderer.material.color; }
        set
        {
            SetColor(value);
        }
    }

    private void SetColor(Color color)
    {
        bodyRenderer.material.SetColor("_Color", color);
        bodyRenderer.material.SetColor("_EmissionColor", color);
        bodyLight.color = color;
        particleColor.startColor = color;
    }
}

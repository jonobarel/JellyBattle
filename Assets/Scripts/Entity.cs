using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : ColorFightersBase

{
        protected Entity entity;
        protected Renderer bodyRenderer;
        protected Light bodyLight;
        protected ParticleSystem particleSys;

        private void Start() {
            entity = GetComponent<Entity>();
            bodyRenderer = GetComponentInChildren<Renderer>();
        }

        
    public Color Color
    {
        get { return bodyRenderer.material.color; }
        set
        {
            SetEntityColor(value);
        }
    }

    private void SetEntityColor(Color color)
    {
        bodyRenderer.material.SetColor("_Color", color);
        bodyRenderer.material.SetColor("_EmissionColor", color);
        bodyLight.color = color;
        particleSys.startColor = color;
    }
}

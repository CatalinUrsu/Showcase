using UnityEngine;
using System.Collections.Generic;

namespace Source.Gameplay
{
public class EnemyAppearence : MonoBehaviour
{
    [SerializeField] EdgeCollider2D _colliderEdge;
    
    public void Set(SpriteRenderer spriteRenderer, Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        SetCollider(spriteRenderer);
    }
    
    void SetCollider(SpriteRenderer spriteRenderer)
    {
        var points = new List<Vector2>(10);
        
        spriteRenderer.sprite.GetPhysicsShape(0, points);
        points.Add(points[0]);
        _colliderEdge.SetPoints(points);
    }
}
}
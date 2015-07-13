using UnityEngine;
using System.Collections.Generic;
using util;

public class Shield : MonoBehaviour
{
    [SerializeField] private Texture2D _explosionMask;
    [SerializeField] private ParticleSystem _ps;

    private Texture2D _texture;

    private Material _material;
    private Sprite _sprite;

    private Transform _hitPoint;

    private IntVec2 textureSize;
    private IntVec2 maskSize;

    void Awake()
    {
        _material = GetComponent<SpriteRenderer>().sharedMaterial;
        _sprite = GetComponent<SpriteRenderer>().sprite;
        _texture = Instantiate(_sprite.texture) as Texture2D;

        textureSize = new IntVec2(_texture.width, _texture.height);
        maskSize = new IntVec2(_explosionMask.width, _explosionMask.height);

        _hitPoint = new GameObject("hitPoint").transform;
        _hitPoint.SetParent(transform);
        _hitPoint.localPosition = Vector3.zero;
    }

    void OnCollisionEnter2D (Collision2D col)
    {
        Vector2 midPoint = new Vector2();
        int collisionsNum = col.contacts.Length;
        for (int i = 0; i < collisionsNum; i++) {
            midPoint.x += col.contacts[i].point.x;
            midPoint.y += col.contacts[i].point.y;
        }
        midPoint.x /= collisionsNum; 
        midPoint.y /= collisionsNum; 
        
        midPoint.x -= transform.position.x;
        midPoint.x /= transform.lossyScale.x; 
        
        midPoint.y -= transform.position.y;
        midPoint.y /= transform.lossyScale.y; 
        _hitPoint.localPosition = midPoint;

        var normalized = new Vector2(midPoint.x / (textureSize.x/100f), midPoint.y / (textureSize.y/100f));

        int explTx = 0;
        int explTy = 0;
        int explW = maskSize.x;
        int explH = maskSize.y;

        int tx = Mathf.FloorToInt(normalized.x * textureSize.x) - maskSize.x/2;
        int ty = Mathf.FloorToInt(normalized.y * textureSize.y) - maskSize.y/2;
        int w = maskSize.x;
        int h = maskSize.y;

        if (tx < 0) {
            explTx = -tx;
            w += tx;
            explW = w;
            tx = 0;
        } else if (tx + w > textureSize.x) {
            w -= (tx + w) - textureSize.x;
            explW = w;
        }

        if (ty < 0) {
            explTy = -ty;
            h += ty;
            explH = h;
            ty = 0;
        } else if (ty + h > textureSize.y) {
            h -= (ty + h) - textureSize.y;
            explH = h;
        }

        var txtColors = _texture.GetPixels(tx, ty, w, h);
        var explMask = _explosionMask.GetPixels(explTx, explTy, explW, explH);

        for(int i = 0; i < txtColors.Length; i++) {
            txtColors[i].a = Mathf.Max(0, txtColors[i].a - explMask[i].a);
        }
        _texture.SetPixels(tx, ty, w, h, txtColors);
        _texture.Apply();
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(_texture, _sprite.rect, _sprite.pivot);

        Destroy (GetComponent<PolygonCollider2D>());
        float alphas = 0;
        var pxls = _texture.GetPixels();
        for (int i = 0; i < pxls.Length; i++) {
            if (pxls[i].a > 0.1f)
                alphas += 1;
        }
        float alphaRatio = alphas / pxls.Length;
        if (alphaRatio > 0.05f) {
            gameObject.AddComponent<PolygonCollider2D>();
        }
        
        var ps = Instantiate(_ps);
        ps.transform.position = new Vector3(_hitPoint.position.x, _hitPoint.position.y, -3);
        ps.Play();
    }

    void OnDrawGizmos()
    {
        if (_hitPoint == null) return;
        Gizmos.color = new Color(0.8f, 0, 0, 0.8f);
        Gizmos.DrawWireSphere(_hitPoint.position, 0.05f);
    }
}

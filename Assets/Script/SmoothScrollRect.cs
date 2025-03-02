using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SmoothScrollRect : ScrollRect
{
    [Header("Smooth Scroll Settings")]
    [SerializeField] private bool smoothScrolling = true; //scrolling animation bool
    [SerializeField] private float smoothScrollTime = 0.08f; // scroll time

    public override void OnScroll(PointerEventData data)
    {
        if (!IsActive()) return; 

        if (smoothScrolling)
        {
            Vector2 positionBefore = normalizedPosition;
            this.DOKill(true);
            base.OnScroll(data);
            Vector2 positionAfter = normalizedPosition;

            normalizedPosition = positionBefore;
            this.DONormalizedPos(positionAfter, smoothScrollTime)
                .SetUpdate(true);
        }
        else
        {
            base.OnScroll(data);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        this.DOKill();
    }
}
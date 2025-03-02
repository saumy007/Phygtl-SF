using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class AnimScript : MonoBehaviour
{
    public SmoothScrollRect scrollRect; // Reference to ScrollRect script
    public float cropAmount = 0.5f; // Percentage to crop
    public float cropDuration = 0.2f; // Animation duration

    public RectTransform imgRect;
    public Vector2 originalSize;

    void Start()
    {
        imgRect = GetComponent<RectTransform>();
        originalSize = imgRect.sizeDelta; // Store original size
        scrollRect.onValueChanged.AddListener(OnValueChange);

        
        
    }

    private void OnValueChange(Vector2 position)
    {
        // Do your code here
        
    }


    void Update()
    {
        if (scrollRect != null && Mathf.Abs(scrollRect.velocity.y) > 0.01f)
        {
            ApplyCropping();  // Crop while scrolling
        }
        else
        {
            ResetCropping();  // Restore when scrolling stops
        }
    }

    private void ApplyCropping()
    {
        Vector2 croppedSize = new Vector2(originalSize.x, originalSize.y * (1 - cropAmount));
        imgRect.DOSizeDelta(croppedSize, cropDuration).SetEase(Ease.InOutQuad);
    }

    private void ResetCropping()
    {
        imgRect.DOSizeDelta(originalSize, cropDuration).SetEase(Ease.InOutQuad);
    }
}


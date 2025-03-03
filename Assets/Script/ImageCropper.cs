using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ImageCropper : MonoBehaviour
{
    Sequence mySequence = DOTween.Sequence();

    [Header("References")]
    [SerializeField] private RawImage targetImage; // sneaker image
    [SerializeField] private RawImage targetImageBG; // background of desc image
    [SerializeField] private TextMeshProUGUI desc;
    [SerializeField] private TextMeshProUGUI header;

    [SerializeField] private Button Btn1;
    [SerializeField] private Button Btn2;
    [SerializeField] private Button Btn3;

    [SerializeField] private Button Leaderboard;



    private Vector3 btn1OriginalPos, btn2OriginalPos, btn3OriginalPos;
    private Vector3 btn1OriginalScale, btn3OriginalScale, btn2OriginalScale;
    private Vector3 leaderboardScale;
    private Vector3 leaderboardOriginalPos;


    [Header("Crop Settings")]
    [SerializeField] [Range(0, 1)] private float cropAmount = 0.5f;
    [SerializeField] [Range(0, 1)] private float cropAmountBG = 0.2f;
    [SerializeField] private float cropDuration = 0.2f;
    [SerializeField] private float resetDelay = 0.3f;
    [SerializeField] private float velocityThreshold = 0.01f;

    

    //-106 to 26

    private ScrollRect scrollRect;
    private Vector2 originalSize;
    private Vector2 originalSizeBG;
    private bool isCropped;
    private Tween resetTween;
    private float lastVelocityY;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        //header = GetComponent<TextMeshProUGUI>();
        

        // Do not overwrite the serialized reference
        if (targetImage != null)
        {
            originalSize = targetImage.rectTransform.sizeDelta;
            originalSizeBG = targetImageBG.rectTransform.sizeDelta;
        }

        // Ensure desc has CanvasGroup for proper fading
        if (desc != null && !desc.TryGetComponent(out CanvasGroup canvasGroup))
        {
            canvasGroup = desc.gameObject.AddComponent<CanvasGroup>();
        }

        btn1OriginalPos = Btn1.transform.localPosition;
        btn1OriginalScale = Btn1.transform.localScale;
        btn2OriginalPos = Btn2.transform.localPosition;
        btn2OriginalScale = Btn2.transform.localScale;
        btn3OriginalPos = Btn3.transform.localPosition;
        btn3OriginalScale = Btn3.transform.localScale;

        leaderboardOriginalPos = Leaderboard.transform.localPosition;
        leaderboardScale = Leaderboard.transform.localScale;

    }

    private void Update()
    {
        if (scrollRect == null || targetImage == null) return;

        float currentVelocityY = scrollRect.velocity.y;

        // Detect scroll direction changes
        if (Mathf.Abs(currentVelocityY) > velocityThreshold)
        {
            if (currentVelocityY > 0 && !isCropped) // Scrolling up
            {
                ApplyCropping();
                textFade();
            }
            else if (currentVelocityY < 0 && isCropped) // Scrolling down
            {
                ResetImage();
                ResetTextFade();
            }
        }
        else if (isCropped) // When scrolling stops
        {
            ScheduleReset();
        }

        lastVelocityY = currentVelocityY;
    }

    private void ApplyCropping()
    {
        isCropped = true;
        Vector2 croppedSize = new Vector2(originalSize.x, originalSize.y * (1 - cropAmount));
        Vector2 croppedSizeBG = new Vector2(originalSizeBG.x, originalSizeBG.y * (1 - cropAmount));

        targetImage.rectTransform.DOKill();
        targetImageBG.rectTransform.DOKill();
        targetImage.rectTransform.DOSizeDelta(croppedSize, cropDuration).SetEase(Ease.InOutQuad);
        targetImageBG.rectTransform.DOSizeDelta(croppedSizeBG, cropDuration).SetEase(Ease.InOutQuad);
        //targetImageBG.rectTransform.DOMove(targetImageBG.transform.position, cropDuration);
        Debug.Log("The place of target is here" + targetImageBG.transform.position.ToString());
        MovementScript();
        MoveButtons(true);
        leaderBoardUI(true);
        textFade();
    }

    private void MovementScript() 
    {
        targetImageBG.transform.DOLocalMove(new Vector3(0, -648, 0), cropDuration);
        Debug.Log("The place of target is here" + targetImageBG.transform.position.ToString());

        if (header == null) return;

        CanvasGroup canvasGroup = desc.GetComponent<CanvasGroup>();

        header.rectTransform.DOLocalMoveY(29f, cropDuration).SetEase(Ease.InOutQuad);




        //canvasGroup.domove(1f, cropDuration).SetEase(Ease.InOutQuad)
        //    .OnComplete(() =>
        //    {
        //        canvasGroup.interactable = true;
        //        canvasGroup.blocksRaycasts = true;
        //    });


    }

    private void MoveButtons(bool moveUp)
    {

        // changing original position

        Vector3 btn1Target = moveUp ? new Vector3(233, 1140, 0) : btn1OriginalPos;
        Vector3 btn2Target = moveUp ? new Vector3(233, 1140, 0) : btn2OriginalPos;
        Vector3 btn3Target = moveUp ? new Vector3(575, 1140, 0) : btn3OriginalPos;

        //changing original scale to new scale
        Vector3 btn1Scale = moveUp ? new Vector3(1.08f, 1.08f, 0) : btn1OriginalScale;
        Vector3 btn2Scale = moveUp ? new Vector3(1.08f, 1.08f, 0) : btn2OriginalScale;
        Vector3 btn3Scale = moveUp ? new Vector3(1.08f, 1.08f, 0) : btn3OriginalScale;

        Btn1.transform.DOLocalMove(btn1Target, cropDuration);
        Btn2.transform.DOLocalMove(btn2Target, cropDuration);
        Btn3.transform.DOLocalMove(btn3Target, cropDuration);

        Btn1.transform.DOScale(btn1Scale, cropDuration);
        Btn2.transform.DOScale(btn2Scale, cropDuration);
        Btn3.transform.DOScale(btn3Scale, cropDuration);

    }

    

    private void leaderBoardUI(bool moveUp)
    {
        Vector3 leaderBoardTarget = moveUp ? new Vector3(32, -284, 0) : leaderboardOriginalPos;
        Vector3 leaderboardScale = moveUp ? new Vector3(1.05f, 1.05f, 0) : btn1OriginalScale;
        Leaderboard.transform.DOLocalMove(leaderBoardTarget, cropDuration);
        Leaderboard.transform.DOScale(leaderboardScale, cropDuration);
        
    }

    //private void threebtns() 
    //{
    //    threeButtons.LeanMoveLocal(new Vector3(0, 636, 0), cropDuration);
    //    threeButtons.


    //}



    private void textFade()
    {
        if (desc == null) return;

        CanvasGroup canvasGroup = desc.GetComponent<CanvasGroup>();

        canvasGroup.DOFade(0f, cropDuration).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
            // Instead of disabling, keep it faded out
            canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            });
    }

    private void ResetImage()
    {
        isCropped = false;
        targetImage.rectTransform.DOKill();
        targetImageBG.rectTransform.DOKill();
        targetImage.rectTransform.DOSizeDelta(originalSize, cropDuration).SetEase(Ease.InOutQuad);
        targetImageBG.rectTransform.DOSizeDelta(originalSizeBG, cropDuration).SetEase(Ease.InOutQuad);
        MoveButtons(false);
        leaderBoardUI(false);

        header.rectTransform.DOLocalMoveY(-146, cropDuration).SetEase(Ease.InOutQuad);
    }

    private void ResetTextFade()
    {
        if (desc == null) return;

        CanvasGroup canvasGroup = desc.GetComponent<CanvasGroup>();

        canvasGroup.DOFade(1f, cropDuration).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            });

        
    }

    private void ScheduleReset()
    {
        if (resetTween != null && resetTween.IsActive())
            resetTween.Kill();

        resetTween = DOVirtual.DelayedCall(resetDelay, () =>
        {
            if (isCropped) ResetImage();
        });
    }

    private void OnDestroy()
    {
        if (targetImage != null)
            targetImage.rectTransform.DOKill();
    }
}
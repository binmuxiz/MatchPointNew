using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Fader: MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static async UniTask FadeIn(CanvasGroup canvasGroup, float duration)
    {
        canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
        await canvasGroup.DOFade(1f, duration).AsyncWaitForCompletion();
    }
    
    public static async UniTask FadeOut(CanvasGroup canvasGroup, float duration)
    {
        await canvasGroup.DOFade(0f, duration).AsyncWaitForCompletion();
        canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
    }

    public static void FadeIn(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
    }

    
    public static void FadeOut(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
    }

    public static void Sliding(CanvasGroup canvasGroup)
    {
        GameObject gameObject = new GameObject();
        
    }
}

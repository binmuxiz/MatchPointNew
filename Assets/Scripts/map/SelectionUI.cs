using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SelectionUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private Button noButton;

    void Start()
    {
        if (noButton != null)
        {
            noButton.onClick.AddListener(Close);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Fader.FadeIn(canvasGroup, fadeDuration).Forget(); 
    }

    public void Close()
    {
        Fader.FadeOut(canvasGroup, fadeDuration).Forget();
        gameObject.SetActive(false);
    }
}
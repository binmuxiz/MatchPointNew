using UnityEngine;

public class OutlineInteraction : MonoBehaviour
{
    // Selection UI 제어를 위한 참조
    [SerializeField] private SelectionUI selectionUI; // SelectionUI 스크립트가 부착된 오브젝트

    // 기본 쉐이더와 아웃라인 쉐이더
    private Material originalMaterial; 
    [SerializeField] private Material outlineMaterial; // Inspector에서 아웃라인 쉐이더 지정

    private Renderer objectRenderer; // 큐브의 Renderer

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            originalMaterial = objectRenderer.material; // 기존 쉐이더 저장
        }
    }

    private void OnMouseEnter()
    {
        // 마우스가 오브젝트 위로 올라가면 아웃라인 쉐이더로 변경
        if (objectRenderer != null)
        {
            objectRenderer.material = outlineMaterial;
        }
    }

    private void OnMouseExit()
    {
        // 마우스가 오브젝트를 벗어나면 원래 쉐이더로 복구
        if (objectRenderer != null)
        {
            objectRenderer.material = originalMaterial;
        }
    }

    private void OnMouseDown()
    {
        // 마우스 클릭 시 SelectionUI의 Show 호출
        if (selectionUI != null)
        {
            selectionUI.Show(); // SelectionUI의 Show 메서드 호출
        }
    }
}
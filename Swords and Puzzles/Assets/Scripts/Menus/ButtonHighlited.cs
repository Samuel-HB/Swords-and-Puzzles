using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHighlited : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI buttonText;
    private Color originalColor;

    private void Start()
    {
        originalColor = buttonText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = new Color(1f, 0.95f, 0.85f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = originalColor;
    }
}

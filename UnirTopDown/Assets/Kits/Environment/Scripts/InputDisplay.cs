using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InputDisplay : MonoBehaviour
{
    [SerializeField] private InputActionReference actionReference;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private string actionName = "Attack";
    [SerializeField] private GameObject canvasMessage;

    private void Awake()
    {
        canvasMessage.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canvasMessage.SetActive(true);
            PrintMessage();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canvasMessage.SetActive(false);
        }
    }

    private void PrintMessage()
    {
        var action = actionReference.action;
        if (action != null && action.controls.Count > 0)
        {
            int bindingIndex = action.GetBindingIndexForControl(action.controls[0]);
            string displayString = action.GetBindingDisplayString(bindingIndex);
            textMeshPro.text = $"Press \"{displayString}\" to {actionName.ToUpper()}";
        }
    }
}

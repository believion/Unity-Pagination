using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaginationButtonScript : MonoBehaviour
{
    public int val = 0;
    public TextMeshProUGUI text_val;
    public Button togglComponent;
    public int index = 0;

    public static event Action<int, int> btnClickAction;
    public bool isDisabled = false;

    private void OnEnable()
    {
        togglComponent.onClick.AddListener(OnValueChangeHandler);
    }

    public void OnValueChangeHandler()
    {
        Debug.Log("event Fired with params" + val + " " + index);
        btnClickAction?.Invoke(val, index);
    }

    public void SetButton(int recVal)
    {
        val = recVal;
        text_val.text = recVal.ToString();
    }

    private void OnDestroy()
    {
        togglComponent.onClick.RemoveListener(OnValueChangeHandler);
    }
}
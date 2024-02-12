using System;
using UnityEngine;
using UnityEngine.UI;

public class PaginationHelper : MonoBehaviour
{
    [Tooltip("Total Max Pages")] [SerializeField]
    private int _totalMaxPages = 20;

    [Tooltip("Max Buttons to be displayed")] [SerializeField]
    private int _maxBtnsToDisplay = 9;

    private int _currRangeMin = 1;
    private int _currRangeMax;

    [Tooltip("Elements Before Selected")] [SerializeField]
    private int _elemBeforeSelected = 2;

    private int _currPage = 1;
    private int _currIndex = 1;

    public int pages = 20;

    [Tooltip("Main Buttons")] [SerializeField]
    private PaginationButtonScript[] mainBtns;

    private void Start()
    {
        PaginationButtonScript.btnClickAction += OnIndexButtonClick;
        InitBtns(pages);
    }

    public void InitBtns(int pages)
    {
        _currRangeMin = 1;
        _currRangeMax = _maxBtnsToDisplay;
        _totalMaxPages = pages;
        if (_totalMaxPages < _maxBtnsToDisplay)
        {
            InitMainButtons(_totalMaxPages);
        }
        else if (_totalMaxPages > _maxBtnsToDisplay + 2)
        {
            InitMainButtons(_maxBtnsToDisplay);
        }
    }

    public void OnIndexButtonClick(int val, int index)
    {
        if (val > _totalMaxPages || val < 1)
        {
            throw new Exception("Invalid Value");
        }

        Debug.Log("val recived is" + val);

        _currPage = val;
        _currIndex = index;
        if (_currPage >= _totalMaxPages - 1)
        {
            _currRangeMax = _totalMaxPages;
            _currRangeMin = _totalMaxPages - _maxBtnsToDisplay;
            SetLastButtons(_totalMaxPages%2 == 0);
            if (_currPage == _totalMaxPages - 1)
            {
                SelectCurrButton(_maxBtnsToDisplay - 2);
            }
            else
            {
                SelectCurrButton(_maxBtnsToDisplay - 1);
            }
        }
        else if (_currPage >= _currRangeMax)
        {
            _currRangeMax = val + ((_maxBtnsToDisplay - _elemBeforeSelected) - 1);
            _currRangeMin = val - _elemBeforeSelected;
            if (_currRangeMax > _totalMaxPages)
            {
                if (_currRangeMin < _totalMaxPages)
                {
                    _currRangeMax = _totalMaxPages;
                    SetButtons();
                    SelectCurrButton(_elemBeforeSelected);
                    ToggleMainButtons(_currRangeMax - _currRangeMin + 2, _maxBtnsToDisplay, false);
                    return;
                }
                else
                {
                    return;
                }
            }

            SetButtons();
            SelectCurrButton(_elemBeforeSelected);
        }
        else if (_currPage <= _currRangeMin)
        {
            _currRangeMax = _currRangeMin + _elemBeforeSelected;
            _currRangeMin = (_currRangeMin + _elemBeforeSelected - _maxBtnsToDisplay) + 1;
            if (_currRangeMin < 1)
            {
                return;
            }

            SetButtons();
            SelectCurrButton(_maxBtnsToDisplay - _elemBeforeSelected - 1);
        }
    }

    public void ToggleMainButtons(int low, int high, bool val)
    {
        for (int i = low; i <= high; i++)
        {
            this.mainBtns[i - 1].gameObject.SetActive(val);
        }
    }

    public void SelectCurrButton(int index)
    {
        if (index > _maxBtnsToDisplay || index < 0)
        {
            throw new Exception("Invalid Button index");
        }

        this.mainBtns[index].GetComponent<Button>().Select();
    }

    public void SetButtons()
    {
        int min = _currRangeMin;
        int max = _currRangeMax;

        for (int i = min; i <= max; i++)
        {
            this.mainBtns[i - min].SetButton(i);
        }
    }

    public void SetLastButtons(bool isEven)
    {
        if (isEven)
        {
            _currRangeMin++;
        }
        int min = _currRangeMin;
        int max = _currRangeMax;
        for (int i = min; i < max; i++)
        {
            this.mainBtns[i-min].SetButton(i);
        }
    }

    public void InitMainButtons(int maxRange)
    {
        for (int i = 1; i <= _maxBtnsToDisplay; i++)
        {
            if (i <= maxRange)
            {
                mainBtns[i - 1].gameObject.SetActive(true);
                mainBtns[i - 1].SetButton(i);
            }
            else
            {
                mainBtns[i - 1].gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        PaginationButtonScript.btnClickAction -= OnIndexButtonClick;
    }
}
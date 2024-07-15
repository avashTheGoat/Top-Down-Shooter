using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class TabsUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> tabs;
    [SerializeField] private List<Button> buttons;

    [SerializeField] private int openTabIndex = DEFAULT_TAB_IDX;

    private const int DEFAULT_TAB_IDX = 0;

    private void Start()
    {
        if (tabs.Count == 0)
        {
            Debug.LogWarning("There are 0 tabs.");
            return;
        }

        if (tabs.Count != buttons.Count)
            Debug.LogError("Number of tabs and number of buttons must be the same.");

        int _count = buttons.Count;
        for (int i = 0; i < _count; i++)
        {
            GameObject _curTab = tabs[i];
            buttons[i].onClick.AddListener(() => SwitchTab(tabs.IndexOf(_curTab)));

            tabs[i].SetActive(false);
        }

        tabs[openTabIndex].SetActive(true);
    }

    public void AddTab(GameObject _tab, Button _tabOpenButton)
    {
        if (_tab == null)
            throw new ArgumentException("Tab cannot be null.");

        if (_tabOpenButton == null)
            throw new ArgumentException("Tab button cannot be null.");

        _tabOpenButton.onClick.AddListener(() => SwitchTab(tabs.IndexOf(_tab)));
        _tab.SetActive(false);
        
        tabs.Add(_tab);
        buttons.Add(_tabOpenButton);

        tabs[openTabIndex].SetActive(true);
    }

    public void RemoveTab(GameObject _tab, Action<GameObject, Button> _onTabRemove = null)
    {
        if (_tab == null)
            throw new ArgumentException("Tab cannot be null");

        if (!tabs.Contains(_tab))
        {
            Debug.LogWarning($"Tab does not exist. {nameof(_tab)} is {_tab}");
            return;
        }

        int _idx = tabs.IndexOf(_tab);
        Button _tabButton = buttons[_idx];

        tabs.RemoveAt(_idx);
        buttons.RemoveAt(_idx);

        _onTabRemove?.Invoke(_tab, _tabButton);

        if (_idx == openTabIndex)
            openTabIndex = DEFAULT_TAB_IDX;

        else if (_idx < openTabIndex)
            openTabIndex--;
    }

    public void RemoveTab(Button _tabButton, Action<GameObject, Button> _onTabRemove = null)
    {
        if (_tabButton == null)
            throw new ArgumentException("Tab cannot be null");

        if (!buttons.Contains(_tabButton))
        {
            Debug.LogWarning($"Tab does not exist. {nameof(_tabButton)} is {_tabButton}");
            return;
        }

        int _deletedIdx = buttons.IndexOf(_tabButton);
        GameObject _tab = tabs[_deletedIdx];

        tabs.RemoveAt(_deletedIdx);
        buttons.RemoveAt(_deletedIdx);

        _onTabRemove?.Invoke(_tab, _tabButton);

        if (_deletedIdx == openTabIndex)
            openTabIndex = DEFAULT_TAB_IDX;

        else if (_deletedIdx < openTabIndex)
            openTabIndex--;
    }

    public void Clear()
    {
        tabs = new();
        buttons = new();
        openTabIndex = 0;
    }

    public void SwitchTab(int _tabIndex)
    {
        if (tabs[openTabIndex] == null)
            throw new Exception($"Open tab cannot be null. {nameof(openTabIndex)} is {openTabIndex}");

        tabs[openTabIndex].SetActive(false);

        if (_tabIndex >= tabs.Count)
            throw new IndexOutOfRangeException($"{nameof(_tabIndex)} is out of bounds. It must be less than {tabs.Count}.");

        if (_tabIndex < 0)
            throw new IndexOutOfRangeException($"{nameof(_tabIndex)} is out of bounds. It must be greater than or equal to 0.");

        if (tabs[_tabIndex] == null)
            throw new Exception($"Tab being switched to cannot be null. {nameof(_tabIndex)} is {_tabIndex}");

        tabs[_tabIndex].SetActive(true);
        openTabIndex = _tabIndex;
    }

    public GameObject GetTabObject(int _tabIndex) => tabs[_tabIndex];

    public Button GetTabButton(int _buttonIndex) => buttons[_buttonIndex];
}
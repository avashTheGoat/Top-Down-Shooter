using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TabsUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> tabs;
    [SerializeField] private List<Button> buttons;

    [SerializeField] private int openTabIndex = 0;

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
            int _iClone = i;
            buttons[i].onClick.AddListener(() => SwitchTab(tabs.IndexOf(tabs[_iClone])));

            tabs[i].SetActive(false);
        }

        tabs[openTabIndex].SetActive(true);
    }

    public void AddTab(GameObject _tab, Button _tabOpenButton)
    {
        tabs.Add(_tab);
        buttons.Add(_tabOpenButton);

        _tab.SetActive(false);
        tabs[openTabIndex].SetActive(true);

        _tabOpenButton.onClick.AddListener(() => SwitchTab(tabs.IndexOf(_tab)));
    }

    public void SwitchTab(int _tabIndex)
    {
        if (tabs[openTabIndex] != null)
            tabs[openTabIndex].SetActive(false);

        if (tabs[_tabIndex] != null)
            tabs[_tabIndex].SetActive(true);

        openTabIndex = _tabIndex;
    }

    public GameObject GetTab(int _tabIndex) => tabs[_tabIndex];

    public Button GetButton(int _buttonIndex) => buttons[_buttonIndex];
}
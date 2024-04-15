using UnityEngine;
using UnityEngine.UI;

public class TabsUI : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;
    [SerializeField] private Button[] buttons;

    [SerializeField] private int openTabIndex = 0;

    private void Start()
    {
        if (tabs.Length != buttons.Length)
            Debug.LogError("Number of tabs and number of buttons must be the same.");

        for (int i = 0; i < buttons.Length; i++)
        {
            int _iClone = i;

            buttons[i].onClick.AddListener(() =>
            {
                tabs[openTabIndex].SetActive(false);
                tabs[_iClone].SetActive(true);
                
                openTabIndex = _iClone;
            });

            tabs[i].SetActive(false);
        }

        tabs[openTabIndex].SetActive(true);
    }
}
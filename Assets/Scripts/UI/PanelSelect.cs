using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSelect : MonoBehaviour
{
    [System.Serializable]
    public struct PanelInfo
    {
        public GameObject panel;
        public Button button;
        public KeyCode keyCode;
        public Action.eActionType actionType;
    }

    public KeyCode toggleKey;
    public GameObject masterPanel;
    public PanelInfo[] panelInfos;
    public Editor editor;

    void Start()
    {
        foreach (PanelInfo p in panelInfos)
        {
            p.button.onClick.AddListener(delegate { ButtonEvent(p); });
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey)) masterPanel.SetActive(!masterPanel.activeSelf);
        foreach(PanelInfo p in panelInfos)
        {
            if(Input.GetKeyDown(p.keyCode))
            {
                SetPanelActive(p);
            }
        }
    }

    void SetPanelActive(PanelInfo panelInfo)
    {
        for(int i = 0; i < panelInfos.Length; i++)
        {
            bool isActive = panelInfos[i].Equals(panelInfo);

            panelInfos[i].panel.SetActive(isActive);
            if (isActive)
            {
                editor.SetAction(panelInfos[i].actionType);
            }
        }
    }

    void ButtonEvent(PanelInfo panelInfo)
    {
        SetPanelActive(panelInfo);
    }
}

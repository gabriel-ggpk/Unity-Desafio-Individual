using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public List<GameObject> panels;
    public GameObject defaultPanel;
    public GameObject currentPanel;
    public Canvas canvas;
    private void Start()
    {
        currentPanel = defaultPanel;
        currentPanel.SetActive(true);
    }
    public void changePanel(string panelName)
    {
        GameObject newPanel = panels.FirstOrDefault(panel => panel.name == panelName);
        if (newPanel != null)
        {
            if(currentPanel != null) currentPanel.SetActive(false);
            
            currentPanel = newPanel;
            currentPanel.SetActive(true);

        }
    }
    public void clearPanel()
    {
        changePanel("Playing");
    }
}

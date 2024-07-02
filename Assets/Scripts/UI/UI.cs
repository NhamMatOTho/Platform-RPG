using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject ingameUI;

    public UI_ItemTooltip itemTooltip;
    public UI_StatTooltip statTooltip;
    public UI_SkillTooltip skillTooltip;
    public UI_CraftWindow craftWindow;

    private void Awake()
    {
        SwitchTo(skillTreeUI); //assign events on skill tree slots before assign events on scripts
    }

    void Start()
    {
        SwitchTo(ingameUI);

        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwitchWithKey(characterUI);

        if (Input.GetKeyDown(KeyCode.B))
            SwitchWithKey(craftUI);

        if (Input.GetKeyDown(KeyCode.K))
            SwitchWithKey(skillTreeUI);

        if (Input.GetKeyDown(KeyCode.O))
            SwitchWithKey(optionUI);
    }

    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
            _menu.SetActive(true);
    }

    public void SwitchWithKey(GameObject _menu)
    {
        if(_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                return;
        }

        SwitchTo(ingameUI);
    }
}

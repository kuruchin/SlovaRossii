using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DictionaryList : MonoBehaviour
{
    //This is the Dropdown
    TMP_Dropdown m_Dropdown;
    [SerializeField]
    private Dictionaries dictionaries;

    void Start()
    {
        //Fetch the Dropdown GameObject the script is attached to
        m_Dropdown = GetComponent<TMP_Dropdown>();
        //Clear the old options of the Dropdown menu
        m_Dropdown.ClearOptions();
        for (int i = 0; i < dictionaries.DictionaryName.Length; i++)
        {
            //Add the options
            m_Dropdown.options.Add(new TMP_Dropdown.OptionData() { text = dictionaries.DictionaryName[i] });
        }
        m_Dropdown.value = 0;
        m_Dropdown.RefreshShownValue();
    }
}

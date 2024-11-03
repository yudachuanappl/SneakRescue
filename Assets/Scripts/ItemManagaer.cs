using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemManagaer : MonoBehaviour
{
    public static ItemManagaer Instance;
    public Text text;
    public GameObject CloestItem;
    public enum INTERACTIVABLE_ITEM_TYPE
    {
        NONE,
        CAR,
        TARGET,
        LIGHT,
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShowItemDescription(string itemDes, bool isCollectable,bool isInteractiveable, GameObject obj)
    {
        if(isInteractiveable && SceneManager.GetActiveScene().buildIndex!=0)
        {
            text.text = itemDes +  "\nPress E To Interact";
        }
        else if(isCollectable)
        {
            text.text = itemDes + "\nPress E To Collect" ;
        }
        else
        {
            text.text = itemDes;
        }
        
        CloestItem = obj;
    }
    public void ResetText()
    {
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.E))
        {
            if (CloestItem != null && CloestItem.GetComponent<ItemSettings>().IsCollectable)
            {
                if(InventorySystem.Instance.HandlePickupItem(CloestItem.GetComponent<ItemSettings>().itemType))
                {
                   // this.GetComponent<AudioSource>().Play();
                    Destroy(CloestItem);
                    CloestItem = null;
                    ResetText();
                }
                else
                {
                    text.text = "INVENTORY FULL";
                }
            }
            else if (CloestItem != null && CloestItem.GetComponent<ItemSettings>().IsInteractiveable)
            {
              //  this.GetComponent<AudioSource>().Play();
                CloestItem.GetComponent<ItemSettings>().IsUsedItem = true;
                CloestItem.GetComponent<ItemSettings>().HandleInteractWithItem();
            }
        }
        
    }
}

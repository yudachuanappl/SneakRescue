using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemSettings : MonoBehaviour
{
    public string ItemDescription;
    public bool IsCollectable;
    public InventorySystem.ITEMS itemType;
    public bool IsUsedItem;
    public bool IsInteractiveable;
    public ItemManagaer.INTERACTIVABLE_ITEM_TYPE interactiveType;
    public float DistanceToTrigger = 2;
    private GameObject player;
    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        isActive = false;
    }

    public void HandleInteractWithItem()
    {
        switch(interactiveType)
        {
            case ItemManagaer.INTERACTIVABLE_ITEM_TYPE.CAR:
                GameManager.Instance.ActiveAudio(this.gameObject);
                break;
            case ItemManagaer.INTERACTIVABLE_ITEM_TYPE.TARGET:
                if(SceneManager.GetActiveScene().buildIndex != 0)
                {
                    UserControl.Instance.HandleCarryTarget();
                    ItemManagaer.Instance.ResetText();
                    GameManager.Instance.HandleEnableGameEndTrigger();
                    Destroy(this.gameObject);
                }
                else
                {
                    SceneManager.LoadScene(1);
                }
                break;
            case ItemManagaer.INTERACTIVABLE_ITEM_TYPE.LIGHT:
                this.GetComponent<LightManager>().HandleLightOn();
                GameManager.Instance.ActiveAudio(this.gameObject);
                ItemManagaer.Instance.ResetText();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(this.transform.position, player.transform.position)<= DistanceToTrigger && !isActive && !IsUsedItem)
        {
            isActive = true;
            ItemManagaer.Instance.ShowItemDescription(ItemDescription, IsCollectable, IsInteractiveable,this.gameObject);
        }
        else if(Vector3.Distance(this.transform.position, player.transform.position) > DistanceToTrigger && isActive)
        {
            isActive = false;
            ItemManagaer.Instance.ResetText();
        }
        
        if(isActive && IsUsedItem)
        {
            isActive = false;
            ItemManagaer.Instance.ResetText();
        }
    }
}

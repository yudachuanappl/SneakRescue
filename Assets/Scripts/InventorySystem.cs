using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;
    public const int INVENTORY_NUM = 2;

    public GameObject[] PrefabsForItems;
    public Sprite[] ImagesForItems;
    public GameObject[] InventorySlots;
    public GameObject[] InventoryBackgrounds;
    public int currentActiveInventoryIndex;
    public ArrayList GeneratedItems;
    public Text ItemTip;
    public GameObject BridgeShort;
    public GameObject BridgeLong;
    public GameObject AirWallForBridge;
    private string itemTipText;

    public enum ITEMS
    {
        PHONE,
        WOOD_SHORT,
        WOOD_LONG,
    }

    public ArrayList Inventory;

    private const string originColor = "FFF8AC";

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void RefreshInventoryUI()
    {
        for(int i = 0;i< Inventory.Count; i++)
        {
            InventorySlots[i].GetComponent<Image>().enabled = true;
            InventorySlots[i].GetComponent<Image>().sprite = ImagesForItems[(int)Inventory[i]];
        }
        for(int i = Inventory.Count;i< INVENTORY_NUM;i++)
        {
            InventorySlots[i].GetComponent<Image>().enabled = false;
        }
        for(int i = 0;i<InventoryBackgrounds.Length;i++)
        {
            Color color;
            ColorUtility.TryParseHtmlString(originColor, out color);
            if (i == currentActiveInventoryIndex)
            {
                InventoryBackgrounds[i].GetComponent<Image>().color = Color.green;
            }
            else
            {
                InventoryBackgrounds[i].GetComponent<Image>().color = color;
            }
        }
    }

    private void SetUpItemTipText(ITEMS item, bool isSelected, bool isActivated)
    {
        switch (item)
        {
            case ITEMS.PHONE:
                {
                    if (isSelected && isActivated)
                    {
                        itemTipText = "PHONE IS RINGING! CLICK TO DROP IT...";
                    }
                    else if (isSelected)
                    {
                        itemTipText = "CLICK TO MAKE SOME NOISE...";
                    }
                    else
                    {
                        itemTipText = "THIS IS A PHONE.";
                    }
                }
                break;
            case ITEMS.WOOD_SHORT:
                {
                    if (isSelected && isActivated)
                    {
                        itemTipText = "WOOD IN YOUR HAND";
                    }
                    else if (isSelected)
                    {
                        itemTipText = "TAKE IT OUT FROM BAG";
                    }
                    else
                    {
                        itemTipText = "THIS IS A SHORT WOOD PLANK.";
                    }
                    break;
                }
            case ITEMS.WOOD_LONG:
                {
                    if (isSelected && isActivated)
                    {
                        itemTipText = "WOOD IN YOUR HAND";
                    }
                    else if (isSelected)
                    {
                        itemTipText = "TAKE IT OUT FROM BAG";
                    }
                    else
                    {
                        itemTipText = "THIS IS A LONG WOOD PLANK.";
                    }
                    break;
                }
        }
    }

    public void HandleActivateSlot(int index)
    {
        if (index >= INVENTORY_NUM) return;
        if (index >= Inventory.Count) return;
        //if already activated
        if (currentActiveInventoryIndex == index)
        {
            if(GeneratedItems.Count == 0)
            {
                HandleUseItem((ITEMS)Inventory[index]);
            }
            else
            {
                ITEMS itemToConsume = (ITEMS)Inventory[index];
                if (IsItemConsumable(itemToConsume))
                {
                    currentActiveInventoryIndex = -1;
                    HandleConsumeItemIfAny(itemToConsume);
                    HandleDropActivedItem(itemToConsume);
                }
            }
        }
        else
        {
            currentActiveInventoryIndex = index;
            RefreshInventoryUI();
        }
        OnMouseHover(index);
    }

    public void OnMouseHover(int index)
    {
        if (index >= INVENTORY_NUM) return;
        if (index >= Inventory.Count) return;
        //if already activated
        if (currentActiveInventoryIndex == index)
        {
            if (GeneratedItems.Count == 0)
            {
                SetUpItemTipText((ITEMS)Inventory[index], true, false);
            }
            else
            {
                SetUpItemTipText((ITEMS)Inventory[index], true, true);
            }
        }
        else
        {
            SetUpItemTipText((ITEMS)Inventory[index], false, false);
        }
        ItemTip.text = itemTipText;
    }
    public void OnMouseHoverExit()
    {
        ItemTip.text = "";
    }


    private void SetUpInitialInventory(int level)
    {
        //switch(level)
        //{
        //    case 0:
        //        Inventory.Add(ITEMS.PHONE);
        //        RefreshInventoryUI();
        //        break;
        //}
    }
    // Start is called before the first frame update
    void Start()
    {
        Inventory = new ArrayList();
        GeneratedItems = new ArrayList();
        currentActiveInventoryIndex = -1;
        SetUpInitialInventory(GameManager.Instance.CurrentLevel);
    }

    public bool HandlePickupItem(ITEMS item)
    {
        if (Inventory.Count >= INVENTORY_NUM) return false;
        Inventory.Add(item);
        RefreshInventoryUI();
        return true;
    }

    public bool IsItemConsumable(ITEMS item)
    {
        switch(item)
        {
            case ITEMS.PHONE:
                return true;
            case ITEMS.WOOD_SHORT:
                if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position,BridgeShort.transform.position)<=1.5f)
                {
                    return true;
                }
                break;
            case ITEMS.WOOD_LONG:
                if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, BridgeLong.transform.position) <= 2.5f)
                {
                    return true;
                }
                break;
        }
        return false;
    }
    public bool HandleConsumeItemIfAny(ITEMS item)
    {
        if(Inventory.Contains(item))
        {
            Inventory.Remove(item);
            RefreshInventoryUI();
            return true;
        }
        return false;
    }

    private void HandleDropActivedItem(ITEMS item)
    {
        GameObject activedItem = (GameObject)GeneratedItems[0];
        switch (item)
        {
            case ITEMS.PHONE:
                {
                    activedItem.transform.parent = null;
                    GeneratedItems.RemoveAt(0);
                    break;
                }
            case ITEMS.WOOD_SHORT:
                {
                    Destroy(activedItem);
                    BridgeShort.SetActive(true);
                    GeneratedItems.RemoveAt(0);
                    break;
                }
            case ITEMS.WOOD_LONG:
                {
                    Destroy(activedItem);
                    BridgeLong.SetActive(true);
                    AirWallForBridge.SetActive(false);
                    GeneratedItems.RemoveAt(0);
                    break;
                }
        }
    }

    private void HandleUseItem(ITEMS item)
    {
        switch (item)
        {
            case ITEMS.PHONE:
                {
                    GameObject phone = Instantiate(PrefabsForItems[(int)item]);
                    phone.GetComponent<ItemSettings>().IsUsedItem = true;
                    phone.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
                    phone.transform.localPosition = Vector3.zero;
                    GameManager.Instance.ActiveAudio(phone);
                    GeneratedItems.Add(phone);
                    break;
                }
            case ITEMS.WOOD_SHORT:
            case ITEMS.WOOD_LONG:
                {
                    GameObject wood = Instantiate(PrefabsForItems[(int)item]);
                    wood.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
                    wood.transform.localPosition = GameObject.FindGameObjectWithTag("Player").transform.forward;
                    GeneratedItems.Add(wood);
                    break;
                }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;
        ItemTip.transform.localPosition = mousePos;
    }
}

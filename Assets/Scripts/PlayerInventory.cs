using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerInventory : Inventory
{
    public int m_Coins;
    [Header("UI")]
    public Transform m_ItemsContainer;
    public Button m_OpenButton;
    public Button m_CloseButton;
    public Button m_EquipButton;
    public TMP_Text m_CoinsText;
    public List<ItemSlot> m_Slots; //Slots to sell items in shop

    private Transform m_SellingContainer;
    private List<ItemSlot> m_ItemSlots;

    private void Awake()
    {
        SetCoins(m_Coins);
        m_Slots = new List<ItemSlot>();
        m_ItemSlots = new List<ItemSlot>();
        onInventoryChangedEvent += FillInventory;
        onInventoryChangedEvent += () => { FillSellingInventory(m_SellingContainer); };
        onInventoryChangedEvent += () => { SetSellingButtonsAction(GameManager.instance.m_ShopInventory.AddToSellingCart); };
        SortInventory();
    }

    private void Start()
    {
        FillInventory();
        m_CloseButton.onClick.AddListener(ClosePlayerInventory);
        m_OpenButton.onClick.AddListener(OpenInventory);
        m_EquipButton.onClick.AddListener(GameManager.instance.m_Player.m_Skin.EquipCurrentSkin);
        m_InventoryTransform.gameObject.SetActive(false);
        for (int i = 0; i < m_ItemSlots.Count; i++)
        {
            GameManager.instance.m_Player.m_Skin.m_TempItemSlots[i] = m_ItemSlots[i];
        }
    }
    public ItemSlot GetItemButtonByID(int a_ItemID)
    {
        for (int i = 0; i < m_ItemSlots.Count; i++)
        {
            if (m_ItemSlots[i].m_ItemID == a_ItemID)
                return m_ItemSlots[i];
        }

        return null;
    }
    /// <summary>
    /// Set amount of coins tha player owns
    /// </summary>
    /// <param name="a_Amount"></param>
    public void SetCoins(int a_Amount)
    {
        m_Coins = a_Amount;
        m_CoinsText.text = a_Amount.ToString();
    }
    /// <summary>
    /// Instantiate UI slots with player inventory items
    /// </summary>
    public void FillInventory()
    {
        m_ItemSlots.Clear();
        foreach (Transform t in m_ItemsContainer)
        {
            Destroy(t.gameObject);
        }
        for (int i = 0; i < m_Items.Count; i++)
        {
            ItemSlot _item = Instantiate(m_ItemSlotPrefab, m_ItemsContainer);
            _item.Set(m_Items[i], true, true);
            _item.SetEquipButtonAction();
            m_ItemSlots.Add(_item);
        }
    }
    /// <summary>
    /// Instantiate UI selling slots with player inventory items
    /// </summary>
    public void FillSellingInventory(Transform a_Container)
    {
        if(m_SellingContainer == null)
            m_SellingContainer = a_Container;

        m_Slots.Clear();
        foreach (Transform t in m_SellingContainer)
        {
            Destroy(t.gameObject);
        }
        for (int i = 0; i < m_Items.Count; i++)
        {
            ItemSlot _item = Instantiate(m_ItemSlotPrefab, a_Container);
            _item.Set(m_Items[i], true);
            m_Slots.Add(_item);
        }
    }
    /// <summary>
    /// Set action of item UI buttons
    /// </summary>
    /// <param name="a_Action"></param>
    public void SetSellingButtonsAction(UnityAction<int> a_Action)
    {
        for(int i = 0; i < m_Slots.Count; i++)
        {
            m_Slots[i].SetButtonAction(a_Action);
            m_Slots[i].SetRemoveButtonAction(GameManager.instance.m_ShopInventory.RemoveFromSellingCart);
        }
    }
    /// <summary>
    /// Close inventory window
    /// </summary>
    public void ClosePlayerInventory()
    {
        GameManager.instance.m_Player.m_Skin.RestoreSkin();
        CloseInventory();
        GameManager.instance.m_Player.m_Skin.m_SkinEquiped = false;
        UpdateItemsButtonsState();
    }
    /// <summary>
    /// Update button to new equiped/sold state and restore to default button sprite
    /// </summary>
    public void UpdateItemsButtonsState()
    {
        for (int i = 0; i < m_ItemSlots.Count; i++)
        {
            m_ItemSlots[i].EnableEquipedText(m_Items[i].m_Equiped);
            m_ItemSlots[i].EnableDefaultButtonSprite();
        }
        for (int i = 0; i < m_Slots.Count; i++)
        {
            m_Slots[i].EnableEquipedText(m_Items[i].m_Equiped);
        }
    }
    /// <summary>
    /// Restore all selling item buttons to default
    /// </summary>
    public void SetDefaultSellingButtons()
    {
        for (int i = 0; i < m_Slots.Count; i++)
        {
            m_Slots[i].EnableRemoveButton(false);
        }
    }
}

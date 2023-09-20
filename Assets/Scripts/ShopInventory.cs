using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopInventory : Inventory
{
    public Transform m_BuySlotsContainer;
    public Transform m_SellSlotsContainer;
    public Transform m_BuyViewTransform;
    public Transform m_SellViewTransform;
    public Button m_BuyTabButton;
    public Button m_SellTabButton;
    public Button m_CloseButton;
    public Button m_BuyButton;
    public Button m_SellButton;
    public Button m_CancelButton;
    public Button m_PreviewButton;
    public TMP_Text m_TotalText;

    private List<Item> m_ShoppingCart;
    private List<int> m_ShoppingCartAmounts;
    private List<ItemSlot> m_ItemsSlots;
    private int m_Total;
    private Item m_CurrentSelectedItem;

    private void Awake()
    {
        ResetItemsToDefaultValues();
        m_ShoppingCart = new List<Item>();
        m_ShoppingCartAmounts = new List<int>();
        m_ItemsSlots = new List<ItemSlot>();
    }
    private void Start()
    {
        //Add buttons listeners
        m_CloseButton.onClick.AddListener(CloseShop);
        m_CancelButton.onClick.AddListener(() => { ClearLists(); });
        m_BuyButton.onClick.AddListener(ConfirmPurchase);
        m_SellButton.onClick.AddListener(ConfirmSale);
        m_BuyTabButton.onClick.AddListener(() => {
            m_BuyTabButton.gameObject.GetComponent<Image>().color = Color.gray;
            m_SellTabButton.gameObject.GetComponent<Image>().color = Color.white;
            m_SellTabButton.interactable = true;
            m_BuyTabButton.interactable = false;
        });
        m_SellTabButton.onClick.AddListener(() => {
            m_BuyTabButton.gameObject.GetComponent<Image>().color = Color.white;
            m_SellTabButton.gameObject.GetComponent<Image>().color = Color.gray;
            m_BuyTabButton.interactable = true;
            m_SellTabButton.interactable = false;
        });
        //---------------------
        SortInventory();
        FillInventory(m_BuySlotsContainer, false);
        GameManager.instance.m_Player.m_Inventory.FillSellingInventory(m_SellSlotsContainer);
        GameManager.instance.m_Player.m_Inventory.SetSellingButtonsAction(AddToSellingCart);
        m_InventoryTransform.gameObject.SetActive(false);
        m_BuyViewTransform.gameObject.SetActive(false);
        m_SellViewTransform.gameObject.SetActive(false);
        m_PreviewButton.onClick.AddListener(() => { GameManager.instance.m_Player.m_Skin.SetClothes(m_CurrentSelectedItem); });
    }
    /// <summary>
    /// Instantiate prefab buttons for each item in inventory
    /// </summary>
    /// <param name="a_Container">Traqnsform parent</param>
    /// <param name="a_ForSelling">If its for selling tab or not</param>
    public void FillInventory(Transform a_Container, bool a_ForSelling)
    {
        for (int i = 0; i < m_Items.Count; i++)
        {
            ItemSlot _item = Instantiate(m_ItemSlotPrefab, a_Container);
            _item.Set(m_Items[i], a_ForSelling);
            _item.SetButtonAction(AddToShoppingCart);
            _item.SetRemoveButtonAction(RemoveFromShoppingCart);
            m_ItemsSlots.Add(_item);
        }
    }
    /// <summary>
    /// Update button to new SOLD state and disable remove button
    /// </summary>
    private void UpdateItemsButtonsState(bool a_SellingButtons = false)
    {
        if(a_SellingButtons)
        {
            GameManager.instance.m_Player.m_Inventory.SetDefaultSellingButtons();
        }
        else
        {
            for(int i = 0; i < m_ItemsSlots.Count; i++)
            {
                m_ItemsSlots[i].EnableSoldText(m_Items[i].m_AvailableToBuy);
                m_ItemsSlots[i].EnableRemoveButton(false);
            }
        }
    }
    /// <summary>
    /// Add item to shopping cart list
    /// </summary>
    /// <param name="a_ItemID">ID of the item to add</param>
    public void AddToShoppingCart(int a_ItemID)
    {
        Item _item = GetItem(a_ItemID);
        int _itemIndex = m_ShoppingCart.IndexOf(_item);
        m_CurrentSelectedItem = _item;

        if (!_item.m_AvailableToBuy) return;
        if (m_ShoppingCart.Contains(_item) && m_ShoppingCartAmounts[_itemIndex] >= _item.m_Amount) return;


        if (m_ShoppingCart.Contains(_item))
        {
            m_ShoppingCartAmounts[_itemIndex]++;
        }
        else
        {
            m_ShoppingCart.Add(_item);
            m_ShoppingCartAmounts.Add(1);
        }

        m_Total += _item.m_Data.m_PurchaseCost;
        m_TotalText.text = m_Total.ToString();

        m_ItemsSlots[m_Items.IndexOf(_item)].EnableRemoveButton(true);
    }
    /// <summary>
    /// Remove item from shopping cart list
    /// </summary>
    /// <param name="a_ItemID">ID of the item to add</param>
    public void RemoveFromShoppingCart(int a_ItemID)
    {
        Item _item = GetItem(a_ItemID);
        int _itemIndex = m_ShoppingCart.IndexOf(_item);

        m_ShoppingCartAmounts[_itemIndex]--;

        if (m_ShoppingCartAmounts[_itemIndex] <= 0)
        {
            m_ShoppingCartAmounts.RemoveAt(_itemIndex);
            m_ShoppingCart.Remove(_item);
            m_ItemsSlots[m_Items.IndexOf(_item)].EnableRemoveButton(false);
        }

        m_Total -= _item.m_Data.m_PurchaseCost;
        m_TotalText.text = m_Total.ToString();
    }
    /// <summary>
    /// Add item to selling cart list
    /// </summary>
    /// <param name="a_ItemID">ID of the item to add</param>
    public void AddToSellingCart(int a_ItemID)
    {
        Item _item = GameManager.instance.m_Player.m_Inventory.GetItem(a_ItemID);
        int _itemIndex = m_ShoppingCart.IndexOf(_item);

        if (_item.m_Equiped) return;
        if(m_ShoppingCart.Contains(_item) && m_ShoppingCartAmounts[_itemIndex] >= _item.m_Amount) return;

        if (m_ShoppingCart.Contains(_item))
        {
            m_ShoppingCartAmounts[_itemIndex]++;
        }
        else
        {
            m_ShoppingCart.Add(_item);
            m_ShoppingCartAmounts.Add(1);
        }

        m_Total += _item.m_Data.m_SellingCost;
        m_TotalText.text = m_Total.ToString();

        GameManager.instance.m_Player.m_Inventory.m_Slots[GameManager.instance.m_Player.m_Inventory.m_Items.IndexOf(_item)].EnableRemoveButton(true);
    }
    /// <summary>
    /// Remove item from selling cart list
    /// </summary>
    /// <param name="a_ItemID">ID of the item to add</param>
    public void RemoveFromSellingCart(int a_ItemID)
    {
        Item _item = GetItem(a_ItemID);
        int _itemIndex = m_ShoppingCart.IndexOf(_item);

        m_ShoppingCartAmounts[_itemIndex]--;

        if (m_ShoppingCartAmounts[_itemIndex] <= 0)
        {
            m_ShoppingCartAmounts.RemoveAt(_itemIndex);
            m_ShoppingCart.Remove(_item);
            GameManager.instance.m_Player.m_Inventory.m_Slots[GameManager.instance.m_Player.m_Inventory.m_Items.IndexOf(_item)].EnableRemoveButton(false);
        }

        m_Total -= _item.m_Data.m_PurchaseCost;
        m_TotalText.text = m_Total.ToString();
    }
    /// <summary>
    /// Confirm purchase of items inside shopping cart
    /// </summary>
    public void ConfirmPurchase()
    {
        if (m_Total > GameManager.instance.m_Player.m_Inventory.m_Coins) return;
        for (int i = 0; i < m_ShoppingCart.Count; i++)
        {
            m_ShoppingCart[i].Remove(m_ShoppingCartAmounts[i]);
        }
        GameManager.instance.m_Player.m_Inventory.AddItems(m_ShoppingCart.ToArray(), m_ShoppingCartAmounts.ToArray());
        GameManager.instance.m_Player.ChangeCoins(-m_Total);
        ClearLists();
        onInventoryChangedEvent?.Invoke();
        GameManager.instance.m_Player.m_Inventory.onInventoryChangedEvent?.Invoke();
        UpdateItemsButtonsState();
    }
    /// <summary>
    /// Confirm sale of items inside selling cart
    /// </summary>
    public void ConfirmSale()
    {
        GameManager.instance.m_Player.m_Inventory.RemoveItems(m_ShoppingCart.ToArray(), m_ShoppingCartAmounts.ToArray());
        AddItems(m_ShoppingCart.ToArray(), m_ShoppingCartAmounts.ToArray());
        GameManager.instance.m_Player.ChangeCoins(m_Total);
        ClearLists();
        onInventoryChangedEvent?.Invoke();
        GameManager.instance.m_Player.m_Inventory.onInventoryChangedEvent?.Invoke();
        UpdateItemsButtonsState();
    }
    /// <summary>
    /// Clear shopping cart and total text
    /// </summary>
    public void ClearLists(bool a_SellingButtons = false)
    {
        UpdateItemsButtonsState(a_SellingButtons);
        m_ShoppingCart.Clear();
        m_ShoppingCartAmounts.Clear();
        m_Total = 0;
        m_TotalText.text = "0";
    }
    /// <summary>
    /// Open shop UI window
    /// </summary>
    public void OpenShop()
    {
        m_BuyTabButton.gameObject.GetComponent<Image>().color = Color.gray;
        m_SellTabButton.gameObject.GetComponent<Image>().color = Color.white;
        m_SellTabButton.interactable = true;
        m_BuyTabButton.interactable = false;

        m_BuyViewTransform.gameObject.SetActive(true);
        OpenInventory();
    }
    /// <summary>
    /// Close shop UI window
    /// </summary>
    public void CloseShop()
    {
        ClearLists();
        GameManager.instance.CameraResetZoom();
        GameManager.instance.m_Player.m_Interacting = false;
        GameManager.instance.m_Player.m_Skin.RestoreSkin();
        m_BuyViewTransform.gameObject.SetActive(false);
        m_SellViewTransform.gameObject.SetActive(false);
        CloseInventory();
    }
}

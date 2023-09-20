using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Class of item view for UI
/// </summary>
public class ItemSlot : MonoBehaviour
{
    public int m_ItemID;
    private Image m_ButtonImage;
    private Button m_ButtonItem; //Main button of this gameobject
    
    public Image m_Icon;
    public TMP_Text m_ItemName;
    public TMP_Text m_Cost;
    public TMP_Text m_Amount;
    public TMP_Text m_Sold;
    public TMP_Text m_EquipedText;
    public Sprite m_SpriteDefault; //Sprite to default button state
    public Sprite m_SpriteSelected; //Sprite to selected button state
    public Button m_ButtonRemove;

    private void Awake()
    {
        m_ButtonItem = GetComponent<Button>();
        m_ButtonImage = GetComponent<Image>();
        m_ButtonItem.onClick.AddListener(() => { 
            m_ButtonImage.sprite = m_SpriteSelected;
            GameManager.instance.PlaySound(0);
        });
        m_ButtonRemove.onClick.AddListener(() => {
            GameManager.instance.PlaySound(2);
        });
    }
    /// <summary>
    /// Config properties of the UI of an item
    /// </summary>
    /// <param name="a_Item">Item data</param>
    /// <param name="a_ForSelling">If its or not for selling list</param>
    /// <param name="a_HideCost">If hide cost on UI is necessary</param>
    public void Set(Item a_Item, bool a_ForSelling, bool a_HideCost = false)
    {
        m_ItemID = a_Item.m_Data.m_ItemID;
        m_Icon.sprite = a_Item.m_Data.m_Icon;
        m_ItemName.text = a_Item.m_Data.m_Name;
        m_Cost.text = a_ForSelling ? a_Item.m_Data.m_SellingCost.ToString() : a_Item.m_Data.m_PurchaseCost.ToString();
        
        if (a_HideCost) m_Cost.transform.parent.gameObject.SetActive(false);

        if (!a_ForSelling)
            EnableSoldText(a_Item.m_AvailableToBuy);
        else
            EnableEquipedText(a_Item.m_Equiped);
    }
    /// <summary>
    /// Set action of main button
    /// </summary>
    /// <param name="a_Action"></param>
    public void SetButtonAction(UnityAction<int> a_Action)
    {
        if(m_ButtonItem == null) 
            m_ButtonItem = GetComponent<Button>();

        m_ButtonItem?.onClick.AddListener(() => a_Action(m_ItemID));
    }
    /// <summary>
    /// Set action of remove button
    /// </summary>
    /// <param name="a_Action"></param>
    public void SetRemoveButtonAction(UnityAction<int> a_Action)
    {
        m_ButtonRemove?.onClick.AddListener(() => a_Action(m_ItemID));
        m_ButtonRemove?.onClick.AddListener(EnableDefaultButtonSprite);
    }
    /// <summary>
    /// Set Equip item action on main button
    /// </summary>
    public void SetEquipButtonAction()
    {
        if (m_ButtonItem == null)
            m_ButtonItem = GetComponent<Button>();

        m_ButtonItem?.onClick.AddListener(EquipItem);
    }
    /// <summary>
    /// Equip item to the player
    /// </summary>
    public void EquipItem()
    {
        Player _player = GameManager.instance.m_Player;
        Item _item = _player.m_Inventory.GetItem(m_ItemID);
        _player.m_Skin.SetClothes(_item);
    }
    /// <summary>
    /// Enable/Disable SOLD text on button
    /// </summary>
    /// <param name="a_Enable">Enable/Disable</param>
    public void EnableSoldText(bool a_Enable)
    {
        if (m_ButtonItem == null)
            m_ButtonItem = GetComponent<Button>();

        m_Sold.enabled = a_Enable ? false : true;
        m_ButtonItem.interactable = a_Enable ? true : false;
    }
    /// <summary>
    /// Enable/Disable EQUIP text on button
    /// </summary>
    /// <param name="a_Enable">Enable/Disable</param>
    public void EnableEquipedText(bool a_Enable)
    {
        if (m_ButtonItem == null)
            m_ButtonItem = GetComponent<Button>();

        m_EquipedText.enabled = a_Enable ? true : false;
        m_ButtonItem.interactable = a_Enable ? false : true;
    }
    /// <summary>
    /// Enable/Disable a button that can remove an item from its list
    /// </summary>
    /// <param name="a_Enable"></param>
    public void EnableRemoveButton(bool a_Enable)
    {
        m_ButtonRemove.gameObject.SetActive(a_Enable);
        if (!a_Enable) EnableDefaultButtonSprite();
    }
    /// <summary>
    /// Change main button sprite to default
    /// </summary>
    public void EnableDefaultButtonSprite()
    {
        m_ButtonImage.sprite = m_SpriteDefault;
    }
}

using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Shop NPC controller
/// </summary>
public class NPC : MonoBehaviour
{
    public string m_WelcomeDialogue;
    public GameObject m_DialogueBox;
    public TMP_Text m_DialogueText;
    public GameObject m_InteractionIcon;
    private WaitForSeconds m_DelayToOpenShop = new WaitForSeconds(1f);

    private void Awake()
    {
        m_DialogueBox.SetActive(false);
        m_DialogueText.text = m_WelcomeDialogue;
    }

    public void StartDialogue()
    {
        StartCoroutine(DialogueCoroutine());
    }

    private IEnumerator DialogueCoroutine()
    {
        m_DialogueBox.SetActive(true);
        yield return m_DelayToOpenShop;
        m_DialogueBox.SetActive(false);
        GameManager.instance.m_ShopInventory.OpenShop();
    }

    public void EnableInteractionIcon(bool a_Enable)
    {
        m_InteractionIcon.SetActive(a_Enable);
    }
}

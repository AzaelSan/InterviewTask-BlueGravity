using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player m_Player;
    public ShopInventory m_ShopInventory;
    public AudioSource m_AudioSource;
    public AudioClip[] m_AudioClips;
    private Camera m_Camera;

    private void Awake()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if(instance == null)
            instance = this;
        DontDestroyOnLoad(instance);
        m_Camera = Camera.main;
    }

    public void CameraZoomInToPlayer()
    {
        float _cameraSize = m_Camera.orthographicSize;
        DOTween.To(() => _cameraSize, x => _cameraSize = x, 7, 0.3f)
            .OnUpdate(() => {
                m_Camera.orthographicSize = _cameraSize;
            });
        Vector3 _objective = new Vector3(m_Player.transform.position.x + 9.5f, m_Player.transform.position.y + 2f, -10f);
        m_Camera.transform.DOMove(_objective, 0.5f);
    }

    public void CameraResetZoom()
    {
        float _cameraSize = m_Camera.orthographicSize;
        DOTween.To(() => _cameraSize, x => _cameraSize = x, 12, 0.5f)
            .OnUpdate(() => {
                m_Camera.orthographicSize = _cameraSize;
            });
        Vector3 _objective = new Vector3(0, 0, -10f);
        m_Camera.transform.DOMove(_objective, 0.5f);
    }

    public void PlaySound(int a_AudioIndex)
    {
        m_AudioSource.PlayOneShot(m_AudioClips[a_AudioIndex]);
    }
}

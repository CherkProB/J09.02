using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI : MonoBehaviour
{
    //������ ����������
    private StartScreen startScreen;
    private InGameScreen gameScreen;
    //������� �������� ������
    private UnityEvent<FieldData> createLevelEvent;
    //������� ������ � �����������
    private UnityEvent<Vector3, Container> showPanelAnimation;
    private UnityEvent hidePanelAnimation;

    private UnityEvent<bool> changeModeText;

    //��������
    public StartScreen StartScreen { set { startScreen = value; } }
    public InGameScreen GameScreen { get { return gameScreen; }  set { gameScreen = value; } }
    public UnityEvent<FieldData> CreateLevelEvent { get { return createLevelEvent; } }
    public UnityEvent<Vector3, Container> ShowPanelAnimation { get { return showPanelAnimation; } }
    public UnityEvent HidePanelAnimation { get { return hidePanelAnimation; } }
    public UnityEvent<bool> ChangeModeText { get { return changeModeText; } }


    /// <summary>
    /// ������������� �������� �����
    /// </summary>
    private void Awake()
    {
        createLevelEvent = new UnityEvent<FieldData>();
        showPanelAnimation = new UnityEvent<Vector3, Container>();
        hidePanelAnimation = new UnityEvent();
        changeModeText = new UnityEvent<bool>();
    }

    /// <summary>
    /// ������������� ����� �������� ��������
    /// �������� �� ��������� ��������� �����
    /// </summary>
    private void Start()
    {
        showPanelAnimation.AddListener(ShowInfo);
        hidePanelAnimation.AddListener(HideInfo);
        changeModeText.AddListener(ChangeModeInfo);

        startScreen.gameObject.SetActive(true);
        gameScreen.gameObject.SetActive(false);

        HideInfo();
    }

    /// <summary>
    /// ������� ������� �� ������ "�������"
    /// </summary>
    public void CreateButtonClick()
    {
        if (!startScreen.CheckCorrectData()) 
        {
            startScreen.SetError();
            return;
        }

        createLevelEvent.Invoke(startScreen.GetFieldSize());

        startScreen.gameObject.SetActive(false);
        gameScreen.gameObject.SetActive(true);
    }

    /// <summary>
    /// ����� �������� ��������� ������ ����������
    /// </summary>
    /// <param name="infoPanelPosition">�������, � ������� ������ �������� ������</param>
    /// <param name="container">���������, �� ������� ������������ ����</param>
    public void ShowInfo(Vector3 infoPanelPosition, Container container) 
    {
        if (!container) return;

        string desc = "";
        desc += "������: " + (container.Position.z + 1) + '\n';
        desc += "���: " + (container.Position.x + 1) + '\n';
        desc += "������: " + (container.Position.y + 1);

        StartCoroutine(gameScreen.Panel.ShowInfoCor("���������", desc, infoPanelPosition));
    }

    /// <summary>
    /// ����� ������� ������������ ������ ����������
    /// </summary>
    public void HideInfo() => StartCoroutine(gameScreen.Panel.HideInfoCor());

    private void ChangeModeInfo(bool flag) => gameScreen.ChangeMode(flag);
}

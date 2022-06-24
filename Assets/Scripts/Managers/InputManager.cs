using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    //������
    private Camera cam;
    //������� ������� �� ������
    private UnityEvent<Collider> leftMouseButton;
    private UnityEvent<Collider> rightMouseButton;
    private UnityEvent qButton;
    //������� ��������� � ������������ ������
    private UnityEvent<Vector3, Container> showInfoPanel;
    private UnityEvent hideInfoPanel;
    //�������� ������������� ����
    private Vector3 mousePositionBuffer;
    private float currentIdleTime;
    private bool mouseIdle;
    private float idleTime;

    //��������
    public Camera Camera { set { cam = value; } }
    public UnityEvent<Collider> LeftMouseButton { get { return leftMouseButton; } }
    public UnityEvent<Collider> RightMouseButton { get { return rightMouseButton; } }
    public UnityEvent QButton { get { return qButton; } }
    public UnityEvent<Vector3, Container> ShowInfoPanel { set { showInfoPanel = value; } get { return showInfoPanel; } }
    public UnityEvent HideInfoPanel { set { hideInfoPanel = value; } get { return hideInfoPanel; } }
    public float IdleTime { set { idleTime = value; } }

    /// <summary>
    /// ������������� �������� �����
    /// </summary>
    private void Awake()
    {
        leftMouseButton = new UnityEvent<Collider>();
        rightMouseButton = new UnityEvent<Collider>();
        qButton = new UnityEvent();
        showInfoPanel = new UnityEvent<Vector3, Container>();
        hideInfoPanel = new UnityEvent();

        mousePositionBuffer = Vector3.zero;
        currentIdleTime = 0;
        mouseIdle = false;
    }

    /// <summary>
    /// �������� �� ������� ��� ��� ���� ������
    /// �������� ��������� �� ����
    /// </summary>
    private void Update()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
            leftMouseButton.Invoke(hit.collider);

        if (Input.GetMouseButtonDown(1) && Physics.Raycast(ray, out hit))
            rightMouseButton.Invoke(hit.collider);

        if (Input.GetKeyDown(KeyCode.Q))
            qButton.Invoke();

        //�������� ��������� �� ����
        //������������ ������� ��������� ���� � ���������� ����������� �����
        currentIdleTime = mousePositionBuffer == Input.mousePosition ? currentIdleTime + Time.deltaTime : 0;
        //���� ������ ��������, � ���� ���������
        //�� �������� ������
        if (mouseIdle && currentIdleTime == 0) 
        {
            hideInfoPanel.Invoke();
            mouseIdle = false;
        }
        
        //���� ������ �� ��������, � ���� �� ��������� �������� �����
        //�� �������� ������
        if (!mouseIdle && currentIdleTime > idleTime && Physics.Raycast(ray, out hit))
        {
            showInfoPanel.Invoke(Input.mousePosition, hit.collider.GetComponent<Container>());
            mouseIdle = true;
        }

        //����������� ������� ����
        mousePositionBuffer = Input.mousePosition;
    }
}

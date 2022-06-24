using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneManager : MonoBehaviour
{
    private List<ContainerRow> containerRows;
    private bool inspectorMode;

    //�������� �������
    private UI ui;
    private InputManager inputManager;
    private CameraController cameraController;
    //���������� � ����
    private FieldData fieldData;
    private Transform spawnPoint;
    //� �����������
    private Vector3Int containerSize;
    private Container containerPrefab;
    private ContainerSpot containerSpotPrefab;
    private ContainerRow containerRowPrefab;
    private ContainerFactory containerFactory;
    //��������� ������ � ��������� ���������
    private UnityEvent<bool> changeModeText;

    //��������
    public UI UI { set { ui = value; } }
    public InputManager InputManager { set { inputManager = value; } }
    public ContainerFactory ContainerFactory { set { containerFactory = value; } }
    public Vector3Int ContainerSize { set { containerSize = value; } }
    public Transform SpawnPoint { set { spawnPoint = value; } }
    public CameraController Camera { set { cameraController = value; } }
    public Container ContainerPrefab { set { containerPrefab = value; } }
    public ContainerSpot ContainerSpotPrefab { set { containerSpotPrefab = value; } }
    public ContainerRow ContainerRowPrefab { set { containerRowPrefab = value; } }
    public UnityEvent<bool> ChangeModeText { set { changeModeText = value; } }

    /// <summary>
    /// ������������� �������� �����
    /// </summary>
    private void Awake()
    {
        containerRows = new List<ContainerRow>();
    }

    /// <summary>
    /// ������������� �������� ��������
    /// </summary>
    private void Start()
    {
        ui.CreateLevelEvent.AddListener(CreateLevel);

        inputManager.LeftMouseButton.AddListener(LeftMouseButtonClick);
        inputManager.RightMouseButton.AddListener(RightMouseButtonClick);
        inputManager.QButton.AddListener(ToggleInspectorMode);

        inspectorMode = false;
    }


    /// <summary>
    /// ����� �������� ���� ��� �����������
    /// </summary>
    /// <param name="data">���������� � ���� ��� �����������</param>
    private void CreateLevel(FieldData data)
    {
        //���������� ��������� � �������� ���� ���������� ����������
        fieldData = data;
        //���������� ��������� � �������� ���� ��� ������� �����������
        containerFactory.FieldSize = fieldData;

        //�� �����
        for (int i = 0; i < fieldData.Length; i++)
        {
            //�������� ���� �����������
            Vector3 newContainerRowPos = new Vector3((fieldData.LengthGap + containerSize.x) * i, 0, 0);
            ContainerRow newContainerRow = Instantiate(containerRowPrefab, newContainerRowPos, Quaternion.identity, spawnPoint);

            //�� �������
            for (int j = 0; j < fieldData.Width; j++)
            {
                //�������� ����� ��������� ����������
                Vector3 containerSpotPos = new Vector3((fieldData.LengthGap + containerSize.x) * i, 0.2f, (fieldData.WidthGap + containerSize.z) * j);
                ContainerSpot newContainerSpot = Instantiate(containerSpotPrefab, containerSpotPos, Quaternion.identity, newContainerRow.transform);

                //���������� ����������� ���������� �������� �����
                newContainerSpot.Position = new Vector3Int(i, -1, j);
                newContainerRow.ContainerSpots.Add(newContainerSpot);
            }

            //���������� ���������� ���� � ������
            containerRows.Add(newContainerRow);
        }

        //��������� ������ �������� � ������
        float sceneCenterX = ((float)(fieldData.Length * (containerSize.x + fieldData.LengthGap) - fieldData.LengthGap) - containerSize.x) / 2;
        float sceneCenterY = 1f;
        float sceneCenterZ = ((float)(fieldData.Width * (containerSize.z + fieldData.WidthGap) - fieldData.WidthGap) - containerSize.z) / 2;

        //���������� ����� �������� ������
        Vector3 newCenter = new Vector3(sceneCenterX, sceneCenterY, sceneCenterZ);
        cameraController.SetSceneCenter(newCenter);
    }

    /// <summary>
    /// �����, ���������� ��� ������� ����� ������ ����
    /// </summary>
    /// <param name="obj">��������� �������, �� ������� ������ ����� �������</param>
    private void LeftMouseButtonClick(Collider obj)
    {
        //��� ����������� �� ����� ������ ������
        ContainerSpot spotSender = obj.GetComponent<ContainerSpot>();
        Container containerSender = obj.GetComponent<Container>();

        //�������� �� null
        //���� ������ �� �� �����, �� �� ���������
        if (spotSender == containerSender) return;

        //�������� ��� ����
        foreach (ContainerRow row in containerRows)
            row.DropRow();


        if (inspectorMode) //������� ���
        {
            //��������� �������, �� ������� ������
            Vector3Int newContainerPos = Vector3Int.zero;
            if (spotSender)
                newContainerPos = spotSender.Position;
            else if (containerSender)
                newContainerPos = containerSender.Position;
            else
                return; //error

            //�������� ���� �� ��������� �������
            containerRows[newContainerPos.x].RaiseRow(containerSize, fieldData.Height);
        }
        else //�������� ����������
        {
            //��������� �������, �� ������� ������
            Vector3Int newContainerPos = Vector3Int.zero;
            if (spotSender)
                newContainerPos = spotSender.Position;
            else if (containerSender)
                newContainerPos = containerSender.Position;
            else
                return; //error

            //����� �������
            Container newContainer = containerFactory.CreateContainer(containerPrefab, containerRows[newContainerPos.x].ContainerSpots[newContainerPos.z], newContainerPos);

            //���� ��������� ������
            if (!newContainer) return;

            //���������� ���������� ��������� � ���������� ����� �� ��������� �������
            containerRows[newContainerPos.x].ContainerSpots[newContainerPos.z].Containers.Add(newContainer);
        }
    }

    /// <summary>
    /// �����, ���������� ��� ������� ������ ������ ����
    /// </summary>
    /// <param name="obj">��������� �������, �� ������� ������ ������ �������</param>
    private void RightMouseButtonClick(Collider obj)
    {
        //��� ����������� �� ����� ������ ������
        ContainerSpot spotSender = obj.GetComponent<ContainerSpot>();
        Container containerSender = obj.GetComponent<Container>();

        //�������� �� null
        //���� ������ �� �� �����, �� �� ���������
        if (spotSender == containerSender) return;

        //��������� ��� ��������, ������� ������ ����
        //��� ��������� ���� ��������
        foreach (ContainerRow row in containerRows) 
        {
            row.StopAllCoroutines();
            row.DropRow();
        }

        if (inspectorMode) //�������� ���
        {
            //��������� �������, �� ������� ������
            Vector3Int newContainerPos = Vector3Int.zero;
            if (spotSender)
                newContainerPos = spotSender.Position;
            else if (containerSender)
                newContainerPos = containerSender.Position;
            else
                return; //error

            //�������� ���� �� ��������� �������
            containerRows[newContainerPos.x].DropRow();
        }
        else
        {
            //��������� �������, �� ������� ������
            Vector3Int newContainerPos = Vector3Int.zero;
            if (spotSender)
                newContainerPos = spotSender.Position;
            else if (containerSender)
                newContainerPos = containerSender.Position;
            else
                return; //error

            //�������� ���������� �� ��������� �������
            containerFactory.DeleteContainer(containerRows[newContainerPos.x].ContainerSpots[newContainerPos.z]);
        }
    }

    /// <summary>
    /// ������������ ������ ���������
    /// </summary>
    public void ToggleInspectorMode() 
    {
        inspectorMode = !inspectorMode;
        changeModeText.Invoke(inspectorMode);
    }
}

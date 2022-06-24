using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerSpot : MonoBehaviour
{
    //����������
    private List<Container> containers;
    //������� � ���� � ������
    private Vector3Int position;

    //��������
    public List<Container> Containers { get { return containers; } }
    public Vector3Int Position { get { return position; } set { position = value; } }

    /// <summary>
    /// ������������� �������� �����
    /// </summary>
    private void Awake()
    {
        containers = new List<Container>();
    }

}

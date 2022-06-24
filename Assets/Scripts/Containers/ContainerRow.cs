using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerRow : MonoBehaviour
{
    //����� ��������� �����������
    private List<ContainerSpot> containerSpots;

    //��������
    public List<ContainerSpot> ContainerSpots { get { return containerSpots; } }

    /// <summary>
    /// ������������� �������� �����
    /// </summary>
    private void Awake()
    {
        containerSpots = new List<ContainerSpot>();
    }

    /// <summary>
    /// �������� ����� ���� �����������
    /// </summary>
    /// <param name="containerSize">���������� � �������� ����������. ��� ��������</param>
    /// <param name="maxHeight">���������� � ������������� ���������� �����������. ��� ��������</param>
    public void RaiseRow(Vector3Int containerSize, int maxHeight)
    {
        StopAllCoroutines();

        foreach (ContainerSpot spot in containerSpots)
            foreach (Container container in spot.Containers)
                StartCoroutine(container.Raise(containerSize, maxHeight));
    }

    /// <summary>
    /// ��������� ����� ���� �����������
    /// </summary>
    public void DropRow()
    {
        StopAllCoroutines();

        foreach (ContainerSpot spot in containerSpots)
            foreach (Container container in spot.Containers)
                StartCoroutine(container.Drop());
    }

}

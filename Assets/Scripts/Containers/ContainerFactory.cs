using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerFactory : MonoBehaviour
{
    //���������� �� ��������
    private AnimationCurve easing;
    private float animTime;
    //���������� � ����
    private FieldData fieldSize;
    //���������� � ������� �����������
    private Vector3Int containerSize;

    //��������
    public AnimationCurve Easing { set { easing = value; } }
    public float AnimTime { set { animTime = value; } }
    public FieldData FieldSize { set { fieldSize = value; } }
    public Vector3Int ContainerSize { set { containerSize = value; } }

    /// <summary>
    /// ����� �������� ������ ����������
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="parent"></param>
    /// <param name="senderPos"></param>
    /// <returns></returns>
    public Container CreateContainer(Container prefab, ContainerSpot parent, Vector3Int senderPos)
    {
        //�������� �� �����, ������� ������� ������ �����, ����������� ��������� ����� ���������� � ������
        if (parent.Containers.Count >= fieldSize.Height) return null;

        //��������� ����������, ������� ��������� �� ����� �����
        Container lastContainerInStack = null;
        if (parent.Containers.Count > 0)
            lastContainerInStack = parent.Containers[parent.Containers.Count - 1];

        //�������� ����������
        Container newContainer = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent.transform);

        //���� ���������� ��� ����������
        if (lastContainerInStack)
        {
            newContainer.StartPosition = lastContainerInStack.transform.position + Vector3.up * containerSize.y;
            newContainer.Position = lastContainerInStack.Position + Vector3Int.up;
        }
        else //���� ��� ������ ���������
        {
            newContainer.StartPosition = parent.transform.position;
            newContainer.Position = senderPos + Vector3Int.up;
        }

        //�������� �������
        Outline newContainerOutLine = newContainer.gameObject.AddComponent<Outline>();
        newContainerOutLine.OutlineColor = Color.cyan;
        newContainerOutLine.OutlineWidth = 10;
        newContainerOutLine.OutlineMode = Outline.Mode.OutlineAll;

        //���������� ���� ����������� ����������
        newContainer.Outline = newContainerOutLine;
        newContainer.Outline.enabled = false;
        newContainer.Easing = easing;
        newContainer.AnimationTime = animTime;
        newContainer.transform.position = newContainer.StartPosition;

        return newContainer;
    }

    /// <summary>
    /// �������� ����������
    /// </summary>
    /// <param name="sender">�����, � ������� ���������� ������� ���������</param>
    public void DeleteContainer(ContainerSpot sender)
    {
        //���� ����������� ���
        //�� ������ �������
        if (sender.Containers.Count <= 0) return;
        //����������� ����������, ������� ���������� �������
        Container containerToDelete = sender.Containers[sender.Containers.Count - 1];
        //�������� �� ������ � �� �����
        sender.Containers.Remove(containerToDelete);
        Destroy(containerToDelete.gameObject);
    }
}

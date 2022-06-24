using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    //�����, � ������� ��������� ���������, ����� �� �� �����
    private Vector3 startPosition;
    //������� ���������� � ����, ������ � � ������
    private Vector3Int position;
    //���������� � ��������
    private AnimationCurve easing;
    private float animationTime;
    //
    private Outline outline;

    //��������
    public Vector3 StartPosition { get { return startPosition; } set { startPosition = value; } }
    public Vector3Int Position { get { return position; } set { position = value; } }
    public AnimationCurve Easing { set { easing = value; } }
    public float AnimationTime { set { animationTime = value; } }
    public Outline Outline { get { return outline; } set { outline = value; } }

    /// <summary>
    /// �������� ���������� ���������� � ������
    /// </summary>
    /// <param name="containerSize">������� �����������, ��� ����������� �����, �� ������� ���������� �������</param>
    /// <param name="maxHeight">������������ ���������� ����������� � ������, ��� ���� ����� ������� ��� ���� ������������</param>
    /// <returns></returns>
    public IEnumerator Raise(Vector3Int containerSize, int maxHeight) 
    {
        Vector3 targetPosition = transform.position;
        targetPosition.y =  containerSize.y * (maxHeight + 1) + containerSize.y * 2 * position.y;

        Vector3 activePosition = transform.position;

        for (float i = 0; i < 1; i += Time.deltaTime / animationTime) 
        {
            transform.position = Vector3.Lerp(activePosition, targetPosition, easing.Evaluate(i));
            yield return null;
        }
        transform.position = targetPosition;
    }

    /// <summary>
    /// ��������� ����������, � ����� � ������� �� ������ ���������
    /// � ��������� �����
    /// </summary>
    /// <returns></returns>
    public IEnumerator Drop() 
    {
        if (!this) Debug.Log("sss");

        Vector3 activePosition = transform.position;

        for (float i = 0; i < 1; i += Time.deltaTime / animationTime) 
        {
            transform.position = Vector3.Lerp(activePosition, startPosition, easing.Evaluate(i));
            yield return null;
        }
        transform.position = startPosition;
    }

    private void OnMouseEnter() => outline.enabled = true;
    private void OnMouseExit() => outline.enabled = false;
}

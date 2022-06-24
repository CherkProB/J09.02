using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowInformationPanel : MonoBehaviour
{
    //������ ��������� � ��������
    private Text title;
    private Text description;
    //����� ��������� ������
    private float appearanceTime;

    //��������
    public Text Title { set { title = value; } }
    public Text Description { set { description = value; } }
    public float AppearanceTime { set { appearanceTime = value; } }

    /// <summary>
    /// �������� ��������� ������ ����������
    /// </summary>
    /// <param name="title">��������� ������ ����������</param>
    /// <param name="description">�������� ������ ����������</param>
    /// <param name="mousePos">�������, � ������ ������ ������������� ������</param>
    /// <returns></returns>
    public IEnumerator ShowInfoCor(string title, string description, Vector3 mousePos)
    {
        //��������� ������, � ����� ��������� ��� ����������� ����������
        gameObject.SetActive(true);
        gameObject.transform.position = mousePos + Vector3.down * 50;
        this.title.text = title;
        this.description.text = description;

        //��������� ������� ���� ������ � �� �����
        Image panelBg =  gameObject.GetComponent<Image>();
        Color panelColor = panelBg.color;

        //��������� ������������ ������� ����
        panelColor.a = 0;
        panelBg.color = panelColor;
        for (float i = 0; i < 1; i += Time.deltaTime / appearanceTime)
        {
            panelColor.a = Mathf.Lerp(0, 1, i);
            panelBg.color = panelColor;
            yield return null;
        }
        panelColor.a = 1;
        panelBg.color = panelColor;
    }

    /// <summary>
    /// �������� ������������ ������ ����������
    /// </summary>
    /// <returns></returns>
    public IEnumerator HideInfoCor() 
    {
        Image panelBg = gameObject.GetComponent<Image>();
        Color panelColor = panelBg.color;

        //panelColor.a = 0;
        panelBg.color = panelColor;

        for (float i = panelColor.a; i > 0; i -= Time.deltaTime / appearanceTime * 2)
        {
            panelColor.a = Mathf.Lerp(0, 1, i);
            panelBg.color = panelColor;
            yield return null;
        }

        panelColor.a = 0;
        panelBg.color = panelColor;

        gameObject.SetActive(false);
    }
}

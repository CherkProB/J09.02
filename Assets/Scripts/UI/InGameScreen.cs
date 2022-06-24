using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameScreen : MonoBehaviour
{
    //���������� � ������
    private ShowInformationPanel panel;
    //
    private Text modeText;
    //��������
    public ShowInformationPanel Panel { get { return panel; } set { panel = value; } }
    public Text ModeText { get { return modeText; } set { modeText = value; } }

    /// <summary>
    /// ������� ������ ������
    /// </summary>
    /// <param name="flag">��������� ������ ���������</param>
    public void ChangeMode(bool flag) => modeText.text = flag ? "��������" : "��������/��������";
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogChange : MonoBehaviour
{
    public TextMeshProUGUI textCharacter;
    public TextMeshProUGUI textContent;

    private int currentNum;

    private string[] textLibrary = new string[5];

    // Start is called before the first frame update
    void Start()
    {
        textLibrary[0] = "Привет! Для переключения диалога нажми - F";
        textLibrary[1] = "Дорогая Лена";
        textLibrary[2] = "Я тебя";
        textLibrary[3] = "Очень люблю)";
        textLibrary[4] = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentNum < textLibrary.Length - 1)
        {
            currentNum++;
            textContent.text = textLibrary[currentNum];
        }
    }
}

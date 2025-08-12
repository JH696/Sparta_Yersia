using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
//    public GameObject go;

//    public TextMeshProUGUI titleText;
//    public TextMeshProUGUI expText;
//    public TextMeshProUGUI ypText;

//    public List<Image> icons;
//    public List<TextMeshProUGUI> stacks;

//    public Button okButton;

//    private void Awake()
//    {
//        B_Manager.Instance.SetBattleUI(this);
//    }

//    private void Start()
//    {
//        okButton.onClick.AddListener(OnOKButton);
//    }

//    private void OnDisable()
//    {
//        okButton.onClick.RemoveAllListeners();
//    }

//    public void DisplayWinUI(int exp, int yp, List<ItemData> itemDatas)
//    {
//        go.SetActive(true);

//        titleText.text = "전투 승리!";
//        expText.text = $"획득 경험치: {exp}";
//        ypText.text = $"획득 YP: {yp}";

//        // 아이템 수량 집계용 딕셔너리
//        Dictionary<ItemData, int> itemCounts = new Dictionary<ItemData, int>();

//        foreach (var item in itemDatas)
//        {
//            if (item == null) continue;

//            if (itemCounts.ContainsKey(item))
//                itemCounts[item]++;
//            else
//                itemCounts[item] = 1;
//        }

//        int index = 0;
//        foreach (var pair in itemCounts)
//        {
//            if (index >= icons.Count || index >= stacks.Count) break;

//            icons[index].sprite = pair.Key.Icon;
//            stacks[index].text = $"{pair.Value}";
//            index++;
//        }
//    }

//    public void DisplayLoseUI()
//    {
//        Debug.Log("디스플레이 유아이");

//        go.SetActive(true);

//        titleText.text = "전투 패배..";
//        expText.text = $"획득 경험치: 0";
//        ypText.text = $"획득 YP: 0";
//    }

//    public void OnOKButton()
//    {
//        SceneManager.LoadSceneAsync("Scene_BJH");
//    }
}

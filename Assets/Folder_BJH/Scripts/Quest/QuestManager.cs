using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("퀘스트 UI")]
    [SerializeField] public QuestUI questUI;

    [Header("현재 스토리 진행 단계")]
    [SerializeField] private int stortStage = 1;

    [Header("수락 가능한 퀘스트 목록")]
    [SerializeField] private List<QuestData> AvailableQuests;

    public event System.Action QuestUpdate;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        NextStoryQuestUnlock();
    }

    public void SetQuestUI(QuestUI questUI)
    {
        this.questUI = questUI;
    }

    // 현재 수락 가능한 퀘스트 목록 반환
    public List<QuestData> GetAvailableQuests()
    {
        return AvailableQuests;
    }

    // 퀘스트 획득
    public void GetQuest(QuestData questData)
    {
        if (questData == null || !AvailableQuests.Contains(questData)) return;

        AvailableQuests.Remove(questData);
        GameManager.Instance.Player.GetComponent<PlayerQuest>().AddMyQ(questData);
        questUI.RefreshQuestUI();
        QuestUpdate?.Invoke();
    }

    // 퀘스트 완료 
    public void QuestClear(QuestData questData)
    {
        if (questData == null) return;

        if (questData.QuestType == EQuestType.Story)
        {
            NextStoryQuestUnlock();
        }

        GameManager.Instance.Player.GetComponent<PlayerQuest>().RemoveMyQ(questData);
        SubmitQItems(questData);
        GetQRawards(questData);
        questUI.RefreshQuestUI();
        QuestUpdate?.Invoke();

        Debug.Log($"퀘스트 완료: {questData.QuestName}");
    }

    // 퀘스트 아이템 제출 (퀘스트 완료시 호출)
    private void SubmitQItems(QuestData questData)
    {
        foreach (var item in questData.TargetItem)
        {
            GameManager.Instance.Player.GetComponent<PlayerInventory>().RemoveItem(item.ItemData, item.ItemCount);
        }
    }

    // 퀘스트 보상 획득
    private void GetQRawards(QuestData questData)
    {
        foreach (ItemData item in questData.RewardItems)
        {
            GameManager.Instance.Player.GetComponent<PlayerInventory>().AddItem(item, 1);
        }
        
        foreach (PetData pet in questData.RewardPets)
        {
            Debug.Log($"펫 획득: {pet.PetName}");
            //TestPlayer.Instance.playerQuest.AddQuestItem(pet); 
        }

        GameManager.Instance.Player.GetComponent<Player>().AddExp(questData.RewardExp);
        GameManager.Instance.Player.GetComponent<Player>().AddYP(questData.RewardYP);
    }

    // 퀘스트 해금
    private void QuestUnlock(string id)
    {
        QuestData quest = Resources.Load<QuestData>($"QuestDatas/{id}");

        if (quest == null)
        { 
            Debug.LogError($"Quest Manager: 해당 퀘스트는 존재하지 않습니다.");
            return;
        }

        AvailableQuests.Add(quest);
        QuestUpdate?.Invoke();
    }

    // 다음 스토리 퀘스트 해금
    private void NextStoryQuestUnlock()
    {
        QuestData nextQuest = Resources.Load<QuestData>($"QuestDatas/Q_s{stortStage:D3}");
        stortStage++;

        if (nextQuest == null)
        {
            Debug.Log($"Quest Manager: 다음 스토리 퀘스트가 존재하지 않습니다.");
            return;
        }

        Debug.Log($"Quest Manager: 다음 스토리 퀘스트 해금: {nextQuest.QuestName}");  
        AvailableQuests.Add(nextQuest);
        QuestUpdate?.Invoke();
    }
}
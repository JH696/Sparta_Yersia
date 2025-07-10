using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class B_PartyUI : MonoBehaviour
{
    [Header("캐릭터")]
    [SerializeField] private B_Characters chars;

    [Header("프로필 프리팹")]
    [SerializeField] private GameObject prefab;

    [Header("프리팹 생성 위치")]
    [SerializeField] private Transform trans;

    [Header("생성된 프리팹")]
    [SerializeField] private List<B_ProfilePrefab> profiles;


    private void Start()
    {
        SetPartyProfile();
        RefreshProfiles();
    }

    public void SetPartyProfile()
    {
        List<B_CharacterSlot> slots = chars.Slots.FindAll(B_CharacterSlot => B_CharacterSlot.Type == ECharacterType.Ally);
        List<B_CharacterSlot> ally = slots.FindAll(B_CharacterSlot => B_CharacterSlot.Character != null);

        foreach (var a in ally)
        {
            GameObject obj = Instantiate(prefab, trans);
            B_ProfilePrefab partyPrefab = obj.GetComponent<B_ProfilePrefab>();

            profiles.Add(partyPrefab);

            partyPrefab.SetProfile(a.Character);
        }
    }

    public void RefreshProfiles()
    {
        foreach (var p in profiles)
        {
            p.RefreshGauge();
        }
    }
}

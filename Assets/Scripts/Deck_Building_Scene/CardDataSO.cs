using UnityEngine;
using System.Collections.Generic;

public enum CardType { Unit, Building, Spell }
public enum EffectTrigger { NoEffect, OnSummon, OnTurnStart, OnAttack, OnDeath }
public enum EffectType { NoEffect, DrawCard, GainCost, Damage, Heal, Buff, Debuff }

[CreateAssetMenu(menuName = "CardGame/CardData", fileName = "NewCardData")]
public class CardDataSO : ScriptableObject
{
    [Header("Identification")]
    public string cardName;
    public Sprite illustration;

    [Header("Core Params")]
    //카드의 종류 유닛, 스펠, 건물
    public CardType cardType;
    //카드를 배치할 수 있는 열의 위치
    public int placementRow;

    
    [Header("Unit Ranges (only for Unit type)")]
    //유닛의 이동범위 및 공격 범위 패러미터
    //2차원 리스트로 관리
    public List<Vector2Int> attackRange = new List<Vector2Int>();
    public List<Vector2Int> moveRange = new List<Vector2Int>();


    [Header("Effects (Building/Spell)")]
    //카드의 효과가 발동하는 트리거(카드 소환시, 플레이어의 턴이 시작할때, 공격시, 유닛 사망시)
    public EffectTrigger effectTrigger;
    //카드 효과의 타입(카드를 뽑는다. 비용을 회복한다. 피해를 준다. 회복한다. 버프를 준다. 디버프를 준다. 등)
    public EffectType effectType;
    //효과의 벨류값
    public int effectValue;
    //효과의 공격범위
    public List<Vector2Int> effectRange = new List<Vector2Int>();

    //공격력, 내구력(체력), 비용
    public int attack, durability, cost;

    //카드 설명 및 설정 텍스트.
    [TextArea] public string effectDescription;
    [TextArea] public string loreDescription;
}
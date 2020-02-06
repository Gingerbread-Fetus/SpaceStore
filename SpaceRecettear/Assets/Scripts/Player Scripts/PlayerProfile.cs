using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player profile holds some basic information about the player.
/// Level, customers sold to, current experience, stamina/max stamina, etc
/// </summary>
[CreateAssetMenu(menuName = "Player/Profile", fileName = "PlayerProfile.asset")]
[System.Serializable]
public class PlayerProfile : ScriptableObject
{
    [SerializeField] string playerName;
    [SerializeField] int stamina;
    [SerializeField] int experience;
    [SerializeField] int level;
    [SerializeField, Range(0,4)] int difficultyLevel;
    [SerializeField] List<Ability> abilities;

    public List<Ability> Abilities => abilities;

    public void SetName(string newName)
    {
        playerName = newName;
    }

    public void SetDifficulty(int newDifficulty)
    {
        if(newDifficulty >= 0 && newDifficulty <= 4)
        {
            difficultyLevel = newDifficulty;
        }
    }

}

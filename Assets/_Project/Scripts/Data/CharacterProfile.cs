using UnityEngine;
using System.Collections.Generic;

namespace TheCommunityFestival.Data
{
    /// <summary>
    /// ScriptableObject defining a character's permanent traits
    /// </summary>
    [CreateAssetMenu(fileName = "Character_", menuName = "Community/Character Profile")]
    public class CharacterProfile : ScriptableObject
    {
        [Header("Identity")]
        public string characterName;
        [TextArea(2, 4)]
        public string bio;
        public CharacterRole role;
        public Sprite portrait;

        [Header("Personality")]
        [Range(0f, 1f)] public float friendliness = 0.7f; // How approachable
        [Range(0f, 1f)] public float energy = 0.6f; // How active
        [Range(0f, 1f)] public float creativity = 0.5f; // How artistic
        [Range(0f, 1f)] public float helpfulness = 0.8f; // How much they help
        [Range(0f, 1f)] public float wisdom = 0.5f; // How much they teach

        [Header("Preferences")]
        public MusicGenre favoriteMusic;
        public List<string> interests = new List<string>();
        public List<string> skills = new List<string>();

        [Header("Dialogue")]
        public List<string> greetings = new List<string>();
        public List<string> helpOffers = new List<string>();
        public List<string> teachings = new List<string>();
        public List<string> encouragements = new List<string>();

        [Header("Visual")]
        public GameObject characterPrefab;
        public Color accentColor = Color.white;
    }

    public enum CharacterRole
    {
        FestivalGoer,
        Volunteer,
        Artist,
        Organizer,
        Helper, // Special guides
        Newcomer
    }

    public enum MusicGenre
    {
        Techno,
        House,
        Bass,
        Breakbeat,
        Downtempo,
        Ambient,
        AllGenres
    }
}


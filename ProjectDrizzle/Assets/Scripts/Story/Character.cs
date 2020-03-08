using UnityEngine;

namespace Story
{
    [CreateAssetMenu(fileName = "Character", menuName = "Story/Character", order = 0)]
    public class Character : ScriptableObject
    {
        public string Name;
    }
}
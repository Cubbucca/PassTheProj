using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Ability")]
public class ScriptableAbility : ScriptableObject {
    public string Name;
    public AbilityType Type;
    public int Cost;
    public GameObject Icon;
    public KeyCode Key; // This is gross, but it's quick and I'm running out of time. Rush Taro!
    //Saul here! - Not sure what is wrong with the above, and don't know a better way. Sorry ;-;
    // Sam: I think the issue is that we are using the new input system and we should be making an action
    // asset instead of using a keycode
}

[Serializable]
public enum AbilityType {
    None = 0,
    Ram = 1,
    Blast = 2
}

using UnityEngine;

public class HitField : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Minigame_Circular.instance.toggleBool(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        Minigame_Circular.instance.toggleBool(false);
    }


}

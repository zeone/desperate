using UnityEngine;
/// <summary>
/// Класс получает объект для масового взрыва Fireblast
/// </summary>
public class Plazma 
{

    /// <summary>
    /// находим первый попавшийся файербласт и активируем его
    /// </summary>
    public static bool EnableFireblast()
    {
        foreach (Transform o in Player.entity.transform)
        {
            if (o.tag == "Fireblast" && !o.gameObject.active)
            {
                o.gameObject.SetActive(true);
                return true;
            }
        }
        return false;
    }

}
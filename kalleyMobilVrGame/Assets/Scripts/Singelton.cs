using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<T>();
        }
        else if (instance != FindObjectOfType<T>())
        {
            Destroy(FindObjectOfType<T>());
        }
        DontDestroyOnLoad(instance.gameObject);
        return instance;
    }
}
// de este singleton generico, heredan el resto de singletons del proyecto
// este singleton permite: 
// guardar la instancia de si mismo 
// acceder a si mismo por medio de un metodo estatico (puede ser llamado sin crear una instancia de la clase) y encuentra la instancia usando finobjectofType
// Si encuentra otro script que tenga el mismo tipo de singleton, lo destruye para asegurar que halla solo este script en el proyecto
// por ultimo permite usar los singletons en todas las escenas, y por lo tanto estos guardan sus parametros valores etc
// permite hacer un debug y un cambio del codigo mucho mas eficiente

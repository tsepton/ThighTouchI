
using System.Collections;
using UnityEngine;

public class LockSphere1 : MonoBehaviour
{
    public bool isGreen = false; // Variable pour déterminer si la sphère est verte

    private Material originalMaterial; // Matériau d'origine de la sphère
    public Material greenMaterial; // Matériau pour la sphère verte

    void Start()
    {
        Debug.Log("test1");
        // Conserver le matériau d'origine
        originalMaterial = GetComponent<Renderer>().material;
    }
    private void OnDisable(){
            isGreen = false;
            GetComponent<Renderer>().material = originalMaterial;
        
    }

    void OnTriggerEnter(Collider other)
    {
        
        // Si la sphère entre en collision avec un autre objet
        // on inverse l'état de la variable isGreen
        isGreen = true;

        // Changer le matériau en conséquence
        if (isGreen)
        {
            GetComponent<Renderer>().material = greenMaterial;
        }
        
    }
}

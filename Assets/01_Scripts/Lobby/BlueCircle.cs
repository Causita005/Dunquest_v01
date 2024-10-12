using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // Importa para manejar TextMeshPro

public class BlueCircle : MonoBehaviour
{
    public ParticleSystem particulas;
    public TextMeshProUGUI mensajeTexto;  // Usar TextMeshPro en lugar de Text
    private bool playerInCircle = false;

    private void Start()
    {
        particulas.Stop();
        mensajeTexto.gameObject.SetActive(false);  // Oculta el texto al inicio
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            particulas.Play();  // Activa las partículas
            playerInCircle = true;
            mensajeTexto.gameObject.SetActive(true);  // Muestra el texto
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            particulas.Stop();
            playerInCircle = false;
            mensajeTexto.gameObject.SetActive(false);  // Oculta el texto
        }
    }

    private void Update()
    {
        if (playerInCircle && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("JapaneseMap");  // Carga la escena "JapaneseMap"
        }
    }
}

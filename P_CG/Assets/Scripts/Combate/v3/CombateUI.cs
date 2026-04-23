using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombateUI : MonoBehaviour
{
    public static CombateUI Instance;

    [Header("Panel Ataques")]
    public GameObject panelAtaques;
    public Button[] botonesAtaque;
    public TextMeshProUGUI[] textosAtaque;

    [Header("Panel Resultados")]
    public GameObject panelResultados;
    public TextMeshProUGUI textoResultados;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void MostrarAtaques(CharacterData data, System.Action<int> onAtaqueElegido)
    {
        panelAtaques.SetActive(true);
        foreach (var btn in botonesAtaque) btn.gameObject.SetActive(false);

        int cantidad = Mathf.Min(data.ataques.Length, botonesAtaque.Length);
        for (int i = 0; i < cantidad; i++)
        {
            int indice = i;
            botonesAtaque[i].gameObject.SetActive(true);

            // Construir descripción de dados: "2d6 + 1d4" etc.
            string dadosDesc = "";
            foreach (var dado in data.ataques[i].dados)
            {
                if (dadosDesc != "") dadosDesc += " + ";
                dadosDesc += $"{dado.cantidadDados}d{dado.caras}";
            }
            textosAtaque[i].text = $"{data.ataques[i].nombreAtaque}\n<size=70%>{dadosDesc}</size>";

            botonesAtaque[i].onClick.RemoveAllListeners();
            botonesAtaque[i].onClick.AddListener(() => {
                OcultarAtaques();
                onAtaqueElegido(indice);
            });
        }
    }

    public void OcultarAtaques()
    {
        panelAtaques.SetActive(false);
    }

    public void MostrarResultado(string mensaje)
    {
        panelResultados.SetActive(true);
        textoResultados.text = mensaje;
    }

    public void OcultarResultado()
    {
        panelResultados.SetActive(false);
    }
}
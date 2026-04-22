using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LanzadorDados : MonoBehaviour
{
    public int LanzarD4()
    {
        return Random.Range(1, 5);
    }

    public int LanzarD6()
    {
        return Random.Range(1, 7);
    }

    public int LanzarD8()
    {
        return Random.Range(1, 9);
    }

    public int LanzarD10()
    {
        return Random.Range(1, 11);
    }

    // Tirada combinada: primer D10 son decenas, segundo D10 son unidades
    // Ejemplo: 6 y 3 → 63
    public int TiradaCombinada()
    {
        int primerDado = LanzarD10();
        int segundoDado = LanzarD10();
        int combinado = (primerDado * 10) + segundoDado;

        Debug.Log("    Tirada combinada: " + primerDado + " y " + segundoDado + " = " + combinado);
        return combinado;
    }

    // Tirada de éxito: 1-3 pierde el turno, 4-10 puede atacar
    public bool TiradaDeExito()
    {
        int resultado = LanzarD10();
        Debug.Log("    Tirada de éxito: " + resultado);

        if (resultado <= 3)
        {
            Debug.Log("    → Error. Pierde el turno.");
            return false;
        }

        Debug.Log("    → Éxito. Puede atacar.");
        return true;
    }

    // Evalúa el resultado combinado y devuelve "pifia", "danio" o "fallo"
    public string EvaluarTiradaDanio(int resultadoCombinado)
    {
        if (resultadoCombinado >= 90)
        {
            Debug.Log("    → PIFIA (90+). Golpe crítico fallido. No hace daño.");
            return "pifia";
        }
        else if (resultadoCombinado >= 50)
        {
            Debug.Log("    → DAÑO (50-89). El ataque conecta.");
            return "danio";
        }
        else
        {
            Debug.Log("    → FALLO (menos de 50). El ataque no conecta.");
            return "fallo";
        }
    }

    // Lanza los dados según la fórmula propia del personaje
    // Ejemplos de fórmula: "1D10+1D4", "2D6", "1D10+2D4", "1D6+1D8"
    // Devuelve el total de los dados lanzados
    public int LanzarFormula(string formula)
    {
        int total = 0;
        string formulaLimpia = formula.Replace(" ", "").ToUpper();

        // Separar por el signo + para obtener cada parte
        string[] partes = formulaLimpia.Split('+');

        foreach (string parte in partes)
        {
            // Cada parte tiene formato "NúmeroDTipoDado", ejemplo: "1D10", "2D6", "1D4"
            string[] separado = parte.Split('D');

            if (separado.Length != 2)
            {
                Debug.Log("    Fórmula no reconocida: " + parte);
                continue;
            }

            int cantidadDados = int.Parse(separado[0]);
            int tipoDado = int.Parse(separado[1]);

            for (int i = 0; i < cantidadDados; i++)
            {
                int resultado = 0;

                if (tipoDado == 4) resultado = LanzarD4();
                else if (tipoDado == 6) resultado = LanzarD6();
                else if (tipoDado == 8) resultado = LanzarD8();
                else if (tipoDado == 10) resultado = LanzarD10();
                else
                {
                    Debug.Log("    Tipo de dado no reconocido: D" + tipoDado);
                    continue;
                }

                Debug.Log("    " + parte + " → D" + tipoDado + " sacó: " + resultado);
                total += resultado;
            }
        }

        Debug.Log("    Total fórmula " + formula + ": " + total);
        return total;
    }
}
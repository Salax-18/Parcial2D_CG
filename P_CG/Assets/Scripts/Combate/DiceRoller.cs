using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public static class DiceRoller
{
    public static int Lanzar(int caras)
    {
        return Random.Range(1, caras + 1);
    }

    public static int D4() { return Lanzar(4); }
    public static int D6() { return Lanzar(6); }
    public static int D8() { return Lanzar(8); }
    public static int D10() { return Lanzar(10); }


    public static int TiradaDeDaño()
    {
        int primerDado = D10();
        int segundoDado = D10();
        int resultado = (primerDado * 10) + segundoDado;
        return resultado;
    }


    public static string EvaluarDaño(int resultado)
    {
        if (resultado >= 90) return "Pifia";
        if (resultado >= 50) return "daño";
        return "fallo";
    }
}
using UnityEngine;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class BigNumber {



    string[] cast = new string[] {
            "",
            "K",
            "M",
            "B",
            "T",
            "aa",
            "bb",
            "cc",
            "dd",
            "ee",
            "ff",
            "gg",
            "hh",
            "ii",
            "jj",
            "kk",
            "ll",
            "mm",
            "nn",
            "oo",
            "pp",
            "qq",
            "rr",
            "ss",
            "tt",
            "uu",
            "vv",
            "xx",
            "ww",
            "yy",
            "zz",
            "AAA",
            "BBB",
            "CCC",
            "DDD",
            "EEE",
            "FFF",
            "GGG",
            "HHH",
            "III",
            "JJJ",
            "KKK",
            "LLL",
            "MMM",
            "NNN",
            "OOO",
            "PPP",
            "QQQ",
            "RRR",
            "SSS",
            "TTT",
            "UUU",
            "VV",
            "XXX",
            "WWW",
            "YYY",
            "ZZZ",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE",
            "IDE"
        };

    public BigNumber() { }


    public static BigNumber getInstance() { return new BigNumber(); }

    public String format(string valor, int index)
    {
        bool negativa = valor.Contains("-");
        valor = Regex.Replace(valor, "[A-Za-z -]", ""); //tira todas as letras pois ja tenho o index

        int dotPosition = valor.IndexOf(".");
        if(dotPosition <= 0) { dotPosition = valor.Length;  }
        string integerDoValor = valor.Substring(0, dotPosition);

        double indexCalc = (double)integerDoValor.Length / 3;
        indexCalc = indexCalc + index;
        valor.Replace(".", "");

        if (integerDoValor.Length == 4)
        {
            if (negativa) {  return "-"+valor.Substring(0, 1) + "." + valor.Substring(1, 2) + cast[(int)indexCalc]; }
            else { return valor.Substring(0, 1) + "." + valor.Substring(1, 2) + cast[(int)indexCalc]; }
        }else if(integerDoValor.Length > 4)
        {
            int l = (integerDoValor.Length % 3 == 0) ? 3 : integerDoValor.Length % 3 ;
            return valor.Substring(0, l)+"."+ valor.Substring(l, 2)+ cast[(int)indexCalc-1];
        }
        else
        {
            if (negativa)  { return "-"+valor + cast[index]; }
            else { return valor + cast[index]; }
        }
    }

    /*

    public bool isLower(BigNumber number)
    {
        if(getIndex(valor) < getIndex(number.valor)) { return true; }
        else if(getIndex(valor) == getIndex(number.valor))
        {
            double tmp;
            double tmp2;
            Double.TryParse(Regex.Replace(valor, "[A-Za-z ]", ""), out tmp);
            Double.TryParse(Regex.Replace(number.valor, "[A-Za-z ]", ""), out tmp2);
            if(tmp < tmp2) { return true; }
            return false;
        }
        return false;
    }

    public static BigNumber somar(string numero1, string numero2)
    {

        double temp1, temp2, resultado;
        int index1=0, index2=0, indexHigher;

        if(Double.TryParse(numero1, out temp1) && numero1.Length <= 3 && Double.TryParse(numero2, out temp2) && numero2.Length <= 3)
        {
            return new BigNumber(""+(temp1+temp2));
        }else
        {
            index1 = getInstance().getIndex(numero1);
            index2 = getInstance().getIndex(numero2);

            numero1 = Regex.Replace(numero1, "[A-Za-z ]", "");
            numero2 = Regex.Replace(numero2, "[A-Za-z ]", "");


            Double.TryParse(numero1, out temp1);
            Double.TryParse(numero2, out temp2);

            if (index1 > index2) { indexHigher = index1; temp2 = temp2 / Math.Pow(10D, ((index1-index2)*3)); }
            else if(index2 > index1) { indexHigher = index2; temp1 = temp1 / Math.Pow(10D, ((index2 - index1) * 3)); }
            else { indexHigher = index1; }

            resultado = temp1 + temp2;
            BigNumber retorno = new BigNumber(String.Format("{0:0.00}", resultado) + getInstance().cast[indexHigher]);
            return retorno;

        }
    }

    public static BigNumber subtrair(string numero1, string numero2)
    {

        double temp1, temp2, resultado;
        int index1 = 0, index2 = 0, indexHigher;

        if (Double.TryParse(numero1, out temp1) && numero1.Length <= 3 && Double.TryParse(numero2, out temp2) && numero2.Length <= 3)
        {
            return new BigNumber("" + (temp1 - temp2));
        }
        else
        {
            index1 = getInstance().getIndex(numero1);
            index2 = getInstance().getIndex(numero2);

            numero1 = Regex.Replace(numero1, "[A-Za-z ]", "");
            numero2 = Regex.Replace(numero2, "[A-Za-z ]", "");


            Double.TryParse(numero1, out temp1);
            Double.TryParse(numero2, out temp2);

            if (index1 > index2) { indexHigher = index1; temp2 = temp2 / Math.Pow(10D, ((index1 - index2) * 3)); }
            else if (index2 > index1) { indexHigher = index2; temp1 = temp1 / Math.Pow(10D, ((index2 - index1) * 3)); }
            else { indexHigher = index1; }

            resultado = temp1 - temp2;
            if(resultado == 0) { return new BigNumber("0"); }
            if(Math.Abs(resultado) < 1) { indexHigher--; resultado *= 1000; }
            BigNumber retorno = new BigNumber(resultado + "" + getInstance().cast[indexHigher]);
            return retorno;
        }
    }

    public static BigNumber multiplicar(string numero1, string numero2)
    {
        double temp1, temp2, resultado;
        int index1 = 0, index2 = 0, indexHigher;

        if (Double.TryParse(numero1, out temp1) && Double.TryParse(numero2, out temp2))
        {
            return new BigNumber("" + (temp1 * temp2));
        }
        else
        {
            index1 = getInstance().getIndex(numero1);
            index2 = getInstance().getIndex(numero2);

            numero1 = Regex.Replace(numero1, "[A-Za-z ]", "");
            numero2 = Regex.Replace(numero2, "[A-Za-z ]", "");

            Double.TryParse(numero1, out temp1);
            Double.TryParse(numero2, out temp2);

            if (index1 == 0) { indexHigher = index2; }
            else if (index2 == 0) { indexHigher = index1; }
            else
            {
                indexHigher = (index1 * index2) + 1;
            }

            resultado = temp1 * temp2;
            BigNumber retorno = new BigNumber(String.Format("{0:0.00}", resultado) + getInstance().cast[indexHigher]);
            return retorno;

        }
    }

    public static BigNumber dividir(string numero1, string numero2)
    {
        double temp1, temp2, resultado;
        int index1 = 0, index2 = 0, indexHigher;

        if (Double.TryParse(numero1, out temp1) && Double.TryParse(numero2, out temp2))
        {
            return new BigNumber("" + (temp1 / temp2));
        }
        else
        {
            index1 = getInstance().getIndex(numero1); //cc 7
            index2 = getInstance().getIndex(numero2); //0

            numero1 = Regex.Replace(numero1, "[A-Za-z ]", "");
            numero2 = Regex.Replace(numero2, "[A-Za-z ]", "");

            Double.TryParse(numero1, out temp1);
            Double.TryParse(numero2, out temp2);

            if (index1 > 0 && index2 > 0) { indexHigher = index1 - index2; }
            else
            {
                if (index1 == 0 && index2 == 0) { indexHigher = 0; }
                else { indexHigher = index1 - index2 - 1; }
            }
            if (indexHigher < 0) { indexHigher = 0; }

            string difference = Math.Pow(10D, ((index1 - index2) * 3)).ToString("f0");

            temp1 *= Double.Parse(difference);
            
            resultado = temp1 / temp2;
            Debug.Log(resultado.ToString("f0") + " divido por " + Double.Parse(difference).ToString("f0"));
            resultado = resultado / (Double.Parse(difference) / temp2)*10;
            Debug.Log(resultado.ToString("f0"));


            BigNumber retorno = new BigNumber(resultado.ToString("f0") + getInstance().cast[indexHigher]);
            
            return retorno;

        }
    }

    public static BigNumber potencia(string numero1, string numero2)
    {
        double temp1, temp2;

        if (Double.TryParse(numero1, out temp1) && Double.TryParse(numero2, out temp2))
        {
            string tmp = Math.Pow(temp1, temp2).ToString("n");
            tmp = tmp.Replace(",", "");
            tmp = tmp.Substring(0, tmp.IndexOf("."));
            BigNumber retorno = new BigNumber(tmp);
            return retorno;
        }
        return new BigNumber("1");
    }

    */
    private int getIndex(string numero)
    {
        string letras = Regex.Replace(numero, @"[\d-.]", string.Empty);
        if(letras.Length == 1) { letras = letras.ToUpper(); }     
        double temp;
        if(!Double.TryParse(letras, out temp))
        {
            int i = 0;
            foreach(string s in cast)
            {
                if (letras.Equals(s))
                {
                    return i;
                }
                i++;
            }
        }
        return 0;
    }


    public static string ceiling(string valor)
    {

        if(BigNumber.getInstance().getIndex(valor) == 0)
        {
            double tmp;
            Double.TryParse(valor, out tmp);
            if(tmp < 1) { return "1"; }
            else{ return Math.Ceiling(tmp).ToString(); }
        }
        return valor;
    }


}

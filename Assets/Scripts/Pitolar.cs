using System;
using UnityEngine;

[Serializable]
public class Pitolar {

    public static Pitolar ZERO { get { return new Pitolar(); } }

    public double valor;
    public int grandeza;

    string[] cast = new string[] { "", "K", "M",  "B", "T", "aa", "bb", "cc", "dd", "ee", "ff", "gg", "hh",
            "ii", "jj", "kk", "ll", "mm", "nn", "oo", "pp", "qq", "rr", "ss", "tt", "uu", "vv", "xx", "ww",
            "yy", "zz", "AAA", "BBB", "CCC", "DDD", "EEE", "FFF", "GGG", "HHH", "III", "JJJ", "KKK", "LLL",
            "MMM", "NNN", "OOO", "PPP", "QQQ", "RRR", "SSS", "TTT", "UUU", "VV", "XXX", "WWW", "YYY", "ZZZ",
            "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE",
            "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE",
            "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE",
            "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE", "IDE",
            "IDE", "IDE", "IDE", "IDE", "IDE", "IDE"
        };


    public Pitolar(double valor, int grandeza)
    {
        this.valor = valor;
        this.grandeza = grandeza;
        fixPitolar();
    }
    public Pitolar()
    {
        this.valor = 0;
        this.grandeza = 0;
    }


    private void fixPitolar()
    {
        if (Math.Abs(valor) < 1)
        {
            for (int i = 0; i <= 500; i++)
            {
                if (Math.Abs(valor) >= 1 || grandeza == 0)
                    break;
                valor *= 1000;
                grandeza -= 1;
            }
        }
        if (Math.Abs(valor) > 1000)
        {
            for (int i = 0; i <= 500; i++)
            {
                valor /= 1000;
                grandeza += 1;
                if (Math.Abs(valor) <= 1000)
                    break;
            }
        }
        if (grandeza < 0)
        {
            valor /= Math.Pow(1000, Math.Abs(grandeza));
            grandeza = 0;
        }


        if (grandeza == 0 && valor > 1)
            valor = Math.Ceiling(valor);
        else
            valor = Math.Round(valor, 2);
    }

    public void forceCeiling()
    {
        valor = Math.Ceiling(valor);
    }

    public static bool operator >(Pitolar a, Pitolar b)
    {
        return a == Pitolar.Max(a, b) && a != b;
    }

    public static bool operator <(Pitolar a, Pitolar b)
    {
        return a == Pitolar.Min(a, b) && a != b;
    }

    public static bool operator >=(Pitolar a, Pitolar b)
    {
        return a > b || a == b;
    }

    public static bool operator <=(Pitolar a, Pitolar b)
    {
        return a < b || a == b;
    }

    public static bool operator ==(Pitolar a, Pitolar b)
    {
        return a.valor == b.valor && a.grandeza == b.grandeza;
    }

    public static bool operator !=(Pitolar a, Pitolar b)
    {
        return a.valor != b.valor || a.grandeza != b.grandeza;
    }

    public static bool operator >(Pitolar a, double b)
    {
        return a > new Pitolar(b, 0);
    }

    public static bool operator <(Pitolar a, double b)
    {
        return a < new Pitolar(b, 0);
    }

    public static bool operator >=(Pitolar a, double b)
    {
        return a >= new Pitolar(b, 0);
    }

    public static bool operator <=(Pitolar a, double b)
    {
        return a <= new Pitolar(b, 0);
    }

    public static bool operator ==(Pitolar a, double b)
    {
        return a == new Pitolar(b, 0);
    }

    public static bool operator !=(Pitolar a, double b)
    {
        return a != new Pitolar(b, 0);
    }

    public static Pitolar operator +(Pitolar a, Pitolar b)
    {
        int diff = Math.Abs(a.grandeza - b.grandeza);
        if (diff <= 1)
        {
            if (a.grandeza > b.grandeza)
                return new Pitolar(a.valor + (b.valor / 1000 * diff), a.grandeza);
            else if (a.grandeza < b.grandeza)
                return new Pitolar((a.valor / 1000 * diff) + b.valor, b.grandeza);
            else
                return new Pitolar(a.valor + b.valor, a.grandeza);
        }
        else return Pitolar.Max(a, b);
    }

    public static Pitolar operator +(Pitolar a, double b)
    {
        return a + new Pitolar(b, 0);
    }

    public static Pitolar operator -(Pitolar a, Pitolar b)
    {
        int diff = Math.Abs(a.grandeza - b.grandeza);
        if (diff <= 1)
        {
            if (a.grandeza > b.grandeza)
                return new Pitolar(a.valor - (b.valor / 1000 * diff), a.grandeza);
            else if (a.grandeza < b.grandeza)
                return new Pitolar((a.valor / 1000 * diff) - b.valor, b.grandeza);
            else
                return new Pitolar(a.valor - b.valor, a.grandeza);
        }
        else
        {
            if (a.grandeza < b.grandeza)
                return new Pitolar(b.valor * (-1), b.grandeza);
            else
                return a;
        }
    }

    public static Pitolar operator -(Pitolar a, double b)
    {
        return a - new Pitolar(b, 0);
    }

    public static Pitolar operator *(Pitolar a, Pitolar b)
    {
        return new Pitolar(a.valor * b.valor, a.grandeza + b.grandeza);
    }

    public static Pitolar operator *(Pitolar a, double b)
    {
        return a * new Pitolar(b, 0);
    }

    public static Pitolar operator /(Pitolar a, Pitolar b)
    {
        return new Pitolar(a.valor / b.valor, a.grandeza - b.grandeza);
    }

    public static Pitolar operator /(Pitolar a, double b)
    {
        return a / new Pitolar(b, 0);
    }

    public static Pitolar Max(Pitolar a, Pitolar b)
    {
        if(Math.Abs(a.grandeza - b.grandeza) == 0)
        {
            if (a.valor > b.valor) return a;
            else return b;
        }
        else
        {
            if (a.grandeza > b.grandeza) return a;
            else return b;
        }
    }

    public static Pitolar Min(Pitolar a, Pitolar b)
    {
        if (Math.Abs(a.grandeza - b.grandeza) == 0)
        {
            if (a.valor > b.valor) return b;
            else return a;
        }
        else
        {
            if (a.grandeza > b.grandeza && a.valor < 0) return a;
            else if (a.grandeza < b.grandeza) return a;
            else return b;
        }

    }

    public override string ToString()
    {
        return valor.ToString() + " "+cast[grandeza];
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }


}

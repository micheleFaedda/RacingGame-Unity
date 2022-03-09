using System.Collections.Generic;
using System.Linq;

/*Struttura per tenere conto della posizione della macchina*/
struct InfoMacchina {

    public string nome;
    public int posizione;
    public float tempo;

    public InfoMacchina(string n, int p, float t) {
        nome = n;
        posizione = p;
        tempo = t;
    }
}

public class Classifica {

    static Dictionary<int, InfoMacchina> infoMacchine = new Dictionary<int, InfoMacchina>();
    static int numMacchineRegistrate = -1;

    public static int RegisteraMacchina(string nome) {
        numMacchineRegistrate++;
        infoMacchine.Add(numMacchineRegistrate, new InfoMacchina(nome, 0, 0.0f));
        return numMacchineRegistrate;
    }

    public static void setPosizione(int id, int giro, int checkpoint, float t) {
        int posizione = giro * 1000 + checkpoint;
        infoMacchine[id] = new InfoMacchina(infoMacchine[id].nome, posizione, t);
    }

    public static string GetPosizione(int id) {
        int index = 0;
        foreach (KeyValuePair<int, InfoMacchina> pos in infoMacchine.OrderByDescending(key => key.Value.posizione).ThenBy(key => key.Value.tempo)) {
            index++;
            if (pos.Key == id) {
                switch (index) {
                    case 1: return "First";
                    case 2: return "Second";
                    case 3: return "Third";
                    case 4: return "Fourth";
                }
            }
        }
        return "Unknown";
    }
}

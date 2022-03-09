using System.Collections.Generic;
using System.Linq;

struct PlayerStats {

    public string nome;
    public int posizione;
    public float tempo;

    public PlayerStats(string n, int p, float t) {
        nome = n;
        posizione = p;
        tempo = t;
    }
}

public class Leaderboard {

    static Dictionary<int, PlayerStats> lb = new Dictionary<int, PlayerStats>();
    static int carsRegistered = -1;

    public static void Reset() {

        lb.Clear();
        carsRegistered = -1;
    }

    public static int RegisterCar(string name) {

        carsRegistered++;
        lb.Add(carsRegistered, new PlayerStats(name, 0, 0.0f));
        return carsRegistered;
    }

    public static void SetPosition(int rego, int lap, int checkpoint, float time) {

        int position = lap * 1000 + checkpoint;
        lb[rego] = new PlayerStats(lb[rego].nome, position, time);
    }

    public static string GetPosition(int rego) {

        int index = 0;
        foreach (KeyValuePair<int, PlayerStats> pos in lb.OrderByDescending(key => key.Value.posizione).ThenBy(key => key.Value.tempo)) {

            index++;
            if (pos.Key == rego) {
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
    public static List<string> GetPlaces() {

        List<string> places = new List<string>();

        foreach (KeyValuePair<int, PlayerStats> pos in lb.OrderByDescending(key => key.Value.posizione).ThenBy(key => key.Value.tempo)) {

            places.Add(pos.Value.nome);
        }
        return places;
    }
}

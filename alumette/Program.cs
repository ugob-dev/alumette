using System;

namespace alumette
{
    class Program {

    const string rules1 = "";
    const string demande1 = "Combien de ligne voulez vous dans cette partie";
    const string demande2 = "Combien d'allumette au maximum Pouvez vous enlever dans cette partie";

        static void Main(string[] args)
        {
            bool itsOk = true;
            Console.WriteLine(rules1);
            Console.WriteLine(demande1);
            int nbDeLigne = Int32.Parse(Console.ReadLine());
            Console.WriteLine(demande2);
            int nbDeAllumette = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Pret a joué? :y/n?");
            string Pret = Console.ReadLine();

            if (Pret == "y")
            {
                int nbDeColonne = 1;
                for (int i = 1; i < nbDeLigne; i++)
                {
                    nbDeColonne += 2;
                }
                bool again = true;
                while (again)
                {
                    startGame(nbDeLigne, nbDeColonne, nbDeAllumette);
                    Console.WriteLine("again ? y/n");
                    if (Console.ReadLine() == "n")
                        again = false;
                }
            }
        }

        static void startGame(int ligne, int colonne,int nbDeAllumetteMax)
        {
            bool fini = true;
            bool IA = false;
            bool[,] PlacementAllumette = initBool(ligne, colonne);
            while (fini)
            {
                if (!IA)
                {
                    Console.WriteLine(recreationDePyramide(PlacementAllumette,ligne,colonne));
                    bool TourFini = true;
                    System.Threading.Thread.Sleep(2000);
                    while (TourFini)
                    {
                        // le joueur choisie une ligne puis un nb d'allumette
                        Console.WriteLine("quelle ligne choisie tu?");
                        int LigneChoisie = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("Combien d'alumette?");
                        int nbAllumette = Int32.Parse(Console.ReadLine());
                        if(nbDeAllumetteMax >= nbAllumette)
                        {
                            if (nbAllumetteByColonne(PlacementAllumette, LigneChoisie, colonne, nbAllumette))
                            {
                                TourDuJoueur(PlacementAllumette, LigneChoisie, colonne, nbAllumette);
                                fini = verifWin(PlacementAllumette, ligne, colonne);
                                TourFini = false;
                                IA = true;
                            }
                            else
                                Console.WriteLine("Il n'ya pas assez d'allumette dans cette colonne");
                        }else
                            Console.WriteLine("tu a choisie trop d'allumette, le max c'est " + nbDeAllumetteMax);
                    }
                }
                else
                {
                    Console.WriteLine(recreationDePyramide(PlacementAllumette, ligne, colonne));
                    bool TourFini = true;
                    System.Threading.Thread.Sleep(2000);
                    while (TourFini)
                    {
                        //le joueur IA choisie une ligne puis un nb d'allumette

                        Random aleatoire = new Random();
                        int nbAllumette = aleatoire.Next(1,nbDeAllumetteMax+1); //Génère un entier compris entre 0 et 9
                        int LigneChoisie = aleatoire.Next(1,ligne+1);
                        if (nbDeAllumetteMax >= nbAllumette)
                            if (nbAllumetteByColonne(PlacementAllumette, LigneChoisie, colonne, nbAllumette))
                            {
                                Console.WriteLine("Billy a choisie la ligne " + LigneChoisie);
                                Console.WriteLine("Billy Retire " + nbAllumette + " Allumette");
                                TourDuJoueur(PlacementAllumette, LigneChoisie, colonne, nbAllumette);
                                fini = verifWin(PlacementAllumette, ligne, colonne);
                                TourFini = false;
                                IA = false;
                            }
                    }

                    fini = verifWin(PlacementAllumette, ligne, colonne);
                }
            }
            if (!IA)
                Console.WriteLine("IA a gagne");
            if (IA)
                Console.WriteLine("Vous avez gagne");
        }
        static bool[,] TourDuJoueur(bool[,] PlacementAllumette,int ligne,int colonne, int nbAllumette)
        {
            
            bool tourFait = true;
            while (tourFait)
            {
                bool IsSupp = false;
                for (int i = 1; i < colonne; i++)
                {
                    if (!PlacementAllumette[ligne - 1, i] && PlacementAllumette[ligne - 1, i - 1])
                    {
                        for (int y = i - nbAllumette; y < i; y++)
                            PlacementAllumette[ligne - 1, y] = false;
                        IsSupp = true;
                        tourFait = false;
                    }
                }
                if (!IsSupp)
                {
                    if(ligne == 1 && PlacementAllumette[ligne - 1, colonne - 1] && PlacementAllumette[ligne - 1, colonne - (nbAllumette - 1)])
                    {
                        for (int z = 0; z < nbAllumette; z++)
                            PlacementAllumette[ligne - 1, colonne - 1 - z] = false;
                        tourFait = false;
                    }
                    else
                    {
                        Console.WriteLine("c'est impossible");
                    }
                }
            }
            return PlacementAllumette;
        }
        static bool nbAllumetteByColonne(bool[,] PlacementAllumette, int ligne, int colonne, int nbAllumette)
        {

            int nbAllumetteInCollonne = 0;
            for (int i = 0; i < colonne; i++)
                if (PlacementAllumette[ligne - 1, i])
                    nbAllumetteInCollonne++;
            return (nbAllumette <= nbAllumetteInCollonne);
        }
        static string recreationDePyramide(bool[,] PlacementAllumette,int ligne, int colonne)
        {
            string allumette = "";

            // **********
            for (int i = 0; i < ligne + 2; i++)
                allumette += "*";
            allumette += '\n';

            for (int i = ligne; i > 0; i--)
            {
                allumette += "*";
                for (int y = colonne; y > 0; y--)
                {
                    if (PlacementAllumette[i-1, y-1])
                        allumette += "|";
                    else
                        allumette += " ";
                }
                allumette += "*"+'\n';
            }

            // **********
            for (int i = 0; i < ligne + 2; i++)
                allumette += "*";
            allumette += '\n';
            return allumette;
        }
        static bool[,] initBool(int ligne, int colonne,bool Desordonne = false)
        {
            int allumetteMax = colonne;
            bool[,] PlacementAllumette = new bool[ligne, colonne];
            for (int i = 0; i < ligne; i++)
            {
                for(int y = i; y < colonne; y++)
                { 
                    int bla = ((colonne - allumetteMax) / 2);
                    int blo = (colonne - ((colonne - allumetteMax) / 2)-1);
                    // colonne - allumette / 2 : systeme de pyramide
                    //colonne - (colonne - allumette / 2) : fin de la chaine d'allumette
                    if (!Desordonne &&
                        y >= bla  &&
                        y <= blo)
                        PlacementAllumette[i, y] = true; // true : il y a une allumette
                }
                allumetteMax = allumetteMax - 2; // allumette max sur la ligne d'apres initialisé
            }
            return PlacementAllumette;
        }

        static bool verifWin(bool[,] PlacementAllumette, int ligne, int colonne)
        {
            for (int i = 0; i < ligne; i++)
                for (int y = i; y < colonne; y++)                    
                    if (PlacementAllumette[i , y ])
                        return true; // partie pas fini , allumette trouvé
            return false; // partie fini
        }
    }
}

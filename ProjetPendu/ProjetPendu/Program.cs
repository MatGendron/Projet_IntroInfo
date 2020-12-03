using System;
using System.IO;

namespace ProjetPendu
{
    class Program
    {

        enum FinDeTour
        {
            Continue,
            Gagne,
            Perdu,
            Abandon
        }
        static void Main(string[] args)
        {
            bool compris;
            string reponse;

            string[] lexique = new string[323_572]; // c'est le nombre de mots dans le lexique
            InitLexique(lexique);

            AfficheRegle();

            bool envie_de_jouer = true;
			while(envie_de_jouer) {
				Partie(lexique);

				compris = false; reponse = "";

				while(!compris) {
					Console.WriteLine("Voulez-vous refaire une partie ? (oui/non)");
					reponse = Console.ReadLine();
					if (String.Equals(reponse,"oui")) {
						envie_de_jouer=true;
						compris=true;
					}
					else {
						if (String.Equals(reponse,"non")) {
							envie_de_jouer=false;
							compris = true;
						}
						else {
							Console.Write("Je n'ai pas compris. ");
						}
					}
					
				}
			}

        }
        /* Permet de demander à l'utilisateur quel type de partie il veut lancer
        */
        static void Partie(string[] lexique)
        {

        }
        /* Initialise une partie où l'ordinateur choisit le mot et l'humain doit le trouver
        */
        static void PartieHumain(string[] lexique)
        {

        }
        /* Permet à l'ordinateur de choisir un mot aléatoirement dans le dictionnaire
        */
        static string ChoisirMotOrdi(string[] lexique)
        {
            Random rnd = new Random();
            int position = rnd.Next(lexique.Length);

            return(lexique[position]);
        }
        /*Cette fonction permet à un joueur humain de jouer un tour
        */
        static void TourHumain()
        {

        }
        /* Affiche les règles
        */
        static void AfficheRegle()
        {

        }
        /* Cette fonction permet d'afficher le pendu et le mot à compléter
         */
        static void AffichePendu(int degre)
        {

        }
        /* Cette fonction permet à un ordinateur de jouer un tour.
         */
        static void TourOrdinateur()
        {

        }
        /* Ouvre le fichier dicoFR.txt et insère les mots dans le tableau.
        */
        static void InitLexique(string[] lexique)
        {
            System.Text.Encoding   encoding = System.Text.Encoding.GetEncoding(  "iso-8859-1"  );
            StreamReader lecteur = new StreamReader("../dicoFR.txt",encoding);

            string mot = lecteur.ReadLine();
            int i = 0;

            while (mot != null)
            {
                // On ne veut pas prendre les mots "A" et "Y", ni la dernière ligne vide.
                if (mot.Length >= 2)
                {
                    lexique[i] = mot;
                    mot = lecteur.ReadLine();
                }
            } 
        }
    }
}


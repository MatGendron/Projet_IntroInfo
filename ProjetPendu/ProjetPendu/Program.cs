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
        /* Cette fonction permet de vérifier si un mot existe dans le lexique
         */
        static bool MotExiste(string[] lexique, string mot)
        {
            int debut = 0;
            int fin = lexique.Length-1;
            int milieu = (fin + debut) / 2;
            while (lexique[milieu] != mot && milieu > debut)
            {
                if (string.Compare(mot, lexique[milieu]) < 0)
                {
                    fin = milieu;
                    milieu = (fin + debut) / 2;
                }
                else if (string.Compare(mot,lexique[milieu]) > 0)
                {
                    debut = milieu;
                    milieu = (fin + debut) / 2;
                }
            }
            if (lexique[milieu] == mot)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /* Permet à un joueur humain de choisir un mot
         */
        static string ChoisirMotHumain(string[] lexique)
        {
            string solution = "";
            do
            {
                Console.WriteLine("Saisissez un mot solution valide :");
                solution = Console.ReadLine().ToUpper();
            } while (!MotExiste(lexique, solution));
            return solution;
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
            Console.Write(
                "==============================================================================================================\n" +
                "                                                  JEU DU PENDU\n" +
                "                                                  RÈGLES DU JEU\n" +
                "==============================================================================================================\n" +
                "Vous êtes sur l'échafaud sur le point d'être pendu haut et court mais vous avez une chance de vous en sortir !\n" +
                "Trouvez le mot solution avant que tout votre corps soit sur la potence et vous serez gracié.\n" +
                "Vous allez tout d'abord décider si vous voulez trouver le mot vous-même ou si vous préférer laisser\n" +
                "l'ordinateur trouver votre mot.\n" +
                "Si vous décidez de trouver le mot vous-même, vous pourrez saisir une lettre à chaque tour si elle est\n" +
                "contenue dans le mot elle s'affichera dans l'indice, en revanche si elle ne l'est pas vous vous rapprocherez\n" +
                "de la pendaison.\n" +
                "Vous pouvez aussi saisir un mot en entier, si celui-ci est le mot solution vous gagnez, sinon vous\n" +
                "perdez immédiatement." +
                "A tout moment pendant votre tour vous pouvez saisir :\n" +
                "- pour abandonner la partie.\n" +
                "? pour afficher à nouveau ces règles.\n" +
                "Appuyez sur [entrer] pour continuer\n");
            Console.ReadLine();
        }
        /* Cette fonction permet d'afficher le pendu et le mot à compléter
         */
        static void AffichePendu(int degre, bool[] estTentee, string indice)
        {
            Console.Write(
                "            _______\n" +
                "           |       |\n" +
                "           |       ");
            if (degre >= 1)
            {
                Console.Write("o");
            }
            else
            {
                Console.Write(" ");
            }
            Console.Write("\n" +
            "           |      ");
            if (degre >= 3) {
                Console.Write("/"); 
            }
            else 
            {
                Console.Write(" ");
            }
            if (degre >= 2){
                Console.Write("|");
            } else {
                Console.Write(" ");
            }
            if (degre >= 4) {
                Console.Write("\\");
            } else {
                Console.Write(" ");
            }
            Console.Write("\n" +
                "           |      ");
            if (degre >= 5) {
                Console.Write("/ ");
            } else {
                Console.Write("  ");
            }
            if (degre >= 6) {
                Console.Write("\\");
            } else {
                Console.Write(" ");
            }Console.Write("\n" +
                "          -^-\n");
            Console.WriteLine(indice);
            for(int i=0; i<estTentee.Length; i++)
            {
                if (estTentee[i])
                {
                    Console.Write(IntToLettre(i)+" ");
                }
            }
            Console.Write("\n");
        }
        /* Fonction convertissant une lettre en sa place dans l'alphabet
         */
        static int LettreToInt(char lettre)
        {
            lettre = Char.ToUpper(lettre);
            int code = Convert.ToInt32(lettre)-Convert.ToInt32('A');
            if(code >=0 && code <= 25)
            {
                return code;
            }
            else
            {
                return -1;
            }
        }
        /* Fonction convertissant un entier en la lettre correspondante dans l'alphabet
         */
        static char IntToLettre(int code)
        {
            if(code >=0 && code <= 25)
            {
                return Convert.ToChar(Convert.ToInt32('A') + code);
            }
            else
            {
                return '\0';
            }
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


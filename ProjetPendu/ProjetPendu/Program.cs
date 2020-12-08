using System;
using System.IO;
using System.Text;

namespace ProjetPendu
{
    class Program
    {

        enum EtatPartie
        {
            Continue,
            Gagne,
            Perdu,
            Abandon
        }
        static void Main(string[] args)
        {

            string[] lexique = new string[323_572]; // c'est le nombre de mots dans le lexique
            InitLexique(lexique);

            AfficheRegle();

            bool envie_de_jouer = true;
			while(envie_de_jouer) {
				Partie(lexique);

				envie_de_jouer = PoseQuestion("Voulez vous recommencer une partie ?");
			}
            Console.WriteLine("à bientôt !");

        }
        /* Permet de demander à l'utilisateur quel type de partie il veut lancer
        */
        static void Partie(string[] lexique)
        {
            bool joueur = !PoseQuestion("Voulez-vous que l'ordinateur joue au pendu ?"); // joueur=true si l'utilisateur joue, false si l'ordinateur joue
            
            string reponse = joueur ? ChoisirMotHumain(lexique) : ChoisirMotOrdi(lexique);
            string indice = ConstruitIndice(reponse);
            bool[] estTentee = new bool[26];
            EtatPartie etat = EtatPartie.Continue;

            while(etat==EtatPartie.Continue)
            {
                
            }



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

        static string ChoisirMotHumain(string[] lexique)
        {
            return("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAH");
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
        /* Pose une question fermée (string question). Si la réponse est oui/non, la fonction renvoie le booléen correspondant. Sinon, repose la question.
        */
        static bool PoseQuestion(string question) {
            string reponse;
            Console.WriteLine(question+" (oui/non)");

            while(true) {
                
                reponse = Console.ReadLine();
                if (String.Equals(reponse,"oui")) {
                    return(true);
                }
                else {
                    if (String.Equals(reponse,"non")) {
                        return(false);
                    }
                    else {
                        Console.Write("Je n'ai pas compris. Réponds par \"oui\" ou \"non\" !");
                    }
                }
                Console.WriteLine(question);
			}

            
        }

        /* Construit la chaîne de caractère indice à partir de la réponse : change tous les caractères en '_' sauf les tirets. 
        */
        static string ConstruitIndice(string reponse)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char lettre in reponse)
            {
                sb.Append(lettre=='-' ? '-' : '_');
            }

            return(sb.ToString());
        }

    }
}


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
            int degre = 0;

            while(etat==EtatPartie.Continue)
            {
                etat = Tour(joueur,reponse,indice,estTentee,degre);
            }
            Console.WriteLine("Merci d'avoir joué !");

        }
        /* Joue un tour.
        */
        static EtatPartie Tour(
                bool joueur,
                string reponse,
                string indice,
                bool[] estTentee,
                int degre
            )
        {
            AffichePendu(1);

            string tentative = joueur ? ChoisirReponseHumain() : ChoisirReponseOrdi();

            if (tentative == "-")
            {
                return(EtatPartie.Abandon);
            }
            if (tentative == "?")
            {
                AfficheRegle();
                return(EtatPartie.Continue);
            }

            if (tentative.Length == 1)
            { // Cas où 1 caractère est entré, ChoisirReponseHumain/ChoisirReponseOrdi suppose que c'est 
                if (estTentee[LettreToInt(tentative[9])])
                {
                    Console.WriteLine("Tu as déjà testé cette lettre !!");
                    return(EtatPartie.Continue);
                }
                
                

            }

            return(EtatPartie.Continue);
        }

        /* Fonction inopérante pour éviter que le vérificateur pleure. 
        */
        static string ChoisirReponseHumain()
        {
            return("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAH");
        }
        /* Fonction inopérante pour éviter que le vérificateur pleure. 
        */
        static string ChoisirReponseOrdi()
        {
            return("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAH");
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


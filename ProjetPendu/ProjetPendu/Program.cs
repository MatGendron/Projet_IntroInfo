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

            /*AfficheRegle();

            bool envie_de_jouer = true;
            while (envie_de_jouer)
            {
                Partie(lexique);

                envie_de_jouer = PoseQuestion("Voulez vous recommencer une partie ?");
            }
            Console.WriteLine("à bientôt !");*/
            Console.WriteLine(lexique[1]);
        }
        /* Permet de demander à l'utilisateur quel type de partie il veut lancer
        */
        static void Partie(string[] lexique)
        {
            bool joueur = PoseQuestion("Voulez-vous que l'ordinateur joue au pendu ?"); // joueur=true si l'utilisateur joue, false si l'ordinateur joue

            string reponse = joueur ? ChoisirMotHumain(lexique) : ChoisirMotOrdi(lexique);
            Console.WriteLine(reponse);
            string indice = ConstruitIndice(reponse);
            bool[] estTentee = new bool[26];
            EtatPartie etat = EtatPartie.Continue;
            int degre = 0;

            while (etat == EtatPartie.Continue)
            {
                etat = Tour(joueur, lexique, reponse, indice, estTentee, ref degre);
            }
            Console.WriteLine("Merci d'avoir joué !");
        }

        /*Cette fonction permet de jouer un tour.
         */
        static EtatPartie Tour(
                bool joueur,
                string[] lexique,
                string reponse,
                string indice,
                bool[] estTentee,
                ref int degre
            )
        {
            AffichePendu(degre, estTentee, indice);

            string tentative = joueur ? ChoisirReponseHumain() : ChoisirReponseOrdi(estTentee);
            if (tentative.Length > 1)
            {
                if (tentative != reponse)
                {
                    return EtatPartie.Perdu;
                }
                else
                {
                    return EtatPartie.Gagne;
                }
            }
            else if(tentative.Length==1)
            {
                if (tentative == "-")
                {
                    return EtatPartie.Abandon;
                }
                if(tentative == "?")
                {
                    AfficheRegle();
                    return EtatPartie.Continue;
                }
                if (reponse.Contains(tentative))
                {
                    char lettre = tentative.ToCharArray()[0];
                    UpdateIndice(reponse, indice, lettre);
                    estTentee[LettreToInt(lettre)] = true;
                }
                else
                {
                    degre++;
                    if (degre >= 6)
                    {
                        return EtatPartie.Perdu;
                    }
                }
            }
            return (EtatPartie.Continue);
        }
        /*Cette fonction permet à un être humain de choisir une réponse*/
        static string ChoisirReponseHumain()
        {
            return Console.ReadLine();
        }

        /*Cette fonction permet à l'ordinateur de choisir une lettre.*/
        static string ChoisirReponseOrdi(bool[] estTentee)
        {
            Random rnd = new Random();
            int position = rnd.Next(estTentee.Length);
            return IntToLettre(position).ToString();
        }

        /* Permet à l'ordinateur de choisir un mot aléatoirement dans le dictionnaire
        */
        static string ChoisirMotOrdi(string[] lexique)
        {
            Random rnd = new Random();
            int position = rnd.Next(lexique.Length);

            return (lexique[position]);
        }
        /* Cette fonction permet de vérifier si un mot existe dans le lexique
         */
        static bool MotExiste(string[] lexique, string mot)
        {
            int debut = 0;
            int fin = lexique.Length - 1;
            int milieu = (fin + debut) / 2;
            while (lexique[milieu] != mot && milieu > debut)
            {
                if (string.Compare(mot, lexique[milieu]) < 0)
                {
                    fin = milieu;
                    milieu = (fin + debut) / 2;
                }
                else if (string.Compare(mot, lexique[milieu]) > 0)
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
            Console.WriteLine("Choisissez un mot solution.");
            string solution = Console.ReadLine();
            while (!MotExiste(lexique, solution))
            {
                Console.WriteLine("Saisissez un mot solution valide :");
                solution = Console.ReadLine().ToUpper();
            }
            return solution;
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
            if (degre >= 3)
            {
                Console.Write("/");
            }
            else
            {
                Console.Write(" ");
            }
            if (degre >= 2)
            {
                Console.Write("|");
            }
            else
            {
                Console.Write(" ");
            }
            if (degre >= 4)
            {
                Console.Write("\\");
            }
            else
            {
                Console.Write(" ");
            }
            Console.Write("\n" +
                "           |      ");
            if (degre >= 5)
            {
                Console.Write("/ ");
            }
            else
            {
                Console.Write("  ");
            }
            if (degre >= 6)
            {
                Console.Write("\\");
            }
            else
            {
                Console.Write(" ");
            }
            Console.Write("\n" +
               "          -^-\n");
            Console.WriteLine(indice);
            for (int i = 0; i < estTentee.Length; i++)
            {
                if (estTentee[i])
                {
                    Console.Write(IntToLettre(i) + " ");
                }
            }
            Console.Write("\n");
        }
        /* Fonction convertissant une lettre en sa place dans l'alphabet
         */
        static int LettreToInt(char lettre)
        {
            lettre = Char.ToUpper(lettre);
            int code = Convert.ToInt32(lettre) - Convert.ToInt32('A');
            if (code >= 0 && code <= 25)
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
            if (code >= 0 && code <= 25)
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
            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            StreamReader lecteur = new StreamReader("../../../../../dicoFR.txt", encoding);

            string mot = lecteur.ReadLine();
            int i = 0;

            while (mot != null)
            {
                // On ne veut pas prendre les mots "A" et "Y", ni la dernière ligne vide.
                if (mot.Length >= 2)
                {
                    lexique[i] = mot;
                }
                    mot = lecteur.ReadLine();
            }
        }
        /* Pose une question fermée (string question). Si la réponse est oui/non, la fonction renvoie le booléen correspondant. Sinon, repose la question.
       */
        static bool PoseQuestion(string question)
        {
            string reponse;
            Console.WriteLine(question + " (oui/non)");

            while (true)
            {

                reponse = Console.ReadLine();
                if (String.Equals(reponse, "oui"))
                {
                    return (true);
                }
                else
                {
                    if (String.Equals(reponse, "non"))
                    {
                        return (false);
                    }
                    else
                    {
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
                sb.Append(lettre == '-' ? '-' : '_');
            }

            return (sb.ToString());
        }

        /* Met à jour l'indice avec une lettre trouvée par le joueur.
         */
        static void UpdateIndice(string reponse, string indice, char lettre)
        {
            for(int i=0; i<reponse.Length; i++)
            {
                if (reponse[i] == lettre)
                {
                    StringBuilder sb = new StringBuilder(indice);
                    sb[i] = lettre;
                    indice = sb.ToString();
                }
            }
        }
    }
}


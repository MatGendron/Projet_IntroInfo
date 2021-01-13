using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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

            bool envieDeJouer = true;
            while (envieDeJouer)
            {
                Partie(lexique);

                envieDeJouer = PoseQuestion("Voulez vous recommencer une partie ?");
            }
            Console.WriteLine("à bientôt !");
        }
        /* Permet de demander à l'utilisateur quel type de partie il veut lancer
        */
        static void Partie(string[] lexique)
        {
            bool joueur = PoseQuestion("Voulez-vous que l'ordinateur joue au pendu ?"); // joueur=true si l'utilisateur joue, false si l'ordinateur joue

            string reponse = joueur ? ChoisirMotHumain(lexique) : ChoisirMotOrdi(lexique);
            //Console.WriteLine(reponse);
            char[] indice = ConstruitIndice(reponse);
            bool[] estTentee = new bool[26];
            string[] recherche = new string[lexique.Length];
            int tailleRecherche = 0;
            List<string> motTente = new List<string>();
            EtatPartie etat = EtatPartie.Continue;
            int degre = 0;

            while (etat == EtatPartie.Continue)
            {
                etat = Tour(joueur, lexique, reponse, indice, estTentee, ref degre, recherche, ref tailleRecherche, motTente);
            }
            if (etat == EtatPartie.Perdu)
            {
                Console.WriteLine("       PERDU!!       ");
                Console.WriteLine("Le mot à deviner était : {0}",reponse);
            }
            if (etat == EtatPartie.Abandon)
            {
                Console.WriteLine("      ABANDON!!      ");
                Console.WriteLine("Le mot à deviner était : {0}",reponse);
            }
            if (etat == EtatPartie.Gagne)
            {
                Console.WriteLine("       BRAVO!!       ");
                Console.WriteLine("Le mot à deviner était bien \"{0}\"",reponse);
            }
            Console.WriteLine("Merci d'avoir joué !");
        }

        /*Cette fonction permet de jouer un tour.
         */
        static EtatPartie Tour(
                bool joueur,
                string[] lexique,
                string reponse,
                char[] indice,
                bool[] estTentee,
                ref int degre,
                string[] recherche,
                ref int tailleRecherche,
                List<string> motTente
            )
        {
            AffichePendu(degre, estTentee, indice);
            string tentative = joueur ? ChoisirReponseOrdi(estTentee, lexique,indice, recherche, ref tailleRecherche, motTente) : ChoisirReponseHumain();

            // Le joueur abandonne
            if (tentative == "-")
            {
                return EtatPartie.Abandon;
            }
            // Le joueur demande les règles
            if(tentative == "?")
            {
                AfficheRegle();
                return EtatPartie.Continue;
            }

            
            if (tentative.Length == 1)
            // Une lettre est tentée
            {

                char lettre = tentative.ToCharArray()[0];

                // La lettre a déjà été proposée.
                if (estTentee[LettreToInt(lettre)])
                {
                    Console.WriteLine("La lettre a déjà été proposée ! Soit plus attentif.");
                    degre++;
                    if (degre >= 6)
                    {
                        AffichePendu(degre, estTentee, indice);
                        return EtatPartie.Perdu;
                    }
                    return EtatPartie.Continue;
                }

                if (reponse.Contains(lettre))
                {
                    UpdateIndice(reponse, indice, lettre);
                    estTentee[LettreToInt(lettre)] = true;
                    string test = new string(indice);
                    if (test == reponse)
                    {
                        AffichePendu(degre, estTentee, indice);
                        return EtatPartie.Gagne;
                    }

                    return EtatPartie.Continue;
                }
                else
                {
                    estTentee[LettreToInt(lettre)] = true;
                    degre++;
                    if (degre >= 6)
                    {
                        AffichePendu(degre, estTentee, indice);
                        return EtatPartie.Perdu;
                    }
                }

            }
            else
            // Un mot est tenté
            {
                if (!MotExiste(lexique,tentative))
                {
                    Console.WriteLine("Ce mot n'existe pas ! Choisissez un mot de la langue française");
                    return EtatPartie.Continue;
                }

                if (tentative == reponse)
                {
                    return EtatPartie.Gagne;
                }
                else // Le mot proposé n'est pas égal au mot à trouver.
                {
                    degre++;
                    if (degre >= 6)
                    {
                        AffichePendu(degre, estTentee, indice);
                        return EtatPartie.Perdu;
                    }
                    return EtatPartie.Continue;
                }
            }

            return EtatPartie.Continue;
        }
        /*Cette fonction permet à un être humain de choisir une réponse*/
        static string ChoisirReponseHumain()
        {
            Regex regex = new Regex(@"\?|[a-zA-Z\-]+");
            
            string entree;

            Console.WriteLine("Entrez une lettre ; un mot ; '?' pour obtenir les règles ; '-' pour abandonner");
            entree = Console.ReadLine();

            while (!regex.IsMatch(entree))
            {
                Console.WriteLine("Respectez les consignes !!! (les accents et apostrophes ne sont pas acceptés)");
                entree = Console.ReadLine();

            }
            return entree.ToUpper();
        }

        /*Cette fonction permet à l'ordinateur de choisir une lettre ou une possible réponse.*/
        static string ChoisirReponseOrdi(bool[] estTentee, string[] lexique, char[] indice, string[] recherche, ref int tailleRecherche, List<string> motTente)
        {
            if (!estTentee[LettreToInt('A')])
            {
                return ("A");
            }
            else if (!estTentee[LettreToInt('E')])
            {
                return ("E");
            }
            else if (!estTentee[LettreToInt('I')])
            {
                return ("I");
            }
            else if (!estTentee[LettreToInt('O')])
            {
                return ("O");
            }
            else if (!estTentee[LettreToInt('U')])
            {
                return ("U");
            }
            else
            {
                if (recherche[0] == null)
                {
                    for(int i=0; i<lexique.Length; i++)
                    {
                        if (indice.Length == lexique[i].Length)
                        {
                            int j = 0;
                            while(j<indice.Length && (indice[j] == lexique[i][j] || (indice[j]=='.' && (lexique[i][j]=='-' || !estTentee[LettreToInt(lexique[i][j])]))))
                            {
                                j++;
                            }
                            if (j == indice.Length)
                            {
                                recherche[tailleRecherche] = lexique[i];
                                tailleRecherche++;
                            }
                        }
                    }
                }
                Random rnd = new Random();
                int position = rnd.Next(tailleRecherche);
                string tentative = recherche[position];
                while (motTente.Contains(tentative))
                {
                    position = rnd.Next(tailleRecherche);
                    tentative = recherche[position];
                }
                motTente.Add(tentative);
                Console.WriteLine(tentative);
                return tentative;
            }
        }

        /* Permet à l'ordinateur de choisir un mot aléatoirement dans le dictionnaire
        */
        static string ChoisirMotOrdi(string[] lexique)
        {
            Random rnd = new Random();
            int position = rnd.Next(lexique.Length);

            return (lexique[position].ToUpper());
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
            return (lexique[milieu] == mot);
            

        }
        /* Permet à un joueur humain de choisir un mot
         */
        static string ChoisirMotHumain(string[] lexique)
        {
            Console.WriteLine("Choisissez un mot solution.");
            string solution = Console.ReadLine().ToUpper();
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
        static void AffichePendu(int degre, bool[] estTentee, char[] indice)
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
            StreamReader lecteur = new StreamReader("dicoFR.txt", encoding);

            string mot = lecteur.ReadLine();
            int i = 0;

            while (mot != null)
            {
                // On ne veut pas prendre les mots "A" et "Y", ni la dernière ligne vide.
                if (mot.Length >= 2)
                {
                    lexique[i] = mot;
                    i++;
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

        /* Construit la chaîne de caractère indice à partir de la réponse : change tous les caractères en '.' sauf les tirets. 
        */
        static char[] ConstruitIndice(string reponse)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char lettre in reponse)
            {
                sb.Append(lettre == '-' ? '-' : '.');
            }

            return (sb.ToString().ToCharArray());
        }

        /* Met à jour l'indice avec une lettre trouvée par le joueur.
         */
        static void UpdateIndice(string reponse, char[] indice, char lettre)
        {
            for(int i=0; i<reponse.Length; i++)
            {
                if (reponse[i] == lettre)
                {
                    indice[i] = lettre;
                }
            }
        }
    }
}


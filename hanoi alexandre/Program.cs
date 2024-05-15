/*
 * 
 * 
 * 
 * 
 * 
 * Lib usada para usar o prolog no c#:
 * https://github.com/jsakamoto/CSharpProlog
 * 
 * Inferencias e regras prolog:
 * https://courses.cs.washington.edu/courses/cse341/03sp/slides/PrologEx/simpleHanoi.pl.txt
 * 
 */


using Prolog;
using System.IO;
using System.Text;
namespace hanoi_alexandre
{


    internal class Program
    {
        public static string vazio ="      |      ";
        public static string a =    "      *      ";
        public static string b =    "     ***     ";
        public static string c =    "    *****    ";
        public static string d =    "  *********  ";
        public static string chao = "TTTTTT|TTTTTT";
        public static string[] pegarQuery(PrologEngine? prolog)
        {
            
            StringBuilder output = new StringBuilder();
            TextWriter originalOut = Console.Out;
            Console.SetOut(new StringWriter(output));

            var solution = prolog.GetFirstSolution(query: "go.");

            Console.SetOut(originalOut); 

            string filePath = "output.txt";
            File.WriteAllText(filePath, output.ToString()); 

            Console.WriteLine("Output saved to output.txt");

            


            
            string[] output2 = File.ReadAllLines(filePath);

            
            return output2;
        }


        public static void imprimeJogo(string[,]  jogo)
        {
            for(int i = 0; i < jogo.GetLength(0); i++)
            {
                for (int j = 0; j < jogo.GetLength(1); j++)
                {
                    Console.Write(jogo[i, j]);
                }
                Console.WriteLine();
            }
        }

        public static void codicoesJogo(ref string arrayLinhas, char colunaAtual, char proxColuna,
            ref int[] ultimaPos, ref string[,] jogo)
        {
           
            if (colunaAtual == 'e' && proxColuna == 'i')
            {
                var pegarElemento = jogo[ultimaPos[0]+1,0];
                jogo[ultimaPos[0] + 1, 0] = vazio;
                ultimaPos[0]++;

                jogo[ultimaPos[1], 1] = pegarElemento;
                ultimaPos[1]--;
            }

            if (colunaAtual == 'e' && proxColuna == 't')
            {
                var pegarElemento = jogo[ultimaPos[0] + 1, 0];
                jogo[ultimaPos[0] + 1, 0] = vazio;
                ultimaPos[0]++;

                jogo[ultimaPos[2], 2] = pegarElemento;
                ultimaPos[2]--;

            }
            //////////////////////////////////////////////
            if (colunaAtual == 'm' && proxColuna == 'd')
            {
                var pegarElemento = jogo[ultimaPos[1] + 1, 1];
                jogo[ultimaPos[1] + 1, 1] = vazio;
                ultimaPos[1]++;

                jogo[ultimaPos[0], 0] = pegarElemento;
                ultimaPos[0]--;
            }

            if (colunaAtual == 'm' && proxColuna == 't')
            {
                var pegarElemento = jogo[ultimaPos[1] + 1, 1];
                jogo[ultimaPos[1] + 1, 1] = vazio;
                ultimaPos[1]++;

                jogo[ultimaPos[2], 2] = pegarElemento;
                ultimaPos[2]--;
            }
            //////////////////////////////////////////////
            if (colunaAtual == 'd' && proxColuna == 'i')
            {
                var pegarElemento = jogo[ultimaPos[2] + 1, 2];
                jogo[ultimaPos[2] + 1, 2] = vazio;
                ultimaPos[2]++;

                jogo[ultimaPos[1], 1] = pegarElemento;
                ultimaPos[1]--;
            }
            if (colunaAtual == 'd' && proxColuna == 'd')
            {
                var pegarElemento = jogo[ultimaPos[2] + 1, 2];
                jogo[ultimaPos[2] + 1, 2] = vazio;
                ultimaPos[2]++;

                jogo[ultimaPos[0], 0] = pegarElemento;
                ultimaPos[0]--;
            }

        }

        public static void jogando(string[] arrayLinhas, ref int [] ultimaPos, ref string[,] jogo)
        {
            imprimeJogo(jogo);
            for (int i = 0; i < arrayLinhas.Length; i++)
            {
                var linha = arrayLinhas[i];
                Console.WriteLine(linha);
           
                var colunaAtual = linha[19];
                var proxColuna = linha[linha.Length - 2];

                
                codicoesJogo(ref linha, colunaAtual, proxColuna, ref ultimaPos , ref jogo);
                imprimeJogo(jogo);
            }
        }

        static void Main(string[] args)
        {
        

            
            //                                colunas
            //                       esq=0     mei=1   dir=2
            string[,] jogo = {     {vazio  ,  vazio ,  vazio  },
                                   { a ,      vazio ,  vazio },
                                  { b ,       vazio ,  vazio  },
                                  { c ,       vazio ,  vazio  },
                                  { d ,       vazio  , vazio   },
                                  { chao ,    chao ,   chao }
                                           
                             };
            //                 esq mei dir
            int[] ultimaPos = { 0,  4,  4 };

            

            var prolog = new PrologEngine(persistentCommandHistory: false);


            prolog.ConsultFromString("go :- solve(4, esquerda, meio, direita).\n\nsolve(N, A,B,C) :- N == 0.\nsolve(N, A,B,C) :-\n  M is N - 1,\n  solve(M, A, C, B),\n  move(A, C),\n  solve(M, B, A, C).\n\nmove(A, B) :- write('Move o disco da(o) '),write(A),write(' para a(o) '),write(B),nl.\n");

            
            var arrayLinhas = pegarQuery(prolog);

            jogando(arrayLinhas, ref ultimaPos, ref jogo);


         

        }
    }
}

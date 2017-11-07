using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioDoCavalo
{
    public class Program
    {
        static int resultado = 1;
        static int Tamanho = 8;
        static long tentativa = 0;

        static List<int[][]> respostas = new List<int[][]>();

        static void Main(string[] args)
        {
            int[][] tabuleiro = CriaTabuleiro();
            int jogadas = 1;
            Posicao posicaoAtual = SetPostInicial(tabuleiro, jogadas);
            Console.Out.WriteLine("Calculando...");
            Jogar(jogadas, posicaoAtual, tabuleiro);

        }

        private static void Jogar(int jogadas, Posicao posicaoAtual, int[][] tabuleiro)
        {
            tentativa++;

            if (jogadas == Tamanho * Tamanho) SalvaResultado(tabuleiro);

            var movimentos = MovimentoCavalor(posicaoAtual, tabuleiro);

            Parallel.ForEach(movimentos, movimento =>
            {
                int[][] copia = CopiaTabuleiro(tabuleiro);
                copia[movimento.linha][movimento.coluna] = jogadas + 1;
                Jogar(jogadas + 1, movimento, copia);
            });
        }

        private static int[][] CriaTabuleiro()
        {
            int[][] tabuleiro = new int[Tamanho][];
            PopulaTabuleiro(tabuleiro);
            return tabuleiro;
        }

        private static int[][] CopiaTabuleiro(int[][] tabuleiro)
        {
            int[][] result = new int[tabuleiro.Length][];
            for (int i = 0; i < tabuleiro.Length; i++)
            {
                result[i] = new int[tabuleiro[i].Length];
                for (int o = 0; o < tabuleiro[i].Length; o++)
                {
                    result[i][o] = tabuleiro[i][o];
                }
            }

            return result;
        }

        private static void ImprimeTabuleiro(int[][] tabuleiro)
        {
            Console.Out.WriteLine("TENTATIVA :" + tentativa);
            for (int i = 0; i < tabuleiro[0].Length; i++)
                Console.Out.Write("-");
            Console.Out.WriteLine();

            foreach (var item in tabuleiro)
            {
                foreach (var i in item)
                {
                    if(i < 10)
                        Console.Out.Write(" " + i + "|");
                    else
                        Console.Out.Write( i + "|");
                }
                Console.Out.WriteLine();
            }
        }

        private static void SalvaResultado(int[][] tabuleiro)
        {
            if (!ExisteResultado(tabuleiro))
            {
                var copia = CopiaTabuleiro(tabuleiro);
                respostas.Add(copia);

                Console.Out.WriteLine();
                Console.Out.WriteLine("RESULTADO " + resultado + ": ");
                resultado++;
                ImprimeTabuleiro(tabuleiro);
                Console.Out.WriteLine();
            }
            
        }

        private static bool ExisteResultado(int[][] tabuleiro)
        {
            if (respostas.Count == 0) return false;

            foreach (var item in respostas)
            {
                for (int i = 0; i < item.Length; i++)
                    for (int o = 0; o < item[i].Length; o++)
                        if (tabuleiro[i][o] != item[i][o])
                            return false;
            }
            return true;
        }

        private static Posicao SetPostInicial(int[][] tabuleiro, int jogadas)
        {
            tabuleiro[0][0] = jogadas;
            return new Posicao
            {
                coluna = 0,
                linha = 0
            };
        }

        private static void PopulaTabuleiro(int[][] tabuleiro)
        {
            for (int i = 0; i < Tamanho; i++)
            {
                tabuleiro[i] = new int[Tamanho];
                for (int j = 0; j < Tamanho; j++)
                    tabuleiro[i][j] = 0;
            }
        }

        static List<Posicao> MovimentoCavalor(Posicao pos, int[][] tabuleiro)
        {
            var result = new List<Posicao>();
            int[] frente = { 2, 1, -1 , -2 };
            int[] direita = { 1, 2, -1 ,-2 };

            foreach (var f in frente)
                if (pos.coluna + f >= 0 && pos.coluna + f < Tamanho)
                    foreach (var d in direita)
                        if (pos.linha + d >= 0 && pos.linha + d < Tamanho)
                            if (Modulo(f) != Modulo(d))
                                if(tabuleiro[pos.linha+ d][pos.coluna + f] == 0)
                                    if(!result.Any(c => c.coluna == pos.coluna + f 
                                        && c.linha == pos.linha + d))
                                        result.Add(new Posicao
                                        {
                                            coluna = pos.coluna + f,
                                            linha = pos.linha + d
                                        });
             return result;
        }

        private static int Modulo(int f)
        {
            return f < 0 ? -f : f;
        }
    }

    public class Posicao
    {
        public int coluna { get; set; }
        public int linha { get; set; }
    }
}

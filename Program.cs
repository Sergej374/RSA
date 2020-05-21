using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSA
{
    class Program
    {
        public static Dictionary<char, int> alphabet = new Dictionary<char, int>()
            {
                { 'А', 10 }, { 'Б', 11 }, { 'В', 12}, { 'Г', 13}, { 'Д', 14}, { 'Е', 15}, { 'Ж', 16}, { 'З', 17}, { 'И', 18}, { 'Й', 19},
                { 'К', 20}, { 'Л', 21}, { 'М', 22}, { 'Н', 23}, { 'О', 24}, { 'П', 25}, { 'Р', 26}, { 'С', 27}, { 'Т', 28}, { 'У', 29}, { 'Ф', 30}, { 'Х', 31},
                { 'Ц', 32}, { 'Ч', 33},{ 'Ш', 34}, { 'Щ', 35}, { 'Ъ', 36}, { 'Ы', 37}, { 'Ь', 38}, { 'Э', 39}, { 'Ю', 40}, { 'Я', 41}, { ' ', 99}
            };

        public static Dictionary<BigInteger, char> reversedAlphabet = new Dictionary<BigInteger, char>()
            {
                { 10, 'А' }, { 11, 'Б' }, { 12, 'В' }, { 13, 'Г' }, { 14, 'Д' }, { 15, 'Е' }, { 16, 'Ж' }, { 17, 'З' }, { 18, 'И' }, { 19, 'Й' },
                { 20, 'К' }, { 21, 'Л' }, { 22, 'М' }, { 23, 'Н' }, { 24, 'О' }, { 25, 'П' }, { 26, 'Р' }, { 27, 'С' }, { 28, 'Т' }, { 29, 'У' }, { 30, 'Ф'}, { 31, 'Х' },
                { 32, 'Ц'}, { 33, 'Ч'},{ 34, 'Ш'}, { 35, 'Щ'}, { 36, 'Ъ'}, {37, 'Ы'}, { 38, 'Ь'}, { 39, 'Э'}, { 40, 'Ю'}, { 41, 'Я'}, { 99, ' '}
            };
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("To encrypt a message press 1\nTo decrypt a message press 2\nTo exit the application press ESC");
                switch (Console.ReadKey().Key)
                {
                    //Encrypting
                    case ConsoleKey.D1:
                        Console.Clear();
                        Console.Write("Enter the p: ");
                        int p = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter the q: ");
                        int q = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter your message below:");
                        string message = Console.ReadLine();
                        Console.WriteLine("\n\n\n");
                        string messToEncr = "";
                        foreach (char c in message)
                        {
                            messToEncr += alphabet[c].ToString();
                        }

                        var cypher = Encrypt(messToEncr, p, q);
                        Console.WriteLine(cypher);

                        Console.WriteLine("\n\n\nPress any key to continue");
                        Console.ReadKey();
                        break;
                    //Decrypting
                    case ConsoleKey.D2:
                        Console.Clear();
                        Console.WriteLine("Enter the message to decrypt:");
                        string encryptedMessage = Console.ReadLine();
                        Console.Write("\nd = ");
                        int d = Convert.ToInt32(Console.ReadLine());
                        Console.Write("n = ");
                        int n = Convert.ToInt32(Console.ReadLine());
                        var decrMessage = Decrypt(encryptedMessage, d, n);
                        Console.WriteLine("Decrypted message: \n\n" + decrMessage);
                        Console.WriteLine("\n\nPress any key to continue");
                        Console.ReadKey();
                        break;
                    //Exit
                    case ConsoleKey.Escape:
                        return;
                }
            };
        }

        public static string Decrypt(string cypher, int d, int n)
        {
            //putting numbers into array
            int clustersSize = Convert.ToInt32(cypher.Substring(cypher.Length - 1));
            cypher = cypher.Remove(cypher.Length - 1);
            BigInteger[] parts = new BigInteger[cypher.Length / clustersSize];
            for (int i = 0, c = 0; c < parts.Length; i += clustersSize, c++)
            {
                parts[c] = Convert.ToInt64(cypher.Substring(i, clustersSize));
            }

            //decrypting
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = BigInteger.Pow(parts[i], d) % n;
            }

            //forming output
            string message = "";
            foreach (var key in parts)
            {
                message += reversedAlphabet[key];
            }

            return message;
        }

        public static string Encrypt(string mStr, int p, int q)
        {
            int n = p * q;
            long f = (p - 1) * (q - 1);
            int e = 1;
            double d = 1;

            //solving e
            for (double i = 1; i <= f; i++)
            {
                if (IsPrime(i) && AreReversedPrime(f, i))
                {
                    e = Convert.ToInt32(i);
                    break;
                }
            }

            //solving d
            for (double i = 1; i <= n; i++)
            {
                if (i * e % f == 1)
                {
                    d = i;
                    break;
                }
            }

            //solving cluster size
            int cluster = 1;
            for (double i = 2; i <= mStr.Length; i++)
            {
                if (mStr.Length / i == Convert.ToInt32(mStr.Length / i))
                {
                    cluster = Convert.ToInt32(i);
                    break;
                }
            }

            //putting numbers into clusters
            BigInteger[] mParts = new BigInteger[Convert.ToInt32(mStr.Length / cluster)];
            for (int i = 0; i < mParts.Length; i++)
            {
                mParts[i] = Convert.ToInt64(mStr.Substring(i * cluster, cluster));
            }

            //encrypting
            for (int i = 0; i < mParts.Length; i++)
            {
                mParts[i] = BigInteger.Pow(mParts[i], e) % n;
            }
            string[] output = new string[mParts.Length];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = mParts[i].ToString();
            }

            //correcting cluster sizes
            string checker = "";
            foreach (var i in output)
            {
                if (i.Length > checker.Length)
                    checker = i;
            }
            int encrClusterSize = checker.Length;
            Console.WriteLine("Encrypted cluster size = " + encrClusterSize + "\n\n");
            for (int i = 0; i < output.Length; i++)
            {
                while (output[i].Length < encrClusterSize)
                {
                    output[i] = output[i].Insert(0, "0");
                }
            }

            string outputString = "";
            foreach (string s in output)
            {
                outputString += s;
            }

            outputString += encrClusterSize.ToString();
            return outputString;
        }

        public static bool AreReversedPrime(double number1, double number2)
        {
            if (number1 / number2 != Convert.ToInt32(number1 / number2))
                return true;
            else
                return false;
        }

        public static bool IsPrime(double number)
        {
            if (number <= 1)
                return false;

            for (double i = 1; i <= number; i++)
            {
                if (i != 1 && i != number)
                {
                    if (number / i == Convert.ToInt32(number / i))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}

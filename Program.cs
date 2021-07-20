using System.Data;
using System.Text;
using System.Security.Cryptography;
using System;

namespace rockpapersciccors
{
    class Program
    {   
        public static byte[] KeyGenerating(){
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            var key = new byte[16];
            rng.GetBytes(key);
            return key;
        }

        public static int GetMove(int amountOfMoves){
            Random rndm = new Random();
            return rndm.Next(1, amountOfMoves)-1;
        }

        public static string GetHMAC(byte[] key, string message){
            var hmac = new HMACSHA256 (key);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();        
        }

        public static void Menu (string[] args){
            Console.WriteLine("Available moves:");
            for (int i = 1; i <= args.Length; i++){
                Console.WriteLine(i+" - " + args[i-1]);
            }
            Console.WriteLine("0 - Exit");
        }

        public static string WhoWins(int plInd, int cmpInd, int length){
            if (plInd <= length/2 && (cmpInd-plInd <= length/2 && cmpInd-plInd > 0) ){
                return "You win!";
            }else if (plInd > length/2 && cmpInd-plInd < length/2 && cmpInd>plInd){
                return "You win!";
            }else if (plInd > length/2 && cmpInd<plInd && plInd-cmpInd > length/2){
                return "You win!";
            }else if (plInd == cmpInd){
                return "Draw!";
            }else{
                return "You lose!";
            }
        }

        static void Main(string[] args)
        {   
            if (args.Length%2 == 1 && args.Length>=3){
                var key = KeyGenerating();
                int computerMoveIndex = GetMove(args.Length);
                string HMAC = GetHMAC(key, args[computerMoveIndex]);
                Console.WriteLine("HMAC: " + HMAC);

                Menu(args);

                //Make movemenents
                Console.Write("Enter your move: ");
                int playerMoveIndex = Convert.ToInt32(Console.ReadLine())-1;
                if (playerMoveIndex == -1) { System.Environment.Exit(1);}
                Console.WriteLine("Your move: " + args[playerMoveIndex]);
                Console.WriteLine("Computer move: " + args[computerMoveIndex]);
                //Who wins the game
                string result = WhoWins(playerMoveIndex, computerMoveIndex, args.Length);
                Console.WriteLine(result);
                Console.WriteLine("HMAC key: " + BitConverter.ToString(key).Replace("-", "").ToLower());

            }else{
                Console.WriteLine("Please input an odd number of any 3 or more arguments like this:");
                Console.WriteLine("For example: `rock scissors paper lizard spook 1 2 3 Rock Paper`");
            }
        }
    }
}

using System;
using Impression;

namespace Example06Model
{
    class Program
    {
        static void Main(string[] args)
		{
			using(var game = new Example06ModelGame())
            {
                game.Run();
            }
		}
    }
}
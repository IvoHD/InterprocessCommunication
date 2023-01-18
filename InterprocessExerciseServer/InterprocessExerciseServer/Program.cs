using System.IO.Pipes;

class PipeServer
{
	static string[] RockPaperScissorArray { get; set; } =
	{
		"r",
		"p",
		"s"
	};
							  
	static Random Random = new();

	static void Main()
	{
		Task write = RockPaperScissors();

		write.Wait();
	}

	static async Task RockPaperScissors()
	{
		using (NamedPipeServerStream pipeServer = new("RockPaperScissor", PipeDirection.InOut))
		{
			await pipeServer.WaitForConnectionAsync();

			using (StreamReader sr = new(pipeServer))
			{
				string temp = null;

				while ((temp = sr.ReadLine()) is not null)
				{
					Console.WriteLine("Received from client: " + temp);

					int res = new();
					string serverAction = RockPaperScissorArray[Random.Next(0, RockPaperScissorArray.Length)];

					switch (temp.ToString())
					{
						case "r":
							if (serverAction == "r")
								res = 0;
							else if (serverAction == "p")
								res = -1;
							else
								res = 1;
							break;
						case "p":
							if (serverAction == "p")
								res = 0;
							else if (serverAction == "s")
								res = -1;
							else
								res = 1;
							break;
						case "s":
							if (serverAction == "s")
								res = 0;
							else if (serverAction == "r")
								res = -1;
							else
								res = 1;
							break;
						default:
							throw new ArgumentException();
							break;
					}		 

					using (StreamWriter sw = new(pipeServer, leaveOpen: true))
					{
						sw.AutoFlush = true;
						sw.WriteLine(res);
						Console.WriteLine("Server action was: " + serverAction);
						Console.WriteLine("Server send: " + res);
					}
					temp = null;
				}
			}
		}
	}
}

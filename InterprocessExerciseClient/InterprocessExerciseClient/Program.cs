using System.IO.Pipes;

class PipeServer
{
	static void Main()
	{
		Task sendTask = RockPaperScissors();

		sendTask.Wait();
	}

	static async Task RockPaperScissors()
	{
		int score = 0;

		using (NamedPipeClientStream pipeClient = new(".", "RockPaperScissor", PipeDirection.InOut))
		{
			await pipeClient.ConnectAsync();

			string input = null;
			PrintScore(score);

			while ((input = Console.ReadLine()) is not null)
			{
				using (StreamWriter sw = new(pipeClient, leaveOpen: true))
				{
					sw.AutoFlush = true;
					if(input == "s" || input == "r" || input == "p")
					{
						sw.WriteLine(input);
						Console.WriteLine("Client send: " + input);
					}
					else
					{
						Console.WriteLine("Invalid input");
						Console.ReadKey();
						PrintScore(score);
						input = null;
						continue;
					}
				}

				using (StreamReader sr = new(pipeClient, leaveOpen: true))
				{
					string temp = null;

					while ((temp = sr.ReadLine()) is not null)
					{
						Console.WriteLine("Client received: " + temp);
						score += Convert.ToInt32(temp);
						Console.ReadKey();
						PrintScore(score);
						input = null;
						break;
					}
				}
			}
		}		
	}

	static void PrintScore(int score, string error = "")
	{
		if (error is not "")
			error += '\n';
		Console.Clear();
		Console.WriteLine("Please input\t\n r for rock\t\n s for scissors\t\n p for paper");
		Console.WriteLine("Current score: " + score);
		Console.Write(error);
	}
}
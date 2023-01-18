using System.IO.Pipes;

class PipeClient
{
	static void Main()
	{
		Task read = ReadText();

		read.Wait();
	}

	static async Task ReadText()
	{
		using (NamedPipeClientStream pipeClient = new(".", "Testpipe", PipeDirection.In))
		{
			await pipeClient.ConnectAsync();

			// Display text sent by server to the console
			using (StreamReader sr = new(pipeClient))
			{
				string temp;

				while ((temp = sr.ReadLine()) != null)
				{
					Console.WriteLine("Received from server: " + temp);
					temp = null;
				}
			}
		}
	}
}
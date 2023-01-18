using System.IO.Pipes;

class PipeServer
{
	static void Main()
	{
		Task write = WriteText();

		write.Wait();
	}

	static async Task WriteText()
	{
		using (NamedPipeServerStream pipeServer = new("Testpipe", PipeDirection.Out))
		{
			await pipeServer.WaitForConnectionAsync();

			// Read user input and send that to the client process.
			using (StreamWriter sw = new(pipeServer))
			{
				sw.AutoFlush = true;
				string input = null;
				while ((input = Console.ReadLine()) != null)
				{
					sw.WriteLine(input);
					Console.WriteLine("Server send: " + input);
				}
			}
		}
	}
}
using ClinetAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPUMProject.Logic.Abstract;

namespace ServerPresentation
{
    internal class ServerPresentation
    {
        private readonly AbstractLogicAPI logicAPI;
        private WebSocketConnection? connection;

        private ServerPresentation(AbstractLogicAPI logicAPI)
        {
            this.logicAPI = logicAPI;
            this.logicAPI.BookService.BookRepositoryChanged += HandleBookRepositoryChanged;
            this.logicAPI.BookService.UserChanged += HandleUserChanged;
        }

        private async Task StartConnection()
        {
            while (true)
            {
                Console.WriteLine("Waiting for connection...");
                await ServerWebSocket.StartServer(42069, OnConnect);
            }
        }

        private void OnConnect(WebSocketConnection connection)
        {
            Console.WriteLine("Connected to " + connection);

            connection.OnMessage = OnMessage;
            connection.OnClose = OnClose;
            connection.OnError = OnError;

            this.connection = connection;
        }

        private async void OnMessage(string message)
        {
            if(connection == null) return;

            Console.WriteLine("Message received: " + message);

            Serializer serializer = Serializer.Create();

            if(serializer.GetCommandHeader(message) == SellBookCommand.StaticHeader)
            {
                SellBookCommand sellBook = serializer.Deserialize<SellBookCommand>(message);
                Task taks = Task.Run(async () => await SendBooks());
            } 
            else if(serializer.GetCommandHeader(message) == GetBooksCommand.StaticHeader)
            {

            }
        }

        private async Task SendBooks()
        {
            if(connection == null) return;

            Console.WriteLine("Sending books' data...");

            AllBooksUpdateResponse allBooksUpdateResponse = new AllBooksUpdateResponse();
            IEnumerable<ILogicBook> books = logicAPI.BookService.GetAvailableBooks();
            allBooksUpdateResponse.Books = books.Select(x => x.ConvertToDTO()).ToArray();

            Serializer serializer = Serializer.Create();
            string json = serializer.Serialize(allBooksUpdateResponse);
            Console.WriteLine(json);

            await connection.SendAsync(json);
        }

        private async void HandleBookRepositoryChanged(object sender, LogicBookRepositoryChangedEventArgs e)
        {
            if(connection == null) return;

            Console.WriteLine("Changed book n.o: " + e.AffectedBook.Id);

            BookChangedResponse bookChangedResponse = new BookChangedResponse(e.ChangedEventType);
            bookChangedResponse.book = e.AffectedBook.ConvertToDTO();
            
            Serializer serializer = Serializer.Create();
            string json = serializer.Serialize(bookChangedResponse);
            Console.WriteLine(json);

            await connection.SendAsync(json);
        }

        private async void HandleUserChanged(object sender, LogicUserChangedEventArgs e)
        {
            if(connection == null) return;

            Console.WriteLine("User changed: " + e.user.Name);

            UserChangedResponse userChangedResponse = new UserChangedResponse();
            userChangedResponse.User = e.user.ConvertToDTO();

            Serializer serializer = Serializer.Create();
            string json = serializer.Serialize(userChangedResponse);
            Console.WriteLine(json);

            await connection.SendAsync(json);
        }

        private void OnClose()
        {
            Console.WriteLine("Connection closed");
            connection = null;
        }

        private void OnError()
        {
            Console.WriteLine("Error!");
        }

    }
}

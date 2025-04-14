﻿using ConnectionAPI;
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

        private Dictionary<Guid, WebSocketConnection> connections = new();

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

            connections.TryAdd(connection.connectionID, connection);
        }

        private async void OnMessage(string message, Guid connectionID)
        {
            if (!connections.TryGetValue(connectionID, out WebSocketConnection? connection) || connection == null) return;

            Console.WriteLine("Message received: " + message);

            Serializer serializer = Serializer.Create();

            if(serializer.GetCommandHeader(message) == SellBookCommand.StaticHeader)
            {
                SellBookCommand sellBookCommand = serializer.Deserialize<SellBookCommand>(message);
                
                int result = logicAPI.BookService.BuyBook(sellBookCommand.BookID, sellBookCommand.Username);
                TransactionResultResponse response = new TransactionResultResponse(sellBookCommand.BookID, sellBookCommand.Username,result);

                string json = serializer.Serialize(response);
                Console.WriteLine(json);

                await connection.SendAsync(json);
            } 
            else if(serializer.GetCommandHeader(message) == GetBooksCommand.StaticHeader)
            {
                GetBooksCommand sellBook = serializer.Deserialize<GetBooksCommand>(message);
                await SendBooks(connectionID);
            }
            else if(serializer.GetCommandHeader(message) == GetUserCommand.StaticHeader)
            {
                GetUserCommand userCommand = serializer.Deserialize<GetUserCommand>(message);
                await SendUser(userCommand.Username,connectionID);
            }
        }

        private async Task SendUser(string username, Guid connectionID)
        {
            if (!connections.TryGetValue(connectionID, out WebSocketConnection? connection) || connection == null) return;

            Console.WriteLine("Sending user data...");

            UserChangedResponse response = new UserChangedResponse();
            response.User = logicAPI.GetUser(username).ConvertToDTO();

            Serializer serializer = Serializer.Create();
            string json = serializer.Serialize(response);
            Console.WriteLine(json);

            await connection.SendAsync(json);
        }

        private async Task SendBooks(Guid connectionID)
        {
            if(!connections.TryGetValue(connectionID, out WebSocketConnection? connection) || connection == null) return;

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
            if(connections.Count == 0) return;

            Console.WriteLine("Changed book n.o: " + e.AffectedBook.Id);

            BookChangedResponse bookChangedResponse = new BookChangedResponse(e.ChangedEventType);
            bookChangedResponse.Book = e.AffectedBook.ConvertToDTO();

            Serializer serializer = Serializer.Create();
            string json = serializer.Serialize(bookChangedResponse);
            Console.WriteLine(json);

            foreach(var connection in connections)
            {
                await connection.Value.SendAsync(json);
            }
        }

        private async void HandleUserChanged(object sender, LogicUserChangedEventArgs e)
        {
            if(connections.Count == 0) return;

            Console.WriteLine("User changed: " + e.user.Name);

            UserChangedResponse userChangedResponse = new UserChangedResponse();
            userChangedResponse.User = e.user.ConvertToDTO();

            Serializer serializer = Serializer.Create();
            string json = serializer.Serialize(userChangedResponse);
            Console.WriteLine(json);

            foreach (var connection in connections)
            {
                await connection.Value.SendAsync(json);
            }
        }

        private void OnClose(Guid connectionID)
        {
            Console.WriteLine("Connection closed");
            connections.Remove(connectionID);
        }

        private void OnError(Guid connectionID)
        {
            Console.WriteLine("Error encountered with connection n.o: " + connectionID);
        }

        private static async Task Main(string[] args)
        {
            ServerPresentation serverPresentation = new ServerPresentation(AbstractLogicAPI.Create());
            await serverPresentation.StartConnection();
        }

    }
}
